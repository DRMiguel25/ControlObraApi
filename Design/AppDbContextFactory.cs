//AppDbContextFactory
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ControlObraApi.Models; // Asegúrate de que el namespace sea correcto

namespace ControlObraApi.Design
{
    // Implementamos la interfaz IDesignTimeDbContextFactory para crear el contexto
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // 1. Configurar la lectura del archivo appsettings.json
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // 2. Obtener la cadena de conexión
            var connectionString = configuration.GetConnectionString("ConexionSQL");
            
            // 3. Configurar DbContextOptions para SQL Server
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseSqlServer(connectionString);

            // 4. Devolver el contexto
            return new AppDbContext(builder.Options);
        }
    }
}