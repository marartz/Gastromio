using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using FoodOrderSystem.Core.Common;
using FoodOrderSystem.Core.Domain.Model.Cuisine;
using FoodOrderSystem.Core.Domain.Model.PaymentMethod;
using FoodOrderSystem.Core.Domain.Model.User;

namespace FoodOrderSystem.Core.Domain.Model.Restaurant
{
    public class Restaurant
    {
        private IDictionary<int, IList<RegularOpeningPeriod>> regularOpeningPeriods;
        private IDictionary<Date, IList<DeviatingOpeningPeriod>> deviatingOpeningPeriods;
        private ISet<CuisineId> cuisines;
        private ISet<PaymentMethodId> paymentMethods;
        private ISet<UserId> administrators;
        private readonly IDictionary<Guid, ExternalMenu> externalMenus;

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
            regularOpeningPeriods = new Dictionary<int, IList<RegularOpeningPeriod>>();
            deviatingOpeningPeriods = new Dictionary<Date, IList<DeviatingOpeningPeriod>>();
            PickupInfo = new PickupInfo(false, 0, null, null);
            DeliveryInfo = new DeliveryInfo(false, 0, null, null, null);
            ReservationInfo = new ReservationInfo(false);
            cuisines = new HashSet<CuisineId>();
            paymentMethods = new HashSet<PaymentMethodId>();
            administrators = new HashSet<UserId>();
            externalMenus = new Dictionary<Guid, ExternalMenu>();
        }

        public Restaurant(
            RestaurantId id,
            string name,
            string alias,
            Address address,
            ContactInfo contactInfo,
            IDictionary<int, IList<RegularOpeningPeriod>> regularOpeningPeriods,
            IDictionary<Date, IList<DeviatingOpeningPeriod>> deviatingOpeningPeriods,
            PickupInfo pickupInfo,
            DeliveryInfo deliveryInfo,
            ReservationInfo reservationInfo,
            string hygienicHandling,
            ISet<CuisineId> cuisines,
            ISet<PaymentMethodId> paymentMethods,
            ISet<UserId> administrators,
            string importId,
            bool isActive,
            bool needsSupport,
            SupportedOrderMode supportedOrderMode,
            IList<ExternalMenu> externalMenus,
            DateTime createdOn,
            UserId createdBy,
            DateTime updatedOn,
            UserId updatedBy
        )
        {
            Id = id;
            Name = name;
            Alias = alias;
            Address = address ?? new Address(null, null, null);
            ContactInfo = contactInfo ?? new ContactInfo(null, null, null, null, null);
            this.regularOpeningPeriods =
                regularOpeningPeriods ?? new Dictionary<int, IList<RegularOpeningPeriod>>();
            this.deviatingOpeningPeriods =
                deviatingOpeningPeriods ?? new Dictionary<Date, IList<DeviatingOpeningPeriod>>();
            PickupInfo = pickupInfo ?? new PickupInfo(false, 0, null, null);
            DeliveryInfo = deliveryInfo ?? new DeliveryInfo(false, 0, null, null, null);
            ReservationInfo = reservationInfo ?? new ReservationInfo(false);
            HygienicHandling = hygienicHandling;
            ImportId = importId;
            IsActive = isActive;
            NeedsSupport = needsSupport;
            SupportedOrderMode = supportedOrderMode;
            this.externalMenus = externalMenus?.ToDictionary(menu => menu.Id, menu => menu) ??
                                 new Dictionary<Guid, ExternalMenu>();
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

        public string Alias { get; }

        public Address Address { get; private set; }

        public ContactInfo ContactInfo { get; private set; }

        public IReadOnlyDictionary<int, IReadOnlyCollection<RegularOpeningPeriod>> RegularOpeningPeriods =>
            regularOpeningPeriods?.ToDictionary(en => en.Key,
                en => en.Value as IReadOnlyCollection<RegularOpeningPeriod>);

        public IReadOnlyDictionary<Date, IReadOnlyCollection<DeviatingOpeningPeriod>> DeviatingOpeningPeriods =>
            deviatingOpeningPeriods?.ToDictionary(en => en.Key,
                en => en.Value as IReadOnlyCollection<DeviatingOpeningPeriod>);

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

        public string ImportId { get; private set; }

        public bool IsActive { get; private set; }

        public bool NeedsSupport { get; private set; }

        public SupportedOrderMode SupportedOrderMode { get; private set; }

        public IReadOnlyCollection<ExternalMenu> ExternalMenus => externalMenus.Values.ToImmutableList();

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
                return FailureResult<bool>.Create(FailureResultCode.FieldValueInvalid, nameof(address.Street),
                    address.Street);

            if (string.IsNullOrEmpty(address.ZipCode))
                return FailureResult<bool>.Create(FailureResultCode.RequiredFieldEmpty, nameof(address.ZipCode));
            if (address.ZipCode.Length != 5 || address.ZipCode.Any(en => !char.IsDigit(en)))
                return FailureResult<bool>.Create(FailureResultCode.FieldValueInvalid, nameof(address.ZipCode),
                    address.ZipCode);
            if (!Validators.IsValidZipCode(address.ZipCode))
                return FailureResult<bool>.Create(FailureResultCode.FieldValueInvalid, nameof(address.ZipCode),
                    address.ZipCode);

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
                return FailureResult<bool>.Create(FailureResultCode.FieldValueInvalid, nameof(contactInfo.Phone),
                    contactInfo.Phone);

            if (!string.IsNullOrEmpty(contactInfo.Fax) && !Validators.IsValidPhoneNumber(contactInfo.Fax))
                return FailureResult<bool>.Create(FailureResultCode.FieldValueInvalid, nameof(contactInfo.Fax),
                    contactInfo.Fax);

            if (!string.IsNullOrEmpty(contactInfo.WebSite) && !Validators.IsValidWebsite(contactInfo.WebSite))
                return FailureResult<bool>.Create(FailureResultCode.FieldValueInvalid, nameof(contactInfo.WebSite),
                    contactInfo.WebSite);

            if (string.IsNullOrEmpty(contactInfo.ResponsiblePerson))
                return FailureResult<bool>.Create(FailureResultCode.RequiredFieldEmpty,
                    nameof(contactInfo.ResponsiblePerson));

            if (string.IsNullOrEmpty(contactInfo.EmailAddress))
                return FailureResult<bool>.Create(FailureResultCode.RequiredFieldEmpty,
                    nameof(contactInfo.EmailAddress));
            if (!Validators.IsValidEmailAddress(contactInfo.EmailAddress))
                return FailureResult<bool>.Create(FailureResultCode.FieldValueInvalid, nameof(contactInfo.EmailAddress),
                    contactInfo.EmailAddress);

            ContactInfo = contactInfo;
            UpdatedOn = DateTime.UtcNow;
            UpdatedBy = changedBy;

            return SuccessResult<bool>.Create(true);
        }
        
