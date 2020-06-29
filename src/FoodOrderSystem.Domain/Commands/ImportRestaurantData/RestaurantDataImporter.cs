using System;
using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Domain.Model;
using FoodOrderSystem.Domain.Model.Cuisine;
using FoodOrderSystem.Domain.Model.PaymentMethod;
using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;
using FoodOrderSystem.Domain.Services;

namespace FoodOrderSystem.Domain.Commands.ImportRestaurantData
{
    public class RestaurantDataImporter : IRestaurantDataImporter
    {
        private readonly IFailureMessageService failureMessageService;
        private readonly IRestaurantFactory restaurantFactory;
        private readonly IRestaurantRepository restaurantRepository;
        private readonly ICuisineFactory cuisineFactory;
        private readonly ICuisineRepository cuisineRepository;
        private readonly IPaymentMethodRepository paymentMethodRepository;
        private readonly IUserFactory userFactory;
        private readonly IUserRepository userRepository;

        public RestaurantDataImporter(
            IFailureMessageService failureMessageService,
            IRestaurantFactory restaurantFactory,
            IRestaurantRepository restaurantRepository,
            ICuisineFactory cuisineFactory,
            ICuisineRepository cuisineRepository,
            IPaymentMethodRepository paymentMethodRepository,
            IUserFactory userFactory,
            IUserRepository userRepository
        )
        {
            this.failureMessageService = failureMessageService;
            this.restaurantFactory = restaurantFactory;
            this.restaurantRepository = restaurantRepository;
            this.cuisineFactory = cuisineFactory;
            this.cuisineRepository = cuisineRepository;
            this.paymentMethodRepository = paymentMethodRepository;
            this.userFactory = userFactory;
            this.userRepository = userRepository;
        }

