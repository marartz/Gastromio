using Gastromio.Core.Application.Ports.Persistence;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDBMigrations;
using MongoDBMigrations.Document;

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

            services.AddTransient<IDbAdminService>(provider => new DbAdminService(provider.GetRequiredService<IMongoClient>(), connectionString));
            services.AddTransient<ICuisineRepository, CuisineRepository>();
            services.AddTransient<IRestaurantRepository, RestaurantRepository>();
            services.AddTransient<IRestaurantImageRepository, RestaurantImageRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();

            // Workaround: https://bitbucket.org/i_am_a_kernel/mongodbmigrations/issues/9/mongodatabasestatecheckerisdatabaseoutdate
            BsonClassMap.RegisterClassMap<SpecificationItem>(cm =>
            {
                cm.AutoMap();
                cm.GetMemberMap(x => x.Ver)
                .SetSerializer(new VerstionSerializer());
            });
        }
    }
}
