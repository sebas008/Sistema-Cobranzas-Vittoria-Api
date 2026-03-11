namespace Cobranzas_Vittoria.Interfaces
{
    public interface IProyectoService
    {
        Task<IEnumerable<Cobranzas_Vittoria.Entities.Proyecto>> ListAsync(bool? activo);
    }
}
