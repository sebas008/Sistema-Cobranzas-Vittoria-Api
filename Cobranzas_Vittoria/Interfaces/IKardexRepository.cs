namespace Cobranzas_Vittoria.Interfaces
{
    public interface IKardexRepository
    {
        Task<IEnumerable<dynamic>> ListMovimientosAsync(int? idMaterial, int? idEspecialidad, string? fechaDesde, string? fechaHasta);
    }
}
