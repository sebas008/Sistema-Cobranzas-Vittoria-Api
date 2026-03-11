namespace Cobranzas_Vittoria.Interfaces
{
    public interface IUsuarioRolService
    {
        Task AssignAsync(int idUsuario, int idRol);
        Task RemoveAsync(int idUsuario, int idRol);
    }
}
