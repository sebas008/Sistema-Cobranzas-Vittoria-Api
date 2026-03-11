using System.Data;
using Cobranzas_Vittoria.Data;
using Cobranzas_Vittoria.Entities;
using Cobranzas_Vittoria.Interfaces;
using Dapper;

namespace Cobranzas_Vittoria.Repositories
{
    public class RolRepository : RepositoryBase, IRolRepository
    {
        public RolRepository(IDbConnectionFactory factory) : base(factory) { }

        public async Task<IEnumerable<Rol>> ListAsync()
        {
            using var db = Open();
            return await db.QueryAsync<Rol>("seguridad.usp_Rol_List", commandType: CommandType.StoredProcedure);
        }
    }
}
