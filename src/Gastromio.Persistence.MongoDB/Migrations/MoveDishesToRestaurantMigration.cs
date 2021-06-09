using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBMigrations;
using System.Collections.Generic;

namespace Gastromio.Persistence.MongoDB.Migrations
{
    public class MoveDishesToRestaurantMigration : IMigration
    {
        private static string DishCategoryCollectionName = "dish_categories";
        private static string DishCollectionName = "dishes";
        private static string RestaurantCollectionName = "restaurants";

        public MongoDBMigrations.Version Version => DatabaseVersions.MoveDishesToRestaurantSchema;

        public string Name => "Change database schema so that DishCategories, Dishes and DishVariants are all directly in the Restaurant Model";

        public void Down(IMongoDatabase database)
        {
        }

        public void Up(IMongoDatabase database)
        {
            var restaurantCollection = database.GetCollection<BsonDocument>(RestaurantCollectionName);
            var dishCategoryCollection = database.GetCollection<BsonDocument>(DishCategoryCollectionName);
            var dishCollection = database.GetCollection<BsonDocument>(DishCollectionName);
            var restaurants = restaurantCollection.Find(_ => true).ToEnumerable();
            foreach (var restaurant in restaurants)
            {
                var restaurantId = restaurant.GetValue("_id");
                var dishCategories = dishCategoryCollection
                    .Find(new BsonDocument { { "RestaurantId", restaurantId } })
                    .Project(Builders<BsonDocument>.Projection
                        .Exclude("RestaurantId")
                        .Exclude("CreatedBy")
                        .Exclude("CreatedOn")
                        .Exclude("UpdatedOn")
                        .Exclude("UpdatedBy")
                    ).ToEnumerable();
                var categories = new List<BsonDocument>();
                foreach (var dishCatogory in dishCategories)
                {
                    var dishCategoryId = dishCatogory.GetValue("_id");
                    var dishes = dishCollection
                        .Find(new BsonDocument { { "RestaurantId", restaurantId }, { "CategoryId", dishCategoryId } })
                        .Project(Builders<BsonDocument>.Projection
                            .Exclude("RestaurantId")
                            .Exclude("CategoryId")
                            .Exclude("CreatedBy")
                            .Exclude("CreatedOn")
                            .Exclude("UpdatedOn")
                            .Exclude("UpdatedBy")
                        ).ToEnumerable();
                    categories.Add(dishCatogory.Set("Dishes", new BsonArray(dishes)));
                }
                restaurantCollection.UpdateOne(
                    Builders<BsonDocument>.Filter.Eq("_id", restaurantId),
                    Builders<BsonDocument>.Update.Set("DishCategories", new BsonArray(categories))
                );
            }

            database.DropCollection(DishCollectionName);
            database.DropCollection(DishCategoryCollectionName);
        }
    }
}