        public async Task ImportRestaurantAsync(RestaurantImportLog log, int rowIndex, RestaurantRow restaurantRow,
            UserId curUserId, bool dryRun, CancellationToken cancellationToken = default)
        {
            Result<bool> boolResult;

            var newRestaurant = false;

            var restaurant = await restaurantRepository.FindByImportIdAsync(restaurantRow.ImportId, cancellationToken);
            if (restaurant == null)
            {
                newRestaurant = true;

                var restaurantResult = restaurantFactory.CreateWithName(restaurantRow.Name, curUserId);
                if (restaurantResult.IsFailure)
                {
                    AddFailureMessageToLog(log, rowIndex, restaurantResult);
                    return;
                }

                restaurant = restaurantResult.Value;

                boolResult = restaurant.ChangeImportId(restaurantRow.ImportId, curUserId);
                if (boolResult.IsFailure)
                {
                    AddFailureMessageToLog(log, rowIndex, boolResult);
                    return;
                }
            }

            var address = new Address(
                restaurantRow.Street,
                restaurantRow.ZipCode,
                restaurantRow.City
            );
            boolResult = restaurant.ChangeAddress(address, curUserId);
            if (boolResult.IsFailure)
            {
                AddFailureMessageToLog(log, rowIndex, boolResult);
                return;
            }

            var contactInfo = new ContactInfo(
                restaurantRow.Phone,
                restaurantRow.Fax,
                restaurantRow.WebSite,
                restaurantRow.ResponsiblePerson,
                restaurantRow.OrderEmailAddress
            );
            boolResult = restaurant.ChangeContactInfo(contactInfo, curUserId);
            if (boolResult.IsFailure)
            {
                AddFailureMessageToLog(log, rowIndex, boolResult);
                return;
            }

            boolResult = AddOpeningHours(restaurant, 0, restaurantRow.OpeningHoursMonday, curUserId);
            if (boolResult.IsFailure)
            {
                AddFailureMessageToLog(log, rowIndex, boolResult);
                return;
            }

            boolResult = AddOpeningHours(restaurant, 1, restaurantRow.OpeningHoursTuesday, curUserId);
            if (boolResult.IsFailure)
            {
                AddFailureMessageToLog(log, rowIndex, boolResult);
                return;
            }

            boolResult = AddOpeningHours(restaurant, 2, restaurantRow.OpeningHoursWednesday, curUserId);
            if (boolResult.IsFailure)
            {
                AddFailureMessageToLog(log, rowIndex, boolResult);
                return;
            }

            boolResult = AddOpeningHours(restaurant, 3, restaurantRow.OpeningHoursThursday, curUserId);
            if (boolResult.IsFailure)
            {
                AddFailureMessageToLog(log, rowIndex, boolResult);
                return;
            }

            boolResult = AddOpeningHours(restaurant, 4, restaurantRow.OpeningHoursFriday, curUserId);
            if (boolResult.IsFailure)
            {
                AddFailureMessageToLog(log, rowIndex, boolResult);
                return;
            }

            boolResult = AddOpeningHours(restaurant, 5, restaurantRow.OpeningHoursSaturday, curUserId);
            if (boolResult.IsFailure)
            {
                AddFailureMessageToLog(log, rowIndex, boolResult);
                return;
            }

            boolResult = AddOpeningHours(restaurant, 6, restaurantRow.OpeningHoursSunday, curUserId);
            if (boolResult.IsFailure)
            {
                AddFailureMessageToLog(log, rowIndex, boolResult);
                return;
            }

            var minimumOrderValuePickup = restaurantRow.MinimumOrderValuePickup.HasValue
                ? (decimal?) restaurantRow.MinimumOrderValuePickup.Value
                : null;

            var minimumOrderValueDelivery = restaurantRow.MinimumOrderValueDelivery.HasValue
                ? (decimal?) restaurantRow.MinimumOrderValueDelivery.Value
                : null;

            var deliveryCosts = restaurantRow.DeliveryCosts.HasValue
                ? (decimal?) restaurantRow.DeliveryCosts.Value
                : null;

            boolResult = AddServices(restaurant, restaurantRow.OrderTypes, restaurantRow.AverageTime,
                minimumOrderValuePickup, minimumOrderValueDelivery, deliveryCosts, curUserId);
            if (boolResult.IsFailure)
            {
                AddFailureMessageToLog(log, rowIndex, boolResult);
                return;
            }

            boolResult = restaurant.ChangeHygienicHandling(restaurantRow.HygienicHandling, curUserId);
            if (boolResult.IsFailure)
            {
                AddFailureMessageToLog(log, rowIndex, boolResult);
                return;
            }

            boolResult = await AddCuisinesAsync(log, rowIndex, restaurant, restaurantRow.Cuisines, dryRun, curUserId);
            if (boolResult.IsFailure)
            {
                AddFailureMessageToLog(log, rowIndex, boolResult);
                return;
            }

            boolResult = await AddPaymentMethodsAsync(restaurant, restaurantRow.PaymentMethods, curUserId);
            if (boolResult.IsFailure)
            {
                AddFailureMessageToLog(log, rowIndex, boolResult);
                return;
            }

            boolResult =
                await AddAdministratorAsync(log, rowIndex, restaurant, restaurantRow.AdministratorUserEmailAddress, dryRun, curUserId);
            if (boolResult.IsFailure)
            {
                AddFailureMessageToLog(log, rowIndex, boolResult);
                return;
            }

            log.AddLine(RestaurantImportLogLineType.Information, rowIndex,
                newRestaurant
                    ? "Lege ein neues Restaurant '{0}' an."
                    : "Aktualisiere das bereits existierende Restaurant '{0}'.", restaurant.Name);

            if (!dryRun)
                await restaurantRepository.StoreAsync(restaurant, cancellationToken);
        }

        private Result<bool> AddOpeningHours(Restaurant restaurant, int dayOfWeek, string openingHoursText,
            UserId curUserId)
        {
            if (string.IsNullOrWhiteSpace(openingHoursText))
                return SuccessResult<bool>.Create(true);
            
            if (openingHoursText.Trim() == "-")
                return SuccessResult<bool>.Create(true);

            openingHoursText = openingHoursText.Replace(",", ";");

            var openingPeriodTexts = openingHoursText.Split(';');
            foreach (var openingPeriodText in openingPeriodTexts)
            {
                var trimmedOpeningPeriodText = openingPeriodText.Trim();
                var openingPeriodParts = trimmedOpeningPeriodText.Split('-');
                if (openingPeriodParts.Length != 2)
                    return FailureResult<bool>.Create(FailureResultCode.ImportOpeningPeriodIsInvalid, openingHoursText);

                var parseStartTimeResult = ParseTime(openingPeriodParts[0]);
                if (parseStartTimeResult.IsFailure)
                    return parseStartTimeResult.Cast<bool>();

                var parseEndTimeResult = ParseTime(openingPeriodParts[1]);
                if (parseEndTimeResult.IsFailure)
                    return parseEndTimeResult.Cast<bool>();

                var addOpeningPeriodResult = restaurant.AddOpeningPeriod(
                    new OpeningPeriod(dayOfWeek, parseStartTimeResult.Value, parseEndTimeResult.Value), curUserId);
                if (addOpeningPeriodResult.IsFailure)
                    return addOpeningPeriodResult;
            }

            return SuccessResult<bool>.Create(true);
        }

