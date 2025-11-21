# 🏗️ Sistema de Control de Presupuestos y Avance de Obra API

Este proyecto implementa una **API RESTful** utilizando **ASP.NET Core (.NET 8.0)** y **Entity Framework Core (EF Core)** para la gestión financiera y el control de integridad de datos en proyectos de construcción e ingeniería.

El sistema está diseñado para ser el **motor de lógica algorítmica** que predice el riesgo de sobrecoste (*Desviación Presupuestal*).

---

## 🚀 Arquitectura y Tecnologías Clave

* **Plataforma Principal:** **ASP.NET Core 8.0 (C#)**.
* **Base de Datos:** **SQL Server** (a través de `localhost, 1433`).
* **Mapeo ORM:** **Entity Framework Core (EF Core)**.
* **Validación:** **FluentValidation** (reglas de negocio para porcentajes y montos).
* **Operaciones Avanzadas:** Soporte para **`PATCH`** (actualización parcial con DTO) y **Control de Concurrencia** (Optimistic Locking con `RowVersion`).

---

## 🛠️ Guía de Instalación y Ejecución

Sigue estos pasos para obtener el código, configurar la base de datos y correr la API.

### 1. Requisitos Previos

1. **SDK de .NET 8.0:** Esencial para compilar y correr el proyecto.
2. **SQL Server (Local o Express):** El servicio debe estar activo y accesible en el puerto **`1433`** con las credenciales **`sa`/`Admin12345`**.
3. **Postman:** Para realizar las pruebas HTTP.

### 2. Clonación y Descarga de Dependencias

Abre tu terminal y ejecuta:

```bash
# 1. Clonar el repositorio
git clone [URL_DE_TU_REPOSITORIO]
cd ControlObraApi/ControlObraApi

# 2. Descargar las dependencias de NuGet
dotnet restore
```

### 3. Configuración y Migraciones de Base de Datos

Tu proyecto usa la clave `ConexionSQL` para la base de datos `ControlObraDB`.

```json
// Fragmento de appsettings.json
"ConnectionStrings": {
  "ConexionSQL": "Server=localhost, 1433;Database=ControlObraDB;User Id=sa;Password=Admin12345;TrustServerCertificate=True"
}
```

Aplica el esquema de la base de datos (creará las tablas `Proyectos`, `EstimacionesCosto`, `AvancesObra`):

```bash
# 1. Aplicar las migraciones de EF Core
dotnet ef database update
```

### 4. Correr la Aplicación

Inicia la API. El puerto predeterminado es **`5119`**.

```bash
dotnet run
```

> El servidor indicará el puerto exacto: `Now listening on: http://localhost:5119`.

---

## 🧪 Pruebas Funcionales en Postman

El ciclo de prueba verifica la **Integridad de Datos** y la **Lógica Predictiva**.

**URL Base para las Pruebas:** `http://localhost:5119/api` (usar el puerto que indique `dotnet run`).

### Flujo 1: Creación y Validación (POST)

| # | Descripción | Método | Endpoint | Body (JSON) | Resultado Esperado |
|:---|:---|:---|:---|:---|:---|
| **1** | **Crear Proyecto** | `POST` | `/Proyectos` | `{"nombreObra": "Torre Alpha", "ubicacion": "Zona Central"}` | **`201 Created`**. (Obtener `ProyectoID` 1) |
| **2** | **Crear Estimación** (Partida Presupuestal) | `POST` | `/Estimaciones` | `{"concepto": "Cimentación", "montoEstimado": 200000.00, "proyectoID": 1}` | **`201 Created`**. (Obtener `CostoID` 1) |
| **3** | **Fallo Esperado** (Validación: % > 100) | `POST` | `/Avances` | `{"montoEjecutado": 50000, "porcentajeCompletado": 105.00, "costoID": 1}` | **`400 Bad Request`** (Rechazo por FluentValidation) |
| **4** | **Registro Válido** (Datos para Cálculo) | `POST` | `/Avances` | `{"montoEjecutado": 50000.00, "porcentajeCompletado": 25.00, "costoID": 1}` | **`201 Created`**. |

---

### Flujo 2: Lógica Algorítmica y Operaciones Avanzadas

| # | Descripción | Método | Endpoint | Body (JSON) | Resultado Esperado |
|:---|:---|:---|:---|:---|:---|
| **5** | **GET Desviación** (Riesgo Predictivo) | **`GET`** | `/Proyectos/Desviacion/1` | *None* | **`200 OK`**. El `CostoProyectadoFinal` debe ser $200,000.00 (porque 50k / 25% = 200k). |
| **6** | **Actualización Parcial (`PATCH`)** | **`PATCH`** | `/Estimaciones/1` | `[{"op": "replace", "path": "/montoEstimado", "value": 250000.00}]` | **`200 OK`**. Solo el `MontoEstimado` debe cambiar a $250,000.00 (demuestra el uso del DTO). |
| **7** | **Eliminación Segura** | **`DELETE`** | `/Avances/1` | *None* | **`204 No Content`**. (Elimina el registro de avance antes de borrar el presupuesto). |
| **8** | **Eliminar Presupuesto** | **`DELETE`** | `/Estimaciones/1` | *None* | **`204 No Content`**. (Elimina el registro final). |
