namespace Cobranzas_Vittoria.Interfaces
{
    public interface IProveedorService
    {
        Task<IEnumerable<Cobranzas_Vittoria.Entities.Proveedor>> ListAsync(bool? activo, int? idEspecialidad);
        Task<object?> GetAsync(int idProveedor);
        Task<int> UpsertAsync(Cobranzas_Vittoria.Dtos.Maestra.ProveedorUpsertDto dto);
        Task SetEspecialidadAsync(int idProveedor, int idEspecialidad, bool activo);
    }
}
