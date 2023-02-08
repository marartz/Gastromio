// using Gastromio.Persistence.MongoDB.Migrations;
// using MongoDB.Driver;
// using MongoDBMigrations;
// using System;
// using System.Reflection;
//
// namespace Gastromio.Persistence.MongoDB.Tests
// {
//     public abstract class DatabaseTestBase : IDisposable
//     {
//         private string DatabaseName { get; }
//
//         private string Connection { get; }
//
//         private Assembly UsedAssembly { get; }
//
//         protected IMongoClient Client { get; }
//
//         protected IMongoDatabase Database { get; }
//
//         protected IMigrationRunner Migration { get; }
//
//         protected bool IsDatabaseOutdated => MongoDatabaseStateChecker.IsDatabaseOutdated(Connection, DatabaseName, UsedAssembly);
//
//         protected MongoDBMigrations.Version CurrentVersion => new DatabaseManager(Database, MongoDBMigrations.Document.MongoEmulationEnum.None).GetVersion();
//
//         protected DatabaseTestBase(string connectionString = null, string databaseName = null)
//         {
//             Connection = connectionString ?? "mongodb://localhost:27017";
//             DatabaseName = databaseName ?? "Gastromio_Integration_Test";
//             UsedAssembly = typeof(DbAdminService).Assembly;
//             Client = new MongoClient(Connection);
//             Database = Client.GetDatabase(DatabaseName);
//             Migration = new MigrationEngine()
//                 .UseDatabase(Connection, DatabaseName)
//                 .UseAssembly(UsedAssembly)
//                 .UseSchemeValidation(false, null);
//
//             SetupDatabase();
//         }
//
//         private void DropDatabase()
//         {
//             Client.DropDatabase(DatabaseName);
//         }
//
//         private void SetupDatabase()
//         {
//             if (Database == null)
//                 throw new InvalidOperationException("Unable to setup Database!");
//
//             Migration.Run(DatabaseVersions.Initial);
//         }
//
//         public void Dispose()
//         {
//             DropDatabase();
//         }
//     }
// }
