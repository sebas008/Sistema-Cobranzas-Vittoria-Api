namespace Cobranzas_Vittoria.Interfaces
{
    public interface IRequerimientoRepository
    {
        Task<IEnumerable<Cobranzas_Vittoria.Entities.Requerimiento>> ListAsync(string? estado, int? idEspecialidad, int? idProyecto);
        Task<(Cobranzas_Vittoria.Entities.Requerimiento? head, List<Cobranzas_Vittoria.Entities.RequerimientoDetalle> items, List<Cobranzas_Vittoria.Entities.RequerimientoValidacion> validaciones)> GetAsync(int idRequerimiento);
        Task<int> CrearAsync(Cobranzas_Vittoria.Dtos.Compras.RequerimientoCreateDto dto);
        Task UpdateEstadoAsync(int idRequerimiento, string estado, string? observacion);
        Task ValidarAlmacenAsync(int idRequerimiento, int idUsuario, string resultado, string? observacion);
    }
}
