using Cobranzas_Vittoria.Dtos.Maestra;
using Cobranzas_Vittoria.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cobranzas_Vittoria.Controllers
{
    [ApiController]
    [Route("api/maestra/proyectos")]
    public class ProyectosController : ControllerBase
    {
        private readonly IProyectoService _service;

        public ProyectosController(IProyectoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] bool? activo)
        {
            var result = await _service.ListAsync(activo);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProyectoUpsertDto dto)
        {
            var id = await _service.UpsertAsync(dto);
            return Ok(new { idProyecto = id });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProyectoUpsertDto dto)
        {
            dto.IdProyecto = id;
            var proyectoId = await _service.UpsertAsync(dto);
            return Ok(new { idProyecto = proyectoId });
        }
    }
}