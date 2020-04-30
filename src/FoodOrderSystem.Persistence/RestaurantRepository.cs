using FoodOrderSystem.Domain.Model.PaymentMethod;
using FoodOrderSystem.Domain.Model.Restaurant;
using Microsoft.EntityFrameworkCore;
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

        public Task<ICollection<Restaurant>> SearchAsync(string searchPhrase, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                var rows = dbContext.Restaurants.Where(en => EF.Functions.Like(en.Name, $"%{searchPhrase}%")).OrderBy(en => en.Name).ToList();
                return (ICollection<Restaurant>)rows.Select(FromRow).ToList();
            }, cancellationToken);
        }

        public Task<ICollection<Restaurant>> FindAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                var rows = dbContext.Restaurants.OrderBy(en => en.Name).ToList();
                return (ICollection<Restaurant>)rows.Select(FromRow).ToList();
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

        public Task StoreAsync(Restaurant restaurant, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                var dbSet = dbContext.Restaurants;

                var row = dbSet.FirstOrDefault(x => x.Id == restaurant.Id.Value);
                if (row != null)
                {
                    ToRow(restaurant, row);
                    dbSet.Update(row);
                }
                else
                {
                    row = new RestaurantRow();
                    ToRow(restaurant, row);
                    dbSet.Add(row);
                }

                dbContext.SaveChanges();
            }, cancellationToken);
        }

        public Task RemoveAsync(RestaurantId restaurantId, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                var dbSet = dbContext.Restaurants;

                var row = dbSet.FirstOrDefault(en => en.Id == restaurantId.Value);
                if (row != null)
                {
                    dbSet.Remove(row);
                    dbContext.SaveChanges();
                }
            }, cancellationToken);
        }

        private static Restaurant FromRow(RestaurantRow row)
        {
            return new Restaurant(new RestaurantId(row.Id),
                row.Name,
                row.Image,
                new Address(row.AddressStreet, row.AddressZipCode, row.AddressCity),
                row.DeliveryTimes.Select(en => new DeliveryTime(en.DayOfWeek, TimeSpan.FromMinutes(en.StartTime), TimeSpan.FromMinutes(en.EndTime))).ToList(),
                row.MinimumOrderValue,
                row.DeliveryCosts,
                row.Phone,
                row.WebSite,
                row.Imprint,
                new HashSet<PaymentMethodId>(row.RestaurantPaymentMethods.Select(en => new PaymentMethodId(en.PaymentMethodId)))
            );
        }

        private static void ToRow(Restaurant obj, RestaurantRow row)
        {
            // TODO
            row.Id = obj.Id.Value;
            row.Name = obj.Name;
            row.Image = obj.Image;
            if (obj.Address != null)
            {
                row.AddressStreet = obj.Address.Street;
                row.AddressZipCode = obj.Address.ZipCode;
                row.AddressCity = obj.Address.City;
            }
            if (obj.DeliveryTimes != null && obj.DeliveryTimes.Count > 0)
            {
                row.DeliveryTimes = new List<DeliveryTimeRow>();
                foreach (var deliveryTime in obj.DeliveryTimes)
                {
                    row.DeliveryTimes.Add(new DeliveryTimeRow
                    {
                        RestaurantId = obj.Id.Value,
                        DayOfWeek = deliveryTime.DayOfWeek,
                        StartTime = (int)deliveryTime.Start.TotalMinutes,
                        EndTime = (int)deliveryTime.End.TotalMinutes
                    });
                }
            }
            row.MinimumOrderValue = obj.MinimumOrderValue;
            row.DeliveryCosts = obj.DeliveryCosts;
            row.Phone = obj.Phone;
            row.WebSite = obj.WebSite;
            row.Imprint = obj.Imprint;
            if (obj.PaymentMethods != null && obj.PaymentMethods.Count > 0)
            {
                row.RestaurantPaymentMethods = new List<RestaurantPaymentMethodRow>();
                foreach (var paymentMethod in obj.PaymentMethods)
                {
                    row.RestaurantPaymentMethods.Add(new RestaurantPaymentMethodRow
                    {
                        RestaurantId = obj.Id.Value,
                        PaymentMethodId = paymentMethod.Value
                    });
                }
            }
        }
    }
}
