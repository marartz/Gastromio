using Gastromio.Core.Application.Ports.Persistence;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Gastromio.Persistence.MongoDB
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMongoDB(this IServiceCollection services, string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            services.AddSingleton<IMongoClient>(client);

            var database = client.GetDatabase(databaseName);
            services.AddSingleton(database);

            services.AddTransient<IDbAdminService>(provider => new DbAdminService(provider.GetRequiredService<IMongoClient>(), connectionString, databaseName));
            services.AddTransient<ICuisineRepository, CuisineRepository>();
            services.AddTransient<IRestaurantRepository, RestaurantRepository>();
            services.AddTransient<IRestaurantImageRepository, RestaurantImageRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
        }
    }
}
