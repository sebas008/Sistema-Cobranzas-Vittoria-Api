using Cobranzas_Vittoria.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cobranzas_Vittoria.Controllers
{
    [ApiController]
    [Route("api/maestra/unidades-medida")]
    public class UnidadMedidaController : ControllerBase
    {
        private readonly IUnidadMedidaService _service;

        public UnidadMedidaController(IUnidadMedidaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] bool? activo)
        {
            var result = await _service.ListAsync(activo);
            return Ok(result);
        }
    }
}