﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Domain.Model.Dish;
using FoodOrderSystem.Domain.Model.DishCategory;
using FoodOrderSystem.Domain.Model.Restaurant;
using MongoDB.Driver;

namespace FoodOrderSystem.Persistence.MongoDB
{
    public class DishRepository : IDishRepository
    {
        private readonly IMongoDatabase database;

        public DishRepository(IMongoDatabase database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<Dish>> FindByRestaurantIdAsync(RestaurantId restaurantId,
            CancellationToken cancellationToken)
        {
            var collection = GetCollection();
            var cursor = await collection.FindAsync(Builders<DishModel>.Filter.Eq(en => en.RestaurantId, restaurantId.Value),
                new FindOptions<DishModel>
                {
                    Sort = Builders<DishModel>.Sort.Combine(Builders<DishModel>.Sort.Ascending(en => en.OrderNo),
                        Builders<DishModel>.Sort.Ascending(en => en.Name))
                },
                cancellationToken);
            return cursor.ToEnumerable().Select(FromDocument);
        }

        public async Task<IEnumerable<Dish>> FindByDishCategoryIdAsync(DishCategoryId dishCategoryId,
            CancellationToken cancellationToken)
        {
            var collection = GetCollection();
            var cursor = await collection.FindAsync(Builders<DishModel>.Filter.Eq(en => en.CategoryId, dishCategoryId.Value),
                new FindOptions<DishModel>
                {
                    Sort = Builders<DishModel>.Sort.Ascending(en => en.OrderNo)
                },
                cancellationToken);
            return cursor.ToEnumerable().Select(FromDocument);
        }

        public async Task<Dish> FindByDishIdAsync(DishId dishId, CancellationToken cancellationToken)
        {
            var collection = GetCollection();
            var cursor = await collection.FindAsync(Builders<DishModel>.Filter.Eq(en => en.Id, dishId.Value),
                cancellationToken: cancellationToken);
            var document = await cursor.FirstOrDefaultAsync(cancellationToken);
            return document != null ? FromDocument(document) : null;
        }

        public async Task StoreAsync(Dish dish, CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            var filter = Builders<DishModel>.Filter.Eq(en => en.Id, dish.Id.Value);
            var document = ToDocument(dish);
            var options = new ReplaceOptions { IsUpsert = true };
            await collection.ReplaceOneAsync(filter, document, options, cancellationToken);
        }

        public async Task RemoveByRestaurantIdAsync(RestaurantId restaurantId, CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            await collection.DeleteManyAsync(Builders<DishModel>.Filter.Eq(en => en.RestaurantId, restaurantId.Value),
                cancellationToken);
        }

        public async Task RemoveByDishCategoryIdAsync(DishCategoryId dishCategoryId, CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            await collection.DeleteManyAsync(Builders<DishModel>.Filter.Eq(en => en.CategoryId, dishCategoryId.Value),
                cancellationToken);
        }

        public async Task RemoveAsync(DishId dishId, CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            await collection.DeleteOneAsync(Builders<DishModel>.Filter.Eq(en => en.Id, dishId.Value),
                cancellationToken);
        }

        private IMongoCollection<DishModel> GetCollection()
        {
            return database.GetCollection<DishModel>(Constants.DishCollectionName);
        }
        
        private static Dish FromDocument(DishModel model)
        {
            return new Dish(
                new DishId(model.Id),
                new RestaurantId(model.RestaurantId),
                new DishCategoryId(model.CategoryId),
                model.Name,
                model.Description,
                model.ProductInfo,
                model.OrderNo,
                model.Variants != null
                    ? model.Variants
                        .Select(variantDocument => new DishVariant(
                            variantDocument.VariantId,
                            variantDocument.Name,
                            variantDocument.Price,
                            new List<DishVariantExtra>()
                        )).ToList()
                    : new List<DishVariant>()
            );
        }

        private static DishModel ToDocument(Dish obj)
        {
            return new DishModel
            {
                Id = obj.Id.Value,
                RestaurantId = obj.RestaurantId.Value,
                CategoryId = obj.CategoryId.Value,
                Name = obj.Name,
                Description = obj.Description,
                ProductInfo = obj.ProductInfo,
                OrderNo = obj.OrderNo,
                Variants = obj.Variants
                    .Select(variant => new DishVariantModel()
                    {
                        VariantId = variant.VariantId,
                        Name = variant.Name,
                        Price = variant.Price
                    }).ToList()
            };
        }
    }
}