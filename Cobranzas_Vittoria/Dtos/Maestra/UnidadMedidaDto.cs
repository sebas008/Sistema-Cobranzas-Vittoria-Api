namespace Cobranzas_Vittoria.Dtos.Maestra
{
    public class UnidadMedidaDto
    {
        public int IdUnidadMedida { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }
}
