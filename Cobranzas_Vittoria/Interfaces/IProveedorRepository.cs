namespace Cobranzas_Vittoria.Interfaces
{
    public interface IProveedorRepository
    {
        Task<IEnumerable<Cobranzas_Vittoria.Entities.Proveedor>> ListAsync(bool? activo, int? idEspecialidad);
        Task<(Cobranzas_Vittoria.Entities.Proveedor? proveedor, List<Cobranzas_Vittoria.Entities.ProveedorEspecialidad> especialidades)> GetAsync(int idProveedor);
        Task<int> UpsertAsync(Cobranzas_Vittoria.Dtos.Maestra.ProveedorUpsertDto dto);
        Task SetEspecialidadAsync(int idProveedor, int idEspecialidad, bool activo);
    }
}
