using Cobranzas_Vittoria.Dtos.Maestra;
using Cobranzas_Vittoria.Entities;
using Cobranzas_Vittoria.Interfaces;

namespace Cobranzas_Vittoria.Services
{
    public class EspecialidadService : IEspecialidadService
    {
        private readonly IEspecialidadRepository _repo;
        public EspecialidadService(IEspecialidadRepository repo) => _repo = repo;
        public Task<IEnumerable<Especialidad>> ListAsync(bool? activo) => _repo.ListAsync(activo);
        public Task<int> UpsertAsync(EspecialidadUpsertDto dto) => _repo.UpsertAsync(dto);
    }
}
