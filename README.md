## Desarrollo de un sistema Web que Gestione inventario de una empresa ¨XWY¨
Este proyecto consiste en un sistema web de inventario y préstamos desarrollado con una arquitectura en capas, orientado a la gestión de artículos, clientes, empleados y préstamos dentro de una organización. Fue construido con un enfoque moderno tanto en el backend como en el frontend, garantizando seguridad, escalabilidad y una interfaz amigable para el usuario.
## 🧱 Tecnologías y Arquitectura
- Backend: ASP.NET Core Web API (.NET 8), Entity Framework Core, JWT para autenticación, arquitectura por capas (Entities, DataAccess, Business, API)
-	Frontend: HTML, CSS moderno, JavaScript puro (sin frameworks externos), utilizando fetch para llamadas a la API y localStorage para gestionar sesiones.
-	Base de datos: SQL Server, con relaciones correctamente modeladas entre usuarios, empleados, roles, artículos y préstamos.
## 🔐 Control de acceso
El sistema diferencia entre:
-	Admin: puede crear, editar, eliminar y visualizar todos los datos.
-	Operator: puede consultar y registrar información limitada (como préstamos o clientes).
-	Autenticación: basada en JWT, con claims personalizados para rol y employeeId.
## 🧭 Funcionalidades principales
-	Registro y login de usuarios (con hash de contraseña).
-	Gestión completa de artículos, clientes, empleados y préstamos.
-	Vistas protegidas por rol con políticas [Authorize(Policy = "...")].
-	Dashboard visual con resumen y gráficas.
-	Exportación a PDF/Excel y diseño responsivo.
-	Alertas visuales, confirmaciones para editar/eliminar y validaciones claras.

## Usuarios de Prueba
- Email: admin@system.com - Contraseña: 123
- Email: operator@system.com - Contraseña: 123

Nota: Estos usuarios son importantes para verificar el Swagger, ya que con estos se puede ingresar y obtener el token sin la necesidad de crear un usuario.

## 🧭 Tabla de contenidos

