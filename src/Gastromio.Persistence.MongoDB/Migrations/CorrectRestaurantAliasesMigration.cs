using MongoDB.Bson;
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
            var collection = database.GetCollection<BsonDocument>(Constants.RestaurantCollectionName);
            collection.Find(_ => true).ForEachAsync(doc =>
            {
                if (doc.TryGetValue("Name", out var name))
                {
                    var alias = RestaurantRepository.CreateAlias(name.AsString);
                    doc.Set("Alias", BsonValue.Create(alias));
                }
            });
        }
    }
}
