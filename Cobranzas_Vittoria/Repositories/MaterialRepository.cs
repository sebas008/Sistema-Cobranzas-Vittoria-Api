using System.Data;
using Cobranzas_Vittoria.Data;
using Cobranzas_Vittoria.Dtos.Maestra;
using Cobranzas_Vittoria.Entities;
using Cobranzas_Vittoria.Interfaces;
using Dapper;

namespace Cobranzas_Vittoria.Repositories
{
    public class MaterialRepository : RepositoryBase, IMaterialRepository
    {
        public MaterialRepository(IDbConnectionFactory factory) : base(factory) { }

        public async Task<IEnumerable<Material>> ListAsync(bool? activo, int? idEspecialidad)
        {
            using var db = Open();
            return await db.QueryAsync<Material>("maestra.usp_Material_List",
                new { Activo = activo, IdEspecialidad = idEspecialidad },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<Material?> GetAsync(int idMaterial)
        {
            using var db = Open();
            return await db.QueryFirstOrDefaultAsync<Material>("maestra.usp_Material_Get", new { IdMaterial = idMaterial }, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> UpsertAsync(MaterialUpsertDto dto)
        {
            using var db = Open();
            return await db.ExecuteScalarAsync<int>("maestra.usp_Material_Upsert", new
            {
                dto.IdMaterial, dto.IdEspecialidad, dto.Codigo, dto.Descripcion, dto.UnidadMedida, dto.StockMinimo, dto.Activo
            }, commandType: CommandType.StoredProcedure);
        }
    }
}
