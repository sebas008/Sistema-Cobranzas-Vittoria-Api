namespace Cobranzas_Vittoria.Dtos.Maestra;
public class EspecialidadUpsertDto
{
    public int? IdEspecialidad { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool Activo { get; set; } = true;
}
