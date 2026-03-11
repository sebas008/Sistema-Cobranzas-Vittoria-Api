namespace Cobranzas_Vittoria.Interfaces
{
    public interface IEspecialidadService
    {
        Task<IEnumerable<Cobranzas_Vittoria.Entities.Especialidad>> ListAsync(bool? activo);
        Task<int> UpsertAsync(Cobranzas_Vittoria.Dtos.Maestra.EspecialidadUpsertDto dto);
    }
}
