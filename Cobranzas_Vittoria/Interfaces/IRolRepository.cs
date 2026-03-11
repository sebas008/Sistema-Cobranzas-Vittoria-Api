namespace Cobranzas_Vittoria.Interfaces
{
    public interface IRolRepository
    {
        Task<IEnumerable<Cobranzas_Vittoria.Entities.Rol>> ListAsync();
    }
}
