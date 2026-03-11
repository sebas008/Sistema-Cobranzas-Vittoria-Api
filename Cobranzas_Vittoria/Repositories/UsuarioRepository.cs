using System.Data;
using Cobranzas_Vittoria.Data;
using Cobranzas_Vittoria.Dtos.Seguridad;
using Cobranzas_Vittoria.Entities;
using Cobranzas_Vittoria.Interfaces;
using Dapper;

namespace Cobranzas_Vittoria.Repositories
{
    public class UsuarioRepository : RepositoryBase, IUsuarioRepository
    {
        public UsuarioRepository(IDbConnectionFactory factory) : base(factory) { }

        public async Task<IEnumerable<Usuario>> ListAsync(bool? activo)
        {
            using var db = Open();
            return await db.QueryAsync<Usuario>("seguridad.usp_Usuario_List", new { Activo = activo }, commandType: CommandType.StoredProcedure);
        }

        public async Task<(Usuario? usuario, List<UsuarioRol> roles)> GetAsync(int idUsuario)
        {
            using var db = Open();
            using var multi = await db.QueryMultipleAsync("seguridad.usp_Usuario_Get", new { IdUsuario = idUsuario }, commandType: CommandType.StoredProcedure);
            var usuario = await multi.ReadFirstOrDefaultAsync<Usuario>();
            var roles = (await multi.ReadAsync<UsuarioRol>()).AsList();
            return (usuario, roles);
        }

        public async Task<int> UpsertAsync(UsuarioUpsertDto dto)
        {
            using var db = Open();
            var p = new DynamicParameters();
            p.Add("IdUsuario", dto.IdUsuario);
            p.Add("Nombres", dto.Nombres);
            p.Add("Apellidos", dto.Apellidos);
            p.Add("Correo", dto.Correo);
            p.Add("UsuarioLogin", dto.UsuarioLogin);
            p.Add("PasswordHash", dto.PasswordHash);
            p.Add("Activo", dto.Activo);
            p.Add("UsuarioCreacion", dto.UsuarioCreacion);
            return await db.ExecuteScalarAsync<int>("seguridad.usp_Usuario_Upsert", p, commandType: CommandType.StoredProcedure);
        }
    }
}
