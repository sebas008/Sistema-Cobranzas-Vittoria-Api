using System.Data;
using Cobranzas_Vittoria.Data;
using Cobranzas_Vittoria.Interfaces;
using Dapper;

namespace Cobranzas_Vittoria.Repositories
{
    public class UsuarioRolRepository : RepositoryBase, IUsuarioRolRepository
    {
        public UsuarioRolRepository(IDbConnectionFactory factory) : base(factory) { }

        public async Task AssignAsync(int idUsuario, int idRol)
        {
            using var db = Open();
            await db.ExecuteAsync("seguridad.usp_UsuarioRol_Asignar", new { IdUsuario = idUsuario, IdRol = idRol }, commandType: CommandType.StoredProcedure);
        }

        public async Task RemoveAsync(int idUsuario, int idRol)
        {
            using var db = Open();
            await db.ExecuteAsync("seguridad.usp_UsuarioRol_Quitar", new { IdUsuario = idUsuario, IdRol = idRol }, commandType: CommandType.StoredProcedure);
        }
    }
}
