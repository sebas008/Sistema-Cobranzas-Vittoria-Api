using Cobranzas_Vittoria.Dtos.Cotizaciones;
using Cobranzas_Vittoria.Entities;
using Cobranzas_Vittoria.Interfaces;

namespace Cobranzas_Vittoria.Services
{
    public class CotizacionesService : ICotizacionesService
    {
        private readonly ICotizacionesRepository _repo;

        public CotizacionesService(ICotizacionesRepository repo)
        {
            _repo = repo;
        }

        public Task<int> CrearAsync(int idRequerimiento, CotizacionCreateDto dto)
            => _repo.CrearAsync(idRequerimiento, dto);

        public async Task<CotizacionGetResponseDto?> ObtenerAsync(int idCotizacion)
        {
            var (head, items) = await _repo.ObtenerAsync(idCotizacion);
            if (head == null) return null;

            return new CotizacionGetResponseDto
            {
                Cotizacion = head,
                Items = items
            };
        }
    }
}