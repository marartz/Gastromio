using MongoDB.Driver;

namespace FoodOrderSystem.Persistence.MongoDB
{
    public class DbAdminService : IDbAdminService
    {
        private readonly IMongoClient client;
        private readonly IMongoDatabase database;

        public DbAdminService(IMongoClient client, IMongoDatabase database)
        {
            this.client = client;
            this.database = database;
        }
        
        public void PurgeDatabase()
        {
            client.DropDatabase(Constants.DatabaseName);
        }

        public void PrepareDatabase()
        {
            var cuisineCollection = database.GetCollection<CuisineModel>(Constants.CuisineCollectionName);
            cuisineCollection.Indexes.CreateOne(
                new CreateIndexModel<CuisineModel>(Builders<CuisineModel>.IndexKeys.Ascending(x => x.Name)));

            var dishCategoryCollection = database.GetCollection<DishCategoryModel>(Constants.DishCategoryCollectionName);
            dishCategoryCollection.Indexes.CreateOne(
                new CreateIndexModel<DishCategoryModel>(Builders<DishCategoryModel>.IndexKeys.Ascending(x => x.RestaurantId)));

            var dishCollection = database.GetCollection<DishModel>(Constants.DishCollectionName);
            dishCollection.Indexes.CreateOne(
                new CreateIndexModel<DishModel>(Builders<DishModel>.IndexKeys.Ascending(x => x.RestaurantId)));
            dishCollection.Indexes.CreateOne(
                new CreateIndexModel<DishModel>(Builders<DishModel>.IndexKeys.Ascending(x => x.CategoryId)));

            var restaurantCollection = database.GetCollection<RestaurantModel>(Constants.RestaurantCollectionName);
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
            
            var restaurantImageCollection = database.GetCollection<RestaurantImageModel>(Constants.RestaurantImageCollectionName);
            restaurantImageCollection.Indexes.CreateOne(
                new CreateIndexModel<RestaurantImageModel>(Builders<RestaurantImageModel>.IndexKeys.Ascending(x => x.RestaurantId)));
            restaurantImageCollection.Indexes.CreateOne(
                new CreateIndexModel<RestaurantImageModel>(Builders<RestaurantImageModel>.IndexKeys.Ascending(x => x.Type)));
            
            var userCollection = database.GetCollection<UserModel>(Constants.UserCollectionName);
            userCollection.Indexes.CreateOne(
                new CreateIndexModel<UserModel>(Builders<UserModel>.IndexKeys.Ascending(x => x.Email)));

            var orderCollection = database.GetCollection<OrderModel>(Constants.OrderCollectionName);
            orderCollection.Indexes.CreateOne(
                new CreateIndexModel<OrderModel>(
                    Builders<OrderModel>.IndexKeys.Ascending(x => x.CartInfo.RestaurantId)));
            
            orderCollection.Indexes.CreateOne(
                new CreateIndexModel<OrderModel>(
                    Builders<OrderModel>.IndexKeys.Ascending(x => x.CustomerNotificationInfo.Status)));
            
            orderCollection.Indexes.CreateOne(
                new CreateIndexModel<OrderModel>(
                    Builders<OrderModel>.IndexKeys.Ascending(x => x.RestaurantNotificationInfo.Status)));
        }

        public void CorrectRestaurantAliases()
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