- [🚀 Despliegue](#-despliegue)
- [🧱 Arquitectura y Patrones](#-arquitectura-y-patrones)
- [🔐 Autenticación y Seguridad](#-autenticación-y-seguridad)
- [📘 Documentación Swagger](#-documentación-swagger)
- [📡 Endpoints de la API](#-endpoints-de-la-api)
  - [Auth](#auth)
  - [Users](#users)
  - [Clients](#clients)
  - [Articles](#articles)
  - [Loans](#loans)
  - [Observations](#observations)
- [🧪 Testing](#-testing)

---

## 🚀 Despliegue

1. Clona el repositorio:
   ```bash
   git clone https://github.com/usuario/inventory-system.git
   ```

2. Abre la solución en **Visual Studio 2022**

3. Verifica el archivo `appsettings.json` (cadena de conexión, clave JWT).

4. Ejecuta las migraciones:
   ```bash
   dotnet ef database update
   ```

5. Inicia el proyecto (`InventorySystem.API`) y navega al login:
   ```
   https://localhost:{puerto}/pages/login.html
   ```

6. Al registrarse se crea un usuario operador por defecto. Los roles deben existir previamente en la base de datos (`Admin`, `Operator`).

---

## 🧱 Arquitectura y Patrones

- **Arquitectura en capas**
  - API (Controllers)
  - Business (Services)
  - DataAccess (UnitOfWork, Repositories)
  - Entities (Modelos)

- **Patrones usados**
  - `Repository`: abstrae acceso a datos
  - `Unit of Work`: coordina múltiples repositorios

- **Frontend**
  - HTML/CSS/JS puro dentro de `wwwroot/`
  - Estilos modernos, dashboard responsive y toasts dinámicos

---

## 🔐 Autenticación y Seguridad

- Login y registro retornan un **JWT token**.
- El token incluye el `role` y `employeeId` en su payload.
- Los endpoints requieren el token en cada llamada (excepto login y register).

### 🔑 Headers obligatorios:
```http
Authorization: Bearer {token}
Content-Type: application/json
```

---

## 📘 Documentación Swagger

Puedes explorar y probar todos los endpoints desde Swagger UI:

```
https://localhost:{puerto}/swagger/index.html
```

---

## 📡 Endpoints de la API

### Auth

#### 🔐 POST `/api/auth/login`

```json
{
  "email": "admin@example.com",
  "password": "admin123"
}
```

**Response:**
```json
{
  "token": "JWT_TOKEN",
  "employeeId": "GUID",
  "role": "Admin"
}
```

#### 📝 POST `/api/auth/register`

```json
{
  "firstName": "Ana",
  "lastName": "Martinez",
  "email": "ana@example.com",
  "password": "clave123",
  "role": "Operator"
}
```

---

### Users (solo Admin)

#### 🔎 GET `/api/users`
#### 🔎 GET `/api/users/{id}`
#### 🆕 POST `/api/users`
```json
{
  "firstName": "Juan",
  "lastName": "Perez",
  "email": "juan@example.com",
  "password": "1234",
  "role": "Admin"
}
```
#### 🖊️ PUT `/api/users/{id}`
```json
{
  "firstName": "Juan",
  "lastName": "Pérez Modificado",
  "email": "juan@example.com",
  "password": "nueva123",
  "role": "Operator"
}
```
#### 🗑️ DELETE `/api/users/{id}`

---

### Clients

#### 🔎 GET `/api/clients`
#### 🔎 GET `/api/clients/{id}`
#### 🆕 POST `/api/clients`
```json
{
  "firstName": "Laura",
  "lastName": "Gonzalez",
  "email": "laura@example.com",
  "phone": "555-1234"
}
```
#### 🖊️ PUT `/api/clients/{id}`
#### 🗑️ DELETE `/api/clients/{id}`

---

### Articles

#### 🔎 GET `/api/articles`
#### 🔎 GET `/api/articles/{id}`
#### 🆕 POST `/api/articles`
```json
{
  "code": "ART123",
  "name": "Proyector Epson",
  "category": "Tecnología",
  "status": "Available",
  "location": "Aula 102"
}
```
#### 🖊️ PUT `/api/articles/{id}`
#### 🗑️ DELETE `/api/articles/{id}`

---

### Loans

#### 🔎 GET `/api/loans`
#### 🔎 GET `/api/loans/{id}`
#### 🆕 POST `/api/loans`
```json
{
  "clientId": "GUID",
  "articleId": "GUID",
  "employeeId": "GUID",
  "deliveredAt": "2025-06-29",
  "status": "Pending"
}
```
#### 🖊️ PUT `/api/loans/{id}`
#### 🗑️ DELETE `/api/loans/{id}`
#### 🆗 PUT `/api/loans/{id}/status`
```json
"Approved"
```

---

### Observations

#### 🔎 GET `/api/observations/loan/{loanId}`
#### 🆕 POST `/api/observations`
```json
{
  "loanId": "GUID",
  "text": "Equipo entregado en buen estado"
}
```

---

## 🧪 Testing

- **5 pruebas unitarias** en `InventorySystem.Tests.Business`
  - Pruebas de lógica de validación y creación en `UserService`, `LoanService`

- **3 pruebas de integración** usando `InMemoryDbContext`
  - Inserción, consulta y borrado de entidades

---

## ✅ Requisitos para acceder

Antes de llamar a cualquier endpoint protegido (todo excepto login/register), el cliente debe:

1. Hacer login con `/api/auth/login`
2. Guardar el `token`, `role` y `employeeId` en `localStorage` o cabecera
3. Agregar el header:
```http
Authorization: Bearer {token}
```

---

## 📌 Tecnologías utilizadas

- ASP.NET Core 8 Web API  
- Entity Framework Core (SQL Server)  
- AutoMapper  
- JWT Authentication  
- HTML/CSS/JS puro (sin frameworks)  
- jsPDF / XLSX para exportación de datos  

---

© 2025 - Sistema de Inventario desarrollado como proyecto educativo.
