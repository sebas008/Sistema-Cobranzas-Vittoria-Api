using Cobranzas_Vittoria.Interfaces;

namespace Cobranzas_Vittoria.Services
{
    public class UsuarioRolService : IUsuarioRolService
    {
        private readonly IUsuarioRolRepository _repo;
        public UsuarioRolService(IUsuarioRolRepository repo) => _repo = repo;
        public Task AssignAsync(int idUsuario, int idRol) => _repo.AssignAsync(idUsuario, idRol);
        public Task RemoveAsync(int idUsuario, int idRol) => _repo.RemoveAsync(idUsuario, idRol);
    }
}
