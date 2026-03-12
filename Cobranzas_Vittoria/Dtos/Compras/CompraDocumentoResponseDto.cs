namespace Cobranzas_Vittoria.Dtos.Compras;

public class CompraDocumentoResponseDto
{
    public int IdCompraDocumento { get; set; }
    public int IdCompra { get; set; }
    public string NombreArchivo { get; set; } = string.Empty;
    public string RutaArchivo { get; set; } = string.Empty;
    public string? Extension { get; set; }
    public DateTime FechaCreacion { get; set; }
}
