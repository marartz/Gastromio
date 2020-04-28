using FoodOrderSystem.Domain.Model.PaymentMethod;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FoodOrderSystem.Domain.Model.Restaurant
{
    public class Restaurant
    {
        private IList<DeliveryTime> deliveryTimes;

        public Restaurant(RestaurantId id, string name, Address address, IList<DeliveryTime> deliveryTimes, decimal minimumOrderValue, decimal deliveryCosts, string webSite, string imprint, ISet<PaymentMethodId> paymentMethods)
        {
            Id = id;
            Name = name;
            Address = address;
            this.deliveryTimes = deliveryTimes;
            MinimumOrderValue = minimumOrderValue;
            DeliveryCosts = deliveryCosts;
            WebSite = webSite;
            Imprint = imprint;
            PaymentMethods = paymentMethods;
        }

        public RestaurantId Id { get; }
        public string Name { get; private set; }
        public Address Address { get; private set; }
        public IReadOnlyCollection<DeliveryTime> DeliveryTimes => new ReadOnlyCollection<DeliveryTime>(deliveryTimes);
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

        public void AddDeliveryTime(int dayOfWeek, TimeSpan start, TimeSpan end)
        {
            deliveryTimes.Add(new DeliveryTime(dayOfWeek, start, end));
        }

        public void RemoveDeliveryTime(int dayOfWeek, TimeSpan start)
        {
            deliveryTimes = deliveryTimes.Where(en => en.DayOfWeek != dayOfWeek || en.Start != start).ToList();
        }
    }
}
