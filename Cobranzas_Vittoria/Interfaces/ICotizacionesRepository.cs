using Cobranzas_Vittoria.Dtos.Cotizaciones;
using Cobranzas_Vittoria.Entities;

namespace Cobranzas_Vittoria.Interfaces
{
    public interface ICotizacionesRepository
    {
        Task<int> CrearAsync(int idRequerimiento, CotizacionCreateDto dto);

        Task<(Cotizacion? cotizacion,
              List<CotizacionDetalle> items)>
              ObtenerAsync(int idCotizacion);
    }
}
