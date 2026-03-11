using Cobranzas_Vittoria.Dtos.OrdenesCompra;

namespace Cobranzas_Vittoria.Interfaces
{
    public interface IOrdenesCompraService
    {
        Task<OrdenCompraGenerarResponseDto> GenerarAsync(OrdenCompraGenerarDto dto);
        Task<OrdenCompraGetResponseDto?> ObtenerAsync(int idOrdenCompra);
    }
}
