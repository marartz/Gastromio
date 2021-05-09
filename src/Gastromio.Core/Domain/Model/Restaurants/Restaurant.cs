using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Cuisines;
using Gastromio.Core.Domain.Model.PaymentMethods;
using Gastromio.Core.Domain.Model.Users;

namespace Gastromio.Core.Domain.Model.Restaurants
{
    public class Restaurant
    {
        private ISet<CuisineId> cuisines;
        private ISet<PaymentMethodId> paymentMethods;
        private ISet<UserId> administrators;

        public Restaurant(
            RestaurantId id,
            string name,
            string alias,
            Address address,
            ContactInfo contactInfo,
            RegularOpeningDays regularOpeningDays,
            DeviatingOpeningDays deviatingOpeningDays,
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
            DishCategories dishCategories,
            ExternalMenus externalMenus,
            DateTimeOffset createdOn,
            UserId createdBy,
            DateTimeOffset updatedOn,
            UserId updatedBy
        )
        {
            ValidateName(name);

            Id = id;
            Name = name;
            Alias = alias;
            Address = address;
            ContactInfo = contactInfo;
            RegularOpeningDays = regularOpeningDays;
            DeviatingOpeningDays = deviatingOpeningDays;
            PickupInfo = pickupInfo ?? new PickupInfo(false, 0, null, null);
            DeliveryInfo = deliveryInfo ?? new DeliveryInfo(false, 0, null, null, null);
            ReservationInfo = reservationInfo ?? new ReservationInfo(false, null);
            HygienicHandling = hygienicHandling;
            ImportId = importId;
            IsActive = isActive;
            NeedsSupport = needsSupport;
            SupportedOrderMode = supportedOrderMode;
            DishCategories = dishCategories;
            ExternalMenus = externalMenus;
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

        public RegularOpeningDays RegularOpeningDays { get; private set; }

        public DeviatingOpeningDays DeviatingOpeningDays { get; private set; }

        public PickupInfo PickupInfo { get; private set; }

        public DeliveryInfo DeliveryInfo { get; private set; }

        public ReservationInfo ReservationInfo { get; private set; }

        public string HygienicHandling { get; private set; }

        public IReadOnlyCollection<CuisineId> Cuisines
        {
            get
            {
                return cuisines != null ? new ReadOnlyCollection<CuisineId>(cuisines.ToList()) : null;
            }
        }

        public IReadOnlyCollection<PaymentMethodId> PaymentMethods
        {
            get
            {
                return paymentMethods != null ? new ReadOnlyCollection<PaymentMethodId>(paymentMethods.ToList()) : null;
            }
        }

        public IReadOnlyCollection<UserId> Administrators
        {
            get
            {
                return administrators != null ? new ReadOnlyCollection<UserId>(administrators.ToList()) : null;
            }
        }

        public string ImportId { get; private set; }

        public bool IsActive { get; private set; }

        public bool NeedsSupport { get; private set; }

        public SupportedOrderMode SupportedOrderMode { get; private set; }

        public DishCategories DishCategories { get; private set; }

        public ExternalMenus ExternalMenus { get; private set; }

        public DateTimeOffset CreatedOn { get; }

        public UserId CreatedBy { get; }

        public DateTimeOffset UpdatedOn { get; private set; }

        public UserId UpdatedBy { get; private set; }

        public void ChangeName(string name, UserId changedBy)
        {
            ValidateName(name);
            Name = name;
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void ChangeAddress(Address address, UserId changedBy)
        {
            Address = address ?? throw DomainException.CreateFrom(new RestaurantAddressRequiredFailure());
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void ChangeContactInfo(ContactInfo contactInfo, UserId changedBy)
        {
            ContactInfo = contactInfo;
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void ChangeRegularOpeningDays(RegularOpeningDays regularOpeningDays, UserId changedBy)
        {
            RegularOpeningDays = regularOpeningDays;
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void AddRegularOpeningPeriod(int dayOfWeek, OpeningPeriod openingPeriod, UserId changedBy)
        {
            RegularOpeningDays = RegularOpeningDays.AddOpeningPeriod(dayOfWeek, openingPeriod);
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void RemoveRegularOpeningPeriod(int dayOfWeek, TimeSpan start, UserId changedBy)
        {
            RegularOpeningDays = RegularOpeningDays.RemoveOpeningPeriod(dayOfWeek, start);
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void ChangeDeviatingOpeningDays(DeviatingOpeningDays deviatingOpeningDays, UserId changedBy)
        {
            DeviatingOpeningDays = deviatingOpeningDays;
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void AddDeviatingOpeningDay(Date date, DeviatingOpeningDayStatus status, UserId changedBy)
        {
            DeviatingOpeningDays = DeviatingOpeningDays.AddOpeningDay(date, status);
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void ChangeDeviatingOpeningDayStatus(Date date, DeviatingOpeningDayStatus status, UserId changedBy)
        {
            DeviatingOpeningDays = DeviatingOpeningDays.ChangeOpeningDayStatus(date, status);
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void RemoveDeviatingOpeningDay(Date date, UserId changedBy)
        {
            DeviatingOpeningDays = DeviatingOpeningDays.RemoveOpeningDay(date);
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void AddDeviatingOpeningPeriod(Date date, OpeningPeriod openingPeriod, UserId changedBy)
        {
            DeviatingOpeningDays = DeviatingOpeningDays.AddOpeningPeriod(date, openingPeriod);
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void RemoveDeviatingOpeningPeriod(Date date, TimeSpan start, UserId changedBy)
        {
            DeviatingOpeningDays = DeviatingOpeningDays.RemoveOpeningPeriod(date, start);
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void RemoveAllOpeningDays(UserId changedBy)
        {
            RegularOpeningDays = new RegularOpeningDays(Enumerable.Empty<RegularOpeningDay>());
            DeviatingOpeningDays = new DeviatingOpeningDays(Enumerable.Empty<DeviatingOpeningDay>());
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public bool IsOrderPossibleAt(DateTimeOffset orderDateTime)
        {
            if (orderDateTime < DateTimeOffset.Now)
            {
                orderDateTime = DateTimeOffset.Now;
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

            if (orderDateTime.ToUtcDate() > DateTimeOffset.UtcNow.ToUtcDate())
            {
                return true;
            }

            var openingPeriodOfNow = FindOpeningPeriod(DateTimeOffset.Now);

            var restaurantIsCurrentlyNotOpen = openingPeriodOfNow == null;
            if (restaurantIsCurrentlyNotOpen)
            {
                return true;
            }

            return openingPeriodOfNow != openingPeriodOfOrderDateTime;
        }

        public void ChangePickupInfo(PickupInfo pickupInfo, UserId changedBy)
        {
            PickupInfo = pickupInfo;
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void ChangeDeliveryInfo(DeliveryInfo deliveryInfo, UserId changedBy)
        {
            DeliveryInfo = deliveryInfo;
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void ChangeReservationInfo(ReservationInfo reservationInfo, UserId changedBy)
        {
            ReservationInfo = reservationInfo;
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void ChangeHygienicHandling(string hygienicHandling, UserId changedBy)
        {
            HygienicHandling = hygienicHandling;
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void ChangeCuisines(IEnumerable<CuisineId> newCuisines, UserId changedBy)
        {
            cuisines = new HashSet<CuisineId>(newCuisines);
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void AddCuisine(CuisineId cuisineId, UserId changedBy)
        {
            if (cuisines != null && cuisines.Contains(cuisineId))
                return;
            if (cuisines == null)
                cuisines = new HashSet<CuisineId>();
            cuisines.Add(cuisineId);
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void RemoveCuisine(CuisineId cuisineId, UserId changedBy)
        {
            if (cuisines == null || !cuisines.Contains(cuisineId))
                return;
            cuisines.Remove(cuisineId);
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void ChangePaymentMethods(IEnumerable<PaymentMethodId> newPaymentMethodIds, UserId changedBy)
        {
            paymentMethods = new HashSet<PaymentMethodId>(newPaymentMethodIds);
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void AddPaymentMethod(PaymentMethodId paymentMethodId, UserId changedBy)
        {
            if (paymentMethods != null && paymentMethods.Contains(paymentMethodId))
                return;
            if (paymentMethods == null)
                paymentMethods = new HashSet<PaymentMethodId>();
            paymentMethods.Add(paymentMethodId);
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void RemovePaymentMethod(PaymentMethodId paymentMethodId, UserId changedBy)
        {
            if (paymentMethodId == PaymentMethodId.Cash)
                throw DomainException.CreateFrom(new RestaurantWithoutCashPaymentNotAllowedFailure());
            if (paymentMethods == null || !paymentMethods.Contains(paymentMethodId))
                return;
            paymentMethods.Remove(paymentMethodId);
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public bool HasAdministrator(UserId userId)
        {
            return administrators != null && administrators.Contains(userId);
        }

        public void ChangeAdministrators(IEnumerable<UserId> newAdministrators, UserId changedBy)
        {
            administrators = new HashSet<UserId>(newAdministrators);
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void AddAdministrator(UserId userId, UserId changedBy)
        {
            if (administrators != null && administrators.Contains(userId))
                return;
            if (administrators == null)
                administrators = new HashSet<UserId>();
            administrators.Add(userId);
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void RemoveAdministrator(UserId userId, UserId changedBy)
        {
            if (administrators == null || !administrators.Contains(userId))
                return;
            administrators.Remove(userId);
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void ChangeImportId(string importId, UserId changedBy)
        {
            ImportId = importId;
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void Deactivate(UserId changedBy)
        {
            if (!IsActive)
                return;
            IsActive = false;
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void Activate(UserId changedBy)
        {
            if (IsActive)
                return;
            IsActive = true;
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void DisableSupport(UserId changedBy)
        {
            if (!NeedsSupport)
                return;
            NeedsSupport = false;
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void EnableSupport(UserId changedBy)
        {
            if (NeedsSupport)
                return;
            NeedsSupport = true;
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void ChangeSupportedOrderMode(SupportedOrderMode supportedOrderMode, UserId changedBy)
        {
            SupportedOrderMode = supportedOrderMode;
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void SetExternalMenu(ExternalMenu menu, UserId changedBy)
        {
            if (menu == null)
                throw new ArgumentNullException(nameof(menu));
            ExternalMenus = ExternalMenus.AddOrReplace(menu);
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void RemoveExternalMenu(ExternalMenuId menuId, UserId changedBy)
        {
            ExternalMenus = ExternalMenus.Remove(menuId);
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        private static (int DayOfWeek, TimeSpan Time) GetDayOfWeekAndTimeInfoFromDateTime(DateTimeOffset dateTime)
        {
            var dayOfWeek = ((int) dateTime.DayOfWeek - 1) % 7;
            if (dayOfWeek < 0)
            {
                dayOfWeek += 7;
            }

            dateTime = dateTime.ToLocalTime();

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

        private OpeningPeriod FindOpeningPeriod(DateTimeOffset dateTime)
        {
            var (dayOfWeek, time) = GetDayOfWeekAndTimeInfoFromDateTime(dateTime);

            var date = new Date(dateTime.Year, dateTime.Month, dateTime.Day);
            if (DeviatingOpeningDays != null &&
                DeviatingOpeningDays.TryGetOpeningDay(date, out var deviatingOpeningDay))
            {
                return deviatingOpeningDay.FindPeriodAtTime(time);
            }

            if (RegularOpeningDays != null &&
                RegularOpeningDays.TryGetOpeningDay(dayOfWeek, out var regularOpeningDay))
            {
                return regularOpeningDay.FindPeriodAtTime(time);
            }

            return null;
        }

        public DishCategory AddDishCategory(string name, DishCategoryId afterCategoryId, UserId changedBy)
        {
            DishCategories = DishCategories.AddNewDishCategory(name, afterCategoryId, out var dishCategory);
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
            return dishCategory;
        }

        public void RemoveAllDishCategoriesDueToImport(UserId changedBy)
        {
            DishCategories = new DishCategories(Enumerable.Empty<DishCategory>());
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void RemoveDishCategory(DishCategoryId dishCategoryId, UserId changedBy)
        {
            DishCategories = DishCategories.RemoveDishCategory(dishCategoryId);
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void ChangeDishCategoryName(DishCategoryId dishCategoryId, string name, UserId changedBy)
        {
            DishCategories = DishCategories.ChangeDishCategoryName(dishCategoryId, name);
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void DecOrderOfDishCategory(DishCategoryId dishCategoryId, UserId changedBy)
        {
            DishCategories = DishCategories.DecOrderOfDishCategory(dishCategoryId);
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void IncOrderOfDishCategory(DishCategoryId dishCategoryId, UserId changedBy)
        {
            DishCategories = DishCategories.IncOrderOfDishCategory(dishCategoryId);
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void EnableDishCategory(DishCategoryId dishCategoryId, UserId changedBy)
        {
            DishCategories = DishCategories.EnableDishCategory(dishCategoryId);
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void DisableDishCategory(DishCategoryId dishCategoryId, UserId changedBy)
        {
            DishCategories = DishCategories.DisableDishCategory(dishCategoryId);
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public Dish AddOrChangeDish(DishCategoryId dishCategoryId, DishId dishId, string name, string description,
            string productInfo, int orderNo, IEnumerable<DishVariant> variants, UserId changedBy)
        {
            DishCategories = DishCategories.AddOrChangeDish(dishCategoryId, dishId, name, description, productInfo,
                orderNo, variants, out var dish);
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
            return dish;
        }

        public void RemoveDish(DishCategoryId dishCategoryId, DishId dishId, UserId changedBy)
        {
            DishCategories = DishCategories.RemoveDish(dishCategoryId, dishId);
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void DecOrderOfDish(DishCategoryId dishCategoryId, DishId dishId, UserId changedBy)
        {
            DishCategories = DishCategories.DecOrderOfDish(dishCategoryId, dishId);
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void IncOrderOfDish(DishCategoryId dishCategoryId, DishId dishId, UserId changedBy)
        {
            DishCategories = DishCategories.IncOrderOfDish(dishCategoryId, dishId);
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void AddDishVariant(DishCategoryId dishCategoryId, DishId dishId, DishVariant dishVariant,
            UserId changedBy)
        {
            DishCategories = DishCategories.AddDishVariant(dishCategoryId, dishId, dishVariant);
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void RemoveDishVariant(DishCategoryId dishCategoryId, DishId dishId, DishVariantId dishVariantId,
            UserId changedBy)
        {
            DishCategories = DishCategories.RemoveDishVariant(dishCategoryId, dishId, dishVariantId);
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void ReplaceDishVariants(DishCategoryId dishCategoryId, DishId dishId, DishVariants dishVariants,
            UserId changedBy)
        {
            DishCategories = DishCategories.ReplaceDishVariants(dishCategoryId, dishId, dishVariants);
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        private static void ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw DomainException.CreateFrom(new RestaurantNameRequiredFailure());
            if (name.Length > 100)
                throw DomainException.CreateFrom(new RestaurantNameTooLongFailure(100));
        }
    }
}
