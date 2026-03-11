using Cobranzas_Vittoria.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cobranzas_Vittoria.Controllers
{
    [ApiController]
    [Route("api/almacen/kardex")]
    public class KardexController : ControllerBase
    {
        private readonly IKardexService _service;
        public KardexController(IKardexService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] int? idMaterial, [FromQuery] int? idEspecialidad, [FromQuery] DateTime? fechaDesde, [FromQuery] DateTime? fechaHasta)
            => Ok(await _service.ListAsync(idMaterial, idEspecialidad, fechaDesde, fechaHasta));

        [HttpGet("resumen")]
        public async Task<IActionResult> Resumen([FromQuery] int? idMaterial, [FromQuery] int? idEspecialidad)
            => Ok(await _service.ResumenAsync(idMaterial, idEspecialidad));
    }
}
