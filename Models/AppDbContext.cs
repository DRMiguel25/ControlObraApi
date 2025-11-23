using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace ControlObraApi.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Proyecto> Proyectos { get; set; }
        public DbSet<EstimacionCosto> EstimacionesCosto { get; set; }
        public DbSet<AvanceObra> AvancesObra { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar relaciones... (Resto de tu código)

            // Configurar explícitamente la relación AvanceObra -> EstimacionCosto
            modelBuilder.Entity<AvanceObra>()
                .HasOne(a => a.EstimacionCosto)
                .WithMany(e => e.Avances)
                .HasForeignKey(a => a.CostoID)
                .OnDelete(DeleteBehavior.Restrict);

            //  Configurar explícitamente la relación EstimacionCosto -> Proyecto
            modelBuilder.Entity<EstimacionCosto>()
                .HasOne(e => e.Proyecto)
                .WithMany(p => p.Estimaciones)
                .HasForeignKey(e => e.ProyectoID)
                .OnDelete(DeleteBehavior.Restrict);


            // ******************************************************
            // *** CONFIGURACIÓN CLAVE: SEED DATA (Datos Iniciales) ***
            // ******************************************************

            // 1. Insertar Proyecto (ID 2)
            modelBuilder.Entity<Proyecto>().HasData(
                new Proyecto { ProyectoID = 2, NombreObra = "Torre Residencial Alpha", Ubicacion = "Zona Central", FechaInicio = new DateTime(2025, 1, 15) }
            );

            // 2. Insertar Estimaciones (ID 1 y 2)
            modelBuilder.Entity<EstimacionCosto>().HasData(
                new EstimacionCosto { CostoID = 1, Concepto = "Cimentación y Estructura", MontoEstimado = 150000.00m, ProyectoID = 2 },
                new EstimacionCosto { CostoID = 2, Concepto = "Instalaciones Eléctricas", MontoEstimado = 45000.00m, ProyectoID = 2 }
            );

            // 3. Insertar Avances (ID 1 y 2)
            modelBuilder.Entity<AvanceObra>().HasData(
                new AvanceObra { AvanceID = 1, MontoEjecutado = 75000.00m, PorcentajeCompletado = 50.00m, CostoID = 1, FechaRegistro = DateTime.Now.AddDays(-10) },
                new AvanceObra { AvanceID = 2, MontoEjecutado = 10000.00m, PorcentajeCompletado = 20.00m, CostoID = 2, FechaRegistro = DateTime.Now.AddDays(-5) }
            );
        }
    }
}