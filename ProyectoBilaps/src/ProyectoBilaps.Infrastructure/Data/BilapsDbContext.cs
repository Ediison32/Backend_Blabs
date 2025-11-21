using Microsoft.EntityFrameworkCore;
using ProyectoBilaps.Domain.Entities;

namespace ProyectoBilaps.Infrastructure.Data
{
    public class BilapsDbContext : DbContext
    {
        public BilapsDbContext(DbContextOptions<BilapsDbContext> options)
            : base(options)
        {
        }

        // ============================================
        //              TABLAS DEL DOMINIO
        // ============================================
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<UsuarioRol> UsuarioRoles { get; set; }
        public DbSet<ActivationToken> ActivationTokens { get; set; }

        // ============================================
        //              CONFIGURACIÓN MODELO
        // ============================================
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --------------------------------------------
            // CONFIGURAR RELACIÓN MUCHOS A MUCHOS
            // Usuario <-> Rol mediante UsuarioRol
            // --------------------------------------------
            modelBuilder.Entity<UsuarioRol>()
                .HasKey(ur => new { ur.UsuarioId, ur.RolId });

            modelBuilder.Entity<UsuarioRol>()
                .HasOne(ur => ur.Usuario)
                .WithMany(u => u.UsuarioRoles)
                .HasForeignKey(ur => ur.UsuarioId);

            modelBuilder.Entity<UsuarioRol>()
                .HasOne(ur => ur.Rol)
                .WithMany(r => r.UsuarioRoles)
                .HasForeignKey(ur => ur.RolId);

            // --------------------------------------------
            // SEED DE ROLES INICIALES
            // --------------------------------------------
            modelBuilder.Entity<Rol>().HasData(
                new Rol { Id = 1, Nombre = "Admin" },
                new Rol { Id = 2, Nombre = "Coder" }
            );
        }
    }
}
