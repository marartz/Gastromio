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
        public string Name { get; private set; }
        public Address Address { get; private set; }
        public IReadOnlyCollection<DeliveryTime> DeliveryTimes { get; }
        public decimal MinimumOrderValue { get; private set; }
        public decimal DeliveryCosts { get; private set; }
        public string WebSite { get; private set; }
        public string Imprint { get; private set; }
        public ISet<PaymentMethodId> PaymentMethods { get; }

        public void ChangeName(string name)
        {
            Name = name;
        }

        public void ChangeAddress(Address address)
        {
            Address = address;
        }

        public void ChangeContactDetails(string webSite, string imprint)
        {
            WebSite = webSite;
            Imprint = imprint;
        }

        public void ChangeDeliveryData(decimal minimumOrderValue, decimal deliveryCosts)
        {
            MinimumOrderValue = minimumOrderValue;
            DeliveryCosts = deliveryCosts;
        }
    }
}
