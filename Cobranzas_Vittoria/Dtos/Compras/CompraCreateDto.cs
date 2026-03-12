namespace Cobranzas_Vittoria.Dtos.Compras;

public class CompraCreateDto
{
    public string NumeroCompra { get; set; } = string.Empty;
    public int IdOrdenCompra { get; set; }
    public int IdProveedor { get; set; }
    public DateTime FechaCompra { get; set; }
    public decimal MontoTotal { get; set; }
    public string? Observacion { get; set; }
}
