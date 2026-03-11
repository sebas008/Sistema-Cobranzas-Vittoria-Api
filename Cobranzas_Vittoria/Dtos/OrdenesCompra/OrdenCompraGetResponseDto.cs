using Cobranzas_Vittoria.Entities;

namespace Cobranzas_Vittoria.Dtos.OrdenesCompra
{
    public class OrdenCompraGetResponseDto
    {
        public OrdenCompra? OrdenCompra { get; set; }
        public List<OrdenCompraDetalle> Items { get; set; } = new();
    }
}
