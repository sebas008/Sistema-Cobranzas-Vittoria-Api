namespace Cobranzas_Vittoria.Interfaces
{
    public interface IRolService
    {
        Task<IEnumerable<Cobranzas_Vittoria.Entities.Rol>> ListAsync();
    }
}
