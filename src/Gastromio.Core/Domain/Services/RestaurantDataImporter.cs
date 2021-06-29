using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gastromio.Core.Application.Ports.Persistence;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Cuisines;
using Gastromio.Core.Domain.Model.PaymentMethods;
using Gastromio.Core.Domain.Model.Restaurants;
using Gastromio.Core.Domain.Model.Users;

namespace Gastromio.Core.Domain.Services
{
    public class RestaurantDataImporter : IRestaurantDataImporter
    {
        private readonly IRestaurantFactory restaurantFactory;
        private readonly IRestaurantRepository restaurantRepository;
        private readonly ICuisineFactory cuisineFactory;
        private readonly ICuisineRepository cuisineRepository;
        private readonly IPaymentMethodRepository paymentMethodRepository;
        private readonly IUserFactory userFactory;
        private readonly IUserRepository userRepository;

        public RestaurantDataImporter(
            IRestaurantFactory restaurantFactory,
            IRestaurantRepository restaurantRepository,
            ICuisineFactory cuisineFactory,
            ICuisineRepository cuisineRepository,
            IPaymentMethodRepository paymentMethodRepository,
            IUserFactory userFactory,
            IUserRepository userRepository
        )
        {
            this.restaurantFactory = restaurantFactory;
            this.restaurantRepository = restaurantRepository;
            this.cuisineFactory = cuisineFactory;
            this.cuisineRepository = cuisineRepository;
            this.paymentMethodRepository = paymentMethodRepository;
            this.userFactory = userFactory;
            this.userRepository = userRepository;
        }

