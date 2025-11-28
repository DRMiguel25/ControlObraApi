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
        public DbSet<AvanceFoto> AvanceFotos { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🆕 Relación User -> Proyecto (1:N) - OWNERSHIP
            modelBuilder.Entity<Proyecto>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

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
            // *** SEED DATA - Usuario Demo y Proyectos de Prueba ***
            // ******************************************************

            // 1. Usuario Demo (para facilitar pruebas del profesor)
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Name = "Usuario Demo",
                    Email = "demo@test.com",
                    Username = "demo",
                    // Hash BCrypt de "Pass123!"
                    PasswordHash = "$2a$11$vY8y8HZg5LqW3z9X2wK7P.7TqK9Z3yN4WqR8P.XmF5Kx9V7Qz8z9e",
                    Role = "User"
                }
            );

            // 2. Proyectos de Demo (pertenecen al usuario demo ID=1)
            modelBuilder.Entity<Proyecto>().HasData(
                new Proyecto
                {
                    ProyectoID = 1,
                    NombreObra = "Torre Residencial Alpha",
                    Ubicacion = "Zona Central",
                    FechaInicio = new DateTime(2025, 1, 15),
                    UserId = 1  // Pertenece al usuario demo
                },
                new Proyecto
                {
                    ProyectoID = 2,
                    NombreObra = "Edificio Comercial Beta",
                    Ubicacion = "Zona Norte",
                    FechaInicio = new DateTime(2025, 2, 1),
                    UserId = 1  // Pertenece al usuario demo
                }
            );

            // 3. Estimaciones de Costo
            modelBuilder.Entity<EstimacionCosto>().HasData(
                new EstimacionCosto { CostoID = 1, Concepto = "Cimentación y Estructura", MontoEstimado = 150000.00m, ProyectoID = 1 },
                new EstimacionCosto { CostoID = 2, Concepto = "Instalaciones Eléctricas", MontoEstimado = 45000.00m, ProyectoID = 1 },
                new EstimacionCosto { CostoID = 3, Concepto = "Acabados Interiores", MontoEstimado = 80000.00m, ProyectoID = 2 }
            );

            // 4. Avances de Obra
            modelBuilder.Entity<AvanceObra>().HasData(
                new AvanceObra { AvanceID = 1, MontoEjecutado = 75000.00m, PorcentajeCompletado = 50.00m, CostoID = 1, FechaRegistro = new DateTime(2025, 1, 20) },
                new AvanceObra { AvanceID = 2, MontoEjecutado = 10000.00m, PorcentajeCompletado = 22.22m, CostoID = 2, FechaRegistro = new DateTime(2025, 1, 25) },
                new AvanceObra { AvanceID = 3, MontoEjecutado = 30000.00m, PorcentajeCompletado = 37.50m, CostoID = 3, FechaRegistro = new DateTime(2025, 2, 5) }
            );
        }
    }
}