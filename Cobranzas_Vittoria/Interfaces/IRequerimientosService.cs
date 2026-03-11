using Cobranzas_Vittoria.Dtos.Requerimientos;

namespace Cobranzas_Vittoria.Interfaces
{
    public interface IRequerimientosService
    {
        Task<int> CrearAsync(RequerimientoCreateDto dto);
        Task<RequerimientoGetResponseDto?> ObtenerAsync(int idRequerimiento);
    }
}
