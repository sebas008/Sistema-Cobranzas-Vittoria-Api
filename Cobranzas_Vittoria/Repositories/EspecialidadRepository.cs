using System.Data;
using Cobranzas_Vittoria.Data;
using Cobranzas_Vittoria.Dtos.Maestra;
using Cobranzas_Vittoria.Entities;
using Cobranzas_Vittoria.Interfaces;
using Dapper;

namespace Cobranzas_Vittoria.Repositories
{
    public class EspecialidadRepository : RepositoryBase, IEspecialidadRepository
    {
        public EspecialidadRepository(IDbConnectionFactory factory) : base(factory) { }

        public async Task<IEnumerable<Especialidad>> ListAsync(bool? activo)
        {
            using var db = Open();
            return await db.QueryAsync<Especialidad>("maestra.usp_Especialidad_List", new { Activo = activo }, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> UpsertAsync(EspecialidadUpsertDto dto)
        {
            using var db = Open();
            return await db.ExecuteScalarAsync<int>("maestra.usp_Especialidad_Upsert",
                new { dto.IdEspecialidad, dto.Nombre, dto.Descripcion, dto.Activo },
                commandType: CommandType.StoredProcedure);
        }
    }
}
