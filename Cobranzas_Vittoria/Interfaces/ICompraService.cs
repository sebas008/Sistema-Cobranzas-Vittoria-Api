namespace Cobranzas_Vittoria.Interfaces
{
    public interface ICompraService
    {
        Task<IEnumerable<Cobranzas_Vittoria.Entities.Compra>> ListAsync(bool? aceptada, int? idProveedor);
        Task<object?> GetAsync(int idCompra);
        Task<object> RegistrarAsync(Cobranzas_Vittoria.Dtos.Compras.CompraCreateDto dto);
        Task AceptarAsync(int idCompra);
    }
}
