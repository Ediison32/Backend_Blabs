namespace ProyectoBilaps.Domain.Entities
{
    public class ActivationToken
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        public string Token { get; set; } = string.Empty;

        public DateTime FechaExpiracion { get; set; }

        public bool Usado { get; set; } = false;
    }
}