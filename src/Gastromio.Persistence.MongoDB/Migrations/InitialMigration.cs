using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gastromio.Persistence.MongoDB.Migrations
{
    public class InitialMigration : IMigration
    {
        public Version Version => DatabaseVersions.Initial;

        private static string CuisineCollectionName = "cuisines";
        private static string DishCategoryCollectionName = "dish_categories";
        private static string DishCollectionName = "dishes";
        private static string RestaurantCollectionName = "restaurants";
        private static string RestaurantImageCollectionName = "restaurant_images";
        private static string UserCollectionName = "users";
        private static string OrderCollectionName = "orders";
        private static string[] CollectionNames = new[] { 
            CuisineCollectionName,
            DishCategoryCollectionName,
            DishCollectionName,
            RestaurantCollectionName,
            RestaurantImageCollectionName,
            UserCollectionName,
            OrderCollectionName
        };

        public string Name => "Initialise Database";

        public void Down(IMongoDatabase database)
        {
            foreach(string collection in CollectionNames)
            {
                database.DropCollection(collection);
            }
        }

        public void Up(IMongoDatabase database)
        {

            foreach (string collection in CollectionNames)
            {
                if(!database.CollectionExists(collection))
                    database.CreateCollection(collection);
            }
            
            var restaurantIdIndex = new CreateIndexModel<BsonDocument>(new BsonDocument("RestaurantId", 1));
            var nameIndex = new CreateIndexModel<BsonDocument>(new BsonDocument("Name", 1));
            var categoryIdIndex = new CreateIndexModel<BsonDocument>(new BsonDocument("CategoryId", 1));
            var importIdIndex = new CreateIndexModel<BsonDocument>(new BsonDocument("ImportId", 1));
            var typeIndex = new CreateIndexModel<BsonDocument>(new BsonDocument("Type", 1));
            var emailIndex = new CreateIndexModel<BsonDocument>(new BsonDocument("Email", 1));

            var pickupInfoEnabledIndex = new CreateIndexModel<BsonDocument>(new BsonDocument("PickupInfo.Enabled", 1));
            var deliveryInfoEnabledIndex = new CreateIndexModel<BsonDocument>(new BsonDocument("DeliveryInfo.Enabled", 1));
            var reservationInfoEnabledIndex = new CreateIndexModel<BsonDocument>(new BsonDocument("ReservationInfo.Enabled", 1));

            var orderCollectionRestaurantIdIndex = new CreateIndexModel<BsonDocument>(new BsonDocument("CartInfo.RestaurantId", 1));
            var orderCollectionCustomerStatusIndex = new CreateIndexModel<BsonDocument>(new BsonDocument("CustomerNotificationInfo.Status", 1));
            var orderCollectionRestaurantStatusIndex = new CreateIndexModel<BsonDocument>(new BsonDocument("RestaurantNotificationInfo.Status", 1));

            var cuisineCollection = database.GetCollection<BsonDocument>(CuisineCollectionName);
            cuisineCollection.Indexes.CreateOne(nameIndex);

            var dishCategoryCollection = database.GetCollection<BsonDocument>(DishCategoryCollectionName);
            dishCategoryCollection.Indexes.CreateOne(restaurantIdIndex);

            var dishCollection = database.GetCollection<BsonDocument>(DishCollectionName);
            dishCollection.Indexes.CreateMany(new[] { restaurantIdIndex, categoryIdIndex });

            var restaurantCollection = database.GetCollection<BsonDocument>(RestaurantCollectionName);
            restaurantCollection.Indexes.CreateMany(new []
            {
                importIdIndex,
                nameIndex,
                pickupInfoEnabledIndex,
                deliveryInfoEnabledIndex,
                reservationInfoEnabledIndex
            });

            var restaurantImageCollection = database.GetCollection<BsonDocument>(RestaurantImageCollectionName);
            restaurantImageCollection.Indexes.CreateMany(new[] { restaurantIdIndex, typeIndex });

            var userCollection = database.GetCollection<BsonDocument>(UserCollectionName);
            userCollection.Indexes.CreateOne(emailIndex);

            var orderCollection = database.GetCollection<BsonDocument>(OrderCollectionName);
            orderCollection.Indexes.CreateMany(new[]{
                orderCollectionRestaurantIdIndex,
                orderCollectionCustomerStatusIndex,
                orderCollectionRestaurantStatusIndex
            });
        }
    }
}
