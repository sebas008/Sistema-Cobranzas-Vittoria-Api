namespace Cobranzas_Vittoria.Interfaces
{
    public interface IOrdenCompraService
    {
        Task<IEnumerable<Cobranzas_Vittoria.Entities.OrdenCompra>> ListAsync(string? estado, int? idProveedor, int? idProyecto);
        Task<object?> GetAsync(int idOrdenCompra);
        Task<object> CrearAsync(Cobranzas_Vittoria.Dtos.Compras.OrdenCompraCreateDto dto);
        Task UpdateAsync(int idOrdenCompra, Cobranzas_Vittoria.Dtos.Compras.OrdenCompraUpdateDto dto);
        Task UpdateEstadoAsync(int idOrdenCompra, string estadoNuevo, int? idUsuario, string? observacion);
    }
}