        public async Task ImportRestaurantAsync(ImportLog log, int rowIndex, RestaurantRow restaurantRow,
            UserId curUserId, bool dryRun, CancellationToken cancellationToken = default)
        {
            try
            {
                var address = new Address(
                    restaurantRow.Street,
                    restaurantRow.ZipCode,
                    restaurantRow.City
                );

                var contactInfo = new ContactInfo(
                    restaurantRow.Phone,
                    restaurantRow.Fax,
                    restaurantRow.WebSite,
                    restaurantRow.ResponsiblePerson,
                    restaurantRow.OrderEmailAddress,
                    null,
                    false
                );

                var regularOpeningDays = new RegularOpeningDays(Enumerable.Empty<RegularOpeningDay>());
                regularOpeningDays = AddRegularOpeningDays(regularOpeningDays, 0, restaurantRow.OpeningHoursMonday);
                regularOpeningDays = AddRegularOpeningDays(regularOpeningDays, 1, restaurantRow.OpeningHoursMonday);
                regularOpeningDays = AddRegularOpeningDays(regularOpeningDays, 2, restaurantRow.OpeningHoursMonday);
                regularOpeningDays = AddRegularOpeningDays(regularOpeningDays, 3, restaurantRow.OpeningHoursMonday);
                regularOpeningDays = AddRegularOpeningDays(regularOpeningDays, 4, restaurantRow.OpeningHoursMonday);
                regularOpeningDays = AddRegularOpeningDays(regularOpeningDays, 5, restaurantRow.OpeningHoursMonday);
                regularOpeningDays = AddRegularOpeningDays(regularOpeningDays, 6, restaurantRow.OpeningHoursMonday);

                var deviatingOpeningDays = GetDeviatingOpeningDays(restaurantRow.DeviatingOpeningHours);

                var minimumOrderValuePickup = restaurantRow.MinimumOrderValuePickup.HasValue
                    ? (decimal?) restaurantRow.MinimumOrderValuePickup.Value
                    : null;

                var minimumOrderValueDelivery = restaurantRow.MinimumOrderValueDelivery.HasValue
                    ? (decimal?) restaurantRow.MinimumOrderValueDelivery.Value
                    : null;

                var deliveryCosts = restaurantRow.DeliveryCosts.HasValue
                    ? (decimal?) restaurantRow.DeliveryCosts.Value
                    : null;

                var (pickupInfo, deliveryInfo, reservationInfo) = GetServices(restaurantRow.OrderTypes,
                    restaurantRow.AverageTime.HasValue ? (int?) restaurantRow.AverageTime.Value.TotalMinutes : null,
                    minimumOrderValuePickup, minimumOrderValueDelivery, deliveryCosts);

                var cuisines = await GetOrAddCuisinesAsync(log, rowIndex, restaurantRow.Cuisines, dryRun, curUserId);

                var paymentMethods = await GetPaymentMethodsAsync(restaurantRow.PaymentMethods);

                var administrators = await GetOrAddAdministratorAsync(log, rowIndex,
                    restaurantRow.AdministratorUserEmailAddress, dryRun, curUserId);

                var supportedOrderMode = GetSupportOrderMode(restaurantRow.SupportedOrderMode);

                ExternalMenu externalMenu = null;
                var externalMenuId = new ExternalMenuId(Guid.Parse("EA9D3F69-4709-4F4A-903C-7EA68C0A36C7"));

                if (!string.IsNullOrWhiteSpace(restaurantRow.ExternalMenuUrl))
                {
                    externalMenu = new ExternalMenu(
                        externalMenuId,
                        !string.IsNullOrWhiteSpace(restaurantRow.ExternalMenuName)
                            ? restaurantRow.ExternalMenuName
                            : "Tageskarte",
                        !string.IsNullOrWhiteSpace(restaurantRow.ExternalMenuDescription)
                            ? restaurantRow.ExternalMenuDescription
                            : "Täglich wechselnde Tageskarte, nur telefonisch bestellbar.",
                        restaurantRow.ExternalMenuUrl
                    );
                }

                var newRestaurant = false;
                var restaurant =
                    await restaurantRepository.FindByImportIdAsync(restaurantRow.ImportId, cancellationToken);
                if (restaurant == null)
                {
                    newRestaurant = true;
                    restaurant = restaurantFactory.CreateWithName(restaurantRow.Name, curUserId);
                }

                restaurant.ChangeAddress(address, curUserId);
                restaurant.ChangeContactInfo(contactInfo, curUserId);
                restaurant.ChangeRegularOpeningDays(regularOpeningDays, curUserId);
                restaurant.ChangeDeviatingOpeningDays(deviatingOpeningDays, curUserId);
                restaurant.ChangePickupInfo(pickupInfo, curUserId);
                restaurant.ChangeDeliveryInfo(deliveryInfo, curUserId);
                restaurant.ChangeReservationInfo(reservationInfo, curUserId);
                restaurant.ChangeHygienicHandling(restaurantRow.HygienicHandling, curUserId);
                restaurant.ChangeCuisines(cuisines, curUserId);
                restaurant.ChangePaymentMethods(paymentMethods, curUserId);
                restaurant.ChangeAdministrators(administrators, curUserId);
                restaurant.ChangeImportId(restaurantRow.ImportId, curUserId);
                if (restaurantRow.IsActive)
                    restaurant.Activate(curUserId);
                else
                    restaurant.Deactivate(curUserId);
                restaurant.DisableSupport(curUserId);
                restaurant.ChangeSupportedOrderMode(supportedOrderMode, curUserId);
                if (externalMenu != null)
                {
                    restaurant.SetExternalMenu(externalMenu, curUserId);
                }
                else if (restaurant.TryGetExternalMenu(externalMenuId, out var _))
                {
                    restaurant.RemoveExternalMenu(externalMenuId, curUserId);
                }

                var activityStatus = restaurantRow.IsActive ? "aktiv" : "inaktiv";
                log.AddLine(ImportLogLineType.Information, rowIndex,
                    newRestaurant
                        ? "Lege ein neues Restaurant '{0}' an ({1})."
                        : "Aktualisiere das bereits existierende Restaurant '{0}' ({1}).", restaurant.Name,
                    activityStatus);

                if (!dryRun)
                    await restaurantRepository.StoreAsync(restaurant, cancellationToken);
            }
            catch (DomainException e)
            {
                AddFailureMessageToLog(log, rowIndex, e.Failure);
            }
        }

