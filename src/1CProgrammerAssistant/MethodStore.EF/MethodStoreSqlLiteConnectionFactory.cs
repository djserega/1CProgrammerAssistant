using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data.SQLite;

namespace _1CProgrammerAssistant.MethodStore.EF
{
    public class MethodStoreSqlLiteConnectionFactory : IDbConnectionFactory
    {
        public DbConnection CreateConnection(string connectionString)
        {
            return new SQLiteConnection(connectionString);
        }
    }
}
