namespace Gastromio.Persistence.MongoDB
{
    public interface IDbAdminService
    {
        void PurgeDatabase();

        void PrepareDatabase();

        void CheckAndRunDatabaseMigrations();
    }
}