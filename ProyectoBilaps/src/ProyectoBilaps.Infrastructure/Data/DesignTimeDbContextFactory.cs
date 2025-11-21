using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ProyectoBilaps.Infrastructure.Data;
using System.IO;

namespace ProyectoBilaps.Infrastructure.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BilapsDbContext>
    {
        public BilapsDbContext CreateDbContext(string[] args)
        {
            // Construir configuración leyendo appsettings.json del proyecto Presentation
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "ProyectoBilaps.Presentation"))
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var builder = new DbContextOptionsBuilder<BilapsDbContext>();
            builder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 36)));

            return new BilapsDbContext(builder.Options);
        }
    }
}