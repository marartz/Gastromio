using FoodOrderSystem.Domain.Model.PaymentMethod;
using FoodOrderSystem.Domain.Model.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Persistence
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly SystemDbContext dbContext;

        public RestaurantRepository(SystemDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<ICollection<Restaurant>> FindAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                return (ICollection<Restaurant>)dbContext.Restaurants.Select(FromRow).ToList();
            }, cancellationToken);
        }

        public Task<Restaurant> FindByRestaurantIdAsync(RestaurantId restaurantId, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                var row = dbContext.Restaurants.FirstOrDefault(en => en.Id == restaurantId.Value);
                if (row == null)
                    return null;
                return FromRow(row);
            }, cancellationToken);
        }

        private static Restaurant FromRow(RestaurantRow row)
        {
            return new Restaurant(new RestaurantId(row.Id),
                row.Name,
                new Address(row.AddressLine1, row.AddressLine2, row.AddressZipCode, row.AddressCity),
                row.DeliveryTimes.Select(en => new DeliveryTime(en.DayOfWeek, TimeSpan.FromMinutes(en.StartTime), TimeSpan.FromMinutes(en.EndTime))).ToList(),
                row.MinimumOrderValue,
                row.DeliveryCosts,
                row.WebSite,
                row.Imprint,
                new HashSet<PaymentMethodId>(row.RestaurantPaymentMethods.Select(en => new PaymentMethodId(en.PaymentMethodId)))
            );
        }

        private static RestaurantRow ToRow(Restaurant obj)
        {
            // TODO
            return new RestaurantRow
            {
                Id = obj.Id.Value,
                Name = obj.Name,
                AddressLine1 = obj.Address?.Line1,
                AddressLine2 = obj.Address?.Line2,
                AddressZipCode = obj.Address?.ZipCode,
                AddressCity = obj.Address?.City,
                MinimumOrderValue = obj.MinimumOrderValue,
                DeliveryCosts = obj.DeliveryCosts,
                WebSite = obj.WebSite,
                Imprint = obj.Imprint,
            };
        }
    }
}
