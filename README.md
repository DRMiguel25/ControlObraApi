# 🏗️ ControlObraApi v2.0

> Sistema integral de gestión y control de proyectos de construcción con sistema multi-usuario - API RESTful

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?logo=microsoft-sql-server&logoColor=white)](https://www.microsoft.com/sql-server)
[![JWT](https://img.shields.io/badge/JWT-Authentication-000000?logo=json-web-tokens)](https://jwt.io/)

---

## 📋 Tabla de Contenidos

- [Descripción](#-descripción)
- [**🆕 Nuevas Características v2.0**](#-nuevas-características-v20)
- [Características Principales](#-características-principales)
- [Tecnologías Utilizadas](#-tecnologías-utilizadas)
- [Arquitectura y Patrones de Diseño](#-arquitectura-y-patrones-de-diseño)
- [**📊 Diagrama de Flujo General**](#-diagrama-de-flujo-general)
- [Requisitos Previos](#-requisitos-previos)
- [Instalación](#-instalación)
- [Configuración](#-configuración)
- [Ejecución](#-ejecución)
- [**🔑 Credenciales de Prueba**](#-credenciales-de-prueba)
- [Documentación de Endpoints](#-documentación-de-endpoints)
- [Ejemplos de Uso en Postman](#-ejemplos-de-uso-en-postman)

---

## 🎯 Descripción

**ControlObraApi** es una API RESTful desarrollada en ASP.NET Core que permite a empresas constructoras, contratistas y gestores de proyectos administrar de manera eficiente todas las fases de una obra de construcción, desde la planificación inicial hasta el seguimiento de avances y el análisis financiero.

### Problema que Soluciona

- ✅ **Centralización de datos**: Unifica información de proyectos, presupuestos y avances
- ✅ **Seguridad multi-usuario**: Cada usuario gestiona sus propios proyectos de forma aislada
- ✅ **Visibilidad financiera**: Análisis automático de desviaciones presupuestales
- ✅ **Control de avances**: Seguimiento detallado del progreso físico y financiero
- ✅ **Consumo de APIs externas**: Integración con servicios externos
- ✅ **Autenticación JWT**: Protección de datos sensibles
- ✅ **Validación de datos**: Integridad garantizada mediante FluentValidation

---

## 🆕 Nuevas Características v2.0

### 🔐 Sistema Multi-Usuario con Ownership

- **Aislamiento de datos**: Cada usuario solo puede ver y gestionar **sus propios** proyectos
- **Control de acceso**: Validaciones automáticas en todos los endpoints (403 Forbidden)
- **Claims JWT extendidos**: Tokens incluyen `UserId` para filtrado
- **Seguridad robusta**: Protección contra acceso no autorizado a recursos ajenos

### 🌐 Consumo de API Externa

- **Nuevo Endpoint**: `GET /api/HttpFactory` - Consume JSONPlaceholder API
- **Patrón HttpClientFactory**: Gestión óptima de conexiones HTTP
- **Prevención de agotamiento de sockets**: Best practices de .NET
- **Manejo robusto de errores**: Códigos HTTP apropiados y logging

### 👤 Usuario Demo Pre-Configurado

Para facilitar las pruebas del profesor, el sistema incluye:
- **Email**: `demo@test.com`
- **Password**: `Pass123!`
- **Proyectos pre-cargados**: 2 proyectos de ejemplo
- **Estimaciones y avances**: Datos de prueba completos

---

## ✨ Características Principales

1. **🔐 Autenticación y Autorización**
   - Registro de usuarios con encriptación BCrypt
   - Login con generación de tokens JWT
   - Tokens válidos por 24 horas
   - **🆕 Claims con UserId para ownership**

2. **👥 Sistema Multi-Usuario**
   - **🆕 Cada usuario solo ve sus propios proyectos**
   - **🆕 Validación automática de ownership en operaciones CRUD**
   - **🆕 Protección 403 Forbidden en accesos no autorizados**

3. **📊 Gestión de Proyectos**
   - CRUD completo de proyectos de construcción
   - Registro de información clave (nombre, ubicación, fecha)
   - Consulta con estimaciones y avances asociados
   - **🆕 Asignación automática de userId**

4. **💰 Administración de Presupuestos**
   - Creación de estimaciones de costos por concepto
   - Actualización parcial (PATCH) o completa (PUT)
   - Validación automática de montos y conceptos
   - **🆕 Validación de ownership del proyecto padre**

5. **📈 Seguimiento de Avances**
   - Registro de avances físicos (% completado)
   - Registro de montos ejecutados
   - Consulta de avances por estimación
   - **🆕 Validación de ownership indirecto vía estimación**

6. **🎯 Análisis de Desviación Financiera**
   - Cálculo automático de desviaciones presupuestales
   - Proyección de costo final basado en avance físico
   - Clasificación de riesgo: BAJO, MEDIO, ALTO

7. **🌐 Integración con APIs Externas**
   - **🆕 Endpoint HttpFactory para consumir JSONPlaceholder**
   - **🆕 Patrón HttpClientFactory**
   - **🆕 Logging y manejo de errores**

---

## 🛠️ Tecnologías Utilizadas

| Tecnología | Versión | Propósito |
|-----------|---------|-----------|
| ASP.NET Core | 8.0 | Framework principal |
| Entity Framework Core | 8.0 | ORM para base de datos |
| SQL Server | 2019+ | Base de datos relacional |
| JWT | - | Autenticación y autorización |
| BCrypt.Net | 4.0.3 | Encriptación de contraseñas |
| FluentValidation | 11.3.1 | Validación de modelos |
| **🆕 HttpClientFactory** | - | **Consumo optimizado de APIs externas** |
| Swagger/OpenAPI | - | Documentación interactiva |

---

## 🏛️ Arquitectura y Patrones de Diseño

Este proyecto ha sido construido siguiendo las mejores prácticas de ingeniería de software para garantizar mantenibilidad, escalabilidad y seguridad:

### 1. **Patrón MVC (Model-View-Controller)**
Separación clara de responsabilidades:
- **Modelos**: Entidades de dominio (`Proyecto`, `User`, etc.)
- **Controladores**: Lógica de negocio y orquestación de peticiones HTTP
- **Vistas**: (Frontend Angular desacoplado)

### 2. **Inyección de Dependencias (DI)**
Uso extensivo del contenedor de DI de .NET Core para desacoplar componentes:
- `AppDbContext` inyectado en controladores
- `IHttpClientFactory` para clientes HTTP
- `IValidator<T>` para validaciones fluidas

### 3. **Data Transfer Objects (DTOs)**
Implementación de DTOs (`ProyectoCreateDTO`, `RegisterDto`) para:
- Ocultar la estructura interna de la base de datos
- Prevenir ataques de *Over-posting*
- Desacoplar la capa de presentación de la capa de datos

### 4. **Repository Pattern (vía EF Core)**
Uso de Entity Framework Core como abstracción de la capa de datos, permitiendo consultas LINQ tipadas y protección contra SQL Injection.

### 5. **HttpClientFactory Pattern**
Gestión eficiente de conexiones HTTP para el consumo de APIs externas, evitando el agotamiento de sockets y permitiendo políticas de reintento (resiliencia).

---

## � Diagrama de Flujo General

El siguiente diagrama muestra el flujo completo del sistema ControlObraApi, desde la autenticación hasta la gestión de recursos:

![Diagrama de Flujo General](https://drive.google.com/file/d/1dTLiU6o3rTlWDaDcif5tZ4cwjJ642p5Q/view?usp=drive_link)
https://drive.google.com/file/d/1dTLiU6o3rTlWDaDcif5tZ4cwjJ642p5Q/view?usp=drive_link
**Componentes principales del diagrama:**

1. **Autenticación**: Flujo de registro y login con validación JWT
2. **Gestión de Proyectos**: CRUD completo con ownership validation
3. **Estimaciones de Costo**: Presupuestos vinculados a proyectos
4. **Avances de Obra**: Seguimiento del progreso físico y financiero
5. **API Externa**: Consumo de servicios externos con HttpClientFactory
6. **Análisis Financiero**: Cálculo automático de desviaciones presupuestales

> **Nota**: El diagrama está disponible en formato DrawIO en la carpeta `Design/` para facilitar su edición y actualización.

---

## �📦 Requisitos Previos

Antes de comenzar, asegúrate de tener instalado:

- [.NET SDK 8.0+](https://dotnet.microsoft.com/download)
- [SQL Server 2019+](https://www.microsoft.com/sql-server) o Docker con SQL Server
- [Postman](https://www.postman.com/downloads/) (para pruebas de API)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) o [VS Code](https://code.visualstudio.com/) (opcional)

---

## 🚀 Instalación

### 1️⃣ Clonar el Repositorio

```bash
git clone https://github.com/DRMiguel25/ControlObraApi.git
cd ControlObraApi/ControlObraApi
```

### 2️⃣ Restaurar Paquetes NuGet

```bash
dotnet restore
```

### 3️⃣ Iniciar SQL Server (si usas Docker)

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Admin12345" \
   -p 1433:1433 --name sqlserver \
   -d mcr.microsoft.com/mssql/server:2022-latest
```

---

## ⚙️ Configuración

### 🔧 Variables de Entorno

El proyecto utiliza el archivo `appsettings.json` para la configuración. A continuación se detallan todas las variables de entorno necesarias:

#### 1️⃣ Cadena de Conexión a Base de Datos

```json
"ConnectionStrings": {
  "ConexionSQL": "Server=localhost,1433;Database=ControlObraDB;User Id=sa;Password=Admin12345;TrustServerCertificate=True"
}
```

| Parámetro | Descripción | Valor por Defecto | Requerido |
|-----------|-------------|-------------------|-----------|
| `Server` | Dirección y puerto del servidor SQL Server | `localhost,1433` | ✅ Sí |
| `Database` | Nombre de la base de datos | `ControlObraDB` | ✅ Sí |
| `User Id` | Usuario de SQL Server | `sa` | ✅ Sí |
| `Password` | Contraseña del usuario | `Admin12345` | ✅ Sí |
| `TrustServerCertificate` | Confiar en certificado autofirmado | `True` | ✅ Sí (desarrollo) |

> [!WARNING]
> **Seguridad en Producción**: En entornos de producción, utiliza variables de entorno del sistema en lugar de valores hardcodeados. Considera usar Azure Key Vault o AWS Secrets Manager.

---

#### 2️⃣ Configuración JWT

```json
"AppSettings": {
  "Token": "my super secret key for jwt token generation that is long enough"
}
```

| Variable | Descripción | Valor Mínimo | Requerido |
|----------|-------------|--------------|-----------|
| `Token` | Clave secreta para firmar tokens JWT | 32 caracteres | ✅ Sí |

> [!CAUTION]
> **Clave Secreta JWT**: Esta clave debe ser:
> - Mínimo 32 caracteres de longitud
> - Única por entorno (desarrollo, staging, producción)
> - Almacenada de forma segura (nunca en repositorios públicos)
> - Rotada periódicamente en producción

**Ejemplo de generación de clave segura:**
```bash
# Linux/Mac
openssl rand -base64 64

# PowerShell
[Convert]::ToBase64String((1..64 | ForEach-Object { Get-Random -Minimum 0 -Maximum 256 }))
```

---

#### 3️⃣ APIs Externas

```json
"ExternalApis": {
  "Base_url": "https://jsonplaceholder.typicode.com"
}
```

| Variable | Descripción | Valor por Defecto | Requerido |
|----------|-------------|-------------------|-----------|
| `Base_url` | URL base de la API externa JSONPlaceholder | `https://jsonplaceholder.typicode.com` | ✅ Sí |

> [!NOTE]
> Esta configuración se utiliza en el endpoint `/api/HttpFactory` para demostrar el consumo de APIs externas mediante el patrón HttpClientFactory.

---

#### 4️⃣ Logging (Opcional)

```json
"Logging": {
  "LogLevel": {
    "Default": "Information",
    "Microsoft.AspNetCore": "Warning"
  }
}
```

| Nivel | Descripción | Uso Recomendado |
|-------|-------------|-----------------|
| `Trace` | Información muy detallada | Debugging profundo |
| `Debug` | Información de depuración | Desarrollo |
| `Information` | Mensajes informativos generales | Desarrollo/Producción |
| `Warning` | Advertencias no críticas | Producción |
| `Error` | Errores que no detienen la app | Producción |
| `Critical` | Errores críticos | Producción |

---

#### 5️⃣ CORS (Configurado en `Program.cs`)

```csharp
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAngularApp", policy => {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
```

> [!TIP]
> Para producción, reemplaza `http://localhost:4200` con la URL de tu frontend desplegado.

---

### 📋 Archivo `appsettings.json` Completo

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AppSettings": {
    "Token": "my super secret key for jwt token generation that is long enough"
  },
  "AllowedHosts": "*",
  "ExternalApis": {
    "Base_url": "https://jsonplaceholder.typicode.com"
  },
  "ConnectionStrings": {
    "ConexionSQL": "Server=localhost,1433;Database=ControlObraDB;User Id=sa;Password=Admin12345;TrustServerCertificate=True"
  }
}
```

---

### 🗄️ Crear la Base de Datos

Una vez configuradas las variables de entorno, ejecuta las migraciones:

```bash
dotnet ef database update
```

**Este comando creará:**
- ✅ Base de datos `ControlObraDB`
- ✅ Tablas: `Users`, `Proyectos`, `EstimacionesCosto`, `AvancesObra`
- ✅ **Usuario demo** (`demo@test.com` / `Pass123!`) con 2 proyectos de ejemplo

---

### 🔐 Configuración de Seguridad Adicional

#### Producción - Variables de Entorno del Sistema

En lugar de usar `appsettings.json` en producción, configura variables de entorno:

**Linux/Mac:**
```bash
export ConnectionStrings__ConexionSQL="Server=prod-server;Database=ControlObraDB;..."
export AppSettings__Token="your-super-secure-production-token-here"
```

**Windows (PowerShell):**
```powershell
$env:ConnectionStrings__ConexionSQL="Server=prod-server;Database=ControlObraDB;..."
$env:AppSettings__Token="your-super-secure-production-token-here"
```

**Docker:**
```yaml
environment:
  - ConnectionStrings__ConexionSQL=Server=db;Database=ControlObraDB;...
  - AppSettings__Token=your-token-here
```

---

## ▶️ Ejecución

```bash
dotnet run
```

La API estará disponible en:
- **HTTP**: `http://localhost:5000`
- **HTTPS**: `https://localhost:7000`
- **Swagger**: `http://localhost:5000/swagger`

---

## 🔑 Credenciales de Prueba

### Usuario Demo (Pre-configurado)

Para facilitar las pruebas, existe un usuario demo con datos de ejemplo:

```
📧 Email: demo@test.com
🔑 Password: Pass123!
```

**Este usuario tiene:**
- ✅ 2 proyectos pre-cargados:
  - Torre Residencial Alpha (Zona Central)
  - Edificio Comercial Beta (Zona Norte)
- ✅ 3 estimaciones de costo
- ✅ 3 avances de obra registrados

**Uso en Postman:**

```json
POST http://localhost:5000/api/Auth/login

{
  "email": "demo@test.com",
  "password": "Pass123!"
}
```

---

## 📚 Documentación de Endpoints

> [!IMPORTANT]
> **🔐 AUTENTICACIÓN JWT REQUERIDA**
> 
> Todos los endpoints excepto `/api/Auth/register` y `/api/Auth/login` requieren un token JWT válido en el header de autorización.
> 
> ```http
> Authorization: Bearer {tu_token_jwt}
> ```

> [!WARNING]
> **🆕 SISTEMA MULTI-USUARIO**
> 
> Cada usuario solo puede ver y gestionar **sus propios recursos**:
> - `GET /api/Proyectos` → Solo tus proyectos
> - `GET /api/Proyectos/1` → 404 si no es tuyo
> - `POST /api/Estimaciones` → Solo en tus proyectos
> - `DELETE /api/Proyectos/5` → 403 si no es tuyo

---

### 📑 Índice de Endpoints

| Categoría | Cantidad | Endpoints |
|-----------|----------|-----------|
| [🔐 Autenticación](#-autenticación) | 2 | Register, Login |
| [🌐 API Externa](#-api-externa-) | 1 | HttpFactory |
| [🏗️ Proyectos](#️-proyectos) | 7 | GET all, GET by ID, POST, PUT, PATCH, DELETE, Desviación |
| [💰 Estimaciones](#-estimaciones-de-costo) | 5 | POST, GET, PUT, PATCH, DELETE |
| [📈 Avances](#-avances-de-obra) | 8 | POST, GET all, GET by ID, PUT, PATCH, DELETE, GET by Estimación |
| **TOTAL** | **23** | |

---

### 🔐 **Autenticación**

#### `POST /api/Auth/register` - Registrar Usuario

Crea un nuevo usuario en el sistema.

**🔓 Autenticación:** No requerida

**Request Body:**
```json
{
  "name": "Miguel Rodríguez",
  "email": "miguel@constructora.com",
  "password": "MiPassword123!"
}
```

**Validaciones:**
- ✅ Email único (no puede existir previamente)
- ✅ Password mínimo 6 caracteres
- ✅ Email formato válido

**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 2,
    "name": "Miguel Rodríguez",
    "email": "miguel@constructora.com",
    "username": "miguel",
    "role": "User"
  }
}
```

**Response (400 Bad Request):**
```json
"User already exists."
```

---

#### `POST /api/Auth/login` - Iniciar Sesión

Autentica un usuario y genera un token JWT válido por 24 horas.

**🔓 Autenticación:** No requerida

**Request Body:**
```json
{
  "email": "demo@test.com",
  "password": "Pass123!"
}
```

**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwidW5pcXVlX25hbWUiOiJkZW1vIiwicm9sZSI6IlVzZXIiLCJuYmYiOjE3MzI1ODQwMDAsImV4cCI6MTczMjY3MDQwMCwiaWF0IjoxNzMyNTg0MDAwfQ..."
}
```

**Response (400 Bad Request):**
```json
"User not found."
// o
"Wrong password."
```

---

### 🌐 **API Externa** 🆕

#### `GET /api/HttpFactory` - Consumir JSONPlaceholder

Obtiene usuarios desde la API pública JSONPlaceholder (demuestra consumo de APIs externas con HttpClientFactory).

**🔐 Autenticación:** JWT Token requerido

**Headers:**
```http
Authorization: Bearer {tu_token}
```

**Response (200 OK):**
```json
{
  "source": "JSONPlaceholder API",
  "endpoint": "/users",
  "count": 10,
  "data": [
    {
      "id": 1,
      "name": "Leanne Graham",
      "username": "Bret",
      "email": "Sincere@april.biz",
      "phone": "1-770-736-8031 x56442",
      "website": "hildegard.org"
    }
    // ... 9 usuarios más
  ]
}
```

**Response (503 Service Unavailable):**
```json
{
  "error": "Servicio externo no disponible",
  "message": "No connection could be made..."
}
```

**Características:**
- ✅ Usa patrón HttpClientFactory
- ✅ Manejo robusto de errores (503 si falla)
- ✅ Logging de peticiones
- ✅ Timeout configurado (30s)

---

### 🏗️ **Proyectos**

#### `GET /api/Proyectos` - Listar Proyectos del Usuario Autenticado 🆕

Obtiene todos los proyectos que pertenecen al usuario autenticado.

**🔐 Autenticación:** JWT Token requerido

**Response (200 OK):**
```json
[
  {
    "proyectoID": 1,
    "nombreObra": "Torre Residencial Alpha",
    "ubicacion": "Zona Central",
    "fechaInicio": "2025-01-15T00:00:00",
    "userId": 1,
    "estimaciones": [
      {
        "costoID": 1,
        "concepto": "Cimentación",
        "montoEstimado": 500000.00,
        "proyectoID": 1
      }
    ]
  }
]
```

> **🆕 Cambio v2.0**: Solo retorna proyectos donde `userId` coincida con el usuario autenticado.

---

#### `GET /api/Proyectos/{id}` - Obtener Proyecto por ID

Obtiene un proyecto específico con todas sus estimaciones y avances.

**🔐 Autenticación:** JWT Token requerido  
**🆕 Validación:** Solo si el proyecto pertenece al usuario

**URL Parameters:**
- `id` (integer, requerido): ID del proyecto

**Response (200 OK):**
```json
{
  "proyectoID": 1,
  "nombreObra": "Torre Residencial Alpha",
  "ubicacion": "Zona Central",
  "fechaInicio": "2025-01-15T00:00:00",
  "userId": 1,
  "estimaciones": [...]
}
```

**Response (404 Not Found):**
```json
"Proyecto con ID 5 no encontrado."
```

> Si el proyecto existe pero no es tuyo, también retorna 404 (por seguridad).

---

#### `POST /api/Proyectos` - Crear Proyecto

Crea un nuevo proyecto asignado automáticamente al usuario autenticado.

**🔐 Autenticación:** JWT Token requerido

**Request Body:**
```json
{
  "nombreObra": "Centro Comercial Norte",
  "ubicacion": "Boulevard Norte 5678",
  "fechaInicio": "2025-02-01"
}
```

**Validaciones:**
- ✅ `nombreObra`: Requerido, máximo 200 caracteres
- ✅ `ubicacion`: Requerido, máximo 300 caracteres
- ✅ `fechaInicio`: Formato ISO 8601 (YYYY-MM-DD)

**Response (201 Created):**
```json
{
  "proyectoID": 3,
  "nombreObra": "Centro Comercial Norte",
  "ubicacion": "Boulevard Norte 5678",
  "fechaInicio": "2025-02-01T00:00:00",
  "userId": 2,  // 🆕 Asignado automáticamente del JWT
  "estimaciones": []
}
```

> **🆕 Cambio v2.0**: El campo `userId` se asigna automáticamente del token JWT. No es necesario enviarlo en el body.

---

#### `PUT /api/Proyectos/{id}` - Actualizar Proyecto Completo

Actualiza todos los campos de un proyecto existente.

**🔐 Autenticación:** JWT Token requerido  
**🆕 Validación:** Solo puedes actualizar tus propios proyectos

**URL Parameters:**
- `id` (integer, requerido): ID del proyecto

**Request Body:**
```json
{
  "proyectoID": 3,
  "nombreObra": "Centro Comercial Norte - ACTUALIZADO",
  "ubicacion": "Boulevard Norte 5678, Local 10",
  "fechaInicio": "2025-02-15"
}
```

**Response (204 No Content):** Actualización exitosa

**Response (403 Forbidden):**
```json
"Forbidden"
```
> Intentaste actualizar un proyecto que no te pertenece.

**Response (400 Bad Request):**
```json
"El ID de la ruta no coincide con el ID del cuerpo."
```

---

#### `PATCH /api/Proyectos/{id}` - Actualizar Proyecto Parcial

Actualiza solo los campos especificados del proyecto.

**🔐 Autenticación:** JWT Token requerido  
**🆕 Validación:** Solo puedes actualizar tus propios proyectos

**URL Parameters:**
- `id` (integer, requerido): ID del proyecto

**Request Body (todos los campos opcionales):**
```json
{
  "nombreObra": "Nuevo Nombre",
  "ubicacion": "Nueva Ubicación",
  "fechaInicio": "2025-03-01"
}
```

**Ejemplo - Solo actualizar ubicación:**
```json
{
  "ubicacion": "Av. Principal 123, Piso 5"
}
```

**Response (200 OK):**
```json
{
  "proyectoID": 3,
  "nombreObra": "Centro Comercial Norte",
  "ubicacion": "Av. Principal 123, Piso 5",  // ✅ Actualizado
  "fechaInicio": "2025-02-01T00:00:00",
  "userId": 2
}
```

**Response (403 Forbidden):** No eres dueño del proyecto

---

#### `DELETE /api/Proyectos/{id}` - Eliminar Proyecto

Elimina un proyecto y todas sus estimaciones y avances asociados (cascada).

**🔐 Autenticación:** JWT Token requerido  
**🆕 Validación:** Solo puedes eliminar tus propios proyectos

**URL Parameters:**
- `id` (integer, requerido): ID del proyecto

**Response (204 No Content):** Eliminado exitosamente

**Response (403 Forbidden):**
```json
"Forbidden"
```
> Intentaste eliminar un proyecto que no te pertenece.

**Response (404 Not Found):**
```json
"Proyecto con ID 99 no encontrado."
```

> [!WARNING]
> **Eliminación en Cascada**: Al eliminar un proyecto, se eliminan automáticamente todas sus estimaciones y avances asociados.

---

#### `GET /api/Proyectos/Desviacion/{proyectoId}` - Análisis de Desviación Financiera 🎯

Calcula la desviación presupuestal y proyecta el costo final basado en el avance físico.

**🔐 Autenticación:** JWT Token requerido  
**🆕 Validación:** Solo puedes analizar tus propios proyectos

**URL Parameters:**
- `proyectoId` (integer, requerido): ID del proyecto

**Response (200 OK):**
```json
{
  "riesgoDesviacion": "MEDIO",
  "desviacionPorcentaje": 3.45,
  "costoEstimado": 1500000.00,
  "costoProyectadoFinal": 1551750.00,
  "mensaje": "El proyecto tiene un avance físico promedio del 45.50%."
}
```

**Clasificación de Riesgo:**
- 🟢 **BAJO**: Desviación ≤ 0%
- 🟡 **MEDIO**: Desviación > 0% y ≤ 5%
- 🔴 **ALTO**: Desviación > 5%

**Response (200 OK) - Sin Datos:**
```json
{
  "riesgo": "SIN DATA",
  "mensaje": "No hay estimaciones registradas para este proyecto.",
  "costoEstimado": 0
}
```

**Fórmula de Cálculo:**
```
Costo Proyectado Final = Costo Ejecutado Total / (Avance Físico % / 100)
Desviación % = ((Costo Proyectado - Costo Estimado) / Costo Estimado) × 100
```

---

### 💰 **Estimaciones de Costo**

#### `POST /api/Estimaciones` - Crear Estimación

Crea una nueva estimación de costo para un proyecto.

**🔐 Autenticación:** JWT Token requerido  
**🆕 Validación:** Solo puedes crear estimaciones en **tus proyectos**

**Request Body:**
```json
{
  "concepto": "Instalaciones Eléctricas",
  "montoEstimado": 350000.00,
  "proyectoID": 1
}
```

**Validaciones (FluentValidation):**
- ✅ `concepto`: Requerido, máximo 200 caracteres
- ✅ `montoEstimado`: Mayor a 0
- ✅ `proyectoID`: Debe existir y pertenecer al usuario

**Response (201 Created):**
```json
{
  "costoID": 5,
  "concepto": "Instalaciones Eléctricas",
  "montoEstimado": 350000.00,
  "proyectoID": 1,
  "avances": []
}
```

**Response (403 Forbidden):**
```json
"Forbidden"
```
> Intentaste crear una estimación en un proyecto que no es tuyo.

**Response (400 Bad Request):**
```json
[
  "'Concepto' must not be empty.",
  "El monto estimado debe ser mayor a cero."
]
```

---

#### `GET /api/Estimaciones/{id}` - Obtener Estimación por ID

Obtiene una estimación específica con todos sus avances.

**🔐 Autenticación:** JWT Token requerido  
**🆕 Validación:** Solo si la estimación pertenece a tu proyecto

**URL Parameters:**
- `id` (integer, requerido): ID de la estimación

**Response (200 OK):**
```json
{
  "costoID": 1,
  "concepto": "Cimentación",
  "montoEstimado": 500000.00,
  "proyectoID": 1,
  "proyecto": {
    "proyectoID": 1,
    "nombreObra": "Torre Residencial Alpha",
    "userId": 1
  },
  "avances": [
    {
      "avanceID": 1,
      "montoEjecutado": 150000.00,
      "porcentajeCompletado": 30.0,
      "fechaRegistro": "2025-01-20T00:00:00",
      "costoID": 1
    }
  ]
}
```

**Response (404 Not Found):** Estimación no encontrada o no es tuya  
**Response (403 Forbidden):** No tienes acceso a esta estimación

---

#### `PUT /api/Estimaciones/{id}` - Actualizar Estimación Completa

Actualiza todos los campos de una estimación.

**🔐 Autenticación:** JWT Token requerido  
**🆕 Validación:** Solo puedes actualizar estimaciones de tus proyectos

**URL Parameters:**
- `id` (integer, requerido): ID de la estimación

**Request Body:**
```json
{
  "costoID": 1,
  "concepto": "Cimentación - Revisado",
  "montoEstimado": 550000.00,
  "proyectoID": 1
}
```

**Response (204 No Content):** Actualización exitosa

**Response (403 Forbidden):** No eres dueño del proyecto asociado

**Response (409 Conflict):**
```json
"Conflicto de Concurrencia: El registro fue modificado por otro usuario."
```

---

#### `PATCH /api/Estimaciones/{id}` - Actualizar Estimación Parcial

Actualiza solo los campos especificados de la estimación.

**🔐 Autenticación:** JWT Token requerido  
**🆕 Validación:** Solo puedes actualizar estimaciones de tus proyectos

**URL Parameters:**
- `id` (integer, requerido): ID de la estimación

**Request Body (todos los campos opcionales):**
```json
{
  "concepto": "Cimentación - Fase 2",
  "montoEstimado": 600000.00
}
```

**Ejemplo - Solo actualizar monto:**
```json
{
  "montoEstimado": 575000.00
}
```

**Response (200 OK):**
```json
{
  "costoID": 1,
  "concepto": "Cimentación",
  "montoEstimado": 575000.00,  // ✅ Actualizado
  "proyectoID": 1
}
```

**Response (400 Bad Request):**
```json
"El monto estimado no puede ser menor o igual a cero."
```

---

#### `DELETE /api/Estimaciones/{id}` - Eliminar Estimación

Elimina una estimación si no tiene avances registrados.

**🔐 Autenticación:** JWT Token requerido  
**🆕 Validación:** Solo puedes eliminar estimaciones de tus proyectos

**URL Parameters:**
- `id` (integer, requerido): ID de la estimación

**Response (204 No Content):** Eliminada exitosamente

**Response (400 Bad Request):**
```json
{
  "error": "No se puede eliminar la estimación",
  "razon": "La estimación tiene avances de obra registrados. Elimínelos primero.",
  "avancesCount": 3
}
```

**Response (403 Forbidden):** No eres dueño del proyecto asociado

> [!WARNING]
> **Restricción de Integridad**: No puedes eliminar una estimación que tenga avances registrados. Primero debes eliminar los avances.

---

### 📈 **Avances de Obra**

#### `POST /api/Avances` - Registrar Avance

Registra un nuevo avance de obra para una estimación.

**🔐 Autenticación:** JWT Token requerido  
**🆕 Validación:** Solo puedes registrar avances en **tus estimaciones**

**Request Body:**
```json
{
  "montoEjecutado": 120000.00,
  "porcentajeCompletado": 35.5,
  "costoID": 1
}
```

**Validaciones (FluentValidation):**
- ✅ `montoEjecutado`: Mayor a 0
- ✅ `porcentajeCompletado`: Entre 0 y 100
- ✅ `costoID`: Debe existir y pertenecer a tu proyecto
- ✅ `fechaRegistro`: Se asigna automáticamente (DateTime.Now)

**Response (201 Created):**
```json
{
  "avanceID": 4,
  "montoEjecutado": 120000.00,
  "porcentajeCompletado": 35.5,
  "fechaRegistro": "2025-11-25T18:30:00",
  "costoID": 1
}
```

**Response (403 Forbidden):**
```json
"Forbidden"
```
> Intentaste registrar un avance en una estimación que no pertenece a tu proyecto.

**Response (400 Bad Request):**
```json
[
  "El monto ejecutado debe ser mayor a cero.",
  "El porcentaje completado debe estar entre 0 y 100."
]
```

---

#### `GET /api/Avances` - Listar Todos los Avances

Obtiene todos los avances de obra con sus estimaciones asociadas.

**🔐 Autenticación:** JWT Token requerido

**Response (200 OK):**
```json
[
  {
    "avanceID": 1,
    "montoEjecutado": 150000.00,
    "porcentajeCompletado": 30.0,
    "fechaRegistro": "2025-01-20T00:00:00",
    "costoID": 1,
    "estimacionCosto": {
      "costoID": 1,
      "concepto": "Cimentación",
      "montoEstimado": 500000.00
    }
  }
]
```

---

#### `GET /api/Avances/{id}` - Obtener Avance por ID

Obtiene un avance específico con su estimación y proyecto asociado.

**🔐 Autenticación:** JWT Token requerido  
**🆕 Validación:** Solo si el avance pertenece a tu proyecto (vía estimación)

**URL Parameters:**
- `id` (integer, requerido): ID del avance

**Response (200 OK):**
```json
{
  "avanceID": 1,
  "montoEjecutado": 150000.00,
  "porcentajeCompletado": 30.0,
  "fechaRegistro": "2025-01-20T00:00:00",
  "costoID": 1,
  "estimacionCosto": {
    "costoID": 1,
    "concepto": "Cimentación",
    "montoEstimado": 500000.00,
    "proyecto": {
      "proyectoID": 1,
      "nombreObra": "Torre Residencial Alpha",
      "userId": 1
    }
  }
}
```

**Response (404 Not Found):** Avance no encontrado  
**Response (403 Forbidden):** No tienes acceso a este avance

---

#### `PUT /api/Avances/{id}` - Actualizar Avance Completo

Actualiza todos los campos de un avance.

**🔐 Autenticación:** JWT Token requerido  
**🆕 Validación:** Solo puedes actualizar avances de tus estimaciones

**URL Parameters:**
- `id` (integer, requerido): ID del avance

**Request Body:**
```json
{
  "avanceID": 1,
  "montoEjecutado": 175000.00,
  "porcentajeCompletado": 35.0
}
```

**Response (204 No Content):** Actualización exitosa

**Response (403 Forbidden):** No eres dueño del proyecto asociado

**Response (400 Bad Request):**
```json
"El ID de la ruta no coincide con el ID del cuerpo."
```

---

#### `PATCH /api/Avances/{id}` - Actualizar Avance Parcial

Actualiza solo los campos especificados del avance.

**🔐 Autenticación:** JWT Token requerido

**URL Parameters:**
- `id` (integer, requerido): ID del avance

**Request Body (todos los campos opcionales):**
```json
{
  "montoEjecutado": 180000.00,
  "porcentajeCompletado": 36.0
}
```

**Ejemplo - Solo actualizar porcentaje:**
```json
{
  "porcentajeCompletado": 40.0
}
```

**Response (200 OK):**
```json
{
  "avanceID": 1,
  "montoEjecutado": 150000.00,
  "porcentajeCompletado": 40.0,  // ✅ Actualizado
  "fechaRegistro": "2025-01-20T00:00:00",
  "costoID": 1
}
```

**Response (400 Bad Request):**
```json
"El porcentaje debe estar entre 0 y 100."
```

---

#### `DELETE /api/Avances/{id}` - Eliminar Avance

Elimina un avance de obra.

**🔐 Autenticación:** JWT Token requerido

**URL Parameters:**
- `id` (integer, requerido): ID del avance

**Response (204 No Content):** Eliminado exitosamente

**Response (404 Not Found):**
```json
"Avance con ID 99 no encontrado."
```

---

#### `GET /api/Avances/porEstimacion/{costoId}` - Obtener Avances por Estimación

Obtiene todos los avances asociados a una estimación específica.

**🔐 Autenticación:** JWT Token requerido

**URL Parameters:**
- `costoId` (integer, requerido): ID de la estimación

**Response (200 OK):**
```json
[
  {
    "avanceID": 1,
    "montoEjecutado": 150000.00,
    "porcentajeCompletado": 30.0,
    "fechaRegistro": "2025-01-20T00:00:00",
    "costoID": 1,
    "estimacionCosto": {
      "costoID": 1,
      "concepto": "Cimentación",
      "montoEstimado": 500000.00
    }
  },
  {
    "avanceID": 2,
    "montoEjecutado": 100000.00,
    "porcentajeCompletado": 20.0,
    "fechaRegistro": "2025-01-25T00:00:00",
    "costoID": 1,
    "estimacionCosto": {
      "costoID": 1,
      "concepto": "Cimentación",
      "montoEstimado": 500000.00
    }
  }
]
```

**Response (404 Not Found):**
```json
"No se encontraron avances para la estimación 5."
```

---

### 📊 Resumen de Endpoints

#### Por Método HTTP

| Método | Cantidad | Uso |
|--------|----------|-----|
| GET | 10 | Consultar recursos |
| POST | 5 | Crear recursos |
| PUT | 4 | Actualización completa |
| PATCH | 3 | Actualización parcial |
| DELETE | 3 | Eliminar recursos |

#### Por Autenticación

| Tipo | Cantidad | Endpoints |
|------|----------|-----------|
| 🔓 Públicos | 2 | `/api/Auth/register`, `/api/Auth/login` |
| 🔐 Protegidos | 21 | Todos los demás |

#### Códigos de Respuesta HTTP

| Código | Significado | Uso |
|--------|-------------|-----|
| 200 OK | Éxito | GET, PATCH |
| 201 Created | Recurso creado | POST |
| 204 No Content | Éxito sin contenido | PUT, DELETE |
| 400 Bad Request | Datos inválidos | Validaciones |
| 401 Unauthorized | Token inválido/ausente | Sin JWT |
| 403 Forbidden | Sin permisos (ownership) | Recursos ajenos |
| 404 Not Found | Recurso no existe | GET, PUT, DELETE |
| 409 Conflict | Conflicto de concurrencia | PUT |
| 503 Service Unavailable | Servicio externo caído | HttpFactory |

---

## 🧪 Ejemplos de Uso en Postman

### Flujo Completo de Pruebas

#### 1️⃣ Login con Usuario Demo

```
POST http://localhost:5000/api/Auth/login
Content-Type: application/json

{
  "email": "demo@test.com",
  "password": "Pass123!"
}
```

**Copiar el token** de la respuesta.

---

#### 2️⃣ Ver Proyectos del Demo

```
GET http://localhost:5000/api/Proyectos
Authorization: Bearer {TOKEN_DEMO}
```

**Respuesta:** 2 proyectos del usuario demo.

---

#### 3️⃣ Consumir API Externa

```
GET http://localhost:5000/api/HttpFactory
Authorization: Bearer {TOKEN_DEMO}
```

**Respuesta:** 10 usuarios de JSONPlaceholder.

---

#### 4️⃣ Registrar Nuevo Usuario

```
POST http://localhost:5000/api/Auth/register
Content-Type: application/json

{
  "name": "Miguel Test",
  "email": "miguel@test.com",
  "password": "Pass123!"
}
```

**Guardar el nuevo token** (`TOKEN_MIGUEL`).

---

#### 5️⃣ Validar Aislamiento de Datos

```
GET http://localhost:5000/api/Proyectos
Authorization: Bearer {TOKEN_MIGUEL}
```

**Respuesta:** `[]` (lista vacía, porque Miguel no tiene proyectos aún).

---

#### 6️⃣ Crear Proyecto como Miguel

```
POST http://localhost:5000/api/Proyectos
Authorization: Bearer {TOKEN_MIGUEL}
Content-Type: application/json

{
  "nombreObra": "Proyecto de Miguel",
  "ubicacion": "Ciudad X",
  "fechaInicio": "2025-03-01"
}
```

**Respuesta:** Proyecto con `userId: 2` (Miguel).

---

#### 7️⃣ Miguel Intenta Ver Proyecto del Demo (FALLA)

```
GET http://localhost:5000/api/Proyectos/1
Authorization: Bearer {TOKEN_MIGUEL}
```

**Respuesta:** `404 Not Found` (no puede ver proyectos ajenos).

---

## 📊 Estructura del Proyecto

```
ControlObraApi/
├── Controllers/
│   ├── AuthController.cs          # Autenticación JWT
│   ├── ProyectosController.cs     # CRUD + Ownership
│   ├── EstimacionesController.cs  # CRUD + Ownership
│   ├── AvancesController.cs       # CRUD + Ownership
│   └── HttpFactoryController.cs   # 🆕 API Externa
├── Models/
│   ├── User.cs                    # Usuario con email único
│   ├── Proyecto.cs                # 🆕 Con UserId
│   ├── EstimacionCosto.cs         # Presupuesto
│   ├── AvanceObra.cs              # Avances
│   └── AppDbContext.cs            # 🆕 Seed data + relaciones
├── DTOs/
│   ├── LoginDto.cs
│   ├── RegisterDto.cs
│   ├── EstimacionCostoCreateDTO.cs
│   └── ...
├── Validators/
│   ├── EstimacionCostoValidator.cs
│   └── AvanceObraValidator.cs
├── Migrations/                     # 🆕 InitialCreateWithOwnershipAndHttpFactory
├── Program.cs                      # 🆕 HttpClientFactory configurado
└── appsettings.json                # 🆕 ExternalApis configuradas
```

---

## 🔒 Seguridad

- ✅ **Contraseñas**: Hasheadas con BCrypt (factor 11)
- ✅ **Tokens JWT**: Firmados con HS512, válidos 24h
- ✅ **CORS**: Configurado para `http://localhost:4200` (Angular)
- ✅ **Validación**: FluentValidation + Data Annotations
- ✅ **SQL Injection**: Prevenido por EF Core parametrizado
- ✅ **🆕 Ownership**: Validación automática en todos los endpoints CRUD

---

## 🎯 Casos de Uso

### Caso 1: Constructora con Múltiples Jefes de Proyecto

Cada jefe de proyecto:
- Se registra con su email corporativo
- Gestiona solo **sus proyectos** asignados
- No puede ver ni modificar proyectos de otros jefes

### Caso 2: Cliente/Profesor Revisando el Sistema

Usa las credenciales demo:
- Email: `demo@test.com`
- Password: `Pass123!`
- Ve 2 proyectos de ejemplo con datos completos

---

## 📝 Changelog

### v2.0 (2025-11-24)
- 🆕 **Sistema multi-usuario con ownership**
- 🆕 **Consumo de API externa JSONPlaceholder**
- 🆕 **HttpClientFactory pattern**
- 🆕 **Usuario demo con seed data**
- 🆕 **Claims JWT extendidos (UserId)**
- 🆕 **Validaciones de ownership en todos los controladores**
- 🆕 **Migración unificada: InitialCreateWithOwnershipAndHttpFactory**

### v1.0 (2025-11-21)
- ✅ CRUD completo de Proyectos, Estimaciones, Avances
- ✅ Autenticación JWT básica
- ✅ Análisis de desviación financiera
- ✅ Validación con FluentValidation

---

## 👥 Autor

**Miguel Diaz**  
GitHub: [@DRMiguel25](https://github.com/DRMiguel25)

---

## 📄 Licencia

Este proyecto es de código abierto para fines educativos.

---

**⭐ Si este proyecto te fue útil, dale una estrella en GitHub!**
