namespace Cobranzas_Vittoria.Dtos.Compras;
public class OrdenCompraCreateDto
{
    public string NumeroOrdenCompra { get; set; } = string.Empty;
    public int IdRequerimiento { get; set; }
    public int IdProveedor { get; set; }
    public int IdProyecto { get; set; }
    public DateTime FechaOrdenCompra { get; set; }
    public string? Descripcion { get; set; }
    public int? IdUsuarioCreacion { get; set; }
    public string? RutaPdf { get; set; }
    public List<OrdenCompraDetalleCreateDto> Items { get; set; } = new();
}
public class OrdenCompraDetalleCreateDto
{
    public int IdMaterial { get; set; }
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
}
