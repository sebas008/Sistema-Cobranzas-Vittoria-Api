namespace Cobranzas_Vittoria.Interfaces
{
    public interface IKardexService
    {
        Task<IEnumerable<Cobranzas_Vittoria.Entities.KardexMovimiento>> ListAsync(int? idMaterial, int? idEspecialidad, DateTime? fechaDesde, DateTime? fechaHasta);
        Task<IEnumerable<Cobranzas_Vittoria.Entities.KardexResumenMaterial>> ResumenAsync(int? idMaterial, int? idEspecialidad);
    }
}
