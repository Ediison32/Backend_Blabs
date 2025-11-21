namespace ProyectoBilaps.Domain.Entities
{
    public class Rol
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty; // admin, coder, externo

        public ICollection<UsuarioRol> UsuarioRoles { get; set; } = new List<UsuarioRol>();
    }
}