namespace Cobranzas_Vittoria.Interfaces
{
    public interface ICompraRepository
    {
        Task<IEnumerable<dynamic>> ListAsync(bool? aceptada, int? idProveedor);
        Task<object?> GetAsync(int idCompra);
        Task<(int IdCompra, decimal MontoTotal)> CrearAsync(Cobranzas_Vittoria.Dtos.Compras.CompraCreateDto dto);
        Task<IEnumerable<dynamic>> ListPendientesDesdeOcAsync();
        Task<IEnumerable<dynamic>> GetDocumentosAsync(int idCompra);
        Task SaveDocumentosAsync(int idCompra, IEnumerable<(string NombreArchivo, string RutaArchivo, string? Extension)> docs);
    }
}
