using Chavez_Logistica.Interfaces;
using Cobranzas_Vittoria.Dtos.Requerimientos;
using Cobranzas_Vittoria.Entities;
using Cobranzas_Vittoria.Interfaces;
using Dapper;
using System.Data;

namespace Cobranzas_Vittoria.Repositories
{
    public class RequerimientosRepository : IRequerimientosRepository
    {
        private readonly IDbConnectionFactory _factory;

        public RequerimientosRepository(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<int> CrearAsync(RequerimientoCreateDto dto)
        {
            using var db = _factory.CreateConnection();

            // TVP dbo.TVP_RequerimientoDetalle: (IdMaterial, Cantidad, Observacion)
            var tvp = new DataTable();
            tvp.Columns.Add("IdMaterial", typeof(int));
            tvp.Columns.Add("Cantidad", typeof(decimal));
            tvp.Columns.Add("Observacion", typeof(string));

            foreach (var it in dto.Items)
            {
                tvp.Rows.Add(
                    it.IdMaterial,
                    it.Cantidad,
                    (object?)it.Observacion ?? DBNull.Value
                );
            }

            var p = new DynamicParameters();
            p.Add("Solicitante", dto.Solicitante);
            p.Add("Items", tvp.AsTableValuedParameter("dbo.TVP_RequerimientoDetalle"));

            // SP retorna SELECT @IdRequerimiento
            return await db.ExecuteScalarAsync<int>(
                "dbo.usp_Requerimiento_Crear",
                p,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<(Requerimiento? requerimiento, List<RequerimientoDetalle> items, List<Cotizacion> cotizaciones)> ObtenerAsync(int idRequerimiento)
        {
            using var db = _factory.CreateConnection();

            using var grid = await db.QueryMultipleAsync(
                "dbo.usp_Requerimiento_Get",
                new { IdRequerimiento = idRequerimiento },
                commandType: CommandType.StoredProcedure
            );

            var head = await grid.ReadFirstOrDefaultAsync<Requerimiento>();
            var items = (await grid.ReadAsync<RequerimientoDetalle>()).AsList();
            var cotizaciones = (await grid.ReadAsync<Cotizacion>()).AsList();

            return (head, items, cotizaciones);
        }
    }
}
