// Program.cs
using ControlObraApi.Models;
using ControlObraApi.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
// Se removió System.Text.Json.Serialization si no se usa (la app usa NewtonSoft)

var builder = WebApplication.CreateBuilder(args);

// ** 1. DEFINICIÓN: Política CORS para Angular **
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins"; 

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          // Permitir peticiones desde tu Front-End de Angular (puerto 4200)
                          policy.WithOrigins("http://localhost:4200") 
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});
// ** FIN DEFINICIÓN CORS **

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

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ** 2. USO: Aplicar la política CORS **
// Esto debe ir antes de UseAuthorization() y MapControllers()
app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();