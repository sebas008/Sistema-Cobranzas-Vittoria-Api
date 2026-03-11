namespace Cobranzas_Vittoria.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Cobranzas_Vittoria.Entities.Usuario>> ListAsync(bool? activo);
        Task<(Cobranzas_Vittoria.Entities.Usuario? usuario, List<Cobranzas_Vittoria.Entities.UsuarioRol> roles)> GetAsync(int idUsuario);
        Task<int> UpsertAsync(Cobranzas_Vittoria.Dtos.Seguridad.UsuarioUpsertDto dto);
    }
}
