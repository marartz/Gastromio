using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Domain.Model.PaymentMethod;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FoodOrderSystem.Persistence.MongoDB
{
    public class PaymentMethodRepository : IPaymentMethodRepository
    {
        private readonly IMongoDatabase database;

        public PaymentMethodRepository(IMongoDatabase database)
        {
            this.database = database;
        }
        
        public async Task<IEnumerable<PaymentMethod>> FindAllAsync(CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            var cursor = await collection.FindAsync(new BsonDocument(),
                new FindOptions<PaymentMethodModel>
                {
                    Sort = Builders<PaymentMethodModel>.Sort.Ascending(en => en.Name)
                },
                cancellationToken);
            return cursor.ToEnumerable().Select(FromDocument);
        }

        public async Task<PaymentMethod> FindByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            var cursor = await collection.FindAsync(Builders<PaymentMethodModel>.Filter.Eq(en => en.Name, name),
                cancellationToken: cancellationToken);
            var document = await cursor.FirstOrDefaultAsync(cancellationToken);
            return document != null ? FromDocument(document) : null;
        }

        public async Task<PaymentMethod> FindByPaymentMethodIdAsync(PaymentMethodId paymentMethodId, CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            var cursor = await collection.FindAsync(
                Builders<PaymentMethodModel>.Filter.Eq(en => en.Id, paymentMethodId.Value),
                cancellationToken: cancellationToken);
            var document = await cursor.FirstOrDefaultAsync(cancellationToken);
            return document != null ? FromDocument(document) : null;
        }

        public async Task StoreAsync(PaymentMethod paymentMethod, CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            var filter = Builders<PaymentMethodModel>.Filter.Eq(en => en.Id, paymentMethod.Id.Value);
            var document = ToDocument(paymentMethod);
            var options = new ReplaceOptions { IsUpsert = true };
            await collection.ReplaceOneAsync(filter, document, options, cancellationToken);
        }

        public async Task RemoveAsync(PaymentMethodId paymentMethodId, CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            await collection.DeleteOneAsync(Builders<PaymentMethodModel>.Filter.Eq(en => en.Id, paymentMethodId.Value),
                cancellationToken);
        }
        
        private IMongoCollection<PaymentMethodModel> GetCollection()
        {
            return database.GetCollection<PaymentMethodModel>(Constants.PaymentMethodCollectionName);
        }

        private static PaymentMethod FromDocument(PaymentMethodModel model)
        {
            return new PaymentMethod(new PaymentMethodId(model.Id),
                model.Name,
                model.Description
            );
        }

        private static PaymentMethodModel ToDocument(PaymentMethod obj)
        {
            return new PaymentMethodModel
            {
                Id = obj.Id.Value,
                Name = obj.Name,
                Description = obj.Description
            };
        }
    }
}