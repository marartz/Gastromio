using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Core.Application.Ports.Persistence;
using FoodOrderSystem.Core.Domain.Model.Cuisine;
using FoodOrderSystem.Core.Domain.Model.Order;
using FoodOrderSystem.Core.Domain.Model.PaymentMethod;
using FoodOrderSystem.Core.Domain.Model.Restaurant;
using FoodOrderSystem.Core.Domain.Model.User;
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

        public async Task<IEnumerable<Restaurant>> SearchAsync(string searchPhrase, OrderType? orderType,
            CuisineId cuisineId, DateTime? openingHour, bool? isActive, CancellationToken cancellationToken = default)
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

            if (orderType.HasValue)
            {
                switch (orderType.Value)
                {
                    case OrderType.Pickup:
                    {
                        filter &= Builders<RestaurantModel>.Filter.Eq(en => en.PickupInfo.Enabled, true);
                        break;
                    }
                    case OrderType.Delivery:
                    {
                        filter &= Builders<RestaurantModel>.Filter.Eq(en => en.DeliveryInfo.Enabled, true);
                        break;
                    }
                    case OrderType.Reservation:
                    {
                        filter &= Builders<RestaurantModel>.Filter.Eq(en => en.ReservationInfo.Enabled, true);
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (cuisineId != null)
            {
                filter &= Builders<RestaurantModel>.Filter.AnyEq(en => en.Cuisines, cuisineId.Value);
            }

            if (openingHour.HasValue)
            {
                filter &= GenerateOpeningHourFilterDefinition(openingHour.Value);
            }

            if (isActive.HasValue)
            {
                filter &= Builders<RestaurantModel>.Filter.Eq(en => en.IsActive, isActive.Value);
            }

            var cursor = await collection.FindAsync(filter,
                new FindOptions<RestaurantModel>
                {
                    Sort = Builders<RestaurantModel>.Sort.Ascending(en => en.Name)
                },
                cancellationToken);
            return cursor.ToEnumerable().Select(FromDocument);
        }

        public async Task<(long total, IEnumerable<Restaurant> items)> SearchPagedAsync(string searchPhrase,
            OrderType? orderType, CuisineId cuisineId, DateTime? openingHour, bool? isActive, int skip = 0,
            int take = -1, CancellationToken cancellationToken = default)
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

            if (orderType.HasValue)
            {
                switch (orderType.Value)
                {
                    case OrderType.Pickup:
                    {
                        filter &= Builders<RestaurantModel>.Filter.Eq(en => en.PickupInfo.Enabled, true);
                        break;
                    }
                    case OrderType.Delivery:
                    {
                        filter &= Builders<RestaurantModel>.Filter.Eq(en => en.DeliveryInfo.Enabled, true);
                        break;
                    }
                    case OrderType.Reservation:
                    {
                        filter &= Builders<RestaurantModel>.Filter.Eq(en => en.ReservationInfo.Enabled, true);
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (cuisineId != null)
            {
                filter &= Builders<RestaurantModel>.Filter.AnyEq(en => en.Cuisines, cuisineId.Value);
            }

            if (openingHour.HasValue)
            {
                filter &= GenerateOpeningHourFilterDefinition(openingHour.Value);
            }

            if (isActive.HasValue)
            {
                filter &= Builders<RestaurantModel>.Filter.Eq(en => en.IsActive, isActive.Value);
            }

            var total = await collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

            if (take == 0)
                return (total, Enumerable.Empty<Restaurant>());

            var findOptions = new FindOptions<RestaurantModel>
            {
                Sort = Builders<RestaurantModel>.Sort.Ascending(en => en.Name)
            };
            if (skip > 0)
                findOptions.Skip = skip;
            if (take >= 0)
                findOptions.Limit = take;

            var cursor = await collection.FindAsync(filter, findOptions, cancellationToken);
            return (total, cursor.ToEnumerable().Select(FromDocument));
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

        public async Task<IEnumerable<Restaurant>> FindByRestaurantNameAsync(string restaurantName, CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            var cursor = await collection.FindAsync(
                Builders<RestaurantModel>.Filter.Where(en => en.Name.ToLower() == restaurantName.ToLower()),
                cancellationToken: cancellationToken);
            return cursor.ToEnumerable().Select(FromDocument);
        }

        public async Task<Restaurant> FindByImportIdAsync(string importId,
            CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            var cursor = await collection.FindAsync(
                Builders<RestaurantModel>.Filter.Eq(en => en.ImportId, importId),
                cancellationToken: cancellationToken);
            var document = await cursor.FirstOrDefaultAsync(cancellationToken);
            return document != null ? FromDocument(document) : null;
        }

        public async Task<IEnumerable<Restaurant>> FindByCuisineIdAsync(CuisineId cuisineId,
            CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            var cursor = await collection.FindAsync(
                Builders<RestaurantModel>.Filter.AnyEq(en => en.Cuisines, cuisineId.Value),
                new FindOptions<RestaurantModel>
                {
                    Sort = Builders<RestaurantModel>.Sort.Ascending(en => en.Name)
                },
                cancellationToken);
            return cursor.ToEnumerable().Select(FromDocument);
        }

        public async Task<IEnumerable<Restaurant>> FindByPaymentMethodIdAsync(PaymentMethodId paymentMethodId,
            CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            var cursor = await collection.FindAsync(
                Builders<RestaurantModel>.Filter.AnyEq(en => en.PaymentMethods, paymentMethodId.Value),
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
            var cursor = await collection.FindAsync(
                Builders<RestaurantModel>.Filter.AnyEq(en => en.Administrators, userId.Value),
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
            var options = new ReplaceOptions {IsUpsert = true};
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

        private FilterDefinition<RestaurantModel> GenerateOpeningHourFilterDefinition(DateTime openingHourFilter)
        {
            int filterDayOfWeek;

            switch (openingHourFilter.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    filterDayOfWeek = 0;
                    break;
                case DayOfWeek.Tuesday:
                    filterDayOfWeek = 1;
                    break;
                case DayOfWeek.Wednesday:
                    filterDayOfWeek = 2;
                    break;
                case DayOfWeek.Thursday:
                    filterDayOfWeek = 3;
                    break;
                case DayOfWeek.Friday:
                    filterDayOfWeek = 4;
                    break;
                case DayOfWeek.Saturday:
                    filterDayOfWeek = 5;
                    break;
                case DayOfWeek.Sunday:
                    filterDayOfWeek = 6;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            const double earliestOpeningHour = 4d;

            int filterMinutes;

            if (openingHourFilter.Hour < earliestOpeningHour)
            {
                filterDayOfWeek = (filterDayOfWeek - 1) % 7;
                filterMinutes = (openingHourFilter.Hour + 24) * 60 + openingHourFilter.Minute;
            }
            else
            {
                filterMinutes = openingHourFilter.Hour * 60 + openingHourFilter.Minute;
            }

            var openingPeriodModelFilter = Builders<OpeningPeriodModel>.Filter.Eq(m => m.DayOfWeek, filterDayOfWeek) &
                                           Builders<OpeningPeriodModel>.Filter.Lte(m => m.StartTime, filterMinutes) &
                                           Builders<OpeningPeriodModel>.Filter.Gte(m => m.EndTime, filterMinutes);

            return Builders<RestaurantModel>.Filter.ElemMatch(m => m.OpeningHours, openingPeriodModelFilter);
        }

        private static Restaurant FromDocument(RestaurantModel document)
        {
            return new Restaurant(
                new RestaurantId(document.Id),
                document.Name,
                document.Address != null
                    ? new Address(
                        document.Address.Street,
                        document.Address.ZipCode,
                        document.Address.City
                    )
                    : null,
                document.ContactInfo != null
                    ? new ContactInfo(
                        document.ContactInfo.Phone,
                        document.ContactInfo.Fax,
                        document.ContactInfo.WebSite,
                        document.ContactInfo.ResponsiblePerson,
                        document.ContactInfo.EmailAddress
                    )
                    : null,
                document.OpeningHours?.Select(en => new OpeningPeriod(
                    en.DayOfWeek,
                    TimeSpan.FromMinutes(en.StartTime),
                    TimeSpan.FromMinutes(en.EndTime))).ToList(),
                document.PickupInfo != null
                    ? new PickupInfo(
                        document.PickupInfo.Enabled,
                        document.PickupInfo.AverageTime,
                        Converter.ToDecimal(document.PickupInfo.MinimumOrderValue),
                        Converter.ToDecimal(document.PickupInfo.MaximumOrderValue)
                    )
                    : null,
                document.DeliveryInfo != null
                    ? new DeliveryInfo(
                        document.DeliveryInfo.Enabled,
                        document.DeliveryInfo.AverageTime,
                        Converter.ToDecimal(document.DeliveryInfo.MinimumOrderValue),
                        Converter.ToDecimal(document.DeliveryInfo.MaximumOrderValue),
                        Converter.ToDecimal(document.DeliveryInfo.Costs)
                    )
                    : null,
                document.ReservationInfo != null
                    ? new ReservationInfo(document.ReservationInfo.Enabled)
                    : null,
                document.HygienicHandling,
                new HashSet<CuisineId>(document.Cuisines.Select(en => new CuisineId(en))),
                new HashSet<PaymentMethodId>(document.PaymentMethods.Select(en => new PaymentMethodId(en))),
                new HashSet<UserId>(document.Administrators.Select(en => new UserId(en))),
                document.ImportId,
                document.IsActive,
                document.NeedsSupport,
                document.CreatedOn,
                new UserId(document.CreatedBy),
                document.UpdatedOn,
                new UserId(document.UpdatedBy)
            );
        }

        private static RestaurantModel ToDocument(Restaurant obj)
        {
            return new RestaurantModel
            {
                Id = obj.Id.Value,
                Name = obj.Name,
                Address = obj.Address != null
                    ? new AddressModel
                    {
                        Street = obj.Address.Street,
                        ZipCode = obj.Address.ZipCode,
                        City = obj.Address.City
                    }
                    : null,
                ContactInfo = obj.ContactInfo != null
                    ? new ContactInfoModel
                    {
                        Phone = obj.ContactInfo.Phone,
                        Fax = obj.ContactInfo.Fax,
                        WebSite = obj.ContactInfo.WebSite,
                        ResponsiblePerson = obj.ContactInfo.ResponsiblePerson,
                        EmailAddress = obj.ContactInfo.EmailAddress
                    }
                    : null,
                OpeningHours = obj.OpeningHours?.Select(en => new OpeningPeriodModel
                {
                    DayOfWeek = en.DayOfWeek,
                    StartTime = (int) en.Start.TotalMinutes,
                    EndTime = (int) en.End.TotalMinutes
                }).ToList(),
                PickupInfo = obj.PickupInfo != null
                    ? new PickupInfoModel
                    {
                        Enabled = obj.PickupInfo.Enabled,
                        AverageTime = obj.PickupInfo.AverageTime,
                        MinimumOrderValue = Converter.ToDouble(obj.PickupInfo.MinimumOrderValue),
                        MaximumOrderValue = Converter.ToDouble(obj.PickupInfo.MaximumOrderValue)
                    }
                    : null,
                DeliveryInfo = obj.DeliveryInfo != null
                    ? new DeliveryInfoModel
                    {
                        Enabled = obj.DeliveryInfo.Enabled,
                        AverageTime = obj.DeliveryInfo.AverageTime,
                        MinimumOrderValue = Converter.ToDouble(obj.DeliveryInfo.MinimumOrderValue),
                        MaximumOrderValue = Converter.ToDouble(obj.DeliveryInfo.MaximumOrderValue),
                        Costs = Converter.ToDouble(obj.DeliveryInfo.Costs)
                    }
                    : null,
                ReservationInfo = obj.ReservationInfo != null
                    ? new ReservationInfoModel
                    {
                        Enabled = obj.ReservationInfo.Enabled
                    }
                    : null,
                HygienicHandling = obj.HygienicHandling,
                Cuisines = obj.Cuisines != null ? obj.Cuisines.Select(en => en.Value).ToList() : new List<Guid>(),
                PaymentMethods = obj.PaymentMethods != null
                    ? obj.PaymentMethods.Select(en => en.Value).ToList()
                    : new List<Guid>(),
                Administrators = obj.Administrators != null
                    ? obj.Administrators.Select(en => en.Value).ToList()
                    : new List<Guid>(),
                ImportId = obj.ImportId,
                IsActive = obj.IsActive,
                NeedsSupport = obj.NeedsSupport,
                CreatedOn = obj.CreatedOn,
                CreatedBy = obj.CreatedBy.Value,
                UpdatedOn = obj.UpdatedOn,
                UpdatedBy = obj.UpdatedBy.Value
            };
        }
    }
}