namespace Cobranzas_Vittoria.Entities
{
    public class CotizacionDetalle
    {
        public int IdCotizacionDetalle { get; set; }
        public int IdMaterial { get; set; }

        public string Descripcion { get; set; } = string.Empty; // join Material

        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
