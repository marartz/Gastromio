using MongoDB.Driver;
using MongoDBMigrations;
using System;

namespace Gastromio.Persistence.MongoDB.Migrations
{
    public class InitialMigration : IMigration
    {
        public MongoDBMigrations.Version Version => DatabaseVersions.Initial;

        private static string CuisineCollectionName = "cuisines";
        private static string DishCategoryCollectionName = "dish_categories";
        private static string DishCollectionName = "dishes";
        private static string RestaurantCollectionName = "restaurants";
        private static string RestaurantImageCollectionName = "restaurant_images";
        private static string UserCollectionName = "users";
        private static string OrderCollectionName = "orders";

        public string Name => "Initialise Database";

        public void Down(IMongoDatabase database)
        {
            throw new NotImplementedException();
        }

        public void Up(IMongoDatabase database)
        {
            var cuisineCollection = database.GetCollection<CuisineModel>(CuisineCollectionName);
            cuisineCollection.Indexes.CreateOne(
                new CreateIndexModel<CuisineModel>(Builders<CuisineModel>.IndexKeys.Ascending(x => x.Name)));

            // TODO: Models of dish category and dishes have been changed!

            // var dishCategoryCollection = database.GetCollection<DishCategoryModel>(DishCategoryCollectionName);
            // dishCategoryCollection.Indexes.CreateOne(
            //     new CreateIndexModel<DishCategoryModel>(Builders<DishCategoryModel>.IndexKeys.Ascending(x => x.RestaurantId)));
            //
            // var dishCollection = database.GetCollection<DishModel>(DishCollectionName);
            // dishCollection.Indexes.CreateOne(
            //     new CreateIndexModel<DishModel>(Builders<DishModel>.IndexKeys.Ascending(x => x.RestaurantId)));
            // dishCollection.Indexes.CreateOne(
            //     new CreateIndexModel<DishModel>(Builders<DishModel>.IndexKeys.Ascending(x => x.CategoryId)));

            var restaurantCollection = database.GetCollection<RestaurantModel>(RestaurantCollectionName);
            restaurantCollection.Indexes.CreateOne(
                new CreateIndexModel<RestaurantModel>(Builders<RestaurantModel>.IndexKeys.Ascending(x => x.ImportId)));
            restaurantCollection.Indexes.CreateOne(
                new CreateIndexModel<RestaurantModel>(Builders<RestaurantModel>.IndexKeys.Ascending(x => x.Name)));
            restaurantCollection.Indexes.CreateOne(
                new CreateIndexModel<RestaurantModel>(Builders<RestaurantModel>.IndexKeys.Ascending(x => x.PickupInfo.Enabled)));
            restaurantCollection.Indexes.CreateOne(
                new CreateIndexModel<RestaurantModel>(Builders<RestaurantModel>.IndexKeys.Ascending(x => x.DeliveryInfo.Enabled)));
            restaurantCollection.Indexes.CreateOne(
                new CreateIndexModel<RestaurantModel>(Builders<RestaurantModel>.IndexKeys.Ascending(x => x.ReservationInfo.Enabled)));

            var restaurantImageCollection = database.GetCollection<RestaurantImageModel>(RestaurantImageCollectionName);
            restaurantImageCollection.Indexes.CreateOne(
                new CreateIndexModel<RestaurantImageModel>(Builders<RestaurantImageModel>.IndexKeys.Ascending(x => x.RestaurantId)));
            restaurantImageCollection.Indexes.CreateOne(
                new CreateIndexModel<RestaurantImageModel>(Builders<RestaurantImageModel>.IndexKeys.Ascending(x => x.Type)));

            var userCollection = database.GetCollection<UserModel>(UserCollectionName);
            userCollection.Indexes.CreateOne(
                new CreateIndexModel<UserModel>(Builders<UserModel>.IndexKeys.Ascending(x => x.Email)));

            var orderCollection = database.GetCollection<OrderModel>(OrderCollectionName);
            orderCollection.Indexes.CreateOne(
                new CreateIndexModel<OrderModel>(
                    Builders<OrderModel>.IndexKeys.Ascending(x => x.CartInfo.RestaurantId)));

            orderCollection.Indexes.CreateOne(
                new CreateIndexModel<OrderModel>(
                    Builders<OrderModel>.IndexKeys.Ascending(x => x.CustomerNotificationInfo.Status)));

            orderCollection.Indexes.CreateOne(
                new CreateIndexModel<OrderModel>(
                    Builders<OrderModel>.IndexKeys.Ascending(x => x.RestaurantNotificationInfo.Status)));

            //var systemAdminAlreadyExists = userCollection.Find<UserModel>(
            //        Builders<UserModel>.Filter.Where(doc => doc.Role.Equals("SystemAdmin")))
            //    .Any();
            //if (!systemAdminAlreadyExists)
            //{
            //    var adminUser = new UserModel()
            //    {
            //        Id = Guid.NewGuid(),
            //        Role = "SystemAdmin",
            //        Email = "admin@gastromio.de",
            //        CreatedOn = DateTime.UtcNow,
            //        CreatedBy = Guid.Empty,
            //        UpdatedOn = DateTime.UtcNow,
            //        UpdatedBy = Guid.Empty,
            //    };
            //    userCollection.InsertOne(adminUser);
            //}
        }
    }
}
