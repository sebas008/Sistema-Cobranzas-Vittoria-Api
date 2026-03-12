using Cobranzas_Vittoria.Data;
using Cobranzas_Vittoria.Interfaces;
using Dapper;
using System.Linq;

namespace Cobranzas_Vittoria.Repositories
{
    public class KardexRepository : RepositoryBase, IKardexRepository
    {
        public KardexRepository(IDbConnectionFactory factory) : base(factory) { }

        public async Task<IEnumerable<dynamic>> ListMovimientosAsync(int? idMaterial, int? idEspecialidad, string? fechaDesde, string? fechaHasta)
        {
            using var db = Open();

            var cols = (await db.QueryAsync<string>(@"
SELECT COLUMN_NAME
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_SCHEMA = 'almacen'
  AND TABLE_NAME = 'KardexMovimiento'
ORDER BY ORDINAL_POSITION")).ToList();

            string Pick(params string[] names) =>
                cols.FirstOrDefault(c => names.Any(n => string.Equals(n, c, StringComparison.OrdinalIgnoreCase))) ?? string.Empty;

            var colId = Pick("IdKardexMovimiento");
            var colIdMaterial = Pick("IdMaterial");
            var colIdEspecialidad = Pick("IdEspecialidad");
            var colFecha = Pick("FechaMovimiento", "Fecha");
            var colTipo = Pick("TipoMovimiento", "Tipo");
            var colDocumento = Pick("DocumentoReferencia", "Documento", "Referencia", "NroDocumento");
            var colEntrada = Pick("CantidadEntrada", "Entrada");
            var colSalida = Pick("CantidadSalida", "Salida");
            var colStock = Pick("StockResultante", "Saldo", "Stock");
            var colObservacion = Pick("Observacion", "Observación");

            var selectId = string.IsNullOrWhiteSpace(colId) ? "NULL AS IdKardexMovimiento" : $"km.[{colId}] AS IdKardexMovimiento";
            var selectFecha = string.IsNullOrWhiteSpace(colFecha) ? "NULL AS FechaMovimiento" : $"km.[{colFecha}] AS FechaMovimiento";
            var selectTipo = string.IsNullOrWhiteSpace(colTipo) ? "NULL AS TipoMovimiento" : $"km.[{colTipo}] AS TipoMovimiento";
            var selectDocumento = string.IsNullOrWhiteSpace(colDocumento) ? "NULL AS DocumentoReferencia" : $"km.[{colDocumento}] AS DocumentoReferencia";
            var selectEntrada = string.IsNullOrWhiteSpace(colEntrada) ? "0 AS CantidadEntrada" : $"km.[{colEntrada}] AS CantidadEntrada";
            var selectSalida = string.IsNullOrWhiteSpace(colSalida) ? "0 AS CantidadSalida" : $"km.[{colSalida}] AS CantidadSalida";
            var selectStock = string.IsNullOrWhiteSpace(colStock) ? "0 AS StockResultante" : $"km.[{colStock}] AS StockResultante";
            var selectObservacion = string.IsNullOrWhiteSpace(colObservacion) ? "NULL AS Observacion" : $"km.[{colObservacion}] AS Observacion";

            var whereIdMaterial = !string.IsNullOrWhiteSpace(colIdMaterial)
                ? "(@IdMaterial IS NULL OR km.[" + colIdMaterial + "] = @IdMaterial)"
                : "1=1";

            var whereIdEspecialidad = !string.IsNullOrWhiteSpace(colIdEspecialidad)
                ? "(@IdEspecialidad IS NULL OR km.[" + colIdEspecialidad + "] = @IdEspecialidad)"
                : "1=1";

            var whereFechaDesde = !string.IsNullOrWhiteSpace(colFecha)
                ? "(@FechaDesde IS NULL OR CONVERT(date, km.[" + colFecha + "]) >= CONVERT(date, @FechaDesde))"
                : "1=1";

            var whereFechaHasta = !string.IsNullOrWhiteSpace(colFecha)
                ? "(@FechaHasta IS NULL OR CONVERT(date, km.[" + colFecha + "]) <= CONVERT(date, @FechaHasta))"
                : "1=1";

            var joinMaterial = !string.IsNullOrWhiteSpace(colIdMaterial)
                ? "INNER JOIN maestra.Material m ON m.IdMaterial = km.[" + colIdMaterial + "]"
                : "LEFT JOIN maestra.Material m ON 1 = 0";

            var joinEspecialidad = !string.IsNullOrWhiteSpace(colIdEspecialidad)
                ? "LEFT JOIN maestra.Especialidad e ON e.IdEspecialidad = km.[" + colIdEspecialidad + "]"
                : "LEFT JOIN maestra.Especialidad e ON 1 = 0";

            var orderBy = !string.IsNullOrWhiteSpace(colFecha)
                ? $"ORDER BY e.Nombre, km.[{colFecha}]"
                : "ORDER BY e.Nombre";

            var sql = $@"
SELECT
    {selectId},
    e.Nombre AS Especialidad,
    {selectFecha},
    m.Descripcion AS Material,
    {selectTipo},
    {selectDocumento},
    {selectEntrada},
    {selectSalida},
    {selectStock},
    {selectObservacion}
FROM almacen.KardexMovimiento km
{joinMaterial}
{joinEspecialidad}
WHERE {whereIdMaterial}
  AND {whereIdEspecialidad}
  AND {whereFechaDesde}
  AND {whereFechaHasta}
{orderBy};";

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
