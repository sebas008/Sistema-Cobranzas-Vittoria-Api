namespace Cobranzas_Vittoria.Interfaces
{
    public interface IRequerimientoService
    {
        Task<IEnumerable<Cobranzas_Vittoria.Entities.Requerimiento>> ListAsync(string? estado, int? idEspecialidad, int? idProyecto);
        Task<object?> GetAsync(int idRequerimiento);
        Task<int> CrearAsync(Cobranzas_Vittoria.Dtos.Compras.RequerimientoCreateDto dto);
        Task UpdateAsync(int idRequerimiento, Cobranzas_Vittoria.Dtos.Compras.RequerimientoUpdateDto dto);
        Task<bool> PuedeEditarAsync(int idRequerimiento);
        Task UpdateEstadoAsync(int idRequerimiento, string estado, string? observacion);
        Task ValidarAlmacenAsync(int idRequerimiento, int idUsuario, string resultado, string? observacion);
    }
}
