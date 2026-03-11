using Cobranzas_Vittoria.Entities;

namespace Cobranzas_Vittoria.Interfaces
{
    public interface IOrdenesCompraRepository
    {
        Task<OrdenCompraGenerada> GenerarAsync(int idRequerimiento, int idProveedor);

        Task<(OrdenCompra? ordenCompra,
              List<OrdenCompraDetalle> items)>
              ObtenerAsync(int idOrdenCompra);
    }
}
