using Cobranzas_Vittoria.Data;
using Cobranzas_Vittoria.Interfaces;
using Dapper;
using System.Linq;

namespace Cobranzas_Vittoria.Repositories
{
    public class CompraRepository : RepositoryBase, ICompraRepository
    {
        public CompraRepository(IDbConnectionFactory factory) : base(factory) { }

        public async Task<IEnumerable<dynamic>> ListAsync(bool? aceptada, int? idProveedor)
        {
            using var db = Open();

            var sql = @"
SELECT
    c.IdCompra,
    c.NumeroCompra,
    c.FechaCompra,
    CASE WHEN c.Aceptada = 1 THEN 'Aceptada' ELSE 'Pendiente' END AS Estado,
    c.MontoTotal,
    c.Observacion,
    p.RazonSocial AS Proveedor,
    e.Nombre AS Especialidad,
    pr.NombreProyecto,
    oc.NumeroOrdenCompra,
    r.NumeroRequerimiento
FROM compras.Compra c
INNER JOIN compras.OrdenCompra oc ON oc.IdOrdenCompra = c.IdOrdenCompra
LEFT JOIN compras.Requerimiento r ON r.IdRequerimiento = oc.IdRequerimiento
LEFT JOIN maestra.Proveedor p ON p.IdProveedor = c.IdProveedor
LEFT JOIN maestra.Especialidad e ON e.IdEspecialidad = r.IdEspecialidad
LEFT JOIN maestra.Proyecto pr ON pr.IdProyecto = r.IdProyecto
WHERE (@Aceptada IS NULL OR c.Aceptada = @Aceptada)
  AND (@IdProveedor IS NULL OR c.IdProveedor = @IdProveedor)
ORDER BY c.IdCompra DESC";

            return await db.QueryAsync(sql, new { Aceptada = aceptada, IdProveedor = idProveedor });
        }

        public async Task<(int IdCompra, decimal MontoTotal)> CrearAsync(Cobranzas_Vittoria.Dtos.Compras.CompraCreateDto dto)
        {
            using var db = Open();

            const string sql = @"
INSERT INTO compras.Compra
(
    NumeroCompra,
    IdOrdenCompra,
    IdProveedor,
    FechaCompra,
    Aceptada,
    MontoTotal,
    Observacion,
    FechaCreacion
)
VALUES
(
    @NumeroCompra,
    @IdOrdenCompra,
    @IdProveedor,
    @FechaCompra,
    0,
    @MontoTotal,
    @Observacion,
    GETDATE()
);

SELECT CAST(SCOPE_IDENTITY() AS INT);";

            var idCompra = await db.ExecuteScalarAsync<int>(sql, new
            {
                dto.NumeroCompra,
                dto.IdOrdenCompra,
                dto.IdProveedor,
                dto.FechaCompra,
                dto.MontoTotal,
                dto.Observacion
            });

            return (idCompra, dto.MontoTotal);
        }

        public async Task<IEnumerable<dynamic>> ListPendientesDesdeOcAsync()
        {
            using var db = Open();

            var sql = @"
SELECT
    oc.IdOrdenCompra,
    oc.NumeroOrdenCompra,
    oc.FechaOrdenCompra,
    oc.Estado,
    oc.Total,
    oc.IdProveedor,
    p.RazonSocial AS Proveedor,
    r.IdRequerimiento,
    r.NumeroRequerimiento,
    e.Nombre AS Especialidad,
    pr.NombreProyecto
FROM compras.OrdenCompra oc
LEFT JOIN compras.Requerimiento r ON r.IdRequerimiento = oc.IdRequerimiento
LEFT JOIN maestra.Proveedor p ON p.IdProveedor = oc.IdProveedor
LEFT JOIN maestra.Especialidad e ON e.IdEspecialidad = r.IdEspecialidad
LEFT JOIN maestra.Proyecto pr ON pr.IdProyecto = oc.IdProyecto
LEFT JOIN compras.Compra c ON c.IdOrdenCompra = oc.IdOrdenCompra
WHERE c.IdCompra IS NULL
ORDER BY oc.IdOrdenCompra DESC";

            return await db.QueryAsync(sql);
        }

