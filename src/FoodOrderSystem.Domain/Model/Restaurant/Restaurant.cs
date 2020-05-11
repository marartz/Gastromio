using FoodOrderSystem.Domain.Model.Cuisine;
using FoodOrderSystem.Domain.Model.PaymentMethod;
using FoodOrderSystem.Domain.Model.User;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FoodOrderSystem.Domain.Model.Restaurant
{
    public class Restaurant
    {
        private IList<DeliveryTime> deliveryTimes;

        public Restaurant(
            RestaurantId id,
            string name,
            byte[] image,
            Address address,
            IList<DeliveryTime> deliveryTimes,
            decimal minimumOrderValue,
            decimal deliveryCosts,
            string phone,
            string webSite,
            string imprint,
            string orderEmailAddress,
            ISet<CuisineId> cuisines,
            ISet<PaymentMethodId> paymentMethods,
            ISet<UserId> administrators
        )
        {
            Id = id;
            Name = name;
            Image = image;
            Address = address;
            this.deliveryTimes = deliveryTimes;
            MinimumOrderValue = minimumOrderValue;
            DeliveryCosts = deliveryCosts;
            Phone = phone;
            WebSite = webSite;
            Imprint = imprint;
            OrderEmailAddress = orderEmailAddress;
            Cuisines = cuisines;
            PaymentMethods = paymentMethods;
            Administrators = administrators;
        }

        public RestaurantId Id { get; }
        public string Name { get; private set; }
        public byte[] Image { get; private set; }
        public Address Address { get; private set; }
        public IReadOnlyCollection<DeliveryTime> DeliveryTimes => new ReadOnlyCollection<DeliveryTime>(deliveryTimes);
        public decimal MinimumOrderValue { get; private set; }
        public decimal DeliveryCosts { get; private set; }
        public string Phone { get; private set; }
        public string WebSite { get; private set; }
        public string Imprint { get; private set; }
        public string OrderEmailAddress { get; private set; }
        public ISet<CuisineId> Cuisines { get; }
        public ISet<PaymentMethodId> PaymentMethods { get; }
        public ISet<UserId> Administrators { get; }

        public DateTime CalculateNextDeliveryTime()
        {
            // TODO
            return DateTime.Now;
        }

        public void ChangeName(string name)
        {
            Name = name;
        }

        public void ChangeImage(byte[] image)
        {
            Image = image;
        }

        public void ChangeAddress(Address address)
        {
            Address = address;
        }

        public void ChangeContactDetails(string phone, string webSite, string imprint, string orderEmailAddress)
        {
            Phone = phone;
            WebSite = webSite;
            Imprint = imprint;
            OrderEmailAddress = orderEmailAddress;
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

        public void AddCuisine(CuisineId cuisineId)
        {
            if (Cuisines.Contains(cuisineId))
                return;
            Cuisines.Add(cuisineId);
        }

        public void RemoveCuisine(CuisineId cuisineId)
        {
            if (!Cuisines.Contains(cuisineId))
                return;
            Cuisines.Remove(cuisineId);
        }

        public void AddPaymentMethod(PaymentMethodId paymentMethodId)
        {
            if (PaymentMethods.Contains(paymentMethodId))
                return;
            PaymentMethods.Add(paymentMethodId);
        }

        public void RemovePaymentMethod(PaymentMethodId paymentMethodId)
        {
            if (!PaymentMethods.Contains(paymentMethodId))
                return;
            PaymentMethods.Remove(paymentMethodId);
        }

        public bool HasAdministrator(UserId userId)
        {
            return Administrators.Contains(userId);
        }

        public void AddAdministrator(UserId userId)
        {
            if (Administrators.Contains(userId))
                return;
            Administrators.Add(userId);
        }

        public void RemoveAdministrator(UserId userId)
        {
            if (!Administrators.Contains(userId))
                return;
            Administrators.Remove(userId);
        }
    }
}
