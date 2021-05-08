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
using Microsoft.Extensions.Logging;

namespace Gastromio.Core.Application.Commands.AddTestData
{
    public class AddTestDataCommandHandler : ICommandHandler<AddTestDataCommand>
    {
        private readonly ILogger<AddTestDataCommandHandler> logger;
        private readonly IUserFactory userFactory;
        private readonly IUserRepository userRepository;
        private readonly ICuisineFactory cuisineFactory;
        private readonly ICuisineRepository cuisineRepository;
        private readonly IPaymentMethodRepository paymentMethodRepository;
        private readonly IRestaurantFactory restaurantFactory;
        private readonly IRestaurantRepository restaurantRepository;

        private readonly Dictionary<string, User> restAdminDict = new Dictionary<string, User>();
        private readonly List<Cuisine> cuisines = new List<Cuisine>();

        private List<PaymentMethod> paymentMethods;

        public AddTestDataCommandHandler(
            ILogger<AddTestDataCommandHandler> logger,
            IUserFactory userFactory,
            IUserRepository userRepository,
            ICuisineFactory cuisineFactory,
            ICuisineRepository cuisineRepository,
            IPaymentMethodRepository paymentMethodRepository,
            IRestaurantFactory restaurantFactory,
            IRestaurantRepository restaurantRepository
        )
        {
            this.logger = logger;
            this.userFactory = userFactory;
            this.userRepository = userRepository;
            this.cuisineFactory = cuisineFactory;
            this.cuisineRepository = cuisineRepository;
            this.paymentMethodRepository = paymentMethodRepository;
            this.restaurantFactory = restaurantFactory;
            this.restaurantRepository = restaurantRepository;
        }

        public async Task HandleAsync(AddTestDataCommand command, User currentUser,
            CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                throw DomainException.CreateFrom(new SessionExpiredFailure());

            if (currentUser.Role < Role.SystemAdmin)
                throw DomainException.CreateFrom(new ForbiddenFailure());

            paymentMethods = (await paymentMethodRepository.FindAllAsync(cancellationToken)).ToList();

            await CreateUsersAsync(currentUser, command.UserCount, cancellationToken);

            await CreateCuisinesAsync(currentUser, cancellationToken);

            await CreateRestaurantsAsync(currentUser, command.RestCount, command.DishCatCount, command.DishCount,
                cancellationToken);
        }

        private async Task CreateUsersAsync(User currentUser, int count, CancellationToken cancellationToken)
        {
            logger.LogInformation("creating test users");

            for (var i = 0; i < count; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                var name = $"sysadmin{(i + 1):D3}";
                var user = userFactory.Create(Role.SystemAdmin, $"{name}@gastromio.de", "Start2020!",
                    true, currentUser.Id);
                await userRepository.StoreAsync(user, cancellationToken);
                logger.LogInformation("    sysadmin user {0} created", name);
            }

            for (var i = 0; i < count; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                var name = $"restadmin{(i + 1):D3}";
                var user = userFactory.Create(Role.SystemAdmin, $"{name}@gastromio.de", "Start2020!",
                    true, currentUser.Id);
                await userRepository.StoreAsync(user, cancellationToken);
                logger.LogInformation("    restadmin user {0} created", name);

                restAdminDict.Add(name, user);
            }

            logger.LogInformation("test users created");
        }

        private async Task CreateCuisinesAsync(User currentUser, CancellationToken cancellationToken)
        {
            logger.LogInformation("creating test cuisines");

            var cuisine = cuisineFactory.Create("Chinesisch", currentUser.Id);
            await cuisineRepository.StoreAsync(cuisine, cancellationToken);
            cuisines.Add(cuisine);

            cuisine = cuisineFactory.Create("Griechisch", currentUser.Id);
            await cuisineRepository.StoreAsync(cuisine, cancellationToken);
            cuisines.Add(cuisine);

            cuisine = cuisineFactory.Create("Italienisch", currentUser.Id);
            await cuisineRepository.StoreAsync(cuisine, cancellationToken);
            cuisines.Add(cuisine);

            cuisine = cuisineFactory.Create("Amerikanisch", currentUser.Id);
            await cuisineRepository.StoreAsync(cuisine, cancellationToken);
            cuisines.Add(cuisine);

            cuisine = cuisineFactory.Create("Mexikanisch", currentUser.Id);
            await cuisineRepository.StoreAsync(cuisine, cancellationToken);
            cuisines.Add(cuisine);

            cuisine = cuisineFactory.Create("Französisch", currentUser.Id);
            await cuisineRepository.StoreAsync(cuisine, cancellationToken);
            cuisines.Add(cuisine);

            cuisine = cuisineFactory.Create("Indisch", currentUser.Id);
            await cuisineRepository.StoreAsync(cuisine, cancellationToken);
            cuisines.Add(cuisine);

            cuisine = cuisineFactory.Create("Mediterran", currentUser.Id);
            await cuisineRepository.StoreAsync(cuisine, cancellationToken);
            cuisines.Add(cuisine);

            cuisine = cuisineFactory.Create("Japnisch", currentUser.Id);
            await cuisineRepository.StoreAsync(cuisine, cancellationToken);
            cuisines.Add(cuisine);

            cuisine = cuisineFactory.Create("Regional", currentUser.Id);
            await cuisineRepository.StoreAsync(cuisine, cancellationToken);
            cuisines.Add(cuisine);

            cuisine = cuisineFactory.Create("Deutsch (gut bürgerlich)", currentUser.Id);
            await cuisineRepository.StoreAsync(cuisine, cancellationToken);
            cuisines.Add(cuisine);

            logger.LogInformation("test cuisines created");
        }

