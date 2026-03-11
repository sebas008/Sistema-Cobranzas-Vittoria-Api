namespace Cobranzas_Vittoria.Entities;
public class CompraDetalle
{
    public int IdCompraDetalle { get; set; }
    public int IdCompra { get; set; }
    public int IdMaterial { get; set; }
    public string? Material { get; set; }
    public string? UnidadMedida { get; set; }
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }
}
