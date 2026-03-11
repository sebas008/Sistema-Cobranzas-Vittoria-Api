namespace Cobranzas_Vittoria.Dtos.Requerimientos
{
    public class RequerimientoCreateDto
    {
        public string Solicitante { get; set; } = string.Empty;
        public List<RequerimientoItemCreateDto> Items { get; set; } = new();
    }

    public class RequerimientoItemCreateDto
    {
        public int IdMaterial { get; set; }
        public decimal Cantidad { get; set; }
        public string? Observacion { get; set; }
    }
}
