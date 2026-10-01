using System.Security.Claims;
using backend.DTOs.Usuarios;
using backend.Security;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;
[ApiController, Route("api/[controller]"), Authorize, ScreenPermission(ScreenKeys.Usuarios)]
public class UsuariosController : ControllerBase
{
    private readonly UsuarioService _service; public UsuariosController(UsuarioService service) { _service = service; }
    [HttpGet] public async Task<ActionResult<List<UsuarioDto>>> GetAll() => Ok(await _service.GetAllAsync());
    [HttpGet("roles")] public async Task<ActionResult<List<UsuarioRolOptionDto>>> GetRoles() => Ok(await _service.GetRolesAsync());
    [HttpGet("alumnos")] public async Task<ActionResult<List<UsuarioAlumnoOptionDto>>> GetAlumnos([FromQuery] Guid? incluir = null) => Ok(await _service.GetAlumnosAsync(incluir));
    [HttpGet("profesores")] public async Task<ActionResult<List<UsuarioProfesorOptionDto>>> GetProfesores([FromQuery] Guid? incluir = null) => Ok(await _service.GetProfesoresAsync(incluir));
    [HttpPost] public async Task<IActionResult> Create(CreateUsuarioDto dto) { var (u,e)=await _service.CreateAsync(dto); return e==null?Ok(u):BadRequest(e); }
    [HttpPut("{id:guid}")] public async Task<IActionResult> Update(Guid id, UpdateUsuarioDto dto) { var current=Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!); var (u,e)=await _service.UpdateAsync(id,dto,current); return e==null?Ok(u):BadRequest(e); }
}
