using System;
using FoodOrderSystem.Core.Application.Ports.Persistence;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace FoodOrderSystem.Persistence.MongoDB
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMongoDB(this IServiceCollection services, string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            services.AddSingleton<IMongoClient>(client);

            var database = client.GetDatabase(databaseName);
            services.AddSingleton(database);

            services.AddTransient<IDbAdminService, DbAdminService>();
            services.AddTransient<ICuisineRepository, CuisineRepository>();
            services.AddTransient<IDishCategoryRepository, DishCategoryRepository>();
            services.AddTransient<IDishRepository, DishRepository>();
            services.AddTransient<IRestaurantRepository, RestaurantRepository>();
            services.AddTransient<IRestaurantImageRepository, RestaurantImageRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
        }
    }
}