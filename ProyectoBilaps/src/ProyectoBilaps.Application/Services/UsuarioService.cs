using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProyectoBilaps.Domain.Entities;
using ProyectoBilaps.Domain.Interfaces;
using ProyectoBilaps.Application.DTOs;
using ProyectoBilaps.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace ProyectoBilaps.Application.Services
{
    public class UsuarioService
    {
        private readonly BilapsDbContext _context;
        private readonly IEmailService _email;
        private readonly IConfiguration _config;

        public UsuarioService(BilapsDbContext context, IEmailService email, IConfiguration config)
        {
            _context = context;
            _email = email;
            _config = config;
        }

        // 🔹 CRUD básico
        public async Task<List<Usuario>> GetAllAsync() => await _context.Usuarios.ToListAsync();

        public async Task<Usuario?> GetByIdAsync(int id) =>
            await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

        public async Task<Usuario> AddAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<bool> UpdateAsync(Usuario usuario)
        {
            var exists = await _context.Usuarios.AnyAsync(u => u.Id == usuario.Id);
            if (!exists) return false;

            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
            if (usuario == null) return false;

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return true;
        }

        // 🔹 Registro + activación
        public async Task<Usuario> RegisterWithActivationAsync(RegisterDto dto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
                throw new Exception("El correo ya está registrado.");

            var tempPasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Cedula);

            var user = new Usuario
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Cedula = dto.Cedula,
                Email = dto.Email,
                PasswordHash = tempPasswordHash,
                Activo = false,
                FechaCreacion = DateTime.UtcNow
            };

            _context.Usuarios.Add(user);
            await _context.SaveChangesAsync();

            _context.UsuarioRoles.Add(new UsuarioRol
            {
                UsuarioId = user.Id,
                RolId = dto.RolId
            });
            await _context.SaveChangesAsync();

            var token = await CreateActivationTokenAsync(user);

            var frontendUrl = _config["FrontendUrl"];
            if (string.IsNullOrWhiteSpace(frontendUrl))
                throw new Exception("No se encontró FrontendUrl en appsettings.json.");

            var link = $"{frontendUrl.TrimEnd('/')}/activar?token={token.Token}";
            var html = $@"
                <p>Hola {user.Nombre},</p>
                <p>Haz click <a href=""{link}"">aquí</a> para activar tu cuenta.</p>
                <p>Este enlace expira el <b>{token.FechaExpiracion:u}</b>.</p>";

            await _email.SendEmailAsync(user.Email, "Activa tu cuenta - Bilaps", html);

            return user;
        }

        public async Task<ActivationToken> CreateActivationTokenAsync(Usuario user)
        {
            var token = new ActivationToken
            {
                UsuarioId = user.Id,
                Token = Guid.NewGuid().ToString("N"),
                FechaExpiracion = DateTime.UtcNow.AddHours(24),
                Usado = false
            };

            _context.ActivationTokens.Add(token);
            await _context.SaveChangesAsync();
            return token;
        }

        public async Task<Usuario> ActivateUserAsync(string token)
        {
            var tokenEntity = await _context.ActivationTokens
                .Include(t => t.Usuario)
                .FirstOrDefaultAsync(t => t.Token == token && !t.Usado);

            if (tokenEntity == null)
                throw new Exception("Token inválido o ya usado.");

            if (tokenEntity.FechaExpiracion < DateTime.UtcNow)
                throw new Exception("El token ha expirado.");

            tokenEntity.Usado = true;
            tokenEntity.Usuario.Activo = true;
            await _context.SaveChangesAsync();

            return tokenEntity.Usuario;
        }

        // 🔹 Login
        public async Task<Usuario> LoginAsync(LoginDto dto)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null) throw new Exception("Credenciales inválidas.");
            if (!user.Activo) throw new Exception("La cuenta no está activa. Revisa tu correo.");

            bool valid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!valid) throw new Exception("Credenciales inválidas.");

            return user;
        }

        // 🔹 Actualizar email y contraseña
        public async Task<Usuario> UpdateEmailAndPasswordAsync(int usuarioId, UpdateUserEmailPasswordDto dto)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == usuarioId);
            if (user == null) throw new Exception("Usuario no encontrado.");

            if (!string.IsNullOrWhiteSpace(dto.Email))
                user.Email = dto.Email;

            if (!string.IsNullOrWhiteSpace(dto.NewPassword))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            await _context.SaveChangesAsync();
            return user;
        }
    }
}
