namespace Cobranzas_Vittoria.Dtos.Compras;
public class RequerimientoValidacionDto
{
    public int IdUsuario { get; set; }
    public string Resultado { get; set; } = string.Empty;
    public string? Observacion { get; set; }
}
