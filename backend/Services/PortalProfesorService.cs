using backend.Data;
using backend.DTOs.Calificaciones;
using backend.DTOs.Cursadas;
using backend.DTOs.PortalProfesor;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

public class PortalProfesorService
{
    private readonly AppDbContext _context;
    private readonly CalificacionService _calificaciones;

    public PortalProfesorService(AppDbContext context, CalificacionService calificaciones)
    {
        _context = context;
        _calificaciones = calificaciones;
    }

    private async Task<Guid?> GetProfesorIdAsync(Guid usuarioId) => await _context.Usuarios.AsNoTracking()
        .Where(u => u.Id == usuarioId && u.Activo && u.Rol.Activo && u.Rol.Nombre.ToUpper() == "PROFESOR")
        .Select(u => u.ProfesorId)
        .FirstOrDefaultAsync();

    public async Task<(MiPerfilProfesorDto? Data, string? Error)> GetMiPerfilAsync(Guid usuarioId)
    {
        var profesorId = await GetProfesorIdAsync(usuarioId);
        if (!profesorId.HasValue) return (null, "El usuario no tiene un profesor asociado.");
        var profesor = await _context.Profesores.AsNoTracking().FirstOrDefaultAsync(p => p.Id == profesorId.Value);
        return profesor == null ? (null, "El profesor asociado no existe.") : (MapPerfil(profesor), null);
    }

    public async Task<(MiPerfilProfesorDto? Data, string? Error)> ActualizarMiPerfilAsync(Guid usuarioId, ActualizarMiPerfilProfesorDto dto)
    {
        var profesorId = await GetProfesorIdAsync(usuarioId);
        if (!profesorId.HasValue) return (null, "El usuario no tiene un profesor asociado.");
        if (string.IsNullOrWhiteSpace(dto.Nombre) || string.IsNullOrWhiteSpace(dto.Apellido))
            return (null, "Nombre y apellido son obligatorios.");

        var profesor = await _context.Profesores.FirstOrDefaultAsync(p => p.Id == profesorId.Value);
        if (profesor == null) return (null, "El profesor asociado no existe.");
        profesor.Nombre = dto.Nombre.Trim();
        profesor.Apellido = dto.Apellido.Trim();
        profesor.Email = string.IsNullOrWhiteSpace(dto.Email) ? null : dto.Email.Trim();
        profesor.Telefono = string.IsNullOrWhiteSpace(dto.Telefono) ? null : dto.Telefono.Trim();
        profesor.Titulo = string.IsNullOrWhiteSpace(dto.Titulo) ? null : dto.Titulo.Trim();
        await _context.SaveChangesAsync();
        return (MapPerfil(profesor), null);
    }

    public async Task<(List<CursadaDto>? Data, string? Error)> GetMisCursadasAsync(Guid usuarioId)
    {
        var profesorId = await GetProfesorIdAsync(usuarioId);
        if (!profesorId.HasValue) return (null, "El usuario no tiene un profesor asociado.");
        var cursadas = await _context.Cursadas.AsNoTracking()
            .Where(c => c.ProfesorId == profesorId.Value)
            .Include(c => c.Materia).Include(c => c.Profesor).Include(c => c.Horarios).Include(c => c.Matriculas)
            .OrderByDescending(c => c.CicloLectivo).ThenBy(c => c.Materia.Nombre)
            .ToListAsync();
        return (cursadas.Select(c => new CursadaDto
        {
            Id = c.Id,
            MateriaId = c.MateriaId,
            MateriaNombre = c.Materia.Nombre,
            ProfesorId = c.ProfesorId,
            ProfesorNombre = c.Profesor == null ? null : c.Profesor.Nombre + " " + c.Profesor.Apellido,
            CicloLectivo = c.CicloLectivo,
            Periodo = c.Periodo,
            FechaInicio = c.FechaInicio,
            FechaFin = c.FechaFin,
            Activa = c.Activa,
            PorcentajePromocion = c.PorcentajePromocion,
            PorcentajeRegularidad = c.PorcentajeRegularidad,
            HorasSemanales = c.Horarios.Sum(h => (decimal)(h.HoraFin.ToTimeSpan() - h.HoraInicio.ToTimeSpan()).TotalHours),
            HorasTotalesPlanificadas = CursadaService.CalcularHorasTotales(c.FechaInicio, c.FechaFin, c.Horarios),
            CantidadAlumnos = c.Matriculas.Count(m => m.Activa),
            Horarios = c.Horarios.OrderBy(h => h.DiaSemana).ThenBy(h => h.HoraInicio)
                .Select(h => new HorarioDto { Id = h.Id, DiaSemana = h.DiaSemana, HoraInicio = h.HoraInicio.ToString("HH:mm"), HoraFin = h.HoraFin.ToString("HH:mm") }).ToList()
        }).ToList(), null);
    }

    public async Task<(PlanillaCalificacionesDto? Data, string? Error)> GetPlanillaAsync(Guid usuarioId, Guid cursadaId)
    {
        if (!await EsCursadaPropiaAsync(usuarioId, cursadaId)) return (null, "La cursada no existe o no está asignada a este profesor.");
        return await _calificaciones.GetPlanillaAsync(cursadaId);
    }

    public async Task<(PlanillaCalificacionesDto? Data, string? Error)> GenerarEvaluacionesAsync(Guid usuarioId, Guid cursadaId)
    {
        if (!await EsCursadaPropiaAsync(usuarioId, cursadaId)) return (null, "La cursada no existe o no está asignada a este profesor.");
        return await _calificaciones.GenerarEvaluacionesAsync(cursadaId);
    }

    public async Task<(PlanillaCalificacionesDto? Data, string? Error)> GuardarCalificacionesAsync(Guid usuarioId, GuardarCalificacionesDto dto)
    {
        if (!await EsCursadaPropiaAsync(usuarioId, dto.CursadaId)) return (null, "La cursada no existe o no está asignada a este profesor.");
        return await _calificaciones.GuardarAsync(dto);
    }

    private async Task<bool> EsCursadaPropiaAsync(Guid usuarioId, Guid cursadaId)
    {
        var profesorId = await GetProfesorIdAsync(usuarioId);
        return profesorId.HasValue && await _context.Cursadas.AnyAsync(c => c.Id == cursadaId && c.ProfesorId == profesorId.Value);
    }

    private static MiPerfilProfesorDto MapPerfil(backend.Entities.Profesor profesor) => new()
    {
        ProfesorId = profesor.Id,
        Nombre = profesor.Nombre,
        Apellido = profesor.Apellido,
        Dni = profesor.Dni,
        Email = profesor.Email,
        Telefono = profesor.Telefono,
        Titulo = profesor.Titulo
    };
}