        private Result<TimeSpan> ParseTime(string timeText)
        {
            var trimmedTimeText = timeText.Trim().Replace(".", ":");
            var timeTextParts = trimmedTimeText.Split(':');
            switch (timeTextParts.Length)
            {
                case 2:
                {
                    var hourText = timeTextParts[0];
                    if (!int.TryParse(hourText, out var hours))
                        return FailureResult<TimeSpan>.Create(FailureResultCode.ImportOpeningPeriodIsInvalid, timeText);
                    var minuteText = timeTextParts[1];
                    if (!int.TryParse(minuteText, out var minutes))
                        return FailureResult<TimeSpan>.Create(FailureResultCode.ImportOpeningPeriodIsInvalid, timeText);
                    return SuccessResult<TimeSpan>.Create(new TimeSpan(0, hours, minutes, 0));
                }
                case 1:
                {
                    var hourText = timeTextParts[0];
                    if (!int.TryParse(hourText, out var hours))
                        return FailureResult<TimeSpan>.Create(FailureResultCode.ImportOpeningPeriodIsInvalid, timeText);
                    return SuccessResult<TimeSpan>.Create(new TimeSpan(0, hours, 0, 0));
                }
                default:
                    return FailureResult<TimeSpan>.Create(FailureResultCode.ImportOpeningPeriodIsInvalid, timeText);
            }
        }

        private Result<bool> AddServices(Restaurant restaurant, string orderTypesText, TimeSpan? averageTime,
            decimal? minimumOrderValuePickup, decimal? minimumOrderValueDelivery, decimal? deliveryCosts,
            UserId curUserId)
        {
            var orderTypeTexts = orderTypesText.Split(',');

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
                        return FailureResult<bool>.Create(FailureResultCode.ImportOrderTypeIsInvalid, orderTypeTextTrimmed);
                }
            }

            var boolResult = restaurant.ChangePickupInfo(
                new PickupInfo(hadPickup, averageTime, minimumOrderValuePickup, null),
                curUserId);
            if (boolResult.IsFailure)
                return boolResult;

            boolResult =
                restaurant.ChangeDeliveryInfo(
                    new DeliveryInfo(hadDelivery, averageTime, minimumOrderValueDelivery, null, deliveryCosts),
                    curUserId);
            if (boolResult.IsFailure)
                return boolResult;

            boolResult = restaurant.ChangeReservationInfo(new ReservationInfo(hadReservation), curUserId);
            if (boolResult.IsFailure)
                return boolResult;

