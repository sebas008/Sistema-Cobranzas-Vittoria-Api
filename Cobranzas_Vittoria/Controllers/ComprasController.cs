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
        private readonly IWebHostEnvironment _env;

        public ComprasController(ICompraService service, IWebHostEnvironment env)
        {
            _service = service;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] bool? aceptada, [FromQuery] int? idProveedor)
            => Ok(await _service.ListAsync(aceptada, idProveedor));

        [HttpGet("pendientes-desde-oc")]
        public async Task<IActionResult> PendientesDesdeOc()
            => Ok(await _service.ListPendientesDesdeOcAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var res = await _service.GetAsync(id);
            return res is null ? NotFound() : Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CompraCreateDto dto)
            => Ok(await _service.CrearAsync(dto));

        [HttpGet("{id:int}/documentos")]
        public async Task<IActionResult> GetDocumentos(int id)
            => Ok(await _service.GetDocumentosAsync(id));

        [HttpPost("{id:int}/documentos")]
        [RequestSizeLimit(50_000_000)]
        public async Task<IActionResult> UploadDocumentos(int id, [FromForm] List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
                return BadRequest(new { message = "Debes adjuntar uno o más archivos." });

            var root = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "uploads", "compras", id.ToString());
            Directory.CreateDirectory(root);

            var docs = new List<(string NombreArchivo, string RutaArchivo, string? Extension)>();

            foreach (var file in files)
            {
                var ext = Path.GetExtension(file.FileName);
                if (!string.Equals(ext, ".pdf", StringComparison.OrdinalIgnoreCase))
                    return BadRequest(new { message = "Solo se permiten archivos PDF." });

                var safeName = $"{Guid.NewGuid():N}_{Path.GetFileName(file.FileName)}";
                var fullPath = Path.Combine(root, safeName);

                using var stream = System.IO.File.Create(fullPath);
                await file.CopyToAsync(stream);

                var relative = Path.Combine("uploads", "compras", id.ToString(), safeName).Replace("\\", "/");
                docs.Add((file.FileName, relative, ext));
            }

            await _service.SaveDocumentosAsync(id, docs);
            return Ok(new { ok = true });
        }
    }
}
