using System.Data;

namespace Cobranzas_Vittoria.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
