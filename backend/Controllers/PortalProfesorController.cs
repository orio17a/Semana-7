using System.Security.Claims;
using backend.DTOs.Calificaciones;
using backend.DTOs.PortalProfesor;
using backend.Security;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PortalProfesorController : ControllerBase
{
    private readonly PortalProfesorService _service;
    public PortalProfesorController(PortalProfesorService service) => _service = service;

    private Guid UsuarioId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet("mis-datos")]
    [ScreenPermission(ScreenKeys.ProfesorDatos)]
    public async Task<IActionResult> MisDatos()
    {
        var (data, error) = await _service.GetMiPerfilAsync(UsuarioId);
        return error == null ? Ok(data) : BadRequest(error);
    }

    [HttpPut("mis-datos")]
    [ScreenPermission(ScreenKeys.ProfesorDatos)]
    public async Task<IActionResult> ActualizarMisDatos(ActualizarMiPerfilProfesorDto dto)
    {
        var (data, error) = await _service.ActualizarMiPerfilAsync(UsuarioId, dto);
        return error == null ? Ok(data) : BadRequest(error);
    }

    [HttpGet("cursadas")]
    [ScreenPermission(ScreenKeys.ProfesorCursadas)]
    public async Task<IActionResult> MisCursadas()
    {
        var (data, error) = await _service.GetMisCursadasAsync(UsuarioId);
        return error == null ? Ok(data) : BadRequest(error);
    }

    [HttpGet("cursadas/{cursadaId:guid}/calificaciones")]
    [ScreenPermission(ScreenKeys.ProfesorCalificaciones)]
    public async Task<IActionResult> Planilla(Guid cursadaId)
    {
        var (data, error) = await _service.GetPlanillaAsync(UsuarioId, cursadaId);
        return error == null ? Ok(data) : BadRequest(error);
    }

    [HttpPost("cursadas/{cursadaId:guid}/evaluaciones")]
    [ScreenPermission(ScreenKeys.ProfesorCalificaciones)]
    public async Task<IActionResult> GenerarEvaluaciones(Guid cursadaId)
    {
        var (data, error) = await _service.GenerarEvaluacionesAsync(UsuarioId, cursadaId);
        return error == null ? Ok(data) : BadRequest(error);
    }

    [HttpPost("calificaciones")]
    [ScreenPermission(ScreenKeys.ProfesorCalificaciones)]
    public async Task<IActionResult> GuardarCalificaciones(GuardarCalificacionesDto dto)
    {
        var (data, error) = await _service.GuardarCalificacionesAsync(UsuarioId, dto);
        return error == null ? Ok(data) : BadRequest(error);
    }
}