        private async Task CreateRestaurantsAsync(User currentUser, int restCount, int dishCatCount, int dishCount, CancellationToken cancellationToken)
        {
            logger.LogInformation("creating test restaurants");

            for (var i = 0; i < restCount; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;
                await CreateRestaurantAsync(currentUser, i, dishCatCount, dishCount, cancellationToken);
            }

            logger.LogInformation("test restaurants created");
        }

        private async Task CreateRestaurantAsync(User currentUser, int index, int dishCatCount, int dishCount, CancellationToken cancellationToken)
        {
            var restaurantName = $"Restaurant {(index + 1):D3}";
            var restAdminName = $"restadmin{(index % restAdminDict.Count + 1):D3}";

            logger.LogInformation("    creating test restaurant {0}", restaurantName);

            var restaurant = restaurantFactory.CreateWithName(restaurantName, currentUser.Id);

            restaurant.ChangeAddress(new Address(
                "Musterstraße 1",
                "12345",
                "Musterstadt"
            ), currentUser.Id);

            restaurant.ChangeContactInfo(new ContactInfo(
                $"02871/1234-{(index + 1):D3}",
                $"02871/1234-5{(index + 1):D3}",
                $"http://www.restaurant{(index + 1):D3}.de",
                $"Verantw. Person {(index + 1):D3}",
                $"order@restaurant{(index + 1):D3}.de",
                "0171/1234-{(index + 1):D3}",
                true
            ), currentUser.Id);

            for (var i = 0; i < 7; i++)
            {
                restaurant.AddRegularOpeningPeriod(
                    i,
                    new OpeningPeriod(
                        TimeSpan.FromHours(10 + (index % 4) * 0.5),
                        TimeSpan.FromHours(20 + (index % 4) * 0.5)
                    ),
                    currentUser.Id
                );
            }

            restaurant.ChangePickupInfo(new PickupInfo(
                true,
                15 + index / 100,
                5 + (decimal) index / 100,
                100 + (decimal) index / 100
            ), currentUser.Id);

            if (index % 2 == 0)
            {
                restaurant.ChangeDeliveryInfo(new DeliveryInfo(
                    true,
                    15 + index / 100,
                    5 + (decimal) index / 100,
                    100 + (decimal) index / 100,
                    3 + (decimal) index / 100
                ), currentUser.Id);
            }

            if (index % 4 == 0)
            {
                restaurant.ChangeReservationInfo(new ReservationInfo(true, null), currentUser.Id);
            }

            restaurant.AddCuisine(cuisines[(index + 0) % cuisines.Count].Id, currentUser.Id);
            restaurant.AddCuisine(cuisines[(index + 1) % cuisines.Count].Id, currentUser.Id);

            restaurant.AddPaymentMethod(paymentMethods[(index + 0) % paymentMethods.Count].Id, currentUser.Id);
            restaurant.AddPaymentMethod(paymentMethods[(index + 1) % paymentMethods.Count].Id, currentUser.Id);

            var restAdmin = restAdminDict[restAdminName];
            restaurant.AddAdministrator(restAdmin.Id, currentUser.Id);

            SupportedOrderMode supportedOrderMode = SupportedOrderMode.OnlyPhone;
            if (index % 3 == 0)
            {
                supportedOrderMode = SupportedOrderMode.OnlyPhone;
            }
            else if (index % 3 == 1)
            {
                supportedOrderMode = SupportedOrderMode.AtNextShift;
            }
            else if (index % 3 == 2)
            {
                supportedOrderMode = SupportedOrderMode.Anytime;
            }

            restaurant.ChangeSupportedOrderMode(supportedOrderMode, currentUser.Id);

            restaurant.Activate(currentUser.Id);

            for (var catIndex = 0; catIndex < dishCatCount; catIndex++)
            {
                var dishCategoryName = $"Kategorie{(catIndex + 1):D2}";
                logger.LogInformation("        creating dish category {0}", dishCategoryName);

                var dishCategory = restaurant.AddDishCategory(dishCategoryName, null, currentUser.Id);

                for (var dishIndex = 0; dishIndex < dishCount; dishIndex++)
                {
                    var dishName = $"Gericht{(dishIndex + 1):D2}";

                    var variant = new DishVariant(
                        new DishVariantId(Guid.NewGuid()),
                        dishName,
                        5 + (decimal) dishIndex / 10
                    );

                    var dishDescription = $"Beschreibung des Gerichts{(dishIndex + 1):D2}";
                    var dishProductInfo = $"Produktinformation des Gerichts{(dishIndex + 1):D2}";
                    var dishVariants = new []{variant};

                    restaurant.AddOrChangeDish(dishCategory.Id, null, dishName, dishDescription, dishProductInfo,
                        dishIndex, dishVariants, currentUser.Id);
                }

                logger.LogInformation("        dish category {0} created", dishCategoryName);
            }

            await restaurantRepository.StoreAsync(restaurant, cancellationToken);

            logger.LogInformation("    test restaurant {0} created", restaurantName);
        }
    }
}
