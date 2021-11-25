using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading;
using System.Threading.Tasks;

namespace Gastromio.Persistence.MongoDB
{
    public static class MongoDatabaseExtensions
    {
        public static bool CollectionExists(this IMongoDatabase db, string collectionName)
        {
            var filter = new BsonDocument("name", collectionName);
            //filter by collection name
            var collections = db.ListCollections(new ListCollectionsOptions { Filter = filter });
            //check for existence
            return collections.Any();
        }

        public async static Task<bool> CollectionExistsAsync(this IMongoDatabase db, string collectionName, CancellationToken cancellationToken = default)
        {
            var filter = new BsonDocument("name", collectionName);
            //filter by collection name
            var collections = await db.ListCollectionsAsync(new ListCollectionsOptions { Filter = filter });
            //check for existence
            return await collections.AnyAsync(cancellationToken);
        }
    }
}
