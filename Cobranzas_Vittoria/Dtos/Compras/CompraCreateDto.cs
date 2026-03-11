namespace Cobranzas_Vittoria.Dtos.Compras;
public class CompraCreateDto
{
    public string NumeroCompra { get; set; } = string.Empty;
    public int IdOrdenCompra { get; set; }
    public int IdProveedor { get; set; }
    public DateTime FechaCompra { get; set; }
    public string? Observacion { get; set; }
    public List<CompraDetalleCreateDto> Items { get; set; } = new();
    public List<CompraDocumentoCreateDto> Documentos { get; set; } = new();
}
public class CompraDetalleCreateDto
{
    public int IdMaterial { get; set; }
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
}
public class CompraDocumentoCreateDto
{
    public string TipoDocumento { get; set; } = string.Empty;
    public string? NumeroDocumento { get; set; }
    public string? RutaArchivo { get; set; }
    public DateTime? FechaDocumento { get; set; }
    public decimal? Monto { get; set; }
    public string? Observacion { get; set; }
}