        private RegularOpeningDays AddRegularOpeningDays(RegularOpeningDays regularOpeningDays, int dayOfWeek, string openingHoursText)
        {
            if (string.IsNullOrWhiteSpace(openingHoursText))
                return regularOpeningDays;
            if (openingHoursText.Trim() == "-")
                return regularOpeningDays;

            openingHoursText = openingHoursText.Replace(",", ";");

            var openingPeriodTexts = openingHoursText.Split(';');

            foreach (var openingPeriodText in openingPeriodTexts)
            {
                var trimmedOpeningPeriodText = openingPeriodText.Trim();
                var openingPeriodParts = trimmedOpeningPeriodText.Split('-');
                if (openingPeriodParts.Length != 2)
                    throw DomainException.CreateFrom(new ImportOpeningPeriodIsInvalidFailure(openingPeriodText));

                var parsedStartTime = ParseTime(openingPeriodParts[0]);
                var parsedEndTime = ParseTime(openingPeriodParts[1]);

                var openingPeriod = new OpeningPeriod(parsedStartTime, parsedEndTime);

                regularOpeningDays = regularOpeningDays.AddOpeningPeriod(dayOfWeek, openingPeriod);
            }

            return regularOpeningDays;
        }

        private DeviatingOpeningDays GetDeviatingOpeningDays(string openingHoursText)
        {
            var deviatingOpeningDays = new DeviatingOpeningDays(Enumerable.Empty<DeviatingOpeningDay>());

            if (string.IsNullOrWhiteSpace(openingHoursText))
                return deviatingOpeningDays;

            openingHoursText = openingHoursText.Replace(",", ";");

            var openingDays = openingHoursText.Split('|');
            foreach (var openingDay in openingDays)
            {
                var tempOpeningDay = openingDay.Trim();

                if (tempOpeningDay.Length == 0)
                    continue;

                var index = tempOpeningDay.IndexOf(':');
                if (index < 1)
                    throw DomainException.CreateFrom(new ImportOpeningPeriodIsInvalidFailure(openingHoursText));
                var dateText = tempOpeningDay.Substring(0, index);
                var dateTextParts = dateText.Split('.');

                if (dateTextParts.Length == 0)
                    throw DomainException.CreateFrom(new ImportOpeningPeriodIsInvalidFailure(openingHoursText));

                var dateParts = new int[dateTextParts.Length];
                for (var partIdx = 0; partIdx < dateParts.Length; partIdx++)
                {
                    var dateTextPart = dateTextParts[partIdx];
                    if (!int.TryParse(dateTextPart, out var datePart))
                        throw DomainException.CreateFrom(new ImportOpeningPeriodIsInvalidFailure(openingHoursText));
                    dateParts[partIdx] = datePart;
                }

                Date date;
                if (dateParts.Length == 3)
                {
                    date = new Date(dateParts[2], dateParts[1], dateParts[0]);
                }
                else if (dateParts.Length == 2)
                {
                    var today = DateTimeOffset.UtcNow.ToUtcDate();

                    date = dateParts[1] >= today.Month
                        ? new Date(today.Year, dateParts[1], dateParts[0])
                        : new Date(today.Year + 1, dateParts[1], dateParts[0]);
                }
                else
                {
                    throw DomainException.CreateFrom(new ImportOpeningPeriodIsInvalidFailure(openingHoursText));
                }

                tempOpeningDay = tempOpeningDay.Substring(index + 1).Trim();

                if (tempOpeningDay == "geschlossen")
                {
                    deviatingOpeningDays = deviatingOpeningDays.AddOpeningDay(date, DeviatingOpeningDayStatus.Closed);
                }
                else if (tempOpeningDay == "ausgebucht")
                {
                    deviatingOpeningDays = deviatingOpeningDays.AddOpeningDay(date, DeviatingOpeningDayStatus.FullyBooked);
                }
                else
                {
                    deviatingOpeningDays = deviatingOpeningDays.AddOpeningDay(date, DeviatingOpeningDayStatus.Open);

                    var openingPeriodTexts = tempOpeningDay.Split(';');
                    foreach (var openingPeriodText in openingPeriodTexts)
                    {
                        var trimmedOpeningPeriodText = openingPeriodText.Trim();
                        var openingPeriodParts = trimmedOpeningPeriodText.Split('-');
                        if (openingPeriodParts.Length != 2)
                            throw DomainException.CreateFrom(new ImportOpeningPeriodIsInvalidFailure(openingHoursText));

                        var parsedStartTime = ParseTime(openingPeriodParts[0]);
                        var parsedEndTime = ParseTime(openingPeriodParts[1]);

                        var openingPeriod = new OpeningPeriod(parsedStartTime, parsedEndTime);

                        deviatingOpeningDays = deviatingOpeningDays.AddOpeningPeriod(date, openingPeriod);
                    }
                }
            }

            return deviatingOpeningDays;
        }

