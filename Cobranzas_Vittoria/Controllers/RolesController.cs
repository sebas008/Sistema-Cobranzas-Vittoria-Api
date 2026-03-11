using Cobranzas_Vittoria.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cobranzas_Vittoria.Controllers
{
    [ApiController]
    [Route("api/seguridad/roles")]
    public class RolesController : ControllerBase
    {
        private readonly IRolService _service;
        public RolesController(IRolService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> List() => Ok(await _service.ListAsync());
    }
}
