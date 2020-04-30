using FoodOrderSystem.Domain.Model.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoodOrderSystem.App.Models
{
    public class RestaurantModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public AddressModel Address { get; set; }
        
        public IList<DeliveryTimeModel> DeliveryTimes { get; set; }

        public DateTime NextDeliveryTime { get; set; }

        public string NextDeliveryTimeText { get; set; }

        public decimal MinimumOrderValue { get; set; }

        public string MinimumOrderValueText { get; set; }

        public decimal DeliveryCosts { get; set; }

        public string DeliveryCostsText { get; set; }

        public string Phone { get; set; }

        public string WebSite { get; set; }

        public string Imprint { get; set; }

        public string OrderEmailAddress { get; set; }

        public IList<Guid> PaymentMethods { get; set; }

        public static RestaurantModel FromRestaurant(Restaurant restaurant)
        {
            string image = null;
            if (restaurant.Image != null && restaurant.Image.Length != 0)
            {
                var sb = new StringBuilder();
                sb.Append("data:image/png;base64,");
                sb.Append(Convert.ToBase64String(restaurant.Image));
                image = sb.ToString();
            }

            var nextDeliveryTime = restaurant.CalculateNextDeliveryTime();

            return new RestaurantModel
            {
                Id = restaurant.Id.Value,
                Name = restaurant.Name,
                Image = image,
                Address = restaurant.Address != null ? new AddressModel
                {
                    Street = restaurant.Address.Street,
                    ZipCode = restaurant.Address.ZipCode,
                    City = restaurant.Address.City
                } : null,
                DeliveryTimes = restaurant.DeliveryTimes != null ? restaurant.DeliveryTimes.Select(en => new DeliveryTimeModel
                {
                    DayOfWeek = en.DayOfWeek,
                    Start = (int)en.Start.TotalMinutes,
                    End = (int)en.End.TotalMinutes
                }).ToList() : new List<DeliveryTimeModel>(),
                NextDeliveryTime = nextDeliveryTime,
                NextDeliveryTimeText = nextDeliveryTime.ToString("hh:mm"),
                MinimumOrderValue = restaurant.MinimumOrderValue,
                MinimumOrderValueText = restaurant.MinimumOrderValue.ToString("0.00"),
                DeliveryCosts = restaurant.DeliveryCosts,
                DeliveryCostsText = restaurant.DeliveryCosts.ToString("0.00"),
                Phone = restaurant.Phone,
                WebSite = restaurant.WebSite,
                Imprint = restaurant.Imprint,
                OrderEmailAddress = restaurant.OrderEmailAddress,
                PaymentMethods = restaurant.PaymentMethods != null ? restaurant.PaymentMethods.Select(en => en.Value).ToList() : new List<Guid>()
            };
        }
    }
}
