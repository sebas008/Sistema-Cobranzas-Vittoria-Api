namespace Cobranzas_Vittoria.Dtos.Almacen
{
    public class KardexSalidaCreateDto
    {
        public int IdCompra { get; set; }
        public int IdMaterial { get; set; }
        public int? IdEspecialidad { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public decimal CantidadSalida { get; set; }
        public string? Observacion { get; set; }
    }
}
