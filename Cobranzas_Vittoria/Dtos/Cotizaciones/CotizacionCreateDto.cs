namespace Cobranzas_Vittoria.Dtos.Cotizaciones
{
    public class CotizacionCreateDto
    {
        public int IdProveedor { get; set; }
        public List<CotizacionItemCreateDto> Items { get; set; } = new();
    }

    public class CotizacionItemCreateDto
    {
        public int IdMaterial { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
