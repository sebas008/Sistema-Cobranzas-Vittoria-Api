using System.Data;
using Cobranzas_Vittoria.Data;
using Cobranzas_Vittoria.Entities;
using Cobranzas_Vittoria.Interfaces;
using Dapper;

namespace Cobranzas_Vittoria.Repositories
{
    public class KardexRepository : RepositoryBase, IKardexRepository
    {
        public KardexRepository(IDbConnectionFactory factory) : base(factory) { }

        public async Task<IEnumerable<KardexMovimiento>> ListAsync(int? idMaterial, int? idEspecialidad, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            using var db = Open();
            return await db.QueryAsync<KardexMovimiento>("almacen.usp_Kardex_List",
                new { IdMaterial = idMaterial, IdEspecialidad = idEspecialidad, FechaDesde = fechaDesde, FechaHasta = fechaHasta },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<KardexResumenMaterial>> ResumenAsync(int? idMaterial, int? idEspecialidad)
        {
            using var db = Open();
            return await db.QueryAsync<KardexResumenMaterial>("almacen.usp_Kardex_ResumenMaterial",
                new { IdMaterial = idMaterial, IdEspecialidad = idEspecialidad },
                commandType: CommandType.StoredProcedure);
        }
    }
}
