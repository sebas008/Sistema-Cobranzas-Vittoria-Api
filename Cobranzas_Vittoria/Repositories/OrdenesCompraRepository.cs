using Chavez_Logistica.Interfaces;
using Cobranzas_Vittoria.Entities;
using Cobranzas_Vittoria.Interfaces;
using Dapper;
using System.Data;

namespace Cobranzas_Vittoria.Repositories
{
    public class OrdenesCompraRepository : IOrdenesCompraRepository
    {
        private readonly IDbConnectionFactory _factory;

        public OrdenesCompraRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<OrdenCompraGenerada> GenerarAsync(int idRequerimiento, int idProveedor)
        {
            using var db = _factory.CreateConnection();

            return await db.QueryFirstAsync<OrdenCompraGenerada>(
                "dbo.usp_OrdenCompra_Generar",
                new { IdRequerimiento = idRequerimiento, IdProveedor = idProveedor },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<(OrdenCompra? ordenCompra, List<OrdenCompraDetalle> items)> ObtenerAsync(int idOrdenCompra)
        {
            using var db = _factory.CreateConnection();

            using var grid = await db.QueryMultipleAsync(
                "dbo.usp_OrdenCompra_Get",
                new { IdOrdenCompra = idOrdenCompra },
                commandType: CommandType.StoredProcedure
            );

            var head = await grid.ReadFirstOrDefaultAsync<OrdenCompra>();
            var items = (await grid.ReadAsync<OrdenCompraDetalle>()).AsList();

            return (head, items);
        }
    }
}
