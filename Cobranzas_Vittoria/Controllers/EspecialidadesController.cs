using Cobranzas_Vittoria.Dtos.Maestra;
using Cobranzas_Vittoria.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cobranzas_Vittoria.Controllers
{
    [ApiController]
    [Route("api/maestra/especialidades")]
    public class EspecialidadesController : ControllerBase
    {
        private readonly IEspecialidadService _service;
        public EspecialidadesController(IEspecialidadService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] bool? activo) => Ok(await _service.ListAsync(activo));

        [HttpPost]
        public async Task<IActionResult> Upsert([FromBody] EspecialidadUpsertDto dto)
        {
            var id = await _service.UpsertAsync(dto);
            return Ok(new { IdEspecialidad = id });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] EspecialidadUpsertDto dto)
        {
            dto.IdEspecialidad = id;
            var espId = await _service.UpsertAsync(dto);
            return Ok(new { IdEspecialidad = espId });
        }
    }
}
