using System.Data;
using Cobranzas_Vittoria.Data;
using Cobranzas_Vittoria.Dtos.Compras;
using Cobranzas_Vittoria.Entities;
using Cobranzas_Vittoria.Interfaces;
using Dapper;

namespace Cobranzas_Vittoria.Repositories
{
    public class CompraRepository : RepositoryBase, ICompraRepository
    {
        public CompraRepository(IDbConnectionFactory factory) : base(factory) { }

        public async Task<IEnumerable<Compra>> ListAsync(bool? aceptada, int? idProveedor)
        {
            using var db = Open();
            return await db.QueryAsync<Compra>("compras.usp_Compra_List", new { Aceptada = aceptada, IdProveedor = idProveedor }, commandType: CommandType.StoredProcedure);
        }

        public async Task<(Compra? head, List<CompraDetalle> items, List<CompraDocumento> docs)> GetAsync(int idCompra)
        {
            using var db = Open();
            using var multi = await db.QueryMultipleAsync("compras.usp_Compra_Get", new { IdCompra = idCompra }, commandType: CommandType.StoredProcedure);
            var head = await multi.ReadFirstOrDefaultAsync<Compra>();
            var items = (await multi.ReadAsync<CompraDetalle>()).AsList();
            var docs = (await multi.ReadAsync<CompraDocumento>()).AsList();
            return (head, items, docs);
        }

        public async Task<(int IdCompra, decimal MontoTotal)> RegistrarAsync(CompraCreateDto dto)
        {
            using var db = Open();

            var tvpItems = new DataTable();
            tvpItems.Columns.Add("IdMaterial", typeof(int));
            tvpItems.Columns.Add("Cantidad", typeof(decimal));
            tvpItems.Columns.Add("PrecioUnitario", typeof(decimal));
            foreach (var it in dto.Items)
                tvpItems.Rows.Add(it.IdMaterial, it.Cantidad, it.PrecioUnitario);

            var tvpDocs = new DataTable();
            tvpDocs.Columns.Add("TipoDocumento", typeof(string));
            tvpDocs.Columns.Add("NumeroDocumento", typeof(string));
            tvpDocs.Columns.Add("RutaArchivo", typeof(string));
            tvpDocs.Columns.Add("FechaDocumento", typeof(DateTime));
            tvpDocs.Columns.Add("Monto", typeof(decimal));
            tvpDocs.Columns.Add("Observacion", typeof(string));
            foreach (var d in dto.Documentos)
                tvpDocs.Rows.Add(d.TipoDocumento, (object?)d.NumeroDocumento ?? DBNull.Value, (object?)d.RutaArchivo ?? DBNull.Value,
                    d.FechaDocumento ?? (object)DBNull.Value, d.Monto ?? (object)DBNull.Value, (object?)d.Observacion ?? DBNull.Value);

            var p = new DynamicParameters();
            p.Add("NumeroCompra", dto.NumeroCompra);
            p.Add("IdOrdenCompra", dto.IdOrdenCompra);
            p.Add("IdProveedor", dto.IdProveedor);
            p.Add("FechaCompra", dto.FechaCompra);
            p.Add("Observacion", dto.Observacion);
            p.Add("Items", tvpItems.AsTableValuedParameter("compras.TVP_CompraDetalle"));
            p.Add("Documentos", tvpDocs.AsTableValuedParameter("compras.TVP_CompraDocumento"));

            var res = await db.QueryFirstAsync<dynamic>("compras.usp_Compra_Registrar", p, commandType: CommandType.StoredProcedure);
            return ((int)res.IdCompra, (decimal)res.MontoTotal);
        }

        public async Task AceptarAsync(int idCompra)
        {
            using var db = Open();
            await db.ExecuteAsync("compras.usp_Compra_Aceptar", new { IdCompra = idCompra }, commandType: CommandType.StoredProcedure);
        }
    }
}
