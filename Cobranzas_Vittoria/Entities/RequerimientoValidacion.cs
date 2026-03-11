namespace Cobranzas_Vittoria.Entities;
public class RequerimientoValidacion
{
    public int IdRequerimientoValidacion { get; set; }
    public int IdRequerimiento { get; set; }
    public int IdUsuario { get; set; }
    public string? Usuario { get; set; }
    public DateTime FechaValidacion { get; set; }
    public string Resultado { get; set; } = string.Empty;
    public string? Observacion { get; set; }
}
