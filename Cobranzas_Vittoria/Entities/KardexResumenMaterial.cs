namespace Cobranzas_Vittoria.Entities;
public class KardexResumenMaterial
{
    public int IdMaterial { get; set; }
    public string Material { get; set; } = string.Empty;
    public int IdEspecialidad { get; set; }
    public string Especialidad { get; set; } = string.Empty;
    public string UnidadMedida { get; set; } = string.Empty;
    public decimal TotalEntradas { get; set; }
    public decimal TotalSalidas { get; set; }
    public decimal StockActual { get; set; }
}
