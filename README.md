# AOI1_Pedetta
Actividad Obligatoria 1 de Mobile III: API de contactos

## Tecnologías Utilizadas

*   **.NET 8 (C#)**: Framework principal para el desarrollo de la Web API.
*   **ASP.NET Core Web API**: Patrón de arquitectura donde se combinan Minimal APIs y Controladores REST clásicos.
*   **JWT (JSON Web Tokens)**: Implementado para la autenticación y autorización segura en los endpoints de los controladores.
*   **Swagger / OpenAPI**: Herramienta integrada para la documentación y prueba interactiva de la API (incluye soporte para enviar el token JWT).

## Funcionalidades del Proyecto

El proyecto consiste en una API para la gestión de contactos, la cual implementa un CRUD completo gestionado en memoria (usando el patrón inyección en Singleton).

*   **Autenticación (`/api/auth/login`)**: Generación de tokens JWT tras validar credenciales.
*   **Minimal API (`/minimal/contactos`)**: Endpoint de acceso público (sin seguridad JWT) para recuperar la lista de todos los contactos.
*   **Controlador REST Protegido (`/api/contacto`)**: Conjunto de endpoints protegidos por JWT (`[Authorize]`):
    *   `GET /{id}`: Obtiene un contacto específico mediante su identificador.
    *   `POST /add`: Agrega un nuevo contacto.
    *   `PUT /edit/{id}`: Modifica por completo un contacto existente.
    *   `PATCH /edit/telefono/{id}`: Actualiza de manera parcial solo el número de teléfono de un contacto.
    *   `PATCH /edit/email/{id}`: Actualiza de manera parcial solo la dirección de email de un contacto.
    *   `DELETE /delete/{id}`: Elimina un contacto del sistema.

## Credenciales para la Demo

Para probar los endpoints protegidos desde Swagger u otra herramienta (como Postman), debes generar un token JWT accediendo al endpoint de Login (`POST /api/auth/login`).

Las credenciales hardcodeadas en el sistema para realizar las pruebas son:

*   **Usuario**: `admin`
*   **Contraseña**: `1234`

> **Nota para la prueba en Swagger:** Una vez obtenido el token en el formato JSON, debes copiar y pegar el valor del token en el candado de autorización ("Authorize" en la esquina superior) con el prefijo estricto `Bearer `.
>
> Ejemplo válido:
> `Bearer eyJhbGciOiJIUzI1Ni...`
