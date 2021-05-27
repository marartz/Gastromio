using Gastromio.Persistence.MongoDB.Migrations;
using MongoDB.Driver;
using MongoDBMigrations;
using System.Reflection;

namespace Gastromio.Persistence.MongoDB
{
    public class DbAdminService : IDbAdminService
    {
        private readonly IMongoClient client;
        private readonly string connectionString;

        private readonly Assembly usedAssembly;
        private readonly IMigrationRunner migration;

        public DbAdminService(IMongoClient client, string connectionString)
        {
            this.client = client;
            this.connectionString = connectionString;
            usedAssembly = typeof(DbAdminService).Assembly;
            migration = new MigrationEngine()
                .UseDatabase(connectionString, Constants.DatabaseName)
                .UseAssembly(usedAssembly)
                .UseSchemeValidation(false, null);
        }

        public void PurgeDatabase()
        {
            client.DropDatabase(Constants.DatabaseName);
        }

        public void PrepareDatabase()
        {
            if (MongoDatabaseStateChecker.IsDatabaseOutdated(connectionString, Constants.DatabaseName, usedAssembly))
            {
                migration.Run();
            }
        }
    }
}
