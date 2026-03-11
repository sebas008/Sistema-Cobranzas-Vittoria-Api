using Cobranzas_Vittoria.Dtos.Seguridad;
using Cobranzas_Vittoria.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cobranzas_Vittoria.Controllers
{
    [ApiController]
    [Route("api/seguridad/usuarios")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _service;
        private readonly IUsuarioRolService _usuarioRolService;

        public UsuariosController(IUsuarioService service, IUsuarioRolService usuarioRolService)
        {
            _service = service;
            _usuarioRolService = usuarioRolService;
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] bool? activo) => Ok(await _service.ListAsync(activo));

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var res = await _service.GetAsync(id);
            return res is null ? NotFound() : Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert([FromBody] UsuarioUpsertDto dto)
        {
            var id = await _service.UpsertAsync(dto);
            return Ok(new { IdUsuario = id });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UsuarioUpsertDto dto)
        {
            dto.IdUsuario = id;
            var userId = await _service.UpsertAsync(dto);
            return Ok(new { IdUsuario = userId });
        }

        [HttpPost("{id:int}/roles")]
        public async Task<IActionResult> AssignRole(int id, [FromBody] UsuarioRolDto dto)
        {
            await _usuarioRolService.AssignAsync(id, dto.IdRol);
            return Ok(new { Ok = true });
        }

        [HttpDelete("{id:int}/roles/{idRol:int}")]
        public async Task<IActionResult> RemoveRole(int id, int idRol)
        {
            await _usuarioRolService.RemoveAsync(id, idRol);
            return Ok(new { Ok = true });
        }
    }
}
