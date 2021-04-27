using MongoDB.Driver;
using MongoDBMigrations;
using System;

namespace Gastromio.Persistence.MongoDB.Migrations
{
    public class CorrectRestaurantAliasesMigration : IMigration
    {
        public MongoDBMigrations.Version Version => new MongoDBMigrations.Version(1, 0, 1);
        public string Name => "Correct Restaurant Aliases";

        public void Down(IMongoDatabase database)
        {
            throw new NotImplementedException();
        }

        public void Up(IMongoDatabase database)
        {
            var collection = database.GetCollection<RestaurantModel>(Constants.RestaurantCollectionName);
            var documents = collection.Find(_ => true).ToList();

            foreach (var document in documents)
            {
                document.Alias = RestaurantRepository.CreateAlias(document.Name);
                var filter = Builders<RestaurantModel>.Filter.Eq(en => en.Id, document.Id);
                collection.ReplaceOne(filter, document);
            }
        }
    }
}
