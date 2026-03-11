namespace Cobranzas_Vittoria.Entities;
public class ProveedorEspecialidad
{
    public int IdProveedorEspecialidad { get; set; }
    public int IdProveedor { get; set; }
    public int IdEspecialidad { get; set; }
    public string? Especialidad { get; set; }
    public bool Activo { get; set; }
}