        public async Task<object?> GetAsync(int idCompra)
        {
            using var db = Open();

            var compra = await db.QueryFirstOrDefaultAsync(@"
SELECT
    c.IdCompra,
    c.NumeroCompra,
    c.FechaCompra,
    CASE WHEN c.Aceptada = 1 THEN 'Aceptada' ELSE 'Pendiente' END AS Estado,
    c.MontoTotal,
    c.Observacion,
    c.IdOrdenCompra,
    oc.NumeroOrdenCompra,
    r.NumeroRequerimiento,
    p.RazonSocial AS Proveedor,
    e.Nombre AS Especialidad,
    pr.NombreProyecto
FROM compras.Compra c
INNER JOIN compras.OrdenCompra oc ON oc.IdOrdenCompra = c.IdOrdenCompra
LEFT JOIN compras.Requerimiento r ON r.IdRequerimiento = oc.IdRequerimiento
LEFT JOIN maestra.Proveedor p ON p.IdProveedor = c.IdProveedor
LEFT JOIN maestra.Especialidad e ON e.IdEspecialidad = r.IdEspecialidad
LEFT JOIN maestra.Proyecto pr ON pr.IdProyecto = r.IdProyecto
WHERE c.IdCompra = @IdCompra", new { IdCompra = idCompra });

            if (compra == null) return null;

            var items = await db.QueryAsync(@"
SELECT
    d.IdOrdenCompraDetalle,
    d.IdMaterial,
    m.Descripcion AS Material,
    m.UnidadMedida,
    d.Cantidad,
    d.PrecioUnitario,
    (d.Cantidad * d.PrecioUnitario) AS Subtotal
FROM compras.Compra c
INNER JOIN compras.OrdenCompra oc ON oc.IdOrdenCompra = c.IdOrdenCompra
INNER JOIN compras.OrdenCompraDetalle d ON d.IdOrdenCompra = oc.IdOrdenCompra
INNER JOIN maestra.Material m ON m.IdMaterial = d.IdMaterial
WHERE c.IdCompra = @IdCompra", new { IdCompra = idCompra });

            var documentos = await GetDocumentosAsync(idCompra);

            return new { compra, items, documentos };
        }

        public async Task<IEnumerable<dynamic>> GetDocumentosAsync(int idCompra)
        {
            using var db = Open();
            var cols = (await db.QueryAsync<string>(@"
SELECT COLUMN_NAME
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_SCHEMA = 'compras'
  AND TABLE_NAME = 'CompraDocumento'
ORDER BY ORDINAL_POSITION")).ToList();

            string Pick(params string[] names) =>
                cols.FirstOrDefault(c => names.Any(n => string.Equals(n, c, StringComparison.OrdinalIgnoreCase))) ?? string.Empty;

            var colId = Pick("IdCompraDocumento");
            var colIdCompra = Pick("IdCompra");
            var colNombre = Pick("NombreArchivo", "NombreDocumento", "Nombre", "Archivo");
            var colRuta = Pick("RutaArchivo", "RutaDocumento", "Ruta", "UrlArchivo", "ArchivoRuta", "PathArchivo");
            var colExt = Pick("Extension", "Ext", "TipoArchivo", "Tipo");
            var colFecha = Pick("FechaCreacion", "FechaRegistro", "Fecha", "FechaCarga");
            var colTipoDocumento = Pick("TipoDocumento");

            if (string.IsNullOrWhiteSpace(colIdCompra))
                return Enumerable.Empty<dynamic>();

            var selectId = string.IsNullOrWhiteSpace(colId) ? "NULL AS IdCompraDocumento" : $"[{colId}] AS IdCompraDocumento";
            var selectNombre = string.IsNullOrWhiteSpace(colNombre) ? "NULL AS NombreArchivo" : $"[{colNombre}] AS NombreArchivo";
            var selectRuta = string.IsNullOrWhiteSpace(colRuta) ? "NULL AS RutaArchivo" : $"[{colRuta}] AS RutaArchivo";
            var selectExt = string.IsNullOrWhiteSpace(colExt) ? "NULL AS Extension" : $"[{colExt}] AS Extension";
            var selectFecha = string.IsNullOrWhiteSpace(colFecha) ? "NULL AS FechaCreacion" : $"[{colFecha}] AS FechaCreacion";
            var selectTipoDoc = string.IsNullOrWhiteSpace(colTipoDocumento) ? "NULL AS TipoDocumento" : $"[{colTipoDocumento}] AS TipoDocumento";

            var sql = $@"
SELECT
    {selectId},
    [{colIdCompra}] AS IdCompra,
    {selectNombre},
    {selectRuta},
    {selectExt},
    {selectFecha},
    {selectTipoDoc}
FROM compras.CompraDocumento
WHERE [{colIdCompra}] = @IdCompra
ORDER BY 1 DESC";

            return await db.QueryAsync(sql, new { IdCompra = idCompra });
        }

        public async Task SaveDocumentosAsync(int idCompra, IEnumerable<(string NombreArchivo, string RutaArchivo, string? Extension)> docs)
        {
            using var db = Open();
            var meta = (await db.QueryAsync(@"
SELECT
    COLUMN_NAME AS ColumnName,
    IS_NULLABLE AS IsNullable
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_SCHEMA = 'compras'
  AND TABLE_NAME = 'CompraDocumento'
ORDER BY ORDINAL_POSITION")).ToList();

            var cols = meta.Select(x => (string)x.ColumnName).ToList();

            string Pick(params string[] names) =>
                cols.FirstOrDefault(c => names.Any(n => string.Equals(n, c, StringComparison.OrdinalIgnoreCase))) ?? string.Empty;

            bool IsNullable(string columnName)
            {
                var row = meta.FirstOrDefault(x => string.Equals((string)x.ColumnName, columnName, StringComparison.OrdinalIgnoreCase));
                return row != null && string.Equals((string)row.IsNullable, "YES", StringComparison.OrdinalIgnoreCase);
            }

            var colIdCompra = Pick("IdCompra");
            var colNombre = Pick("NombreArchivo", "NombreDocumento", "Nombre", "Archivo");
            var colRuta = Pick("RutaArchivo", "RutaDocumento", "Ruta", "UrlArchivo", "ArchivoRuta", "PathArchivo");
            var colExt = Pick("Extension", "Ext", "TipoArchivo", "Tipo");
            var colFecha = Pick("FechaCreacion", "FechaRegistro", "Fecha", "FechaCarga");
            var colTipoDocumento = Pick("TipoDocumento");

            if (string.IsNullOrWhiteSpace(colIdCompra) || string.IsNullOrWhiteSpace(colNombre) || string.IsNullOrWhiteSpace(colRuta))
                throw new InvalidOperationException("La tabla compras.CompraDocumento no tiene las columnas mínimas requeridas para guardar documentos.");

            foreach (var doc in docs)
            {
                var columns = new List<string> { $"[{colIdCompra}]", $"[{colNombre}]", $"[{colRuta}]" };
                var values = new List<string> { "@IdCompra", "@NombreArchivo", "@RutaArchivo" };

                if (!string.IsNullOrWhiteSpace(colExt))
                {
                    columns.Add($"[{colExt}]");
                    values.Add("@Extension");
                }

                if (!string.IsNullOrWhiteSpace(colTipoDocumento))
                {
                    columns.Add($"[{colTipoDocumento}]");
                    values.Add("@TipoDocumento");
                }

                if (!string.IsNullOrWhiteSpace(colFecha))
                {
                    columns.Add($"[{colFecha}]");
                    values.Add("GETDATE()");
                }

                var sql = $@"
INSERT INTO compras.CompraDocumento
(
    {string.Join(", ", columns)}
)
VALUES
(
    {string.Join(", ", values)}
)";

                await db.ExecuteAsync(sql, new
                {
                    IdCompra = idCompra,
                    NombreArchivo = doc.NombreArchivo,
                    RutaArchivo = doc.RutaArchivo,
                    Extension = doc.Extension,
                    TipoDocumento = "PDF"
                });
            }
        }
    }
}
