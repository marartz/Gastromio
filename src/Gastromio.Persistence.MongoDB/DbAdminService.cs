using MongoDB.Driver;
using MongoDBMigrations;
using System.Reflection;

namespace Gastromio.Persistence.MongoDB
{
    public class DbAdminService : IDbAdminService
    {
        private readonly IMongoClient client;
        private readonly string connectionString;
        private readonly string databaseName;

        private readonly Assembly usedAssembly;
        private readonly IMigrationRunner migration;

        public DbAdminService(IMongoClient client, string connectionString, string databaseName)
        {
            this.client = client;
            this.connectionString = connectionString;
            this.databaseName = databaseName;
            usedAssembly = typeof(DbAdminService).Assembly;
            migration = new MigrationEngine()
                .UseDatabase(connectionString, databaseName)
                .UseAssembly(usedAssembly)
                .UseSchemeValidation(false);
        }

        public void PurgeDatabase()
        {
            client.DropDatabase(databaseName);
        }

        public void PrepareDatabase()
        {
            if (MongoDatabaseStateChecker.IsDatabaseOutdated(connectionString, databaseName, usedAssembly))
            {
                migration.Run();
            }
        }
    }
}
