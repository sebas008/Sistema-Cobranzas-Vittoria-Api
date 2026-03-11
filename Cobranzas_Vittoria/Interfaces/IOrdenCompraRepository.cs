namespace Cobranzas_Vittoria.Interfaces
{
    public interface IOrdenCompraRepository
    {
        Task<IEnumerable<Cobranzas_Vittoria.Entities.OrdenCompra>> ListAsync(string? estado, int? idProveedor, int? idProyecto);
        Task<(Cobranzas_Vittoria.Entities.OrdenCompra? head, List<Cobranzas_Vittoria.Entities.OrdenCompraDetalle> items, List<Cobranzas_Vittoria.Entities.OrdenCompraHistorial> historial)> GetAsync(int idOrdenCompra);
        Task<(int IdOrdenCompra, decimal Total)> CrearAsync(Cobranzas_Vittoria.Dtos.Compras.OrdenCompraCreateDto dto);
        Task UpdateEstadoAsync(int idOrdenCompra, string estadoNuevo, int? idUsuario, string? observacion);
    }
}
