using Cobranzas_Vittoria.Dtos.Maestra;
using Cobranzas_Vittoria.Entities;

namespace Cobranzas_Vittoria.Interfaces
{
    public interface IProyectoService
    {
        Task<IEnumerable<Proyecto>> ListAsync(bool? activo);
        Task<int> UpsertAsync(ProyectoUpsertDto dto);
    }
}