        public bool IsOpen(DateTime dateTime)
        {
            return FindOpeningPeriod(dateTime) != null;
        }

        public Result<bool> AddRegularOpeningPeriod(RegularOpeningPeriod openingPeriod, UserId changedBy)
        {
            var isNew = false;
            
            if (!regularOpeningPeriods.TryGetValue(openingPeriod.DayOfWeek, out var openingPeriods))
            {
                openingPeriods = new List<RegularOpeningPeriod>();
                isNew = true;
            }

            var result = AddOpeningPeriod(openingPeriods, openingPeriod);
            if (result.IsFailure)
                return result;

            if (isNew)
            {
                regularOpeningPeriods.Add(openingPeriod.DayOfWeek, openingPeriods);
            }

            UpdatedOn = DateTime.UtcNow;
            UpdatedBy = changedBy;

            return result;
        }

        public Result<bool> RemoveRegularOpeningPeriod(int dayOfWeek, TimeSpan start, UserId changedBy)
        {
            if (!regularOpeningPeriods.TryGetValue(dayOfWeek, out var openingPeriods) || openingPeriods == null)
            {
                return SuccessResult<bool>.Create(true);
            }

            var index = openingPeriods.ToList().FindIndex(en => en.Start == start);
            openingPeriods.RemoveAt(index);

            if (openingPeriods.Count == 0)
            {
                regularOpeningPeriods.Remove(dayOfWeek);
            }

            UpdatedOn = DateTime.UtcNow;
            UpdatedBy = changedBy;

            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> AddDeviatingOpeningDay(Date date, UserId changedBy)
        {
            if (!deviatingOpeningPeriods.TryGetValue(date, out var openingPeriods))
            {
                openingPeriods = new List<DeviatingOpeningPeriod>();
                deviatingOpeningPeriods.Add(date, openingPeriods);
            }

            UpdatedOn = DateTime.UtcNow;
            UpdatedBy = changedBy;

            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> RemoveDeviatingOpeningDay(Date date, UserId changedBy)
        {
            if (!deviatingOpeningPeriods.ContainsKey(date))
            {
                return SuccessResult<bool>.Create(true);
            }

            deviatingOpeningPeriods.Remove(date);

            UpdatedOn = DateTime.UtcNow;
            UpdatedBy = changedBy;

            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> AddDeviatingOpeningPeriod(DeviatingOpeningPeriod openingPeriod,
            UserId changedBy)
        {
            if (!deviatingOpeningPeriods.TryGetValue(openingPeriod.Date, out var openingPeriods))
            {
                openingPeriods = new List<DeviatingOpeningPeriod>();
                deviatingOpeningPeriods.Add(openingPeriod.Date, openingPeriods);
            }

            var result = AddOpeningPeriod(openingPeriods, openingPeriod);
            if (result.IsFailure)
                return result;

            UpdatedOn = DateTime.UtcNow;
            UpdatedBy = changedBy;

            return result;
        }

        public Result<bool> RemoveDeviatingOpeningPeriod(Date date, TimeSpan start, UserId changedBy)
        {
            if (!deviatingOpeningPeriods.TryGetValue(date, out var openingPeriods))
            {
                return SuccessResult<bool>.Create(true);
            }

            var index = openingPeriods.ToList().FindIndex(en => en.Start == start);
            openingPeriods.RemoveAt(index);

            UpdatedOn = DateTime.UtcNow;
            UpdatedBy = changedBy;

            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> RemoveAllOpeningPeriods(UserId changedBy)
        {
            regularOpeningPeriods = new Dictionary<int, IList<RegularOpeningPeriod>>();
            deviatingOpeningPeriods = new Dictionary<Date, IList<DeviatingOpeningPeriod>>();
            UpdatedOn = DateTime.UtcNow;
            UpdatedBy = changedBy;
            return SuccessResult<bool>.Create(true);
        }

        public bool IsOrderPossibleAt(DateTime orderDateTime)
        {
            if (orderDateTime < DateTime.Now)
            {
                orderDateTime = DateTime.Now;
            }

            if (SupportedOrderMode == SupportedOrderMode.OnlyPhone)
            {
                return false;
            }

            var openingPeriodOfOrderDateTime = FindOpeningPeriod(orderDateTime);
            if (openingPeriodOfOrderDateTime == null)
            {
                return false;
            }

            if (SupportedOrderMode != SupportedOrderMode.AtNextShift)
            {
                return true;
            }
            
            if (orderDateTime.Date > DateTime.Today)
            {
                return true;
            }

            var openingPeriodOfNow = FindOpeningPeriod(DateTime.Now);

            var restaurantIsCurrentlyNotOpen = openingPeriodOfNow == null;
            if (restaurantIsCurrentlyNotOpen)
            {
                return true;
            }
            
            return openingPeriodOfNow != openingPeriodOfOrderDateTime;
        }

        public Result<bool> ChangePickupInfo(PickupInfo pickupInfo, UserId changedBy)
        {
            if (!pickupInfo.Enabled)
            {
                PickupInfo = pickupInfo;
                return SuccessResult<bool>.Create(true);
            }

            if (pickupInfo.AverageTime.HasValue && pickupInfo.AverageTime.Value < 5)
                return FailureResult<bool>.Create(FailureResultCode.RestaurantAveragePickupTimeTooLow);
            if (pickupInfo.AverageTime.HasValue && pickupInfo.AverageTime.Value > 120)
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

            if (deliveryInfo.AverageTime.HasValue && deliveryInfo.AverageTime.Value < 5)
                return FailureResult<bool>.Create(FailureResultCode.RestaurantAverageDeliveryTimeTooLow);
            if (deliveryInfo.AverageTime.HasValue && deliveryInfo.AverageTime.Value > 120)
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
            if (paymentMethodId == PaymentMethodId.Cash)
                return FailureResult<bool>.Create(FailureResultCode.RestaurantWithoutCashPaymentNotAllowed);

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

        public Result<bool> ChangeImportId(string importId, UserId changedBy)
        {
            ImportId = importId;
            UpdatedOn = DateTime.UtcNow;
            UpdatedBy = changedBy;
            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> Deactivate(UserId changedBy)
        {
            if (!IsActive)
                return SuccessResult<bool>.Create(true);
            IsActive = false;
            UpdatedOn = DateTime.UtcNow;
            UpdatedBy = changedBy;
            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> Activate(UserId changedBy)
        {
            if (IsActive)
                return SuccessResult<bool>.Create(true);
            IsActive = true;
            UpdatedOn = DateTime.UtcNow;
            UpdatedBy = changedBy;
            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> DisableSupport(UserId changedBy)
        {
            if (!NeedsSupport)
                return SuccessResult<bool>.Create(true);
            NeedsSupport = false;
            UpdatedOn = DateTime.UtcNow;
            UpdatedBy = changedBy;
            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> EnableSupport(UserId changedBy)
        {
            if (NeedsSupport)
                return SuccessResult<bool>.Create(true);
            NeedsSupport = true;
            UpdatedOn = DateTime.UtcNow;
            UpdatedBy = changedBy;
            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> ChangeSupportedOrderMode(SupportedOrderMode supportedOrderMode, UserId changedBy)
        {
            SupportedOrderMode = supportedOrderMode;
            UpdatedOn = DateTime.UtcNow;
            UpdatedBy = changedBy;
            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> SetExternalMenu(ExternalMenu menu, UserId changedBy)
        {
            if (menu == null)
                throw new ArgumentNullException(nameof(menu));
            if (string.IsNullOrWhiteSpace(menu.Name))
                return FailureResult<bool>.Create(FailureResultCode.ExternalMenuHasNoName);
            if (string.IsNullOrWhiteSpace(menu.Description))
                return FailureResult<bool>.Create(FailureResultCode.ExternalMenuHasNoDescription);
            if (string.IsNullOrWhiteSpace(menu.Url))
                return FailureResult<bool>.Create(FailureResultCode.ExternalMenuHasNoUrl);
            if (!externalMenus.ContainsKey(menu.Id))
            {
                externalMenus.Add(menu.Id, menu);
            }
            else
            {
                externalMenus[menu.Id] = menu;
            }

            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> RemoveExternalMenu(Guid menuId, UserId changedBy)
        {
            if (menuId == default)
                throw new ArgumentException($"is default", nameof(menuId));
            if (!externalMenus.ContainsKey(menuId))
                return FailureResult<bool>.Create(FailureResultCode.ExternalMenuDoesNotExist);
            externalMenus.Remove(menuId);
            return SuccessResult<bool>.Create(true);
        }

        private (int DayOfWeek, TimeSpan Time) GetDayOfWeekAndTimeInfoFromDateTime(DateTime dateTime)
        {
            var dayOfWeek = ((int) dateTime.DayOfWeek - 1) % 7;
            if (dayOfWeek < 0)
            {
                dayOfWeek += 7;
            }

            var time = dateTime.Hour * 60 + dateTime.Minute;
            if (dateTime.Hour < OpeningPeriod.EarliestOpeningTime)
            {
                dayOfWeek = (dayOfWeek - 1) % 7;
                if (dayOfWeek < 0)
                {
                    dayOfWeek += 7;
                }

                time += 24 * 60;
            }

            return (dayOfWeek, TimeSpan.FromMinutes(time));
        }

        public OpeningPeriod FindOpeningPeriod(DateTime dateTime)
        {
            var (dayOfWeek, time) = GetDayOfWeekAndTimeInfoFromDateTime(dateTime);

            var date = new Date(dateTime.Year, dateTime.Month, dateTime.Day);
            if (this.deviatingOpeningPeriods != null &&
                this.deviatingOpeningPeriods.TryGetValue(date, out var deviatingOpeningPeriods))
            {
                return deviatingOpeningPeriods.SingleOrDefault(en => en.Start <= time && time <= en.End);
            }

            if (this.regularOpeningPeriods != null &&
                this.regularOpeningPeriods.TryGetValue(dayOfWeek, out var regularOpeningPeriods))
            {
                return regularOpeningPeriods.SingleOrDefault(en => en.Start <= time && time <= en.End);
            }

            return null;
        }

        private static Result<bool> AddOpeningPeriod<T>(ICollection<T> openingPeriods, T openingPeriod)
            where T : OpeningPeriod
        {
            if (openingPeriods.Any(en => en.Start == openingPeriod.Start && en.End == openingPeriod.End))
            {
                return SuccessResult<bool>.Create(true);
            }

            if (openingPeriod.Start.TotalHours < OpeningPeriod.EarliestOpeningTime)
                return FailureResult<bool>.Create(FailureResultCode.RestaurantOpeningPeriodBeginsTooEarly);
            if (!(openingPeriod.End.TotalHours > openingPeriod.Start.TotalHours))
                return FailureResult<bool>.Create(FailureResultCode.RestaurantOpeningPeriodEndsBeforeStart);

            var anyOverlapping = openingPeriods
                .Any(period => PeriodsOverlapping(openingPeriod, period));

            if (anyOverlapping)
                return FailureResult<bool>.Create(FailureResultCode.RestaurantOpeningPeriodIntersects);

            openingPeriods.Add(openingPeriod);

            return SuccessResult<bool>.Create(true);
        }

        private static bool PeriodsOverlapping(OpeningPeriod x, OpeningPeriod y)
        {
            return x.Start.TotalHours <= y.End.TotalHours && y.Start.TotalHours <= x.End.TotalHours;
        }
    }
}