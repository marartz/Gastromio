using Gastromio.Persistence.MongoDB.Migrations;
using MongoDB.Driver;
using MongoDBMigrations;
using System.Reflection;

namespace Gastromio.Persistence.MongoDB
{
    public class DbAdminService : IDbAdminService
    {
        private readonly IMongoClient Client;

        private readonly Assembly UsedAssembly;
        private readonly IMigrationRunner Migration;

        public DbAdminService(IMongoClient client)
        {
            Client = client;
            UsedAssembly = typeof(DbAdminService).Assembly;
            Migration = new MigrationEngine()
                .UseDatabase("mongodb://localhost:27017", Constants.DatabaseName)
                .UseAssembly(UsedAssembly)
                .UseSchemeValidation(false, null);
        }

        public void PurgeDatabase()
        {
            Client.DropDatabase(Constants.DatabaseName);
        }

        public void PrepareDatabase()
        {
            Migration.Run(DatabaseVersions.Initial);
        }

        public void CheckAndRunDatabaseMigrations()
        {
            if (MongoDatabaseStateChecker.IsDatabaseOutdated("mongodb://localhost:27017", Constants.DatabaseName, UsedAssembly))
            {
                Migration.Run();
            }
        }
    }
}