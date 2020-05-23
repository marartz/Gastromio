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
                return FailureResult<bool>.Create(FailureResultCode.RequiredFieldEmpty, nameof(name));
            if (name.Length > 100)
                return FailureResult<bool>.Create(FailureResultCode.FieldValueTooLong, nameof(name), 100);

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

            if (image.Length > 1024 * 1024) // 1 MB
                return FailureResult<bool>.Create(FailureResultCode.RestaurantImageDataTooBig);

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
                return FailureResult<bool>.Create(FailureResultCode.RequiredFieldEmpty, nameof(address));

            if (string.IsNullOrEmpty(address.Street))
                return FailureResult<bool>.Create(FailureResultCode.RequiredFieldEmpty, nameof(address.Street));
            if (address.Street.Length > 100)
                return FailureResult<bool>.Create(FailureResultCode.FieldValueTooLong, nameof(address.Street), 100);
            if (!Validators.IsValidStreet(address.Street))
                return FailureResult<bool>.Create(FailureResultCode.FieldValueInvalid, nameof(address.Street));

            if (string.IsNullOrEmpty(address.ZipCode))
                return FailureResult<bool>.Create(FailureResultCode.RequiredFieldEmpty, nameof(address.ZipCode));
            if (address.ZipCode.Length != 5 || address.ZipCode.Any(en => !char.IsDigit(en)))
                return FailureResult<bool>.Create(FailureResultCode.FieldValueInvalid, nameof(address.ZipCode));
            if (!Validators.IsValidZipCode(address.ZipCode))
                return FailureResult<bool>.Create(FailureResultCode.FieldValueInvalid, nameof(address.ZipCode));

            if (string.IsNullOrEmpty(address.City))
                return FailureResult<bool>.Create(FailureResultCode.RequiredFieldEmpty, nameof(address.City));
            if (address.City.Length > 50)
                return FailureResult<bool>.Create(FailureResultCode.FieldValueTooLong, nameof(address.City), 50);

            Address = address;

            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> ChangeContactDetails(string phone, string webSite, string imprint, string orderEmailAddress)
        {
            if (string.IsNullOrEmpty(phone))
                return FailureResult<bool>.Create(FailureResultCode.RequiredFieldEmpty, nameof(phone));
            if (!Validators.IsValidPhoneNumber(phone))
                return FailureResult<bool>.Create(FailureResultCode.FieldValueInvalid, nameof(phone));
            
            if (!string.IsNullOrEmpty(webSite) && !Validators.IsValidWebsite(webSite))
                return FailureResult<bool>.Create(FailureResultCode.FieldValueInvalid, nameof(webSite));

            if (string.IsNullOrEmpty(imprint))
                return FailureResult<bool>.Create(FailureResultCode.RequiredFieldEmpty, nameof(imprint));

            if (string.IsNullOrEmpty(orderEmailAddress))
                return FailureResult<bool>.Create(FailureResultCode.RequiredFieldEmpty, nameof(orderEmailAddress));
            if (!Validators.IsValidEmailAddress(orderEmailAddress))
                return FailureResult<bool>.Create(FailureResultCode.FieldValueInvalid, nameof(orderEmailAddress));

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
            var newStartHour = start.TotalHours;
            var newEndHour = end.TotalHours;
            if (newEndHour < 4)
                newEndHour += 24;

            if (newStartHour < 4)
                return FailureResult<bool>.Create(FailureResultCode.RestaurantDeliveryTimeBeginTooEarly);
            if (!(newEndHour > newStartHour))
                return FailureResult<bool>.Create(FailureResultCode.RestaurantDeliveryTimeEndBeforeStart);

            var curDeliveryTimes = deliveryTimes
                .Where(en => en.DayOfWeek == dayOfWeek)
                .OrderBy(en => en.Start.TotalHours)
                .ToList();

            foreach (var curDeliveryTime in curDeliveryTimes)
            {
                var curStartHour = curDeliveryTime.Start.TotalHours;
                var curEndHour = curDeliveryTime.End.TotalHours;
                if (curEndHour < 4)
                    curEndHour += 24;

                if (curStartHour < newStartHour && newStartHour < curEndHour) // either start is between current
                    return FailureResult<bool>.Create(FailureResultCode.RestaurantDeliveryTimeIntersects);
                if (curStartHour < newEndHour && newEndHour < curEndHour) // or end is between current
                    return FailureResult<bool>.Create(FailureResultCode.RestaurantDeliveryTimeIntersects);
            }
            
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
