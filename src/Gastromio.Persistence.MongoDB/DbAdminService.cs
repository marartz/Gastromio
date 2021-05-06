using Gastromio.Persistence.MongoDB.Migrations;
using MongoDB.Driver;
using MongoDBMigrations;
using System.Reflection;

namespace Gastromio.Persistence.MongoDB
{
    public class DbAdminService : IDbAdminService
    {
        private readonly IMongoClient client;
        private readonly IMongoDatabase database;

        private readonly Assembly UsedAssembly;
        private readonly MigrationEngine Migration;

        public DbAdminService(IMongoClient client, IMongoDatabase database)
        {
            this.client = client;
            this.database = database;
        }

        public void PurgeDatabase()
        {
            client.DropDatabase(Constants.DatabaseName);
        }

        public void PrepareDatabase()
        {
            var assembly = typeof(DbAdminService).Assembly;

            new MigrationEngine().UseDatabase("mongodb://localhost:27017", Constants.DatabaseName)
                .UseAssembly(assembly)
                .UseSchemeValidation(false)
                .Run(DatabaseVersions.Initial);
        }

        public void CheckAndRunDatabaseMigrations()
        {
            var assembly = typeof(DbAdminService).Assembly;
            if (MongoDatabaseStateChecker.IsDatabaseOutdated("mongodb://localhost:27017", Constants.DatabaseName, assembly))
            {
                new MigrationEngine().UseDatabase("mongodb://localhost:27017", Constants.DatabaseName)
                    .UseAssembly(assembly)
                    .UseSchemeValidation(false)
                    .Run();
            }
        }
    }
}