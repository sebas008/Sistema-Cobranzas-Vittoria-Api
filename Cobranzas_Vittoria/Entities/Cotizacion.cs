namespace Cobranzas_Vittoria.Entities
{
    public class Cotizacion
    {
        public int IdCotizacion { get; set; }
        public int IdRequerimiento { get; set; }
        public int IdProveedor { get; set; }

        public string RazonSocial { get; set; } = string.Empty; // join Proveedor
        public DateTime Fecha { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}
