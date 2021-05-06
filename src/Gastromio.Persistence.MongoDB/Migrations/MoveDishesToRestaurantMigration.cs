using MongoDB.Driver;
using MongoDBMigrations;
using System;

namespace Gastromio.Persistence.MongoDB.Migrations
{
    public class MoveDishesToRestaurantMigration : IMigration
    {
        public MongoDBMigrations.Version Version => DatabaseVersions.MoveDishesToRestaurantSchema;

        public string Name => "Change database schema so that DishCategories, Dishes and DishVariants are all directly in the Restaurant Model";

        public void Down(IMongoDatabase database)
        {
            throw new NotImplementedException();
        }

        public void Up(IMongoDatabase database)
        {
            // Dummy
        }
    }
}
