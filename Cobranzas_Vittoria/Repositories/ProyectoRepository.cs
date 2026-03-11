using System.Data;
using Cobranzas_Vittoria.Data;
using Cobranzas_Vittoria.Entities;
using Cobranzas_Vittoria.Interfaces;
using Dapper;

namespace Cobranzas_Vittoria.Repositories
{
    public class ProyectoRepository : RepositoryBase, IProyectoRepository
    {
        public ProyectoRepository(IDbConnectionFactory factory) : base(factory) { }

        public async Task<IEnumerable<Proyecto>> ListAsync(bool? activo)
        {
            using var db = Open();
            return await db.QueryAsync<Proyecto>("maestra.usp_Proyecto_List", new { Activo = activo }, commandType: CommandType.StoredProcedure);
        }
    }
}
