using Cobranzas_Vittoria.Dtos.Seguridad;
using Cobranzas_Vittoria.Entities;
using Cobranzas_Vittoria.Interfaces;

namespace Cobranzas_Vittoria.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repo;
        public UsuarioService(IUsuarioRepository repo) => _repo = repo;

        public Task<IEnumerable<Usuario>> ListAsync(bool? activo) => _repo.ListAsync(activo);

        public async Task<object?> GetAsync(int idUsuario)
        {
            var (usuario, roles) = await _repo.GetAsync(idUsuario);
            return usuario is null ? null : new { usuario, roles };
        }

        public Task<int> UpsertAsync(UsuarioUpsertDto dto) => _repo.UpsertAsync(dto);
    }
}
