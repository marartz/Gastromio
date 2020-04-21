using FoodOrderSystem.Domain.Model.PaymentMethod;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FoodOrderSystem.Domain.Model.Restaurant
{
    public class Restaurant
    {
        public Restaurant(RestaurantId id, string name, Address address, IList<DeliveryTime> deliveryTimes, decimal minimumOrderValue, decimal deliveryCosts, string webSite, string imprint, ISet<PaymentMethodId> paymentMethods)
        {
            Id = id;
            Name = name;
            Address = address;
            DeliveryTimes = new ReadOnlyCollection<DeliveryTime>(deliveryTimes);
            MinimumOrderValue = minimumOrderValue;
            DeliveryCosts = deliveryCosts;
            WebSite = webSite;
            Imprint = imprint;
            PaymentMethods = paymentMethods;
        }

        public RestaurantId Id { get; }
        public string Name { get; }
        public Address Address { get; }
        public IReadOnlyCollection<DeliveryTime> DeliveryTimes { get; }
        public decimal MinimumOrderValue { get; }
        public decimal DeliveryCosts { get; }
        public string WebSite { get; }
        public string Imprint { get; }
        public ISet<PaymentMethodId> PaymentMethods { get; }
    }
}
