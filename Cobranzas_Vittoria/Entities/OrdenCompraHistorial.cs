namespace Cobranzas_Vittoria.Entities;
public class OrdenCompraHistorial
{
    public int IdOrdenCompraHistorial { get; set; }
    public int IdOrdenCompra { get; set; }
    public string? EstadoAnterior { get; set; }
    public string EstadoNuevo { get; set; } = string.Empty;
    public DateTime FechaCambio { get; set; }
    public int? IdUsuario { get; set; }
    public string? Usuario { get; set; }
    public string? Observacion { get; set; }
}
