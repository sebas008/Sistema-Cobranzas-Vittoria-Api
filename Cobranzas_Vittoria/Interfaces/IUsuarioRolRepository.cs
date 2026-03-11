namespace Cobranzas_Vittoria.Interfaces
{
    public interface IUsuarioRolRepository
    {
        Task AssignAsync(int idUsuario, int idRol);
        Task RemoveAsync(int idUsuario, int idRol);
    }
}
