﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.RestaurantImage;
using FoodOrderSystem.Domain.Model.User;
using MongoDB.Driver;

namespace FoodOrderSystem.Persistence.MongoDB
{
    public class RestaurantImageRepository : IRestaurantImageRepository
    {
        private readonly IMongoDatabase database;

        public RestaurantImageRepository(IMongoDatabase database)
        {
            this.database = database;
        }

        public async Task<RestaurantImage> FindByRestaurantImageIdAsync(RestaurantImageId restaurantImageId,
            CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            var cursor = await collection.FindAsync(
                Builders<RestaurantImageModel>.Filter.Eq(en => en.Id, restaurantImageId.Value),
                cancellationToken: cancellationToken);
            var document = await cursor.FirstOrDefaultAsync(cancellationToken);
            return document != null ? FromDocument(document) : null;
        }

        public async Task<RestaurantImage> FindByRestaurantIdAndTypeAsync(RestaurantId restaurantId, string type,
            CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            var filter = Builders<RestaurantImageModel>.Filter.Eq(en => en.RestaurantId, restaurantId.Value);
            filter &= Builders<RestaurantImageModel>.Filter.Eq(en => en.Type, type);
            var cursor = await collection.FindAsync(filter, cancellationToken: cancellationToken);
            var document = await cursor.FirstOrDefaultAsync(cancellationToken);
            return document != null ? FromDocument(document) : null;
        }

        public async Task<IEnumerable<RestaurantImage>> FindByRestaurantIdAsync(RestaurantId restaurantId,
            CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            var cursor = await collection.FindAsync(
                Builders<RestaurantImageModel>.Filter.Eq(en => en.RestaurantId, restaurantId.Value),
                new FindOptions<RestaurantImageModel>
                {
                    Sort = Builders<RestaurantImageModel>.Sort.Ascending(en => en.Type)
                },
                cancellationToken);
            return cursor.ToEnumerable().Select(FromDocument);
        }

        public async Task StoreAsync(RestaurantImage restaurantImage, CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            var filter = Builders<RestaurantImageModel>.Filter.Eq(en => en.Id, restaurantImage.Id.Value);
            var document = ToDocument(restaurantImage);
            var options = new ReplaceOptions {IsUpsert = true};
            await collection.ReplaceOneAsync(filter, document, options, cancellationToken);
        }

        public async Task RemoveByRestaurantImageId(RestaurantImageId restaurantImageId, string type,
            CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            await collection.DeleteOneAsync(
                Builders<RestaurantImageModel>.Filter.Eq(en => en.Id, restaurantImageId.Value), cancellationToken);
        }

        public async Task RemoveByRestaurantIdAndTypeAsync(RestaurantId restaurantId, string type,
            CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            var filter = Builders<RestaurantImageModel>.Filter.Eq(en => en.RestaurantId, restaurantId.Value);
            filter &= Builders<RestaurantImageModel>.Filter.Eq(en => en.Type, type);
            await collection.DeleteManyAsync(filter, cancellationToken);
        }

        public async Task RemoveByRestaurantIdAsync(RestaurantId restaurantId, CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            var filter = Builders<RestaurantImageModel>.Filter.Eq(en => en.RestaurantId, restaurantId.Value);
            await collection.DeleteManyAsync(filter, cancellationToken);
        }

        private IMongoCollection<RestaurantImageModel> GetCollection()
        {
            return database.GetCollection<RestaurantImageModel>(Constants.RestaurantImageCollectionName);
        }

        private static RestaurantImage FromDocument(RestaurantImageModel document)
        {
            return new RestaurantImage(
                new RestaurantImageId(document.Id), 
                new RestaurantId(document.RestaurantId),
                document.Type,
                document.Data,
                document.CreatedOn,
                new UserId(document.CreatedBy),
                document.UpdatedOn,
                new UserId(document.UpdatedBy)
            );
        }

        private static RestaurantImageModel ToDocument(RestaurantImage obj)
        {
            return new RestaurantImageModel
            {
                Id = obj.Id.Value,
                RestaurantId = obj.RestaurantId.Value,
                Type = obj.Type,
                Data = obj.Data,
                CreatedOn = obj.CreatedOn, 
                CreatedBy = obj.CreatedBy.Value,
                UpdatedOn = obj.UpdatedOn,
                UpdatedBy = obj.UpdatedBy.Value
            };
        }
    }
}