using backend.Data;
using backend.DTOs.Cuotas;
using backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

public class CuotaService
{
    private readonly AppDbContext _context;
    public CuotaService(AppDbContext context) => _context = context;

    public async Task<List<EstadoCuotaDto>> GetAllAsync()
    {
        return await _context.Alumnos.AsNoTracking()
            .OrderBy(x => x.Apellido).ThenBy(x => x.Nombre)
            .Select(a => new EstadoCuotaDto
            {
                AlumnoId = a.Id,
                AlumnoNombre = a.Apellido + ", " + a.Nombre,
                AlumnoDni = a.Dni,
                Registrado = _context.EstadosCuotaAlumno.Any(c => c.AlumnoId == a.Id),
                AlDia = _context.EstadosCuotaAlumno.Where(c => c.AlumnoId == a.Id).Select(c => c.AlDia).FirstOrDefault(),
                UltimoPeriodoPagado = _context.EstadosCuotaAlumno.Where(c => c.AlumnoId == a.Id).Select(c => c.UltimoPeriodoPagado).FirstOrDefault(),
                FechaUltimoPago = _context.EstadosCuotaAlumno.Where(c => c.AlumnoId == a.Id).Select(c => c.FechaUltimoPago).FirstOrDefault(),
                ImporteUltimoPago = _context.EstadosCuotaAlumno.Where(c => c.AlumnoId == a.Id).Select(c => c.ImporteUltimoPago).FirstOrDefault(),
                Observacion = _context.EstadosCuotaAlumno.Where(c => c.AlumnoId == a.Id).Select(c => c.Observacion).FirstOrDefault(),
                FechaActualizacion = _context.EstadosCuotaAlumno.Where(c => c.AlumnoId == a.Id).Select(c => (DateTime?)c.FechaActualizacion).FirstOrDefault(),
                AnioMatricula = DateTime.UtcNow.Year,
                MatriculaAnualPagada = _context.PagosAlumnos.Any(p => p.AlumnoId == a.Id && p.Concepto == TipoPagoAlumno.MatriculaAnual && p.Periodo == DateTime.UtcNow.Year.ToString())
            }).ToListAsync();
    }

    public async Task<(EstadoCuotaDto? data, string? error)> GuardarAsync(Guid alumnoId, GuardarEstadoCuotaDto dto)
    {
        if (!await _context.Alumnos.AnyAsync(x => x.Id == alumnoId)) return (null, "El alumno no existe.");
        if (dto.ImporteUltimoPago < 0) return (null, "El importe no puede ser negativo.");
        var fechaPago = dto.FechaUltimoPago.HasValue ? DateTime.SpecifyKind(dto.FechaUltimoPago.Value.Date, DateTimeKind.Utc) : (DateTime?)null;

        var existente = await _context.EstadosCuotaAlumno.FirstOrDefaultAsync(x => x.AlumnoId == alumnoId);
        if (existente == null)
        {
            existente = new EstadoCuotaAlumno { Id = Guid.NewGuid(), AlumnoId = alumnoId };
            _context.EstadosCuotaAlumno.Add(existente);
        }
        existente.AlDia = dto.AlDia;
        existente.UltimoPeriodoPagado = string.IsNullOrWhiteSpace(dto.UltimoPeriodoPagado) ? null : dto.UltimoPeriodoPagado.Trim();
        existente.FechaUltimoPago = fechaPago;
        existente.ImporteUltimoPago = dto.ImporteUltimoPago;
        existente.Observacion = string.IsNullOrWhiteSpace(dto.Observacion) ? null : dto.Observacion.Trim();
        existente.FechaActualizacion = DateTime.UtcNow;

        var periodo = NormalizarPeriodoMensual(existente.UltimoPeriodoPagado);
        if (periodo != null && fechaPago.HasValue && dto.ImporteUltimoPago.HasValue)
            await GuardarPagoAsync(alumnoId, TipoPagoAlumno.CuotaMensual, periodo, fechaPago.Value, dto.ImporteUltimoPago.Value, existente.Observacion);

        await _context.SaveChangesAsync();

        var all = await GetAllAsync();
        return (all.First(x => x.AlumnoId == alumnoId), null);
    }

    public async Task<(EstadoCuotaDto? data, string? error)> RegistrarPagoAsync(RegistrarPagoAlumnoDto dto)
    {
        if (!await _context.Alumnos.AnyAsync(x => x.Id == dto.AlumnoId && x.Activo)) return (null, "El alumno no existe o está inactivo.");
        if (dto.Importe < 0) return (null, "El importe no puede ser negativo.");
        var concepto = dto.Concepto.Trim().ToUpperInvariant();
        var periodo = dto.Periodo.Trim();
        if (concepto == TipoPagoAlumno.CuotaMensual)
        {
            periodo = NormalizarPeriodoMensual(periodo) ?? string.Empty;
            if (periodo.Length == 0) return (null, "El período de cuota debe tener formato AAAA-MM.");
        }
        else if (concepto == TipoPagoAlumno.MatriculaAnual)
        {
            if (!int.TryParse(periodo, out var anio) || anio < 2000 || anio > 2100) return (null, "El período de matrícula debe ser un año válido.");
        }
        else return (null, "El concepto de pago no es válido.");

        var fecha = DateTime.SpecifyKind(dto.FechaPago.Date, DateTimeKind.Utc);
        await GuardarPagoAsync(dto.AlumnoId, concepto, periodo, fecha, dto.Importe, dto.Observacion);
        await _context.SaveChangesAsync();
        return ((await GetAllAsync()).First(x => x.AlumnoId == dto.AlumnoId), null);
    }

    private async Task GuardarPagoAsync(Guid alumnoId, string concepto, string periodo, DateTime fecha, decimal importe, string? observacion)
    {
        var pago = await _context.PagosAlumnos.FirstOrDefaultAsync(p => p.AlumnoId == alumnoId && p.Concepto == concepto && p.Periodo == periodo);
        if (pago == null)
        {
            pago = new PagoAlumno { Id = Guid.NewGuid(), AlumnoId = alumnoId, Concepto = concepto, Periodo = periodo };
            _context.PagosAlumnos.Add(pago);
        }
        pago.FechaPago = fecha;
        pago.Importe = importe;
        pago.Observacion = string.IsNullOrWhiteSpace(observacion) ? null : observacion.Trim();
        pago.FechaRegistro = DateTime.UtcNow;
    }

    private static string? NormalizarPeriodoMensual(string? periodo)
    {
        if (string.IsNullOrWhiteSpace(periodo)) return null;
        var value = periodo.Trim();
        if (DateTime.TryParseExact(value, "yyyy-MM", null, System.Globalization.DateTimeStyles.None, out var yearMonth))
            return yearMonth.ToString("yyyy-MM");
        if (DateTime.TryParseExact(value, "MM/yyyy", null, System.Globalization.DateTimeStyles.None, out yearMonth))
            return yearMonth.ToString("yyyy-MM");
        return null;
    }
}
