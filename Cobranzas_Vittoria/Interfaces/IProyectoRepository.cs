namespace Cobranzas_Vittoria.Interfaces
{
    public interface IProyectoRepository
    {
        Task<IEnumerable<Cobranzas_Vittoria.Entities.Proyecto>> ListAsync(bool? activo);
    }
}
