using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.Data;
using backend.DTOs.Auth;
using backend.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace backend.Services;

public class AuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    public AuthService(AppDbContext context, IConfiguration configuration) { _context = context; _configuration = configuration; }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var nombre = request.NombreUsuario.Trim().ToLowerInvariant();
        var credencial = request.Password.Trim();
        var usuario = await _context.Usuarios
            .Include(u => u.Rol).ThenInclude(r => r.Permisos)
            .Include(u => u.Alumno)
            .Include(u => u.Profesor)
            .FirstOrDefaultAsync(u => u.Activo && u.Rol.Activo &&
                ((u.Rol.Nombre.ToUpper() == "ALUMNO" && u.Alumno != null && u.Alumno.Activo && u.Alumno.Dni == credencial &&
                    ((u.Alumno.Nombre + " " + u.Alumno.Apellido).ToLower() == nombre ||
                    u.Alumno.Nombre.ToLower() == nombre ||
                    (u.Alumno.Apellido + " " + u.Alumno.Nombre).ToLower() == nombre ||
                    (u.Alumno.Apellido + ", " + u.Alumno.Nombre).ToLower() == nombre)) ||
                (u.Rol.Nombre.ToUpper() == "PROFESOR" && u.Profesor != null && u.Profesor.Activo && u.Profesor.Dni == credencial &&
                    ((u.Profesor.Nombre + " " + u.Profesor.Apellido).ToLower() == nombre ||
                    u.Profesor.Nombre.ToLower() == nombre ||
                    (u.Profesor.Apellido + " " + u.Profesor.Nombre).ToLower() == nombre ||
                    (u.Profesor.Apellido + ", " + u.Profesor.Nombre).ToLower() == nombre))));

        if (usuario == null)
        {
            usuario = await _context.Usuarios.Include(u => u.Rol).ThenInclude(r => r.Permisos)
                .FirstOrDefaultAsync(u => u.NombreUsuario.ToLower() == nombre && u.Activo && u.Rol.Activo);
            if (usuario == null || usuario.Rol.Nombre.Equals("ALUMNO", StringComparison.OrdinalIgnoreCase) || usuario.Rol.Nombre.Equals("PROFESOR", StringComparison.OrdinalIgnoreCase) || !PasswordHasher.Verify(credencial, usuario.PasswordHash)) return null;
        }

        var key = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("Falta Jwt:Key");
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()), new Claim(ClaimTypes.Name, usuario.NombreUsuario), new Claim(ClaimTypes.Role, usuario.Rol.Nombre) };
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"], audience: _configuration["Jwt:Audience"], claims: claims,
            expires: DateTime.UtcNow.AddHours(12), signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256));

        var nombreCompleto = usuario.Alumno != null ? $"{usuario.Alumno.Nombre} {usuario.Alumno.Apellido}".Trim()
            : usuario.Profesor != null ? $"{usuario.Profesor.Nombre} {usuario.Profesor.Apellido}".Trim()
            : $"{usuario.Nombre} {usuario.Apellido}".Trim();
        return new LoginResponse { Token = new JwtSecurityTokenHandler().WriteToken(token), UsuarioId = usuario.Id, NombreUsuario = usuario.NombreUsuario, NombreCompleto = nombreCompleto, Rol = usuario.Rol.Nombre, AlumnoId = usuario.AlumnoId, ProfesorId = usuario.ProfesorId, Permisos = usuario.Rol.EsSistema ? ScreenKeys.Todas.Keys.OrderBy(x => x).ToList() : usuario.Rol.Permisos.Select(p => p.Pantalla).OrderBy(x => x).ToList() };
    }
}
