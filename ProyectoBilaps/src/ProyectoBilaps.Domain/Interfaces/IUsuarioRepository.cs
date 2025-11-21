using ProyectoBilaps.Domain.Entities;

namespace ProyectoBilaps.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> GetAll();
        Task<Usuario?> GetById(int id);
        Task<Usuario> Add(Usuario usuario);
        Task<Usuario> Update(Usuario usuario);
        Task<bool> Delete(int id);
    }
}