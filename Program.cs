using ControlObraApi.Models;
using ControlObraApi.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ** 1. DEFINICIÓN: Política CORS para Angular **
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins"; 

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          // Permitir peticiones desde tu Front-End de Angular
                          policy.WithOrigins("http://localhost:4200") 
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

// ** 2. BASE DE DATOS (CORREGIDO Y OPTIMIZADO) **
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSQL"), 
        sqlOptions => {
            // ESTO SOLUCIONA EL  WARNING DE RENDIMIENTO:
            // Divide las consultas complejas (con muchos Includes) en varias consultas SQL pequeñas
            // en lugar de una gigante, evitando datos duplicados y mejorando velocidad.
            sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        }));

//  3. CONTROLADORES Y SERIALIZACIÓN JSON 
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        // Ignorar referencias circulares (Indispensable para relaciones Padre-Hijo)
        options.SerializerSettings.ReferenceLoopHandling = 
            Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

// 4. VALIDADORES (FLUENT VALIDATION) 
builder.Services.AddScoped<IValidator<EstimacionCosto>, EstimacionCostoValidator>();
builder.Services.AddScoped<IValidator<AvanceObra>, AvanceObraValidator>();

//  5. SWAGGER 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//  6. PIPELINE DE MIDDLEWARES (ORDEN ESTRICTO) 

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// A. Redirección HTTPS (Opcional, pero recomendado si no estás solo en local)
// app.UseHttpsRedirection(); 

// B. Routing
app.UseRouting();

// C. CORS (Debe ir entre UseRouting y UseAuthorization)
app.UseCors(MyAllowSpecificOrigins);

// D. Autorización
app.UseAuthorization();

// E. Mapeo de Controladores
app.MapControllers(); 

app.Run();