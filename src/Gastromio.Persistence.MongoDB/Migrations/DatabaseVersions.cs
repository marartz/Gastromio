namespace Gastromio.Persistence.MongoDB.Migrations
{
    public static class DatabaseVersions
    {
        public static MongoDBMigrations.Version Initial => new MongoDBMigrations.Version(1, 0, 0);
        public static MongoDBMigrations.Version CorrectRestaurantAliases => new MongoDBMigrations.Version(1, 0, 1);
        public static MongoDBMigrations.Version MoveDishesToRestaurantSchema => new MongoDBMigrations.Version(2, 0, 0);
    }
}
