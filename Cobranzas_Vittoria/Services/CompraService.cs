using Cobranzas_Vittoria.Dtos.Compras;
using Cobranzas_Vittoria.Entities;
using Cobranzas_Vittoria.Interfaces;

namespace Cobranzas_Vittoria.Services
{
    public class CompraService : ICompraService
    {
        private readonly ICompraRepository _repo;
        public CompraService(ICompraRepository repo) => _repo = repo;

        public Task<IEnumerable<Compra>> ListAsync(bool? aceptada, int? idProveedor)
            => _repo.ListAsync(aceptada, idProveedor);

        public async Task<object?> GetAsync(int idCompra)
        {
            var (head, items, docs) = await _repo.GetAsync(idCompra);
            return head is null ? null : new { compra = head, items, documentos = docs };
        }

        public async Task<object> RegistrarAsync(CompraCreateDto dto)
        {
            var res = await _repo.RegistrarAsync(dto);
            return new { IdCompra = res.IdCompra, MontoTotal = res.MontoTotal };
        }

        public Task AceptarAsync(int idCompra) => _repo.AceptarAsync(idCompra);
    }
}
