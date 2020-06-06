using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Domain.Model.Cuisine;
using FoodOrderSystem.Domain.Model.PaymentMethod;
using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FoodOrderSystem.Persistence.MongoDB
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly IMongoDatabase database;

        public RestaurantRepository(IMongoDatabase database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<Restaurant>> SearchAsync(string searchPhrase,
            CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            
            FilterDefinition<RestaurantModel> filter;
            if (!string.IsNullOrEmpty(searchPhrase))
            {
                var bsonRegEx = new BsonRegularExpression($".*{Regex.Escape(searchPhrase)}.*", "i");
                filter = Builders<RestaurantModel>.Filter.Regex(en => en.Name, bsonRegEx);
            }
            else
            {
                filter = new BsonDocument();
            }

            var cursor = await collection.FindAsync(filter,
                new FindOptions<RestaurantModel>
                {
                    Sort = Builders<RestaurantModel>.Sort.Ascending(en => en.Name)
                },
                cancellationToken);
            return cursor.ToEnumerable().Select(FromDocument);
        }

        public async Task<Restaurant> FindByRestaurantIdAsync(RestaurantId restaurantId,
            CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            var cursor = await collection.FindAsync(
                Builders<RestaurantModel>.Filter.Eq(en => en.Id, restaurantId.Value),
                cancellationToken: cancellationToken);
            var document = await cursor.FirstOrDefaultAsync(cancellationToken);
            return document != null ? FromDocument(document) : null;
        }

        public async Task<IEnumerable<Restaurant>> FindByCuisineIdAsync(CuisineId cuisineId, CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            var cursor = await collection.FindAsync(Builders<RestaurantModel>.Filter.AnyEq(en => en.Cuisines, cuisineId.Value),
                new FindOptions<RestaurantModel>
                {
                    Sort = Builders<RestaurantModel>.Sort.Ascending(en => en.Name)
                },
                cancellationToken);
            return cursor.ToEnumerable().Select(FromDocument);
        }

        public async Task<IEnumerable<Restaurant>> FindByPaymentMethodIdAsync(PaymentMethodId paymentMethodId, CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            var cursor = await collection.FindAsync(Builders<RestaurantModel>.Filter.AnyEq(en => en.PaymentMethods, paymentMethodId.Value),
                new FindOptions<RestaurantModel>
                {
                    Sort = Builders<RestaurantModel>.Sort.Ascending(en => en.Name)
                },
                cancellationToken);
            return cursor.ToEnumerable().Select(FromDocument);
        }

        public async Task<IEnumerable<Restaurant>> FindByUserIdAsync(UserId userId,
            CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            var cursor = await collection.FindAsync(Builders<RestaurantModel>.Filter.AnyEq(en => en.Administrators, userId.Value),
                new FindOptions<RestaurantModel>
                {
                    Sort = Builders<RestaurantModel>.Sort.Ascending(en => en.Name)
                },
                cancellationToken);
            return cursor.ToEnumerable().Select(FromDocument);
        }

        public async Task<IEnumerable<Restaurant>> FindAllAsync(CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            var cursor = await collection.FindAsync(new BsonDocument(),
                new FindOptions<RestaurantModel>
                {
                    Sort = Builders<RestaurantModel>.Sort.Ascending(en => en.Name)
                },
                cancellationToken);
            return cursor.ToEnumerable().Select(FromDocument);
        }

        public async Task StoreAsync(Restaurant restaurant, CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            var filter = Builders<RestaurantModel>.Filter.Eq(en => en.Id, restaurant.Id.Value);
            var document = ToDocument(restaurant);
            var options = new ReplaceOptions { IsUpsert = true };
            await collection.ReplaceOneAsync(filter, document, options, cancellationToken);
        }

        public async Task RemoveAsync(RestaurantId restaurantId, CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            await collection.DeleteOneAsync(Builders<RestaurantModel>.Filter.Eq(en => en.Id, restaurantId.Value),
                cancellationToken);
        }

        private IMongoCollection<RestaurantModel> GetCollection()
        {
            return database.GetCollection<RestaurantModel>(Constants.RestaurantCollectionName);
        }

        private static Restaurant FromDocument(RestaurantModel document)
        {
            return new Restaurant(new RestaurantId(document.Id),
                document.Name,
                document.Image,
                new Address(document.AddressStreet, document.AddressZipCode, document.AddressCity),
                document.DeliveryTimes.Select(en => new DeliveryTime(en.DayOfWeek, TimeSpan.FromMinutes(en.StartTime),
                    TimeSpan.FromMinutes(en.EndTime))).ToList(),
                document.MinimumOrderValue,
                document.DeliveryCosts,
                document.Phone,
                document.WebSite,
                document.Imprint,
                document.OrderEmailAddress,
                new HashSet<CuisineId>(document.Cuisines.Select(en => new CuisineId(en))),
                new HashSet<PaymentMethodId>(document.PaymentMethods.Select(en => new PaymentMethodId(en))),
                new HashSet<UserId>(document.Administrators.Select(en => new UserId(en)))
            );
        }

        private static RestaurantModel ToDocument(Restaurant obj)
        {
            return new RestaurantModel
            {
                Id = obj.Id.Value,
                Name = obj.Name,
                Image = obj.Image,
                AddressStreet = obj.Address?.Street,
                AddressZipCode = obj.Address?.ZipCode,
                AddressCity = obj.Address?.City,
                DeliveryTimes = obj.DeliveryTimes != null
                    ? obj.DeliveryTimes.Select(en => new DeliveryTimeModel
                    {
                        DayOfWeek = en.DayOfWeek,
                        StartTime = (int) en.Start.TotalMinutes,
                        EndTime = (int) en.End.TotalMinutes
                    }).ToList()
                    : new List<DeliveryTimeModel>(),
                MinimumOrderValue = obj.MinimumOrderValue,
                DeliveryCosts = obj.DeliveryCosts,
                Phone = obj.Phone,
                WebSite = obj.WebSite,
                Imprint = obj.Imprint,
                OrderEmailAddress = obj.OrderEmailAddress,
                Cuisines = obj.Cuisines != null ? obj.Cuisines.Select(en => en.Value).ToList() : new List<Guid>(),
                PaymentMethods = obj.PaymentMethods != null
                    ? obj.PaymentMethods.Select(en => en.Value).ToList()
                    : new List<Guid>(),
                Administrators = obj.Administrators != null
                    ? obj.Administrators.Select(en => en.Value).ToList()
                    : new List<Guid>()
            };
        }
    }
}