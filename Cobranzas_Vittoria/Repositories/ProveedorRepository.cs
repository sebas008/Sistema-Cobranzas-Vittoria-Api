using System.Data;
using Cobranzas_Vittoria.Data;
using Cobranzas_Vittoria.Dtos.Maestra;
using Cobranzas_Vittoria.Entities;
using Cobranzas_Vittoria.Interfaces;
using Dapper;

namespace Cobranzas_Vittoria.Repositories
{
    public class ProveedorRepository : RepositoryBase, IProveedorRepository
    {
        public ProveedorRepository(IDbConnectionFactory factory) : base(factory) { }

        public async Task<IEnumerable<Proveedor>> ListAsync(bool? activo, int? idEspecialidad)
        {
            using var db = Open();
            return await db.QueryAsync<Proveedor>("maestra.usp_Proveedor_List",
                new { Activo = activo, IdEspecialidad = idEspecialidad },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<(Proveedor? proveedor, List<ProveedorEspecialidad> especialidades)> GetAsync(int idProveedor)
        {
            using var db = Open();
            using var multi = await db.QueryMultipleAsync("maestra.usp_Proveedor_Get", new { IdProveedor = idProveedor }, commandType: CommandType.StoredProcedure);
            var proveedor = await multi.ReadFirstOrDefaultAsync<Proveedor>();
            var especialidades = (await multi.ReadAsync<ProveedorEspecialidad>()).AsList();
            return (proveedor, especialidades);
        }

        public async Task<int> UpsertAsync(ProveedorUpsertDto dto)
        {
            using var db = Open();
            return await db.ExecuteScalarAsync<int>("maestra.usp_Proveedor_Upsert", new
            {
                dto.IdProveedor, dto.RazonSocial, dto.Ruc, dto.Contacto, dto.Telefono, dto.Correo,
                dto.Direccion, dto.Banco, dto.CuentaCorriente, dto.CCI, dto.CuentaDetraccion,
                dto.DescripcionServicio, dto.Observacion, dto.TrabajamosConProveedor, dto.Activo
            }, commandType: CommandType.StoredProcedure);
        }

        public async Task SetEspecialidadAsync(int idProveedor, int idEspecialidad, bool activo)
        {
            using var db = Open();
            await db.ExecuteAsync("maestra.usp_ProveedorEspecialidad_Set",
                new { IdProveedor = idProveedor, IdEspecialidad = idEspecialidad, Activo = activo },
                commandType: CommandType.StoredProcedure);
        }
    }
}