            return SuccessResult<bool>.Create(true);
        }

        private async Task<Result<bool>> AddCuisinesAsync(RestaurantImportLog log, int rowIndex, Restaurant restaurant,
            string cuisinesText, bool dryRun, UserId curUserId)
        {
            if (string.IsNullOrWhiteSpace(cuisinesText))
                return FailureResult<bool>.Create(FailureResultCode.RequiredFieldEmpty, "Cuisines");

            var cuisineNames = cuisinesText.Split(',');
            foreach (var cuisineName in cuisineNames)
            {
                var boolResult =
                    await AddCuisineAsync(log, rowIndex, restaurant, cuisineName.Trim(), dryRun, curUserId);
                if (boolResult.IsFailure)
                    return boolResult;
            }

            return SuccessResult<bool>.Create(true);
        }

        private async Task<Result<bool>> AddCuisineAsync(RestaurantImportLog log, int rowIndex, Restaurant restaurant,
            string cuisineName, bool dryRun, UserId curUserId)
        {
            var cuisine = await cuisineRepository.FindByNameAsync(cuisineName);

            if (cuisine == null)
            {
                var createNewCuisineResult = await CreateNewCuisineAsync(log, rowIndex, cuisineName, dryRun, curUserId);
                if (createNewCuisineResult.IsFailure)
                    return createNewCuisineResult.Cast<bool>();
                cuisine = createNewCuisineResult.Value;
            }

            return restaurant.AddCuisine(cuisine.Id, curUserId);
        }

        private async Task<Result<Cuisine>> CreateNewCuisineAsync(RestaurantImportLog log, int rowIndex,
            string cuisineName, bool dryRun, UserId curUserId)
        {
            var createCuisineResult = cuisineFactory.Create(cuisineName, curUserId);
            if (createCuisineResult.IsFailure)
                return createCuisineResult;
            log.AddLine(RestaurantImportLogLineType.Information, rowIndex, "Lege eine neue Cuisine mit Namen '{0}' an.",
                cuisineName);
            if (!dryRun)
                await cuisineRepository.StoreAsync(createCuisineResult.Value);
            return createCuisineResult;
        }

        private async Task<Result<bool>> AddPaymentMethodsAsync(Restaurant restaurant, string paymentMethodsText,
            UserId curUserId)
        {
            if (string.IsNullOrWhiteSpace(paymentMethodsText))
                return FailureResult<bool>.Create(FailureResultCode.RequiredFieldEmpty, "PaymentMethods");

            var paymentMethodNames = paymentMethodsText.Split(',');
            foreach (var paymentMethodName in paymentMethodNames)
            {
                var boolResult = await AddPaymentMethodAsync(restaurant, paymentMethodName.Trim(), curUserId);
                if (boolResult.IsFailure)
                    return boolResult;
            }

            return SuccessResult<bool>.Create(true);
        }

        private async Task<Result<bool>> AddPaymentMethodAsync(Restaurant restaurant, string paymentMethodName,
            UserId curUserId)
        {
            var paymentMethodNameTrimmed = paymentMethodName.Trim();
            var paymentMethod = await paymentMethodRepository.FindByNameAsync(paymentMethodNameTrimmed, true);
            return paymentMethod != null
                ? restaurant.AddPaymentMethod(paymentMethod.Id, curUserId)
                : FailureResult<bool>.Create(FailureResultCode.ImportPaymentMethodNotFound, paymentMethodNameTrimmed);
        }

        private async Task<Result<bool>> AddAdministratorAsync(RestaurantImportLog log, int rowIndex,
            Restaurant restaurant, string administratorEmailAddress, bool dryRun, UserId curUserId)
        {
            if (string.IsNullOrWhiteSpace(administratorEmailAddress))
                return SuccessResult<bool>.Create(true);

            var user = await userRepository.FindByEmailAsync(administratorEmailAddress.Trim().ToLowerInvariant());
            
            if (user == null)
            {
                var createNewUserResult =
                    await CreateNewUserAsync(log, rowIndex, administratorEmailAddress, dryRun, curUserId);
                if (createNewUserResult.IsFailure)
                    return createNewUserResult.Cast<bool>();
                user = createNewUserResult.Value;
            }

            return restaurant.AddAdministrator(user.Id, curUserId);
        }

        private async Task<Result<User>> CreateNewUserAsync(RestaurantImportLog log, int rowIndex,
            string userEmailAddress, bool dryRun, UserId curUserId)
        {
            var password = "Start2020!";
            var createUserResult =
                userFactory.Create(Role.RestaurantAdmin, userEmailAddress, password, false, curUserId);
            if (createUserResult.IsFailure)
                return createUserResult;
            log.AddLine(RestaurantImportLogLineType.Information, rowIndex, "Lege einen neuen Benutzer mit Email-Adresse '{0}' und Passwort '{1}' an.",
                userEmailAddress, password);
            if (!dryRun)
                await userRepository.StoreAsync(createUserResult.Value);
            return createUserResult;
        }

        private void AddFailureMessageToLog<T>(RestaurantImportLog log, int rowIndex, Result<T> result)
        {
            var message = failureMessageService.GetTranslatedMessage((FailureResult<T>) result);
            log.AddLine(RestaurantImportLogLineType.Error, rowIndex, message);
        }
    }
}