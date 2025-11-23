# Diagramas de Flujo - ControlObraApi

Este documento contiene los diagramas de flujo completos del proyecto **ControlObraApi**, una API RESTful para la gestión y seguimiento de proyectos de construcción.

## Tabla de Contenidos
- [1. Arquitectura General del Sistema](#1-arquitectura-general-del-sistema)
- [2. Modelo de Datos y Relaciones](#2-modelo-de-datos-y-relaciones)
- [3. Flujo de Datos por Endpoint](#3-flujo-de-datos-por-endpoint)
- [4. Flujo Completo del Sistema](#4-flujo-completo-del-sistema)

---

## 1. Arquitectura General del Sistema

Este diagrama muestra la arquitectura en capas de la aplicación:

```mermaid
graph TB
    subgraph "Cliente Frontend"
        A[Angular App<br/>localhost:4200]
    end
    
    subgraph "API Layer - ASP.NET Core"
        B[Program.cs<br/>Configuración CORS, DbContext, Validators]
        C[Controllers<br/>ProyectosController<br/>EstimacionesController<br/>AvancesController]
    end
    
    subgraph "Business Logic Layer"
        D[FluentValidation<br/>EstimacionCostoValidator<br/>AvanceObraValidator]
        E[DTOs<br/>AvanceObraCreateDTO<br/>EstimacionCostoCreateDTO<br/>PatchDTOs]
    end
    
    subgraph "Data Access Layer"
        F[Entity Framework Core<br/>AppDbContext]
        G[Models<br/>Proyecto<br/>EstimacionCosto<br/>AvanceObra]
    end
    
    subgraph "Database"
        H[(SQL Server<br/>ConexionSQL)]
    end
    
    A -->|HTTP Requests| B
    B --> C
    C --> E
    C --> D
    D -->|Validación| C
    E -->|Mapping| G
    C --> F
    F --> G
    G --> H
    H -->|Data| G
    G -->|Response| C
    C -->|JSON| A
    
    style A fill:#e1f5ff
    style C fill:#fff4e1
    style D fill:#ffe1f5
    style E fill:#ffe1f5
    style F fill:#e1ffe1
    style G fill:#e1ffe1
    style H fill:#f0f0f0
```

---

## 2. Modelo de Datos y Relaciones

Este diagrama muestra las entidades del sistema y sus relaciones:

```mermaid
erDiagram
    PROYECTO ||--o{ ESTIMACION_COSTO : "tiene"
    ESTIMACION_COSTO ||--o{ AVANCE_OBRA : "registra"
    
    PROYECTO {
        int ProyectoID PK
        string NombreObra
        string Ubicacion
        datetime FechaInicio
    }
    
    ESTIMACION_COSTO {
        int CostoID PK
        int ProyectoID FK
        string Concepto
        decimal MontoEstimado
        byte[] RowVersion
    }
    
    AVANCE_OBRA {
        int AvanceID PK
        int CostoID FK
        decimal MontoEjecutado
        decimal PorcentajeCompletado
        datetime FechaRegistro
    }
```

**Relaciones:**
- Un `Proyecto` puede tener **muchas** `EstimacionCosto`
- Una `EstimacionCosto` puede tener **muchos** `AvanceObra`
- Cada `AvanceObra` pertenece a **una** `EstimacionCosto`

---

## 3. Flujo de Datos por Endpoint

### 3.1 Flujo: Crear Avance de Obra (POST /api/Avances)

```mermaid
sequenceDiagram
    participant C as Cliente Angular
    participant AC as AvancesController
    participant V as AvanceObraValidator
    participant DB as AppDbContext
    participant SQL as SQL Server
    
    C->>AC: POST /api/Avances<br/>{AvanceObraCreateDTO}
    AC->>AC: Convertir DTO a AvanceObra
    AC->>V: ValidateAsync(avance)
    
    alt Validación Fallida
        V-->>AC: ValidationResult (Errors)
        AC-->>C: 400 BadRequest<br/>(Lista de errores)
    else Validación Exitosa
        V-->>AC: ValidationResult (IsValid)
        AC->>DB: Add(avance)
        DB->>SQL: INSERT INTO AvancesObra
        SQL-->>DB: Success
        DB-->>AC: SaveChangesAsync()
        AC-->>C: 201 Created<br/>{avance con AvanceID}
    end
```

### 3.2 Flujo: Obtener Análisis Financiero (GET /api/Proyectos/{id}/desviacion)

```mermaid
sequenceDiagram
    participant C as Cliente Angular
    participant PC as ProyectosController
    participant DB as AppDbContext
    participant SQL as SQL Server
    
    C->>PC: GET /api/Proyectos/5/desviacion
    PC->>DB: Proyectos.Include(Estimaciones).ThenInclude(Avances)
    DB->>SQL: SELECT con JOINS
    
    alt Proyecto No Encontrado
        SQL-->>DB: NULL
        DB-->>PC: NULL
        PC-->>C: 404 NotFound
    else Proyecto Encontrado
        SQL-->>DB: Proyecto con datos
        DB-->>PC: Proyecto completo
        
        PC->>PC: Calcular métricas:<br/>- PresupuestoTotal<br/>- MontoEjecutado<br/>- Desviacion<br/>- PorcentajeDesviacion
        
        PC-->>C: 200 OK<br/>{análisis financiero}
    end
```

### 3.3 Flujo: Actualización Parcial (PATCH /api/Estimaciones/{id})

```mermaid
sequenceDiagram
    participant C as Cliente Angular
    participant EC as EstimacionesController
    participant DB as AppDbContext
    participant SQL as SQL Server
    
    C->>EC: PATCH /api/Estimaciones/10<br/>{EstimacionPatchDTO}
    EC->>DB: FindAsync(id)
    
    alt Estimación No Existe
        DB-->>EC: NULL
        EC-->>C: 404 NotFound
    else Estimación Existe
        DB-->>EC: EstimacionCosto
        
        EC->>EC: Aplicar cambios parciales:<br/>- Concepto (si presente)<br/>- MontoEstimado (si presente)
        
        alt MontoEstimado <= 0
            EC-->>C: 400 BadRequest
        else Datos Válidos
            EC->>DB: SaveChangesAsync()
            DB->>SQL: UPDATE EstimacionesCosto
            SQL-->>DB: Success
            DB-->>EC: Updated
            EC-->>C: 200 OK<br/>{estimación actualizada}
        end
    end
```

### 3.4 Flujo: Eliminar Proyecto con Validación (DELETE /api/Proyectos/{id})

```mermaid
sequenceDiagram
    participant C as Cliente Angular
    participant PC as ProyectosController
    participant DB as AppDbContext
    participant SQL as SQL Server
    
    C->>PC: DELETE /api/Proyectos/3
    PC->>DB: Proyectos.Include(Estimaciones)<br/>.ThenInclude(Avances)
    DB->>SQL: SELECT con JOINS
    
    alt Proyecto No Existe
        SQL-->>DB: NULL
        DB-->>PC: NULL
        PC-->>C: 404 NotFound
    else Proyecto Existe
        SQL-->>DB: Proyecto con relaciones
        DB-->>PC: Proyecto
        
        alt Tiene Estimaciones
            PC-->>C: 400 BadRequest<br/>"Elimine estimaciones primero"
        else Sin Estimaciones
            PC->>DB: Remove(proyecto)
            DB->>SQL: DELETE FROM Proyectos
            SQL-->>DB: Success
            DB-->>PC: Deleted
            PC-->>C: 204 NoContent
        end
    end
```

---

## 4. Flujo Completo del Sistema

Este diagrama muestra el flujo de trabajo completo desde la creación de un proyecto hasta el análisis de desviación:

```mermaid
graph TD
    Start([Usuario inicia])
    
    A[1. Crear Proyecto<br/>POST /api/Proyectos]
    B[2. Proyecto creado<br/>ProyectoID generado]
    
    C[3. Crear Estimación de Costo<br/>POST /api/Estimaciones]
    D{¿Validación<br/>exitosa?}
    E[4. Estimación creada<br/>CostoID generado]
    
    F[5. Registrar Avance de Obra<br/>POST /api/Avances]
    G{¿Validación<br/>exitosa?}
    H[6. Avance registrado<br/>AvanceID generado]
    
    I{¿Más<br/>avances?}
    
    J[7. Consultar Análisis Financiero<br/>GET /api/Proyectos/ID/desviacion]
    K[8. Sistema calcula:<br/>- Presupuesto Total<br/>- Monto Ejecutado<br/>- Desviación<br/>- % Desviación]
    L[9. Mostrar resultados en Frontend]
    
    M([Fin])
    
    Error1[Error: Mostrar mensajes<br/>de validación]
    Error2[Error: Mostrar mensajes<br/>de validación]
    
    Start --> A
    A --> B
    B --> C
    C --> D
    D -->|No| Error1
    Error1 --> C
    D -->|Sí| E
    E --> F
    F --> G
    G -->|No| Error2
    Error2 --> F
    G -->|Sí| H
    H --> I
    I -->|Sí| F
    I -->|No| J
    J --> K
    K --> L
    L --> M
    
    style Start fill:#90EE90
    style M fill:#FFB6C1
    style Error1 fill:#FF6B6B
    style Error2 fill:#FF6B6B
    style A fill:#87CEEB
    style C fill:#87CEEB
    style F fill:#87CEEB
    style J fill:#87CEEB
    style K fill:#FFD700
```

---

## Endpoints Disponibles

### **Proyectos** (`/api/Proyectos`)
| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/Proyectos` | Listar todos los proyectos |
| GET | `/api/Proyectos/{id}` | Obtener proyecto con estimaciones |
| POST | `/api/Proyectos` | Crear nuevo proyecto |
| PUT | `/api/Proyectos/{id}` | Actualización completa |
| PATCH | `/api/Proyectos/{id}` | Actualización parcial |
| DELETE | `/api/Proyectos/{id}` | Eliminar proyecto |
| GET | `/api/Proyectos/{id}/desviacion` | **Análisis financiero** |

### **Estimaciones** (`/api/Estimaciones`)
| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/Estimaciones/{id}` | Obtener estimación con avances |
| POST | `/api/Estimaciones` | Crear nueva estimación |
| PUT | `/api/Estimaciones/{id}` | Actualización completa |
| PATCH | `/api/Estimaciones/{id}` | Actualización parcial |
| DELETE | `/api/Estimaciones/{id}` | Eliminar estimación |

### **Avances** (`/api/Avances`)
| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/Avances` | Listar todos los avances |
| GET | `/api/Avances/{id}` | Obtener avance específico |
| POST | `/api/Avances` | Registrar nuevo avance |
| PUT | `/api/Avances/{id}` | Actualización completa |
| PATCH | `/api/Avances/{id}` | Actualización parcial |
| DELETE | `/api/Avances/{id}` | Eliminar avance |
| GET | `/api/Avances/porEstimacion/{costoId}` | Avances por estimación |

---

## Características Técnicas

### **1. CORS Configuration**
- Permitido: `http://localhost:4200` (Angular)
- Headers: Cualquiera
- Métodos: Cualquiera

### **2. Base de Datos**
- **Motor**: SQL Server
- **ORM**: Entity Framework Core
- **Optimización**: Split Query Behavior (evita cartesian explosion)

### **3. Validación**
- **FluentValidation** para lógica de negocio
- Validaciones aplicadas antes de guardar en BD

### **4. Serialización JSON**
- **Newtonsoft.Json** 
- `ReferenceLoopHandling.Ignore` (evita ciclos en relaciones)

### **5. Optimistic Concurrency**
- Campo `RowVersion` en `EstimacionCosto`
- Detecta modificaciones concurrentes

### **6. DTOs**
- **Create DTOs**: Para inserción (sin IDs)
- **Patch DTOs**: Para actualización parcial (campos opcionales)

---

## Flujo de Validaciones

```mermaid
graph LR
    A[Request con DTO] --> B{¿DTO válido?}
    B -->|No| C[400 BadRequest]
    B -->|Sí| D[Convertir a Entidad]
    D --> E{¿FluentValidation?}
    E -->|Errores| F[400 BadRequest<br/>Lista de errores]
    E -->|Válido| G[Guardar en BD]
    G --> H{¿Conflicto<br/>Concurrencia?}
    H -->|Sí| I[409 Conflict]
    H -->|No| J[Éxito: 201/200/204]
    
    style C fill:#FF6B6B
    style F fill:#FF6B6B
    style I fill:#FFA500
    style J fill:#90EE90
```

---

## Notas Importantes

> [!IMPORTANT]
> **Optimistic Concurrency**: La tabla `EstimacionCosto` usa `RowVersion` para Control de Concurrencia Optimista. Si dos usuarios modifican la misma estimación simultáneamente, el segundo recibirá un error 409.

> [!WARNING]
> **Eliminación en Cascada**: Para eliminar un `Proyecto`, primero deben eliminarse todas sus `Estimaciones`. Para eliminar una `Estimación`, primero deben eliminarse todos sus `Avances`.

> [!TIP]
> **Análisis Financiero**: El endpoint `/api/Proyectos/{id}/desviacion` calcula automáticamente la desviación presupuestal comparando el presupuesto total con los montos ejecutados.

---

**Fecha de Generación**: 2025-11-21  
**Versión API**: ASP.NET Core con Entity Framework Core  
**Base de Datos**: SQL Server
