using Cobranzas_Vittoria.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cobranzas_Vittoria.Controllers
{
    [ApiController]
    [Route("api/maestra/proyectos")]
    public class ProyectosController : ControllerBase
    {
        private readonly IProyectoService _service;
        public ProyectosController(IProyectoService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] bool? activo) => Ok(await _service.ListAsync(activo));
    }
}
