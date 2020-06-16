using System;
using FoodOrderSystem.Domain.Model.Cuisine;
using FoodOrderSystem.Domain.Model.Dish;
using FoodOrderSystem.Domain.Model.DishCategory;
using FoodOrderSystem.Domain.Model.Order;
using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace FoodOrderSystem.Persistence.MongoDB
{
    public static class Initializer
    {
        public static void ConfigureServices(IServiceCollection services, string connectionString)
        {
            var client = new MongoClient(connectionString);
            services.AddSingleton<IMongoClient>(client);

            var database = client.GetDatabase(Constants.DatabaseName);
            services.AddSingleton(database);

            services.AddTransient<ICuisineRepository, CuisineRepository>();
            services.AddTransient<IDishCategoryRepository, DishCategoryRepository>();
            services.AddTransient<IDishRepository, DishRepository>();
            services.AddTransient<IRestaurantRepository, RestaurantRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
        }

        public static void PurgeDatabase(IServiceProvider serviceProvider)
        {
            var client = serviceProvider.GetService<IMongoClient>();
            client.DropDatabase(Constants.DatabaseName);
        }

        public static void PrepareDatabase(IServiceProvider serviceProvider)
        {
            var database = serviceProvider.GetService<IMongoDatabase>();

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
                new CreateIndexModel<RestaurantModel>(Builders<RestaurantModel>.IndexKeys.Ascending(x => x.Name)));
            
            var userCollection = database.GetCollection<UserModel>(Constants.UserCollectionName);
            userCollection.Indexes.CreateOne(
                new CreateIndexModel<UserModel>(Builders<UserModel>.IndexKeys.Ascending(x => x.Email)));

            var orderCollection = database.GetCollection<OrderModel>(Constants.OrderCollectionName);
            orderCollection.Indexes.CreateOne(
                new CreateIndexModel<OrderModel>(
                    Builders<OrderModel>.IndexKeys.Ascending(x => x.CartInfo.RestaurantId)));
        }
    }
}