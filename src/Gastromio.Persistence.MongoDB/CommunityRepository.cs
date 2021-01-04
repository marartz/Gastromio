using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gastromio.Core.Application.Ports.Persistence;
using Gastromio.Core.Domain.Model.Community;
using Gastromio.Core.Domain.Model.User;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gastromio.Persistence.MongoDB
{
    public class CommunityRepository : ICommunityRepository
    {
         private readonly IMongoDatabase database;

        public CommunityRepository(IMongoDatabase database)
        {
            this.database = database;
        }
        
        public async Task<IEnumerable<Community>> FindAllAsync(CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            var cursor = await collection.FindAsync(new BsonDocument(),
                new FindOptions<CommunityModel>
                {
                    Sort = Builders<CommunityModel>.Sort.Ascending(en => en.Name)
                },
                cancellationToken);
            return cursor.ToEnumerable().Select(FromDocument);
        }

        public async Task<Community> FindByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            
            var cursor = await collection.FindAsync(Builders<CommunityModel>.Filter.Eq(en => en.Name, name),
                cancellationToken: cancellationToken);
            var document = await cursor.FirstOrDefaultAsync(cancellationToken);
            return document != null ? FromDocument(document) : null;
        }

        public async Task<Community> FindByCommunityIdAsync(CommunityId communityId, CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            var cursor = await collection.FindAsync(Builders<CommunityModel>.Filter.Eq(en => en.Id, communityId.Value),
                cancellationToken: cancellationToken);
            var document = await cursor.FirstOrDefaultAsync(cancellationToken);
            return document != null ? FromDocument(document) : null;
        }

        public async Task StoreAsync(Community community, CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            var filter = Builders<CommunityModel>.Filter.Eq(en => en.Id, community.Id.Value);
            var document = ToDocument(community);
            var options = new ReplaceOptions { IsUpsert = true };
            await collection.ReplaceOneAsync(filter, document, options, cancellationToken);
        }

        public async Task RemoveAsync(CommunityId communityId, CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            await collection.DeleteOneAsync(Builders<CommunityModel>.Filter.Eq(en => en.Id, communityId.Value),
                cancellationToken);
        }

        private IMongoCollection<CommunityModel> GetCollection()
        {
            return database.GetCollection<CommunityModel>(Constants.CommunityCollectionName);
        }
        
        private static Community FromDocument(CommunityModel row)
        {
            return new Community(
                new CommunityId(row.Id),
                row.Name,
                row.CreatedOn,
                new UserId(row.CreatedBy),
                row.UpdatedOn,
                new UserId(row.UpdatedBy)
            );
        }

        private static CommunityModel ToDocument(Community obj)
        {
            return new CommunityModel
            {
                Id = obj.Id.Value,
                Name = obj.Name,
                CreatedOn = obj.CreatedOn,
                CreatedBy = obj.CreatedBy.Value,
                UpdatedOn = obj.UpdatedOn,
                UpdatedBy = obj.UpdatedBy.Value
            };
        }
        
    }
}