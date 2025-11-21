namespace ProyectoBilaps.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;

        public string Cedula { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        // La contraseña todavía NO existe hasta que el usuario active su cuenta
        public string? PasswordHash { get; set; }

        public bool Activo { get; set; } = false;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Relación con roles
        public ICollection<UsuarioRol> UsuarioRoles { get; set; } = new List<UsuarioRol>();
    }
}