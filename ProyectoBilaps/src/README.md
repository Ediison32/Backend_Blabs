# Proyecto Bilaps – Backend .NET 9

Este proyecto implementa la plataforma Bilaps para gestionar usuarios, activación por correo, login, recuperación de contraseña y administración general.

Incluye arquitectura limpia con capas **Domain**, **Infrastructure**, **Application** y **Presentation**, además de tokens de activación y envío de correos.

---

## Tecnologías Utilizadas

- **.NET 9**
- **Entity Framework Core**
- **MailKit**
- **BCrypt.Net** para encriptación
- **PostgreSQL / SQL Server** (según configuración)
- **Arquitectura Limpia (Clean Architecture)**

---

##  Estructura del Proyecto

```
ProyectoBilaps
│
├── ProyectoBilaps.Domain
│   ├── Entities
│   ├── Interfaces
│
├── ProyectoBilaps.Infrastructure
│   ├── Data
│   ├── Repository
│
├── ProyectoBilaps.Application
│   ├── DTOs
│   ├── Services
│
├── ProyectoBilaps.Presentation
│   ├── Controllers
│   ├── Program.cs
│   ├── appsettings.json
```

---

#  API – Endpoints

A continuación están TODOS los endpoints disponibles agrupados por funcionalidad.

---

#  AUTH

## **POST /api/auth/register**
Registra un usuario y envía un enlace de activación al correo.

### Body:
```json
{
  "nombre": "Daniel",
  "apellido": "Romero",
  "cedula": "123456789",
  "email": "correo@example.com",
  "password": "Temporal123"
}
```

---

## **POST /api/auth/activate**
Activa la cuenta usando el token enviado al correo.

### Body:
```json
{
  "token": "token_que_llega_por_correo"
}
```

---

## **POST /api/auth/login**
Inicia sesión con correo y contraseña.

### Body:
```json
{
  "email": "correo@example.com",
  "password": "123456"
}
```

---

## **POST /api/auth/set-password**
El usuario asigna una contraseña definitiva después de activar su cuenta.

### Body:
```json
{
  "email": "correo@example.com",
  "password": "NuevaPassword123"
}
```

---

#  USUARIOS (CRUD – Admin)

## **GET /api/usuario**
Obtiene todos los usuarios.

---

## **GET /api/usuario/{id}**
Obtiene un usuario por ID.

---

## **POST /api/usuario**
Crea un usuario manualmente (modo administrador).

---

## **PUT /api/usuario/{id}**
Actualiza información de un usuario.

---

## **DELETE /api/usuario/{id}**
Elimina un usuario.

---

#  Correo y Activación

El sistema envía:
- Correo con **link de activación**
- Opcional: notificación de contraseña temporal
- Recordatorio de cuenta activada

Formato del enlace:
```
https://frontend.com/activar?token=ABC123
```

---

#  Configuración

En `appsettings.json`:

```json
"EmailSettings": {
  "Host": "smtp.gmail.com",
  "Port": 587,
  "Username": "tucorreo@gmail.com",
  "Password": "app_password"
},
"FrontendUrl": "https://tufrontend.com"
```

---

#  Migraciones

Crear migración:
```
dotnet ef migrations add Initial
```

Aplicar migración:
```
dotnet ef database update
```

---


#  Cómo Ejecutar

1. Configurar base de datos y variables.
2. Restaurar paquetes:
```
dotnet restore
```
3. Compilar:
```
dotnet build
```
4. Ejecutar:
```
dotnet run
```

---

#  Notas Finales

El proyecto está listo para integrarse con un **frontend de login, activación y cambio de contraseña**, usando los endpoints definidos arriba.

---

**Bilaps Team ** 

