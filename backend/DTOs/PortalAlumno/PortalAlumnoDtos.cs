using backend.DTOs.Calificaciones;

namespace backend.DTOs.PortalAlumno;

public class MiPerfilAlumnoDto
{
    public Guid AlumnoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Dni { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public DateTime FechaIngreso { get; set; }
    public string? CarreraNombre { get; set; }
}

public class MiCursadaDisponibleDto
{
    public Guid CursadaId { get; set; }
    public Guid MateriaId { get; set; }
    public string Materia { get; set; } = string.Empty;
    public string? Profesor { get; set; }
    public int CicloLectivo { get; set; }
    public string Periodo { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public decimal HorasSemanales { get; set; }
    public decimal HorasTotalesPlanificadas { get; set; }
    public decimal PorcentajePromocion { get; set; }
    public decimal PorcentajeRegularidad { get; set; }
    public bool Matriculado { get; set; }
    public Guid? MatriculaId { get; set; }
    public DateTime? FechaMatriculacion { get; set; }
    public bool PuedeDesmatricularse { get; set; }
    public List<MiHorarioCursadaDto> Horarios { get; set; } = new();
}

public class MiHorarioCursadaDto
{
    public int DiaSemana { get; set; }
    public string Dia { get; set; } = string.Empty;
    public string HoraInicio { get; set; } = string.Empty;
    public string HoraFin { get; set; } = string.Empty;
}

public class MiCalificacionAlumnoDto
{
    public Guid MatriculaId { get; set; }
    public string Materia { get; set; } = string.Empty;
    public string? Profesor { get; set; }
    public int CicloLectivo { get; set; }
    public string Periodo { get; set; } = string.Empty;
    public decimal? Promedio { get; set; }
    public string Condicion { get; set; } = "EN_CURSO";
    public List<EvaluacionDto> Evaluaciones { get; set; } = new();
    public List<NotaEvaluacionDto> Notas { get; set; } = new();
}

public class MiPagoAlumnoDto
{
    public Guid Id { get; set; }
    public string Concepto { get; set; } = string.Empty;
    public string Periodo { get; set; } = string.Empty;
    public DateTime FechaPago { get; set; }
    public decimal Importe { get; set; }
    public string? Observacion { get; set; }
}

public class MiEstadoPagosAlumnoDto
{
    public bool CuotaAlDia { get; set; }
    public string? UltimoPeriodoPagado { get; set; }
    public int AnioMatricula { get; set; }
    public bool MatriculaAnualPagada { get; set; }
    public List<MiPagoAlumnoDto> Pagos { get; set; } = new();
}
