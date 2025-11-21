namespace ProyectoBilaps.Application.DTOs
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public bool Activo { get; set; }
        public List<int> Roles { get; set; } = new();
    }
}