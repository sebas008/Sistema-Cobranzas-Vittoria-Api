using Cobranzas_Vittoria.Dtos.Cotizaciones;

namespace Cobranzas_Vittoria.Interfaces
{
    public interface ICotizacionesService
    {
        Task<int> CrearAsync(int idRequerimiento, CotizacionCreateDto dto);
        Task<CotizacionGetResponseDto?> ObtenerAsync(int idCotizacion);
    }
}
