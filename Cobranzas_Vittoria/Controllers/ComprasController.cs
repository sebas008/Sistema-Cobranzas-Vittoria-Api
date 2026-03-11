using Cobranzas_Vittoria.Dtos.Compras;
using Cobranzas_Vittoria.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cobranzas_Vittoria.Controllers
{
    [ApiController]
    [Route("api/compras/compras")]
    public class ComprasController : ControllerBase
    {
        private readonly ICompraService _service;
        public ComprasController(ICompraService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] bool? aceptada, [FromQuery] int? idProveedor)
            => Ok(await _service.ListAsync(aceptada, idProveedor));

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var res = await _service.GetAsync(id);
            return res is null ? NotFound() : Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> Registrar([FromBody] CompraCreateDto dto)
            => Ok(await _service.RegistrarAsync(dto));

        [HttpPost("{id:int}/aceptar")]
        public async Task<IActionResult> Aceptar(int id)
        {
            await _service.AceptarAsync(id);
            return Ok(new { Ok = true });
        }
    }
}
