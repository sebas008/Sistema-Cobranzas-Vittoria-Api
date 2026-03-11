using Cobranzas_Vittoria.Dtos.Compras;
using Cobranzas_Vittoria.Entities;
using Cobranzas_Vittoria.Interfaces;

namespace Cobranzas_Vittoria.Services
{
    public class RequerimientoService : IRequerimientoService
    {
        private readonly IRequerimientoRepository _repo;
        public RequerimientoService(IRequerimientoRepository repo) => _repo = repo;

        public Task<IEnumerable<Requerimiento>> ListAsync(string? estado, int? idEspecialidad, int? idProyecto)
            => _repo.ListAsync(estado, idEspecialidad, idProyecto);

        public async Task<object?> GetAsync(int idRequerimiento)
        {
            var (head, items, validaciones) = await _repo.GetAsync(idRequerimiento);
            return head is null ? null : new { requerimiento = head, items, validaciones };
        }

        public Task<int> CrearAsync(RequerimientoCreateDto dto) => _repo.CrearAsync(dto);
        public Task UpdateEstadoAsync(int idRequerimiento, string estado, string? observacion) => _repo.UpdateEstadoAsync(idRequerimiento, estado, observacion);
        public Task ValidarAlmacenAsync(int idRequerimiento, int idUsuario, string resultado, string? observacion) => _repo.ValidarAlmacenAsync(idRequerimiento, idUsuario, resultado, observacion);
    }
}
