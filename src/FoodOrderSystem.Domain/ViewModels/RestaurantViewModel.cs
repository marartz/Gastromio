using FoodOrderSystem.Domain.Model.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoodOrderSystem.Domain.ViewModels
{
    public class RestaurantViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public AddressViewModel Address { get; set; }

        public IList<DeliveryTimeViewModel> DeliveryTimes { get; set; }

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

        public IList<PaymentMethodViewModel> PaymentMethods { get; set; }

        public static RestaurantViewModel FromRestaurant(Restaurant restaurant, IDictionary<Guid, PaymentMethodViewModel> allPaymentMethods)
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

            return new RestaurantViewModel
            {
                Id = restaurant.Id.Value,
                Name = restaurant.Name,
                Image = image,
                Address = restaurant.Address != null ? AddressViewModel.FromAddress(restaurant.Address) : null,
                DeliveryTimes = restaurant.DeliveryTimes != null ? restaurant.DeliveryTimes
                    .Select(en => DeliveryTimeViewModel.FromDeliveryTime(en)).ToList() : new List<DeliveryTimeViewModel>(),
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
                PaymentMethods = restaurant.PaymentMethods != null ? restaurant.PaymentMethods
                    .Select(en => RetrievePaymentMethodModel(allPaymentMethods, en.Value)).ToList() : new List<PaymentMethodViewModel>()
            };
        }

        public static PaymentMethodViewModel RetrievePaymentMethodModel(IDictionary<Guid, PaymentMethodViewModel> allPaymentMethods, Guid paymentMethodId)
        {
            return allPaymentMethods.TryGetValue(paymentMethodId, out var model) ? model : null;
        }
    }
}
