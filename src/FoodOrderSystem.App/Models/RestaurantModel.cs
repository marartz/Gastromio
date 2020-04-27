using FoodOrderSystem.Domain.Model.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodOrderSystem.App.Models
{
    public class RestaurantModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public AddressModel Address { get; set; }
        
        public IList<DeliveryTimeModel> DeliveryTimes { get; set; }

        public decimal MinimumOrderValue { get; set; }

        public decimal DeliveryCosts { get; set; }

        public string WebSite { get; set; }

        public string Imprint { get; set; }

        public IList<Guid> PaymentMethods { get; set; }

        public static RestaurantModel FromRestaurant(Restaurant restaurant)
        {
            return new RestaurantModel
            {
                Id = restaurant.Id.Value,
                Name = restaurant.Name,
                Address = restaurant.Address != null ? new AddressModel
                {
                    Line1 = restaurant.Address.Line1,
                    Line2 = restaurant.Address.Line2,
                    ZipCode = restaurant.Address.ZipCode,
                    City = restaurant.Address.City
                } : null,
                DeliveryTimes = restaurant.DeliveryTimes != null ? restaurant.DeliveryTimes.Select(en => new DeliveryTimeModel
                {
                    DayOfWeek = en.DayOfWeek,
                    Start = (int)en.Start.TotalMinutes,
                    End = (int)en.End.TotalMinutes
                }).ToList() : new List<DeliveryTimeModel>(),
                MinimumOrderValue = restaurant.MinimumOrderValue,
                DeliveryCosts = restaurant.DeliveryCosts,
                WebSite = restaurant.WebSite,
                Imprint = restaurant.Imprint,
                PaymentMethods = restaurant.PaymentMethods != null ? restaurant.PaymentMethods.Select(en => en.Value).ToList() : new List<Guid>()
            };
        }
    }
}
