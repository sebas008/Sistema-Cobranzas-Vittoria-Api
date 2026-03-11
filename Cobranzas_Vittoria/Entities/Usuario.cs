namespace Cobranzas_Vittoria.Entities;
public class Usuario
{
    public int IdUsuario { get; set; }
    public string Nombres { get; set; } = string.Empty;
    public string? Apellidos { get; set; }
    public string? Correo { get; set; }
    public string UsuarioLogin { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool Activo { get; set; }
    public DateTime FechaCreacion { get; set; }
    public string? UsuarioCreacion { get; set; }
}
