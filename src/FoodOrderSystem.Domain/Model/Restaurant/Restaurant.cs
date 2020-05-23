using FoodOrderSystem.Domain.Model.Cuisine;
using FoodOrderSystem.Domain.Model.PaymentMethod;
using FoodOrderSystem.Domain.Model.User;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

        public Result<bool> ChangeName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return FailureResult<bool>.Create(FailureResultCode.RestaurantHasToHaveAName);
            Name = name;
            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> ChangeImage(byte[] image)
        {
            if (image == null)
            {
                Image = null;
                return SuccessResult<bool>.Create(true);
            }

            try
            {
                using (var ms = new MemoryStream(image))
                {
                    var imageObj = System.Drawing.Image.FromStream(ms);
                    if (imageObj == null)
                        return FailureResult<bool>.Create(FailureResultCode.RestaurantImageNotValid);
                }
            }
            catch (Exception exc)
            {
                // TODO: Log error
                return FailureResult<bool>.Create(FailureResultCode.RestaurantImageNotValid);
            }

            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> ChangeAddress(Address address)
        {
            if (address == null)
                return FailureResult<bool>.Create(FailureResultCode.RequiredFieldEmpty, args: nameof(address));

            if (string.IsNullOrEmpty(address.Street))
                return FailureResult<bool>.Create(FailureResultCode.RequiredFieldEmpty, args: nameof(address.Street));

            if (string.IsNullOrEmpty(address.ZipCode))
                return FailureResult<bool>.Create(FailureResultCode.RequiredFieldEmpty, args: nameof(address.ZipCode));

            if (string.IsNullOrEmpty(address.City))
                return FailureResult<bool>.Create(FailureResultCode.RequiredFieldEmpty, args: nameof(address.City));

            Address = address;

            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> ChangeContactDetails(string phone, string webSite, string imprint, string orderEmailAddress)
        {
            if (string.IsNullOrEmpty(phone))
                return FailureResult<bool>.Create(FailureResultCode.RequiredFieldEmpty, args: nameof(phone));
            if (string.IsNullOrEmpty(imprint))
                return FailureResult<bool>.Create(FailureResultCode.RequiredFieldEmpty, args: nameof(imprint));
            if (string.IsNullOrEmpty(orderEmailAddress))
                return FailureResult<bool>.Create(FailureResultCode.RequiredFieldEmpty, args: nameof(orderEmailAddress));

            Phone = phone;
            WebSite = webSite;
            Imprint = imprint;
            OrderEmailAddress = orderEmailAddress;

            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> ChangeDeliveryData(decimal minimumOrderValue, decimal deliveryCosts)
        {
            if (minimumOrderValue < 0)
                return FailureResult<bool>.Create(FailureResultCode.ValueMustNotBeNegative, args: nameof(minimumOrderValue));
            if (minimumOrderValue > 50)
                return FailureResult<bool>.Create(FailureResultCode.RestaurantMinimumOrderValueTooHigh);

            if (deliveryCosts < 0)
                return FailureResult<bool>.Create(FailureResultCode.ValueMustNotBeNegative, args: nameof(deliveryCosts));
            if (deliveryCosts > 10)
                return FailureResult<bool>.Create(FailureResultCode.RestaurantDeliveryCostsTooHigh);

            MinimumOrderValue = minimumOrderValue;
            DeliveryCosts = deliveryCosts;

            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> AddDeliveryTime(int dayOfWeek, TimeSpan start, TimeSpan end)
        {
            deliveryTimes.Add(new DeliveryTime(dayOfWeek, start, end));
            return SuccessResult<bool>.Create(true);
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
