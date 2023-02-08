using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gastromio.Persistence.MongoDB.Migrations
{
    public class CorrectRestaurantAliasesMigration : IMigration
    {
        public Version Version => DatabaseVersions.CorrectRestaurantAliases;
        public string Name => "Correct Restaurant Aliases";

        public void Down(IMongoDatabase database)
        {
            // Old restaurant names are not stored -> no point of return!
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
