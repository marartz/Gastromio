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
        private IList<OpeningPeriod> openingHours;
        private ISet<CuisineId> cuisines;
        private ISet<PaymentMethodId> paymentMethods;
        private ISet<UserId> administrators;
        
        public Restaurant(
            RestaurantId id,
            DateTime createdOn,
            UserId createdBy,
            DateTime updatedOn,
            UserId updatedBy
        )
        {
            Id = id;
            CreatedOn = createdOn;
            CreatedBy = createdBy;
            UpdatedOn = updatedOn;
            UpdatedBy = updatedBy;
            Address = new Address(null, null, null);
            ContactInfo = new ContactInfo(null, null, null, null, null);
            openingHours = new List<OpeningPeriod>();
            PickupInfo = new PickupInfo(false, TimeSpan.Zero, null, null);
            DeliveryInfo = new DeliveryInfo(false, TimeSpan.Zero, null, null, null);
            ReservationInfo = new ReservationInfo(false);
            cuisines = new HashSet<CuisineId>();
            paymentMethods = new HashSet<PaymentMethodId>();
            administrators = new HashSet<UserId>();
        }

        public Restaurant(
            RestaurantId id,
            string name,
            Address address,
            ContactInfo contactInfo,
            IList<OpeningPeriod> openingHours,
            PickupInfo pickupInfo,
            DeliveryInfo deliveryInfo,
            ReservationInfo reservationInfo,
            string hygienicHandling,
            ISet<CuisineId> cuisines,
            ISet<PaymentMethodId> paymentMethods,
            ISet<UserId> administrators,
            DateTime createdOn,
            UserId createdBy,
            DateTime updatedOn,
            UserId updatedBy
        )
        {
            Id = id;
            Name = name;
            Address = address ?? new Address(null, null, null);
            ContactInfo = contactInfo ?? new ContactInfo(null, null, null, null, null);
            this.openingHours = openingHours ?? new List<OpeningPeriod>();
            PickupInfo = pickupInfo ?? new PickupInfo(false, TimeSpan.Zero, null, null);
            DeliveryInfo = deliveryInfo ?? new DeliveryInfo(false, TimeSpan.Zero, null, null, null);
            ReservationInfo = reservationInfo ?? new ReservationInfo(false);
            HygienicHandling = hygienicHandling;
            CreatedOn = createdOn;
            CreatedBy = createdBy;
            UpdatedOn = updatedOn;
            UpdatedBy = updatedBy;
            this.cuisines = cuisines ?? new HashSet<CuisineId>();
            this.paymentMethods = paymentMethods ?? new HashSet<PaymentMethodId>();
            this.administrators = administrators ?? new HashSet<UserId>();
        }

        public RestaurantId Id { get; }

        public string Name { get; private set; }
        
        public Address Address { get; private set; }
        
        public ContactInfo ContactInfo { get; private set; }
        
        public IReadOnlyCollection<OpeningPeriod> OpeningHours =>
            openingHours != null ? new ReadOnlyCollection<OpeningPeriod>(openingHours) : null;
        
        public PickupInfo PickupInfo { get; private set; }
        
        public DeliveryInfo DeliveryInfo { get; private set; }
        
        public ReservationInfo ReservationInfo { get; private set; }
        
        public string HygienicHandling { get; private set; }
        
        public IReadOnlyCollection<CuisineId> Cuisines =>
            cuisines != null ? new ReadOnlyCollection<CuisineId>(cuisines.ToList()) : null;
        
        public IReadOnlyCollection<PaymentMethodId> PaymentMethods =>
            paymentMethods != null ? new ReadOnlyCollection<PaymentMethodId>(paymentMethods.ToList()) : null;

        public IReadOnlyCollection<UserId> Administrators =>
            administrators != null ? new ReadOnlyCollection<UserId>(administrators.ToList()) : null;

        public DateTime CreatedOn { get; }
        
        public UserId CreatedBy { get; }
        
        public DateTime UpdatedOn { get; private set; }
        
        public UserId UpdatedBy { get; private set; }
        
        public Result<bool> ChangeName(string name, UserId changedBy)
        {
            if (string.IsNullOrEmpty(name))
                return FailureResult<bool>.Create(FailureResultCode.RequiredFieldEmpty, nameof(name));
            if (name.Length > 100)
                return FailureResult<bool>.Create(FailureResultCode.FieldValueTooLong, nameof(name), 100);

            Name = name;
            UpdatedOn = DateTime.UtcNow;
            UpdatedBy = changedBy;
            
            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> ChangeAddress(Address address, UserId changedBy)
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
            UpdatedOn = DateTime.UtcNow;
            UpdatedBy = changedBy;

            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> ChangeContactInfo(ContactInfo contactInfo, UserId changedBy)
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
            UpdatedOn = DateTime.UtcNow;
            UpdatedBy = changedBy;

            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> AddOpeningPeriod(OpeningPeriod openingPeriod, UserId changedBy)
        {
            var earliestOpeningTime = 4d;

            var newPeriod = GetDayOverflowCorrected(openingPeriod);

            if (openingPeriod.Start.TotalHours < earliestOpeningTime)
                return FailureResult<bool>.Create(FailureResultCode.RestaurantOpeningPeriodBeginsTooEarly);
            if (!(newPeriod.End.TotalHours > newPeriod.Start.TotalHours))
                return FailureResult<bool>.Create(FailureResultCode.RestaurantOpeningPeriodEndsBeforeStart);

            var anyOverlapping = openingHours?
                .Where(p => p.DayOfWeek == openingPeriod.DayOfWeek)
                .Select(p => GetDayOverflowCorrected(p))
                .Any(period => PeriodsOverlapping(newPeriod, period));

            if (anyOverlapping == true)
                return FailureResult<bool>.Create(FailureResultCode.RestaurantOpeningPeriodIntersects);

            if (openingHours == null)
            {
                openingHours = new List<OpeningPeriod>();
            }

            openingHours.Add(openingPeriod);
            UpdatedOn = DateTime.UtcNow;
            UpdatedBy = changedBy;

            return SuccessResult<bool>.Create(true);

            bool PeriodsOverlapping(OpeningPeriod x, OpeningPeriod y)
            {
                if (x.Start.TotalHours <= y.End.TotalHours && y.Start.TotalHours <= x.End.TotalHours)
                    return true;
                return false;
            }

            OpeningPeriod GetDayOverflowCorrected(OpeningPeriod period)
            {
                if (period.End.TotalHours < earliestOpeningTime)
                {
                    return new OpeningPeriod(period.DayOfWeek, period.Start,
                        TimeSpan.FromHours(period.End.TotalHours + 24d));
                }
                return period;
            }
        }

        public Result<bool> RemoveOpeningPeriod(int dayOfWeek, TimeSpan start, UserId changedBy)
        {
            if (openingHours == null)
            {
                return SuccessResult<bool>.Create(true);
            }
            
            openingHours = openingHours.Where(en => en.DayOfWeek != dayOfWeek || en.Start != start).ToList();
            UpdatedOn = DateTime.UtcNow;
            UpdatedBy = changedBy;
            
            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> ChangePickupInfo(PickupInfo pickupInfo, UserId changedBy)
        {
            if (!pickupInfo.Enabled)
            {
                PickupInfo = pickupInfo;
                return SuccessResult<bool>.Create(true);
            }

            if (!pickupInfo.AverageTime.HasValue || pickupInfo.AverageTime.Value.TotalMinutes < 5)
                return FailureResult<bool>.Create(FailureResultCode.RestaurantAveragePickupTimeTooLow);
            if (pickupInfo.AverageTime.Value.TotalMinutes > 120)
                return FailureResult<bool>.Create(FailureResultCode.RestaurantAveragePickupTimeTooHigh);
            
            if (pickupInfo.MinimumOrderValue.HasValue && pickupInfo.MinimumOrderValue < 0)
                return FailureResult<bool>.Create(FailureResultCode.RestaurantMinimumPickupOrderValueTooLow);
            if (pickupInfo.MinimumOrderValue.HasValue && pickupInfo.MinimumOrderValue > 50)
                return FailureResult<bool>.Create(FailureResultCode.RestaurantMinimumPickupOrderValueTooHigh);
            
            if (pickupInfo.MaximumOrderValue.HasValue && pickupInfo.MaximumOrderValue < 0)
                return FailureResult<bool>.Create(FailureResultCode.RestaurantMaximumPickupOrderValueTooLow);

            PickupInfo = pickupInfo;
            UpdatedOn = DateTime.UtcNow;
            UpdatedBy = changedBy;

            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> ChangeDeliveryInfo(DeliveryInfo deliveryInfo, UserId changedBy)
        {
            if (!deliveryInfo.Enabled)
            {
                DeliveryInfo = deliveryInfo;
                return SuccessResult<bool>.Create(true);
            }

            if (!deliveryInfo.AverageTime.HasValue || deliveryInfo.AverageTime.Value.TotalMinutes < 5)
                return FailureResult<bool>.Create(FailureResultCode.RestaurantAverageDeliveryTimeTooLow);
            if (deliveryInfo.AverageTime.Value.TotalMinutes > 120)
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
            UpdatedOn = DateTime.UtcNow;
            UpdatedBy = changedBy;

            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> ChangeReservationInfo(ReservationInfo reservationInfo, UserId changedBy)
        {
            ReservationInfo = reservationInfo;
            UpdatedOn = DateTime.UtcNow;
            UpdatedBy = changedBy;
            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> ChangeHygienicHandling(string hygienicHandling, UserId changedBy)
        {
            HygienicHandling = hygienicHandling;
            UpdatedOn = DateTime.UtcNow;
            UpdatedBy = changedBy;
            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> AddCuisine(CuisineId cuisineId, UserId changedBy)
        {
            if (cuisines != null && cuisines.Contains(cuisineId))
                return SuccessResult<bool>.Create(true);
            if (cuisines == null)
                cuisines = new HashSet<CuisineId>();
            cuisines.Add(cuisineId);
            UpdatedOn = DateTime.UtcNow;
            UpdatedBy = changedBy;
            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> RemoveCuisine(CuisineId cuisineId, UserId changedBy)
        {
            if (cuisines == null || !cuisines.Contains(cuisineId))
                return SuccessResult<bool>.Create(true);
            cuisines.Remove(cuisineId);
            UpdatedOn = DateTime.UtcNow;
            UpdatedBy = changedBy;
            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> AddPaymentMethod(PaymentMethodId paymentMethodId, UserId changedBy)
        {
            if (paymentMethods != null && paymentMethods.Contains(paymentMethodId))
                return SuccessResult<bool>.Create(true);
            if (paymentMethods == null)
                paymentMethods = new HashSet<PaymentMethodId>();
            paymentMethods.Add(paymentMethodId);
            UpdatedOn = DateTime.UtcNow;
            UpdatedBy = changedBy;
            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> RemovePaymentMethod(PaymentMethodId paymentMethodId, UserId changedBy)
        {
            if (paymentMethods == null || !paymentMethods.Contains(paymentMethodId))
                return SuccessResult<bool>.Create(true);
            paymentMethods.Remove(paymentMethodId);
            UpdatedOn = DateTime.UtcNow;
            UpdatedBy = changedBy;
            return SuccessResult<bool>.Create(true);
        }

        public bool HasAdministrator(UserId userId)
        {
            return administrators != null && administrators.Contains(userId);
        }

        public Result<bool> AddAdministrator(UserId userId, UserId changedBy)
        {
            if (administrators != null && administrators.Contains(userId))
                return SuccessResult<bool>.Create(true);
            if (administrators == null)
                administrators = new HashSet<UserId>();
            administrators.Add(userId);
            UpdatedOn = DateTime.UtcNow;
            UpdatedBy = changedBy;
            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> RemoveAdministrator(UserId userId, UserId changedBy)
        {
            if (administrators == null || !administrators.Contains(userId))
                return SuccessResult<bool>.Create(true);
            administrators.Remove(userId);
            UpdatedOn = DateTime.UtcNow;
            UpdatedBy = changedBy;
            return SuccessResult<bool>.Create(true);
        }
    }
}
