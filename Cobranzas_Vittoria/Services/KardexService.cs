using Cobranzas_Vittoria.Entities;
using Cobranzas_Vittoria.Interfaces;

namespace Cobranzas_Vittoria.Services
{
    public class KardexService : IKardexService
    {
        private readonly IKardexRepository _repo;
        public KardexService(IKardexRepository repo) => _repo = repo;

        public Task<IEnumerable<KardexMovimiento>> ListAsync(int? idMaterial, int? idEspecialidad, DateTime? fechaDesde, DateTime? fechaHasta)
            => _repo.ListAsync(idMaterial, idEspecialidad, fechaDesde, fechaHasta);

        public Task<IEnumerable<KardexResumenMaterial>> ResumenAsync(int? idMaterial, int? idEspecialidad)
            => _repo.ResumenAsync(idMaterial, idEspecialidad);
    }
}
