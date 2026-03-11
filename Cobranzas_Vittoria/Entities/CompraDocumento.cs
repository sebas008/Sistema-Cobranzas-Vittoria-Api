namespace Cobranzas_Vittoria.Entities;
public class CompraDocumento
{
    public int IdCompraDocumento { get; set; }
    public int IdCompra { get; set; }
    public string TipoDocumento { get; set; } = string.Empty;
    public string? NumeroDocumento { get; set; }
    public string? RutaArchivo { get; set; }
    public DateTime? FechaDocumento { get; set; }
    public decimal? Monto { get; set; }
    public string? Observacion { get; set; }
}
