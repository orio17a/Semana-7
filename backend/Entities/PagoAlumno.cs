namespace backend.Entities;

public static class TipoPagoAlumno
{
    public const string CuotaMensual = "CUOTA_MENSUAL";
    public const string MatriculaAnual = "MATRICULA_ANUAL";
}

public class PagoAlumno
{
    public Guid Id { get; set; }
    public Guid AlumnoId { get; set; }
    public Alumno Alumno { get; set; } = null!;
    public string Concepto { get; set; } = string.Empty;
    public string Periodo { get; set; } = string.Empty;
    public DateTime FechaPago { get; set; }
    public decimal Importe { get; set; }
    public string? Observacion { get; set; }
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
}