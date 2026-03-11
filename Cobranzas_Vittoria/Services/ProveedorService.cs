using Cobranzas_Vittoria.Dtos.Maestra;
using Cobranzas_Vittoria.Entities;
using Cobranzas_Vittoria.Interfaces;

namespace Cobranzas_Vittoria.Services
{
    public class ProveedorService : IProveedorService
    {
        private readonly IProveedorRepository _repo;
        public ProveedorService(IProveedorRepository repo) => _repo = repo;

        public Task<IEnumerable<Proveedor>> ListAsync(bool? activo, int? idEspecialidad) => _repo.ListAsync(activo, idEspecialidad);

        public async Task<object?> GetAsync(int idProveedor)
        {
            var (proveedor, especialidades) = await _repo.GetAsync(idProveedor);
            return proveedor is null ? null : new { proveedor, especialidades };
        }

        public Task<int> UpsertAsync(ProveedorUpsertDto dto) => _repo.UpsertAsync(dto);
        public Task SetEspecialidadAsync(int idProveedor, int idEspecialidad, bool activo) => _repo.SetEspecialidadAsync(idProveedor, idEspecialidad, activo);
    }
}
