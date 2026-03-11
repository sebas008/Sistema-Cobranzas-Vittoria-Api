namespace Cobranzas_Vittoria.Interfaces
{
    public interface ICompraRepository
    {
        Task<IEnumerable<Cobranzas_Vittoria.Entities.Compra>> ListAsync(bool? aceptada, int? idProveedor);
        Task<(Cobranzas_Vittoria.Entities.Compra? head, List<Cobranzas_Vittoria.Entities.CompraDetalle> items, List<Cobranzas_Vittoria.Entities.CompraDocumento> docs)> GetAsync(int idCompra);
        Task<(int IdCompra, decimal MontoTotal)> RegistrarAsync(Cobranzas_Vittoria.Dtos.Compras.CompraCreateDto dto);
        Task AceptarAsync(int idCompra);
    }
}
