using Cobranzas_Vittoria.Entities;

namespace Cobranzas_Vittoria.Dtos.Cotizaciones
{
    public class CotizacionGetResponseDto
    {
        public Cotizacion? Cotizacion { get; set; }
        public List<CotizacionDetalle> Items { get; set; } = new();
    }
}
