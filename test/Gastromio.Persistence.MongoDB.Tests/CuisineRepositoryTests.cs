using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gastromio.Core.Domain.Model.Cuisine;
using Gastromio.Core.Domain.Model.User;
using MongoDB.Bson;
using MongoDB.Driver;
using Xunit;

namespace Gastromio.Persistence.MongoDB.Tests
{
    public class CuisineRepositoryTests
    {
        private readonly IMongoCollection<CuisineModel> cuisineCollection;
        private readonly IMongoDatabase database;

        public CuisineRepositoryTests()
        {
            var client = new MongoClient();
            client.DropDatabase(Constants.DatabaseName);

            database = client.GetDatabase(ConstantsExt.TestDatabaseName);
            database.DropCollection(Constants.CuisineCollectionName);

            cuisineCollection = database.GetCollection<CuisineModel>(Constants.CuisineCollectionName);
        }

        [Fact]
        public async Task FindAllAsync()
        {
            var cuisineDocuments = CreateTestData();
            await cuisineCollection.InsertManyAsync(cuisineDocuments);

            var target = new CuisineRepository(database);
            var result = (await target.FindAllAsync()).ToList();

            Assert.NotNull(result);
            Assert.Equal(100, result.Count());
        }

        [Fact]
        public async Task StoreAsync_NonExistent_InsertsCuisine()
        {
            var collection = database.GetCollection<CuisineModel>(Constants.CuisineCollectionName);
            var count = await collection.CountDocumentsAsync(new BsonDocument());
            Assert.Equal(0, count);

            var userId = new UserId(Guid.NewGuid());

            var target = new CuisineRepository(database);
            var cuisineId = Guid.NewGuid();
            await target.StoreAsync(new Cuisine(
                new CuisineId(cuisineId),
                "test",
                "italienisch",
                DateTime.UtcNow,
                userId,
                DateTime.UtcNow,
                userId
            ));

            count = await collection.CountDocumentsAsync(new BsonDocument());
            Assert.Equal(1, count);

            var cursor = await collection.FindAsync(Builders<CuisineModel>.Filter.Eq(en => en.Id, cuisineId));
            var document = await cursor.FirstOrDefaultAsync();
            Assert.NotNull(document);
            Assert.Equal(cuisineId, document.Id);
            Assert.Equal("test", document.Name);
        }

        private List<CuisineModel> CreateTestData()
        {
            var cuisineDocuments = new List<CuisineModel>();

            for (var i = 0; i < 100; i++)
            {
                cuisineDocuments.Add(new CuisineModel
                {
                    Id = Guid.NewGuid(),
                    Name = $"cuisine{i + 1:D2}"
                });
            }

            return cuisineDocuments;
        }
    }
}