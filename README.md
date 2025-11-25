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

## 📦 Requisitos Previos

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

### 1️⃣ Configurar SQL Server

El archivo `appsettings.json` ya está configurado:

```json
{
  "ConnectionStrings": {
    "ConexionSQL": "Server=localhost,1433;Database=ControlObraDB;User Id=sa;Password=Admin12345;TrustServerCertificate=True"
  },
  "AppSettings": {
    "Token": "my super secret key for jwt token generation that is long enough"
  },
  "ExternalApis": {
    "Base_url": "https://jsonplaceholder.typicode.com"
  }
}
```

> ⚠️ **Importante**: Ajusta las credenciales si tu SQL Server usa otras.

### 2️⃣ Crear la Base de Datos

```bash
dotnet ef database update
```

Este comando creará:
- Base de datos `ControlObraDB`
- Tablas: `Users`, `Proyectos`, `EstimacionesCosto`, `AvancesObra`
- **🆕 Usuario demo con 2 proyectos de ejemplo**

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

### 🔐 **Autenticación**

#### `POST /api/Auth/register` - Registrar Usuario

Crea un nuevo usuario en el sistema.

**Request Body:**
```json
{
  "name": "Miguel Rodríguez",
  "email": "miguel@constructora.com",
  "password": "MiPassword123!"
}
```

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

---

#### `POST /api/Auth/login` - Iniciar Sesión

Autentica un usuario y genera un token JWT.

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
  "token": "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9..."
}
```

---

### 🌐 **API Externa** 🆕

#### `GET /api/HttpFactory` - Consumir JSONPlaceholder

Obtiene usuarios desde la API pública JSONPlaceholder (demuestra consumo de APIs externas).

**Headers:**
```
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
  ]
}
```

**Características:**
- ✅ Usa patrón HttpClientFactory
- ✅ Manejo robusto de errores (503 si falla)
- ✅ Logging de peticiones
- ✅ Timeout configurado (30s)

---

### 🏗️ **Proyectos**

#### `GET /api/Proyectos` - Listar Proyectos **del Usuario Autenticado** 🆕

**🔐 Requiere:** JWT Token

**Response (200 OK):**
```json
[
  {
    "proyectoID": 1,
    "nombreObra": "Torre Residencial Alpha",
    "ubicacion": "Zona Central",
    "fechaInicio": "2025-01-15T00:00:00",
    "userId": 1,
    "estimaciones": [...]
  }
]
```

> **🆕 Cambio**: Solo retorna proyectos donde `userId` coincida con el usuario autenticado.

---

#### `GET /api/Proyectos/{id}` - Obtener Proyecto por ID

**🔐 Requiere:** JWT Token  
**🆕 Validación:** Solo si el proyecto pertenece al usuario

**Response (200 OK):** Proyecto completo  
**Response (404 Not Found):** Si no es tuyo o no existe

---

#### `POST /api/Proyectos` - Crear Proyecto

**🔐 Requiere:** JWT Token

**Request Body:**
```json
{
  "nombreObra": "Centro Comercial Norte",
  "ubicacion": "Boulevard Norte 5678",
  "fechaInicio": "2025-02-01"
}
```

**Response (201 Created):**
```json
{
  "proyectoID": 3,
  "nombreObra": "Centro Comercial Norte",
  "ubicacion": "Boulevard Norte 5678",
  "fechaInicio": "2025-02-01T00:00:00",
  "userId": 2,  // 🆕 Asignado automáticamente
  "estimaciones": []
}
```

> **🆕 Cambio**: El campo `userId` se asigna automáticamente del token JWT.

---

#### `DELETE /api/Proyectos/{id}` - Eliminar Proyecto

**🔐 Requiere:** JWT Token  
**🆕 Validación:** Solo puedes eliminar tus propios proyectos

**Response (204 No Content):** Eliminado exitosamente  
**Response (403 Forbidden):** Si intentas eliminar un proyecto que no es tuyo  
**Response (404 Not Found):** Si no existe

---

### 💰 **Estimaciones de Costo**

#### `POST /api/Estimaciones` - Crear Estimación

**🔐 Requiere:** JWT Token  
**🆕 Validación:** Solo puedes crear estimaciones en **tus proyectos**

**Request Body:**
```json
{
  "concepto": "Instalaciones Eléctricas",
  "montoEstimado": 350000.00,
  "proyectoID": 1
}
```

**Response (201 Created):** Si el proyecto es tuyo  
**Response (403 Forbidden):** Si intentas crear en proyecto ajeno

---

### 📈 **Avances de Obra**

#### `POST /api/Avances` - Registrar Avance

**🔐 Requiere:** JWT Token  
**🆕 Validación:** Solo puedes registrar avances en **tus estimaciones**

**Request Body:**
```json
{
  "montoEjecutado": 120000.00,
  "porcentajeCompletado": 35.5,
  "costoID": 1
}
```

**Response (201 Created):** Si la estimación pertenece a tu proyecto  
**Response (403 Forbidden):** Si intentas registrar en estimación ajena

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

**Miguel Rodríguez**  
GitHub: [@DRMiguel25](https://github.com/DRMiguel25)

---

## 📄 Licencia

Este proyecto es de código abierto para fines educativos.

---

**⭐ Si este proyecto te fue útil, dale una estrella en GitHub!**
