using Cobranzas_Vittoria.Dtos.Maestra;
using Cobranzas_Vittoria.Entities;
using Cobranzas_Vittoria.Interfaces;

namespace Cobranzas_Vittoria.Services
{
    public class ProyectoService : IProyectoService
    {
        private readonly IProyectoRepository _repo;

        public ProyectoService(IProyectoRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<Proyecto>> ListAsync(bool? activo)
            => _repo.ListAsync(activo);

        public Task<int> UpsertAsync(ProyectoUpsertDto dto)
            => _repo.UpsertAsync(dto);
    }
}