        private TimeSpan ParseTime(string timeText)
        {
            var trimmedTimeText = timeText.Trim().Replace(".", ":");
            var timeTextParts = trimmedTimeText.Split(':');
            switch (timeTextParts.Length)
            {
                case 2:
                {
                    var hourText = timeTextParts[0];
                    if (!int.TryParse(hourText, out var hours))
                        throw DomainException.CreateFrom(new ImportOpeningPeriodIsInvalidFailure(timeText));
                    var minuteText = timeTextParts[1];
                    if (!int.TryParse(minuteText, out var minutes))
                        throw DomainException.CreateFrom(new ImportOpeningPeriodIsInvalidFailure(timeText));
                    return new TimeSpan(0, hours, minutes, 0);
                }
                case 1:
                {
                    var hourText = timeTextParts[0];
                    if (!int.TryParse(hourText, out var hours))
                        throw DomainException.CreateFrom(new ImportOpeningPeriodIsInvalidFailure(timeText));
                    return new TimeSpan(0, hours, 0, 0);
                }
                default:
                    throw DomainException.CreateFrom(new ImportOpeningPeriodIsInvalidFailure(timeText));
            }
        }

        private (PickupInfo, DeliveryInfo, ReservationInfo) GetServices(string orderTypesText, int? averageTime,
            decimal? minimumOrderValuePickup, decimal? minimumOrderValueDelivery, decimal? deliveryCosts)
        {
            var orderTypeTexts = orderTypesText?.Split(',') ?? new string[0];

            var hadPickup = false;
            var hadDelivery = false;
            var hadReservation = false;

            foreach (var orderTypeText in orderTypeTexts)
            {
                var orderTypeTextTrimmed = orderTypeText.Trim();

                switch (orderTypeTextTrimmed)
                {
                    case "Abholung":
                    {
                        if (hadPickup)
                            continue;
                        hadPickup = true;
                        break;
                    }
                    case "Lieferung":
                    {
                        if (hadDelivery)
                            continue;
                        hadDelivery = true;
                        break;
                    }
                    case "Tischreservierung":
                    {
                        if (hadReservation)
                            continue;
                        hadReservation = true;
                        break;
                    }
                    default:
                        throw DomainException.CreateFrom(new ImportOrderTypeIsInvalidFailure(orderTypeText));
                }
            }

            var pickupInfo =
                new PickupInfo(hadPickup, averageTime, minimumOrderValuePickup, null);
            var deliveryInfo =
                new DeliveryInfo(hadDelivery, averageTime, minimumOrderValueDelivery, null, deliveryCosts);
            var reservationInfo =
                new ReservationInfo(hadReservation, null);

            return (pickupInfo, deliveryInfo, reservationInfo);
        }

        private async Task<ISet<CuisineId>> GetOrAddCuisinesAsync(ImportLog log, int rowIndex, string cuisinesText, bool dryRun, UserId curUserId)
        {
            if (string.IsNullOrWhiteSpace(cuisinesText))
                throw DomainException.CreateFrom(new ImportCuisinesNotFoundFailure());

            var cuisineIds = new HashSet<CuisineId>();

            var cuisineNames = cuisinesText.Split(',');
            foreach (var cuisineName in cuisineNames)
            {
                var cuisineId = await GetOrAddCuisineAsync(log, rowIndex, cuisineName.Trim(), dryRun, curUserId);
                cuisineIds.Add(cuisineId);
            }

            return cuisineIds;
        }

