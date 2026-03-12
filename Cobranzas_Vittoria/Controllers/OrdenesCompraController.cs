using Cobranzas_Vittoria.Dtos.Compras;
using Cobranzas_Vittoria.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cobranzas_Vittoria.Controllers
{
    [ApiController]
    [Route("api/compras/ordenes-compra")]
    public class OrdenesCompraController : ControllerBase
    {
        private readonly IOrdenCompraService _service;

        public OrdenesCompraController(IOrdenCompraService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] string? estado, [FromQuery] int? idProveedor, [FromQuery] int? idProyecto)
            => Ok(await _service.ListAsync(estado, idProveedor, idProyecto));

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var res = await _service.GetAsync(id);
            return res is null ? NotFound() : Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] OrdenCompraCreateDto dto)
            => Ok(await _service.CrearAsync(dto));

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrdenCompraUpdateDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return Ok(new { ok = true });
        }

        [HttpPatch("{id:int}/estado")]
        public async Task<IActionResult> UpdateEstado(int id, [FromBody] OrdenCompraEstadoDto dto)
        {
            await _service.UpdateEstadoAsync(id, dto.EstadoNuevo, dto.IdUsuario, dto.Observacion);
            return Ok(new { ok = true });
        }
    }
}
