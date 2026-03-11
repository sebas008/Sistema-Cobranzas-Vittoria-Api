namespace Cobranzas_Vittoria.Entities;
public class KardexMovimiento
{
    public int IdKardexMovimiento { get; set; }
    public int IdMaterial { get; set; }
    public string? Material { get; set; }
    public int IdEspecialidad { get; set; }
    public string? Especialidad { get; set; }
    public string TipoMovimiento { get; set; } = string.Empty;
    public DateTime FechaMovimiento { get; set; }
    public decimal CantidadEntrada { get; set; }
    public decimal CantidadSalida { get; set; }
    public decimal StockResultante { get; set; }
    public int? IdCompra { get; set; }
    public int? IdOrdenCompra { get; set; }
    public string? Observacion { get; set; }
    public DateTime? FechaIngresoAlmacen { get; set; }
    public DateTime? FechaSalidaAlmacen { get; set; }
    public DateTime FechaCreacion { get; set; }
}
