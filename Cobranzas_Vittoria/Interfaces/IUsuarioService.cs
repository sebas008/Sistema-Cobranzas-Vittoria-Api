namespace Cobranzas_Vittoria.Interfaces
{
    public interface IUsuarioService
    {
        Task<IEnumerable<Cobranzas_Vittoria.Entities.Usuario>> ListAsync(bool? activo);
        Task<object?> GetAsync(int idUsuario);
        Task<int> UpsertAsync(Cobranzas_Vittoria.Dtos.Seguridad.UsuarioUpsertDto dto);
    }
}
