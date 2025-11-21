using Microsoft.EntityFrameworkCore;
using ProyectoBilaps.Domain.Entities;
using ProyectoBilaps.Domain.Interfaces;
using ProyectoBilaps.Infrastructure.Data;

namespace ProyectoBilaps.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly BilapsDbContext _context;

        public UsuarioRepository(BilapsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Usuario>> GetAll()
        {
            return await _context.Usuarios
                .Include(u => u.UsuarioRoles)
                .ThenInclude(ur => ur.Rol)
                .ToListAsync();
        }

        public async Task<Usuario?> GetById(int id)
        {
            return await _context.Usuarios
                .Include(u => u.UsuarioRoles)
                .ThenInclude(ur => ur.Rol)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Usuario> Add(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<Usuario> Update(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<bool> Delete(int id)
        {
            var user = await _context.Usuarios.FindAsync(id);
            if (user == null) return false;

            _context.Usuarios.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}