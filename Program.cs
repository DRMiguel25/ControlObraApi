// Program.cs (C# API - Versión FINAL y FUNCIONAL)
using ControlObraApi.Models;
using ControlObraApi.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder; 
using System.Text.Json.Serialization; 

var builder = WebApplication.CreateBuilder(args);

// ** 1. DEFINICIÓN: Política CORS para Angular **
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins"; 

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          // Permitir peticiones desde tu Front-End de Angular (http://localhost:4200)
                          policy.WithOrigins("http://localhost:4200") 
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

// A. Base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSQL")));

// B. Controladores con configuración JSON
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        // Ignorar referencias circulares
        options.SerializerSettings.ReferenceLoopHandling = 
            Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

// C. Validadores
builder.Services.AddScoped<IValidator<EstimacionCosto>, EstimacionCostoValidator>();
builder.Services.AddScoped<IValidator<AvanceObra>, AvanceObraValidator>();

// D. HttpClientFactory - Consumo de APIs Externas (JSONPlaceholder)
builder.Services.AddHttpClient("jsonplaceholder", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ExternalApis:Base_url"]!);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.Timeout = TimeSpan.FromSeconds(30);
});

// JWT Configuration
builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// Swagger y servicios
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ** ORDEN DE MIDDLEWARES (CRÍTICO) **

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 1. USO DE ROUTING (Debe ir antes de CORS y Autorización)
app.UseRouting();

// 2. USO DE CORS (Debe ir DESPUÉS de UseRouting)
app.UseCors(MyAllowSpecificOrigins);

// 3. AUTORIZACIÓN
app.UseAuthentication();
app.UseAuthorization();

// 4. MAPEO FINAL DE CONTROLADORES (Forma recomendada que resuelve el 404)
app.MapControllers(); 

app.Run();
