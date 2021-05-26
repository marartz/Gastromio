using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBMigrations;
using System;

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
                var restaurantId = restaurant.GetValue("Id");
                var dishCategories = dishCategoryCollection
                    .Find(Builders<BsonDocument>.Filter.Where(categoryDoc => categoryDoc.GetValue("RestaurantId").Equals(restaurantId)))
                    .Project(Builders<BsonDocument>.Projection
                        .Exclude("RestaurantId")
                        .Exclude("CreateyBy")
                        .Exclude("CreatedOn")
                        .Exclude("UpdatedOn")
                        .Exclude("UpdatedBy")
                    ).ToEnumerable();
                foreach (var dishCatogory in dishCategories)
                {
                    var dishCategoryId = dishCatogory.GetValue("Id");
                    var dishes = dishCollection
                        .Find(
                            Builders<BsonDocument>.Filter.Where(dishDoc => dishDoc.GetValue("RestaurantId").Equals(restaurantId)
                                && dishDoc.GetValue("CategoryId").Equals(dishCategoryId))
                        ).Project(Builders<BsonDocument>.Projection
                            .Exclude("RestaurantId")
                            .Exclude("CategoryId")
                            .Exclude("CreateyBy")
                            .Exclude("CreatedOn")
                            .Exclude("UpdatedOn")
                            .Exclude("UpdatedBy")
                        ).ToEnumerable();
                    dishCatogory.Add(new BsonElement("Dishes", new BsonArray(dishes)));
                }
                restaurant.Add(new BsonElement("DishCategories", new BsonArray(dishCategories)));
            }

            database.DropCollection(DishCollectionName);
            database.DropCollection(DishCategoryCollectionName);
        }
    }
}
