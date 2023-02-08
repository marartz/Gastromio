using MongoDB.Driver;
using System.Reflection;

namespace Gastromio.Persistence.MongoDB
{
    public class DbAdminService : IDbAdminService
    {
        private readonly IMongoClient client;
        private readonly string connectionString;
        private readonly string databaseName;

        private readonly Assembly usedAssembly;

        public DbAdminService(IMongoClient client, string connectionString, string databaseName)
        {
            this.client = client;
            this.connectionString = connectionString;
            this.databaseName = databaseName;
            usedAssembly = typeof(DbAdminService).Assembly;
        }

        public void PurgeDatabase()
        {
            client.DropDatabase(databaseName);
        }

        public void PrepareDatabase()
        {
        }
    }
}