        private async Task<CuisineId> GetOrAddCuisineAsync(ImportLog log, int rowIndex, string cuisineName, bool dryRun,
            UserId curUserId)
        {
            var cuisine = await cuisineRepository.FindByNameAsync(cuisineName) ??
                          await CreateNewCuisineAsync(log, rowIndex, cuisineName, dryRun, curUserId);
            return cuisine.Id;
        }

        private async Task<Cuisine> CreateNewCuisineAsync(ImportLog log, int rowIndex, string cuisineName, bool dryRun,
            UserId curUserId)
        {
            var cuisine = cuisineFactory.Create(cuisineName, curUserId);

            log.AddLine(ImportLogLineType.Information, rowIndex, "Lege eine neue Cuisine mit Namen '{0}' an.",
                cuisineName);

            if (!dryRun)
                await cuisineRepository.StoreAsync(cuisine);

            return cuisine;
        }

        private async Task<ISet<PaymentMethodId>> GetPaymentMethodsAsync(string paymentMethodsText)
        {
            if (string.IsNullOrWhiteSpace(paymentMethodsText))
                throw DomainException.CreateFrom(new ImportPaymentMethodsNotFoundFailure());

            var paymentMethodIds = new HashSet<PaymentMethodId>();

            var paymentMethodNames = paymentMethodsText.Split(',');
            foreach (var paymentMethodName in paymentMethodNames)
            {
                var paymentMethodId = await GetPaymentMethodAsync(paymentMethodName.Trim());
                paymentMethodIds.Add(paymentMethodId);
            }

            return paymentMethodIds;
        }

        private async Task<PaymentMethodId> GetPaymentMethodAsync(string paymentMethodName)
        {
            var paymentMethodNameTrimmed = paymentMethodName.Trim();
            var paymentMethod = await paymentMethodRepository.FindByNameAsync(paymentMethodNameTrimmed, true);
            return paymentMethod != null
                ? paymentMethod.Id
                : throw DomainException.CreateFrom(new ImportPaymentMethodNotFoundFailure(paymentMethodName));
        }

        private async Task<ISet<UserId>> GetOrAddAdministratorAsync(ImportLog log, int rowIndex, string administratorEmailAddress, bool dryRun, UserId curUserId)
        {
            var administrators = new HashSet<UserId>();

            if (string.IsNullOrWhiteSpace(administratorEmailAddress))
                return administrators;

            var user = await userRepository.FindByEmailAsync(administratorEmailAddress.Trim().ToLowerInvariant());
            if (user == null)
            {
                user = await CreateNewUserAsync(log, rowIndex, administratorEmailAddress.Trim().ToLowerInvariant(),
                    dryRun, curUserId);
            }

            administrators.Add(user.Id);

            return administrators;
        }

        private async Task<User> CreateNewUserAsync(ImportLog log, int rowIndex,
            string userEmailAddress, bool dryRun, UserId curUserId)
        {
            var password = "Start2020!";
            var user = userFactory.Create(Role.RestaurantAdmin, userEmailAddress, password, false, curUserId);

            log.AddLine(ImportLogLineType.Information, rowIndex,
                "Lege einen neuen Benutzer mit Email-Adresse '{0}' und Passwort '{1}' an.",
                userEmailAddress, password);

            if (!dryRun)
                await userRepository.StoreAsync(user);

            return user;
        }

        private SupportedOrderMode GetSupportOrderMode(string supportedOrderModeText)
        {
            if (string.IsNullOrEmpty(supportedOrderModeText))
                return SupportedOrderMode.OnlyPhone;

            switch (supportedOrderModeText)
            {
                case "Telefonisch":
                    return SupportedOrderMode.OnlyPhone;
                case "Schicht":
                    return SupportedOrderMode.AtNextShift;
                case "Jederzeit":
                    return SupportedOrderMode.Anytime;
                default:
                    throw DomainException.CreateFrom(new ImportUnknownSupportedOrderModeFailure(supportedOrderModeText));
            }
        }

        private static void AddFailureMessageToLog(ImportLog log, int rowIndex, Failure failure)
        {
            log.AddLine(ImportLogLineType.Error, rowIndex, failure.Message);
        }
    }
}
