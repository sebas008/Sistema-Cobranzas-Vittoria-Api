using Cobranzas_Vittoria.Dtos.Compras;
using Cobranzas_Vittoria.Entities;
using Cobranzas_Vittoria.Interfaces;

namespace Cobranzas_Vittoria.Services
{
    public class OrdenCompraService : IOrdenCompraService
    {
        private readonly IOrdenCompraRepository _repo;
        public OrdenCompraService(IOrdenCompraRepository repo) => _repo = repo;

        public Task<IEnumerable<OrdenCompra>> ListAsync(string? estado, int? idProveedor, int? idProyecto)
            => _repo.ListAsync(estado, idProveedor, idProyecto);

        public async Task<object?> GetAsync(int idOrdenCompra)
        {
            var (head, items, historial) = await _repo.GetAsync(idOrdenCompra);
            return head is null ? null : new { ordenCompra = head, items, historial };
        }

        public async Task<object> CrearAsync(OrdenCompraCreateDto dto)
        {
            var res = await _repo.CrearAsync(dto);
            return new { IdOrdenCompra = res.IdOrdenCompra, Total = res.Total };
        }

        public Task UpdateEstadoAsync(int idOrdenCompra, string estadoNuevo, int? idUsuario, string? observacion)
            => _repo.UpdateEstadoAsync(idOrdenCompra, estadoNuevo, idUsuario, observacion);
    }
}
