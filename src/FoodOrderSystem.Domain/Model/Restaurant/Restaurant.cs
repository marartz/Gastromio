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
        private IList<OpeningPeriod> openingHours;
        private ISet<CuisineId> cuisines;
        private ISet<PaymentMethodId> paymentMethods;
        private ISet<UserId> administrators;
        
        public Restaurant(RestaurantId id)
        {
            Id = id;
        }

        public Restaurant(
            RestaurantId id,
            string name,
            byte[] image,
            Address address,
            ContactInfo contactInfo,
            IList<OpeningPeriod> openingHours,
            PickupInfo pickupInfo,
            DeliveryInfo deliveryInfo,
            ReservationInfo reservationInfo,
            ISet<CuisineId> cuisines,
            ISet<PaymentMethodId> paymentMethods,
            ISet<UserId> administrators
        )
        {
            Id = id;
            Name = name;
            Image = image;
            Address = address;
            ContactInfo = contactInfo;
            this.openingHours = openingHours;
            PickupInfo = pickupInfo;
            DeliveryInfo = deliveryInfo;
            ReservationInfo = reservationInfo;
            this.cuisines = cuisines;
            this.paymentMethods = paymentMethods;
            this.administrators = administrators;
        }

        public RestaurantId Id { get; }

        public string Name { get; private set; }
        
        public byte[] Image { get; private set; }
        
        public Address Address { get; private set; }
        
        public ContactInfo ContactInfo { get; private set; }
        
        public IReadOnlyCollection<OpeningPeriod> OpeningHours =>
            openingHours != null ? new ReadOnlyCollection<OpeningPeriod>(openingHours) : null;
        
        public PickupInfo PickupInfo { get; private set; }
        
        public DeliveryInfo DeliveryInfo { get; private set; }
        
        public ReservationInfo ReservationInfo { get; private set; }
        
        public IReadOnlyCollection<CuisineId> Cuisines =>
            cuisines != null ? new ReadOnlyCollection<CuisineId>(cuisines.ToList()) : null;
        
        public IReadOnlyCollection<PaymentMethodId> PaymentMethods =>
            paymentMethods != null ? new ReadOnlyCollection<PaymentMethodId>(paymentMethods.ToList()) : null;

        public IReadOnlyCollection<UserId> Administrators =>
            administrators != null ? new ReadOnlyCollection<UserId>(administrators.ToList()) : null;

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
                    var imageObj = SixLabors.ImageSharp.Image.Load(ms);
                    if (imageObj == null)
                        return FailureResult<bool>.Create(FailureResultCode.RestaurantImageNotValid);
                }
            }
            catch
            {
                // TODO: Log error
                return FailureResult<bool>.Create(FailureResultCode.RestaurantImageNotValid);
            }

            Image = image;
            
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

        public Result<bool> ChangeContactInfo(ContactInfo contactInfo)
        {
            if (string.IsNullOrEmpty(contactInfo.Phone))
                return FailureResult<bool>.Create(FailureResultCode.RequiredFieldEmpty, nameof(contactInfo.Phone));
            if (!Validators.IsValidPhoneNumber(contactInfo.Phone))
                return FailureResult<bool>.Create(FailureResultCode.FieldValueInvalid, nameof(contactInfo.Phone));
            
            if (!Validators.IsValidPhoneNumber(contactInfo.Fax))
                return FailureResult<bool>.Create(FailureResultCode.FieldValueInvalid, nameof(contactInfo.Fax));
            
            if (!string.IsNullOrEmpty(contactInfo.WebSite) && !Validators.IsValidWebsite(contactInfo.WebSite))
                return FailureResult<bool>.Create(FailureResultCode.FieldValueInvalid, nameof(contactInfo.WebSite));

            if (string.IsNullOrEmpty(contactInfo.ResponsiblePerson))
                return FailureResult<bool>.Create(FailureResultCode.RequiredFieldEmpty, nameof(contactInfo.ResponsiblePerson));

            if (string.IsNullOrEmpty(contactInfo.EmailAddress))
                return FailureResult<bool>.Create(FailureResultCode.RequiredFieldEmpty, nameof(contactInfo.EmailAddress));
            if (!Validators.IsValidEmailAddress(contactInfo.EmailAddress))
                return FailureResult<bool>.Create(FailureResultCode.FieldValueInvalid, nameof(contactInfo.EmailAddress));

            ContactInfo = contactInfo;
            
            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> AddOpeningPeriod(OpeningPeriod openingPeriod)
        {
            var newStartHour = openingPeriod.Start.TotalHours;
            var newEndHour = openingPeriod.End.TotalHours;
            if (newEndHour < 4)
                newEndHour += 24;

            if (newStartHour < 4)
                return FailureResult<bool>.Create(FailureResultCode.RestaurantOpeningPeriodBeginsTooEarly);
            if (!(newEndHour > newStartHour))
                return FailureResult<bool>.Create(FailureResultCode.RestaurantOpeningPeriodEndsBeforeStart);

            var curOpeningPeriods = openingHours != null
                ? openingHours.Where(en => en.DayOfWeek == openingPeriod.DayOfWeek).OrderBy(en => en.Start.TotalHours)
                    .ToList()
                : Enumerable.Empty<OpeningPeriod>();

            foreach (var curOpeningPeriod in curOpeningPeriods)
            {
                var curStartHour = curOpeningPeriod.Start.TotalHours;
                var curEndHour = curOpeningPeriod.End.TotalHours;
                if (curEndHour < 4)
                    curEndHour += 24;

                if (curStartHour < newStartHour && newStartHour < curEndHour) // either start is between current
                    return FailureResult<bool>.Create(FailureResultCode.RestaurantOpeningPeriodIntersects);
                if (curStartHour < newEndHour && newEndHour < curEndHour) // or end is between current
                    return FailureResult<bool>.Create(FailureResultCode.RestaurantOpeningPeriodIntersects);
            }

            if (openingHours == null)
            {
                openingHours = new List<OpeningPeriod>();
            }
            
            openingHours.Add(openingPeriod);
            
            return SuccessResult<bool>.Create(true);
        }

        public void RemoveOpeningPeriod(int dayOfWeek, TimeSpan start)
        {
            if (openingHours == null)
            {
                return;
            }
            
            openingHours = openingHours.Where(en => en.DayOfWeek != dayOfWeek || en.Start != start).ToList();
        }

        public Result<bool> EnablePickup(PickupInfo pickupInfo)
        {
            return pickupInfo == null
                ? FailureResult<bool>.Create(FailureResultCode.NoRestaurantPickupInfosSpecified)
                : ChangePickupInfo(pickupInfo);
        }

        public Result<bool> ChangePickupInfo(PickupInfo pickupInfo)
        {
            if (pickupInfo.AverageTime.TotalMinutes < 5)
                return FailureResult<bool>.Create(FailureResultCode.RestaurantAveragePickupTimeTooLow);
            if (pickupInfo.AverageTime.TotalMinutes > 120)
                return FailureResult<bool>.Create(FailureResultCode.RestaurantAveragePickupTimeTooHigh);
            
            if (pickupInfo.MinimumOrderValue.HasValue && pickupInfo.MinimumOrderValue < 0)
                return FailureResult<bool>.Create(FailureResultCode.RestaurantMinimumPickupOrderValueTooLow);
            if (pickupInfo.MinimumOrderValue.HasValue && pickupInfo.MinimumOrderValue > 50)
                return FailureResult<bool>.Create(FailureResultCode.RestaurantMinimumPickupOrderValueTooHigh);
            
            if (pickupInfo.MaximumOrderValue.HasValue && pickupInfo.MaximumOrderValue < 0)
                return FailureResult<bool>.Create(FailureResultCode.RestaurantMaximumPickupOrderValueTooLow);

            PickupInfo = pickupInfo;
            
            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> DisablePickup()
        {
            PickupInfo = null;
            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> EnableDelivery(DeliveryInfo deliveryInfo)
        {
            return deliveryInfo == null
                ? FailureResult<bool>.Create(FailureResultCode.NoRestaurantDeliveryInfosSpecified)
                : ChangeDeliveryInfo(deliveryInfo);
        }

        public Result<bool> ChangeDeliveryInfo(DeliveryInfo deliveryInfo)
        {
            if (deliveryInfo.AverageTime.TotalMinutes < 5)
                return FailureResult<bool>.Create(FailureResultCode.RestaurantAverageDeliveryTimeTooLow);
            if (deliveryInfo.AverageTime.TotalMinutes > 120)
                return FailureResult<bool>.Create(FailureResultCode.RestaurantAverageDeliveryTimeTooHigh);
            
            if (deliveryInfo.MinimumOrderValue.HasValue && deliveryInfo.MinimumOrderValue < 0)
                return FailureResult<bool>.Create(FailureResultCode.RestaurantMinimumDeliveryOrderValueTooLow);
            if (deliveryInfo.MinimumOrderValue.HasValue && deliveryInfo.MinimumOrderValue > 50)
                return FailureResult<bool>.Create(FailureResultCode.RestaurantMinimumDeliveryOrderValueTooHigh);
            
            if (deliveryInfo.MaximumOrderValue.HasValue && deliveryInfo.MaximumOrderValue < 0)
                return FailureResult<bool>.Create(FailureResultCode.RestaurantMaximumDeliveryOrderValueTooLow);

            if (deliveryInfo.Costs.HasValue && deliveryInfo.Costs < 0)
                return FailureResult<bool>.Create(FailureResultCode.RestaurantDeliveryCostsTooLow);
            if (deliveryInfo.Costs.HasValue && deliveryInfo.Costs > 10)
                return FailureResult<bool>.Create(FailureResultCode.RestaurantDeliveryCostsTooHigh);

            DeliveryInfo = deliveryInfo;
            
            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> DisableDelivery()
        {
            DeliveryInfo = null;
            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> EnableReservation(ReservationInfo reservationInfo)
        {
            return reservationInfo == null
                ? FailureResult<bool>.Create(FailureResultCode.NoRestaurantReservationInfosSpecified)
                : ChangeReservationInfo(reservationInfo);
        }

        public Result<bool> ChangeReservationInfo(ReservationInfo reservationInfo)
        {
            ReservationInfo = reservationInfo;
            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> DisableReservation()
        {
            ReservationInfo = null;
            return SuccessResult<bool>.Create(true);
        }

        public void AddCuisine(CuisineId cuisineId)
        {
            if (cuisines != null && cuisines.Contains(cuisineId))
                return;
            if (cuisines == null)
                cuisines = new HashSet<CuisineId>();
            cuisines.Add(cuisineId);
        }

        public void RemoveCuisine(CuisineId cuisineId)
        {
            if (cuisines == null || !cuisines.Contains(cuisineId))
                return;
            cuisines.Remove(cuisineId);
        }

        public void AddPaymentMethod(PaymentMethodId paymentMethodId)
        {
            if (paymentMethods != null && paymentMethods.Contains(paymentMethodId))
                return;
            if (paymentMethods == null)
                paymentMethods = new HashSet<PaymentMethodId>();
            paymentMethods.Add(paymentMethodId);
        }

        public void RemovePaymentMethod(PaymentMethodId paymentMethodId)
        {
            if (paymentMethods == null || !paymentMethods.Contains(paymentMethodId))
                return;
            paymentMethods.Remove(paymentMethodId);
        }

        public bool HasAdministrator(UserId userId)
        {
            return administrators != null && administrators.Contains(userId);
        }

        public void AddAdministrator(UserId userId)
        {
            if (administrators != null && administrators.Contains(userId))
                return;
            if (administrators == null)
                administrators = new HashSet<UserId>();
            administrators.Add(userId);
        }

        public void RemoveAdministrator(UserId userId)
        {
            if (administrators == null || !administrators.Contains(userId))
                return;
            administrators.Remove(userId);
        }
    }
}
