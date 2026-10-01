namespace backend.DTOs.PortalProfesor;

public class MiPerfilProfesorDto
{
    public Guid ProfesorId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Dni { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public string? Titulo { get; set; }
}

public class ActualizarMiPerfilProfesorDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public string? Titulo { get; set; }
}