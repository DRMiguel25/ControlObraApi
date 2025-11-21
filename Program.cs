using ControlObraApi.Models;
using ControlObraApi.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization; 

var builder = WebApplication.CreateBuilder(args);

// A. Base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSQL")));

// B. Controladores con configuración JSON
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        //  Ignorar referencias circulares
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

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
