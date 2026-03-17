using Cobranzas_Vittoria.Data;
using Cobranzas_Vittoria.Interfaces;
using Dapper;

namespace Cobranzas_Vittoria.Repositories
{
    public class KardexRepository : RepositoryBase, IKardexRepository
    {
        public KardexRepository(IDbConnectionFactory factory) : base(factory) { }

        public async Task<IEnumerable<dynamic>> ListMovimientosAsync(int? idMaterial, int? idEspecialidad, string? fechaDesde, string? fechaHasta)
        {
            using var db = Open();

            var hasCompraDetalle = await db.ExecuteScalarAsync<int>(@"
SELECT COUNT(*)
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = 'compras'
  AND TABLE_NAME = 'CompraDetalle'") > 0;

            var detalleSource = hasCompraDetalle
                ? @"FROM compras.Compra c
INNER JOIN compras.CompraDetalle d ON d.IdCompra = c.IdCompra
INNER JOIN maestra.Material m ON m.IdMaterial = d.IdMaterial
LEFT JOIN maestra.Especialidad e ON e.IdEspecialidad = m.IdEspecialidad"
                : @"FROM compras.Compra c
INNER JOIN compras.OrdenCompra oc ON oc.IdOrdenCompra = c.IdOrdenCompra
INNER JOIN compras.OrdenCompraDetalle d ON d.IdOrdenCompra = oc.IdOrdenCompra
INNER JOIN maestra.Material m ON m.IdMaterial = d.IdMaterial
LEFT JOIN maestra.Especialidad e ON e.IdEspecialidad = m.IdEspecialidad";

            var idDetalleExpr = hasCompraDetalle ? "d.IdCompraDetalle" : "d.IdOrdenCompraDetalle";
            var precioExpr = hasCompraDetalle ? "ISNULL(d.PrecioUnitario, 0)" : "ISNULL(d.PrecioUnitario, 0)";
            var subtotalExpr = hasCompraDetalle ? "ISNULL(d.Subtotal, d.Cantidad * ISNULL(d.PrecioUnitario, 0))" : "d.Cantidad * ISNULL(d.PrecioUnitario, 0)";

            var sql = $@"
WITH KardexBase AS (
    SELECT
        CAST(c.IdCompra AS BIGINT) * 100000 + CAST({idDetalleExpr} AS BIGINT) AS IdKardexMovimiento,
        m.IdMaterial,
        m.Descripcion AS Material,
        e.IdEspecialidad,
        e.Nombre AS Especialidad,
        c.FechaCompra AS FechaMovimiento,
        CAST(d.Cantidad AS DECIMAL(18,2)) AS CantidadEntrada,
        CAST(0 AS DECIMAL(18,2)) AS CantidadSalida,
        CAST(d.Cantidad AS DECIMAL(18,2)) AS DeltaStock,
        c.Observacion AS Observacion,
        {precioExpr} AS PrecioUnitario,
        {subtotalExpr} AS Subtotal
    {detalleSource}
    WHERE (@IdMaterial IS NULL OR m.IdMaterial = @IdMaterial)
      AND (@IdEspecialidad IS NULL OR e.IdEspecialidad = @IdEspecialidad)
      AND (@FechaDesde IS NULL OR CONVERT(date, c.FechaCompra) >= CONVERT(date, @FechaDesde))
      AND (@FechaHasta IS NULL OR CONVERT(date, c.FechaCompra) <= CONVERT(date, @FechaHasta))
)
SELECT
    IdKardexMovimiento,
    IdMaterial,
    Material,
    IdEspecialidad,
    COALESCE(Especialidad, '-') AS Especialidad,
    FechaMovimiento,
    CantidadEntrada,
    CantidadSalida,
    SUM(DeltaStock) OVER (PARTITION BY IdMaterial ORDER BY FechaMovimiento, IdKardexMovimiento ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) AS StockResultante,
    Observacion,
    PrecioUnitario,
    Subtotal
FROM KardexBase
ORDER BY Especialidad, FechaMovimiento, Material;";

            return await db.QueryAsync(sql, new
            {
                IdMaterial = idMaterial,
                IdEspecialidad = idEspecialidad,
                FechaDesde = fechaDesde,
                FechaHasta = fechaHasta
            });
        }
    }
}
