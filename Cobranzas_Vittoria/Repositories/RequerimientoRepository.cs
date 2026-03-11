using System.Data;
using Cobranzas_Vittoria.Data;
using Cobranzas_Vittoria.Dtos.Compras;
using Cobranzas_Vittoria.Entities;
using Cobranzas_Vittoria.Interfaces;
using Dapper;

namespace Cobranzas_Vittoria.Repositories
{
    public class RequerimientoRepository : RepositoryBase, IRequerimientoRepository
    {
        public RequerimientoRepository(IDbConnectionFactory factory) : base(factory) { }

        public async Task<IEnumerable<Requerimiento>> ListAsync(string? estado, int? idEspecialidad, int? idProyecto)
        {
            using var db = Open();
            return await db.QueryAsync<Requerimiento>("compras.usp_Requerimiento_List",
                new { Estado = estado, IdEspecialidad = idEspecialidad, IdProyecto = idProyecto },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<(Requerimiento? head, List<RequerimientoDetalle> items, List<RequerimientoValidacion> validaciones)> GetAsync(int idRequerimiento)
        {
            using var db = Open();
            using var multi = await db.QueryMultipleAsync("compras.usp_Requerimiento_Get",
                new { IdRequerimiento = idRequerimiento }, commandType: CommandType.StoredProcedure);

            var head = await multi.ReadFirstOrDefaultAsync<Requerimiento>();
            var items = (await multi.ReadAsync<RequerimientoDetalle>()).AsList();
            var validaciones = (await multi.ReadAsync<RequerimientoValidacion>()).AsList();
            return (head, items, validaciones);
        }

        public async Task<int> CrearAsync(RequerimientoCreateDto dto)
        {
            using var db = Open();

            var tvp = new DataTable();
            tvp.Columns.Add("IdMaterial", typeof(int));
            tvp.Columns.Add("Cantidad", typeof(decimal));
            tvp.Columns.Add("Observacion", typeof(string));
            foreach (var it in dto.Items)
                tvp.Rows.Add(it.IdMaterial, it.Cantidad, (object?)it.Observacion ?? DBNull.Value);

            var p = new DynamicParameters();
            p.Add("NumeroRequerimiento", dto.NumeroRequerimiento);
            p.Add("FechaRequerimiento", dto.FechaRequerimiento);
            p.Add("IdEspecialidad", dto.IdEspecialidad);
            p.Add("IdProyecto", dto.IdProyecto);
            p.Add("Descripcion", dto.Descripcion);
            p.Add("FechaEntrega", dto.FechaEntrega);
            p.Add("IdUsuarioSolicitante", dto.IdUsuarioSolicitante);
            p.Add("Observacion", dto.Observacion);
            p.Add("Items", tvp.AsTableValuedParameter("compras.TVP_RequerimientoDetalle"));

            return await db.ExecuteScalarAsync<int>("compras.usp_Requerimiento_Crear", p, commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateEstadoAsync(int idRequerimiento, string estado, string? observacion)
        {
            using var db = Open();
            await db.ExecuteAsync("compras.usp_Requerimiento_ActualizarEstado",
                new { IdRequerimiento = idRequerimiento, Estado = estado, Observacion = observacion },
                commandType: CommandType.StoredProcedure);
        }

        public async Task ValidarAlmacenAsync(int idRequerimiento, int idUsuario, string resultado, string? observacion)
        {
            using var db = Open();
            await db.ExecuteAsync("compras.usp_Requerimiento_ValidarAlmacen",
                new { IdRequerimiento = idRequerimiento, IdUsuario = idUsuario, Resultado = resultado, Observacion = observacion },
                commandType: CommandType.StoredProcedure);
        }
    }
}
