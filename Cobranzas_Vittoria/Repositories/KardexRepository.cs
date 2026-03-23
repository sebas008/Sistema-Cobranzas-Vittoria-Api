using Cobranzas_Vittoria.Data;
using Cobranzas_Vittoria.Dtos.Almacen;
using Cobranzas_Vittoria.Interfaces;
using Dapper;
using System.Data;

namespace Cobranzas_Vittoria.Repositories
{
    public class KardexRepository : RepositoryBase, IKardexRepository
    {
        public KardexRepository(IDbConnectionFactory factory) : base(factory) { }

        public async Task<IEnumerable<dynamic>> ListMovimientosAsync(int? idMaterial, int? idEspecialidad, string? fechaDesde, string? fechaHasta)
        {
            using var db = Open();

            var sql = @"
WITH EntradasCompra AS
(
    SELECT
        c.IdCompra,
        c.NumeroCompra,
        cd.IdMaterial,
        m.Descripcion AS Material,
        m.IdEspecialidad,
        e.Nombre AS Especialidad,
        CAST(c.FechaCompra AS date) AS FechaMovimiento,
        CAST(ISNULL(cd.Cantidad, 0) AS DECIMAL(18,2)) AS Entrada,
        CAST(0 AS DECIMAL(18,2)) AS Salida,
        ISNULL(NULLIF(LTRIM(RTRIM(c.Observacion)), ''), CONCAT('Ingreso por compra ', c.NumeroCompra)) AS Observacion
    FROM compras.Compra c
    INNER JOIN compras.CompraDetalle cd ON cd.IdCompra = c.IdCompra
    INNER JOIN maestra.Material m ON m.IdMaterial = cd.IdMaterial
    LEFT JOIN maestra.Especialidad e ON e.IdEspecialidad = m.IdEspecialidad
    WHERE (@IdMaterial IS NULL OR cd.IdMaterial = @IdMaterial)
      AND (@IdEspecialidad IS NULL OR m.IdEspecialidad = @IdEspecialidad)
      AND (@FechaDesde IS NULL OR c.FechaCompra >= CONVERT(date, @FechaDesde))
      AND (@FechaHasta IS NULL OR c.FechaCompra <= CONVERT(date, @FechaHasta))
),
SalidasManual AS
(
    SELECT
        km.IdCompra,
        c.NumeroCompra,
        km.IdMaterial,
        m.Descripcion AS Material,
        km.IdEspecialidad,
        e.Nombre AS Especialidad,
        CAST(km.FechaMovimiento AS date) AS FechaMovimiento,
        CAST(0 AS DECIMAL(18,2)) AS Entrada,
        CAST(ISNULL(km.CantidadSalida, 0) AS DECIMAL(18,2)) AS Salida,
        ISNULL(NULLIF(LTRIM(RTRIM(km.Observacion)), ''), 'Salida de almacén') AS Observacion
    FROM almacen.KardexMovimiento km
    INNER JOIN maestra.Material m ON m.IdMaterial = km.IdMaterial
    LEFT JOIN maestra.Especialidad e ON e.IdEspecialidad = km.IdEspecialidad
    LEFT JOIN compras.Compra c ON c.IdCompra = km.IdCompra
    WHERE km.TipoMovimiento = 'SALIDA'
      AND (@IdMaterial IS NULL OR km.IdMaterial = @IdMaterial)
      AND (@IdEspecialidad IS NULL OR km.IdEspecialidad = @IdEspecialidad)
      AND (@FechaDesde IS NULL OR km.FechaMovimiento >= CONVERT(date, @FechaDesde))
      AND (@FechaHasta IS NULL OR km.FechaMovimiento <= CONVERT(date, @FechaHasta))
),
Movs AS
(
    SELECT * FROM EntradasCompra
    UNION ALL
    SELECT * FROM SalidasManual
),
Agrupado AS
(
    SELECT
        IdCompra,
        NumeroCompra,
        IdMaterial,
        IdEspecialidad,
        Especialidad,
        MAX(FechaMovimiento) AS FechaMovimiento,
        MAX(Material) AS Material,
        SUM(Entrada) AS Entrada,
        SUM(Salida) AS Salida,
        SUM(Entrada - Salida) AS Stock,
        MAX(Observacion) AS Observacion
    FROM Movs
    GROUP BY IdCompra, NumeroCompra, IdMaterial, IdEspecialidad, Especialidad
)
SELECT
    IdCompra,
    NumeroCompra,
    IdMaterial,
    IdEspecialidad,
    Especialidad,
    FechaMovimiento,
    Material,
    CAST(Entrada AS DECIMAL(18,2)) AS Entrada,
    CAST(Salida AS DECIMAL(18,2)) AS Salida,
    CAST(Stock AS DECIMAL(18,2)) AS Stock,
    Observacion
FROM Agrupado
ORDER BY FechaMovimiento DESC, NumeroCompra DESC, Material ASC;";

            return await db.QueryAsync(sql, new
            {
                IdMaterial = idMaterial,
                IdEspecialidad = idEspecialidad,
                FechaDesde = fechaDesde,
                FechaHasta = fechaHasta
            });
        }

        public async Task<object> RegistrarSalidaAsync(KardexSalidaCreateDto dto)
        {
            using var db = Open();
            var res = await db.QueryFirstAsync("almacen.usp_Kardex_RegistrarSalida", new
            {
                dto.IdCompra,
                dto.IdMaterial,
                dto.IdEspecialidad,
                dto.FechaMovimiento,
                dto.CantidadSalida,
                dto.Observacion
            }, commandType: CommandType.StoredProcedure);

            return new
            {
                ok = true,
                stockActual = (decimal?)res.StockActual ?? 0m,
                mensaje = (string?)res.Mensaje ?? "Salida registrada correctamente."
            };
        }
    }
}
