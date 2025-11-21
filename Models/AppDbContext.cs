//AppDbContext
using Microsoft.EntityFrameworkCore;

namespace ControlObraApi.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Proyecto> Proyectos { get; set; }
        public DbSet<EstimacionCosto> EstimacionesCosto { get; set; }
        public DbSet<AvanceObra> AvancesObra { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
        }
    }
}
