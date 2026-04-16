using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AOI1_Pedetta.Models;
using AOI1_Pedetta.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. SERVICIOS

// ContactoService como Singleton: la lista en memoria persiste entre peticiones HTTP.
builder.Services.AddSingleton<ContactoService>();

// Controladores (necesarios para el ContactosController)
builder.Services.AddControllers();

// Swagger/OpenAPI con configuración para JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AOI1 Pedetta - Contactos API",
        Version = "v1",
        Description = "API con autenticación JWT para gestión de contactos."
    });

    // Definición del esquema de seguridad JWT en Swagger.
    // Usamos ApiKey para que el usuario escriba el valor completo del header:
    //   "Bearer eyJhbGci..."   ← con el prefijo incluido
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,   // ← ApiKey controla el header completo
        In = ParameterLocation.Header,
        Description = "Ingresá el token con el prefijo 'Bearer '. Ejemplo: Bearer eyJhbGci..."
    });

    // Requisito global: todos los endpoints protegidos muestran el candado
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// 2. AUTENTICACIÓN JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["Key"]!;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

builder.Services.AddAuthorization();

// 3. BUILD
var app = builder.Build();

// 4. PIPELINE DE MIDDLEWARES
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();   // Primero autenticación
app.UseAuthorization();    // Luego autorización

// Mapeo de controladores
app.MapControllers();

// 5. MINIMAL API ENDPOINTS

// GET /minimal/contactos – sin autenticación, usa el servicio inyectado
app.MapGet("/minimal/contactos", (ContactoService service) =>
{
    return Results.Ok(service.ObtenerTodos());
})
.WithName("GetContactosMinimal")
.WithOpenApi()
.WithTags("Minimal API");

// POST /api/auth/login – genera token JWT con credenciales hardcodeadas
app.MapPost("/api/auth/login", (LoginRequest request) =>
{
    // Credenciales hardcodeadas para la demo
    if (request.Usuario != "admin" || request.Contrasena != "1234")
        return Results.Unauthorized();

    // Generación del token JWT
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var claims = new[]
    {
        new Claim(ClaimTypes.Name, request.Usuario),
        new Claim(ClaimTypes.Role, "Admin")
    };

    var expiracion = int.Parse(jwtSettings["ExpiresInMinutes"] ?? "60");
    var token = new JwtSecurityToken(
        issuer: jwtSettings["Issuer"],
        audience: jwtSettings["Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(expiracion),
        signingCredentials: creds
    );

    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
    return Results.Ok(new { token = tokenString, expiraEn = $"{expiracion} minutos" });
})
.WithName("Login")
.WithOpenApi()
.WithTags("Autenticación");

app.Run();

// 6. RECORDS / DTOs LOCALES

// DTO para el body del endpoint de login
record LoginRequest(string Usuario, string Contrasena);
