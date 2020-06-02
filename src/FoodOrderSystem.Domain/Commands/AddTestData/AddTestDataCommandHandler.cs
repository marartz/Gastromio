using FoodOrderSystem.Domain.Model;
using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Domain.Model.Cuisine;
using FoodOrderSystem.Domain.Model.Dish;
using FoodOrderSystem.Domain.Model.DishCategory;
using FoodOrderSystem.Domain.Model.PaymentMethod;
using Microsoft.Extensions.Logging;

namespace FoodOrderSystem.Domain.Commands.AddTestData
{
    public class AddTestDataCommandHandler : ICommandHandler<AddTestDataCommand, bool>
    {
        private readonly ILogger<AddTestDataCommandHandler> logger;
        private readonly IUserFactory userFactory;
        private readonly IUserRepository userRepository;
        private readonly ICuisineFactory cuisineFactory;
        private readonly ICuisineRepository cuisineRepository;
        private readonly IPaymentMethodFactory paymentMethodFactory;
        private readonly IPaymentMethodRepository paymentMethodRepository;
        private readonly IRestaurantFactory restaurantFactory;
        private readonly IRestaurantRepository restaurantRepository;
        private readonly IDishCategoryFactory dishCategoryFactory;
        private readonly IDishCategoryRepository dishCategoryRepository;
        private readonly IDishFactory dishFactory;
        private readonly IDishRepository dishRepository;

        private readonly Dictionary<string, User> restAdminDict = new Dictionary<string, User>();
        private readonly List<Cuisine> cuisines = new List<Cuisine>();
        private readonly List<PaymentMethod> paymentMethods = new List<PaymentMethod>();

        public AddTestDataCommandHandler(
            ILogger<AddTestDataCommandHandler> logger,
            IUserFactory userFactory,
            IUserRepository userRepository,
            ICuisineFactory cuisineFactory,
            ICuisineRepository cuisineRepository,
            IPaymentMethodFactory paymentMethodFactory,
            IPaymentMethodRepository paymentMethodRepository,
            IRestaurantFactory restaurantFactory,
            IRestaurantRepository restaurantRepository,
            IDishCategoryFactory dishCategoryFactory,
            IDishCategoryRepository dishCategoryRepository,
            IDishFactory dishFactory,
            IDishRepository dishRepository
        )
        {
            this.logger = logger;
            this.userFactory = userFactory;
            this.userRepository = userRepository;
            this.cuisineFactory = cuisineFactory;
            this.cuisineRepository = cuisineRepository;
            this.paymentMethodFactory = paymentMethodFactory;
            this.paymentMethodRepository = paymentMethodRepository;
            this.restaurantFactory = restaurantFactory;
            this.restaurantRepository = restaurantRepository;
            this.dishCategoryFactory = dishCategoryFactory;
            this.dishCategoryRepository = dishCategoryRepository;
            this.dishFactory = dishFactory;
            this.dishRepository = dishRepository;
        }

        public async Task<Result<bool>> HandleAsync(AddTestDataCommand command, User currentUser,
            CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                return FailureResult<bool>.Unauthorized();

            if (currentUser.Role < Role.SystemAdmin)
                return FailureResult<bool>.Forbidden();

            var tempResult = await CreateUsersAsync(command.UserCount, cancellationToken);
            if (tempResult.IsFailure)
                return tempResult;

            tempResult = await CreateCuisinesAsync(cancellationToken);
            if (tempResult.IsFailure)
                return tempResult;

            tempResult = await CreatePaymentMethodsAsync(cancellationToken);
            if (tempResult.IsFailure)
                return tempResult;

            tempResult = await CreateRestaurantsAsync(command.RestCount, command.DishCatCount, command.DishCount,
                cancellationToken);
            if (tempResult.IsFailure)
                return tempResult;

            return SuccessResult<bool>.Create(true);
        }

        private async Task<Result<bool>> CreateUsersAsync(int count, CancellationToken cancellationToken)
        {
            logger.LogInformation("creating test users");
            
            for (var i = 0; i < count; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                var name = $"sysadmin{(i + 1):D3}";
                var tempResult = userFactory.Create(name, Role.SystemAdmin, $"{name}@gastromio.de", "Start2020!");
                if (tempResult.IsFailure)
                    return tempResult.Cast<bool>();

                var user = ((SuccessResult<User>) tempResult).Value;

                await userRepository.StoreAsync(user, cancellationToken);
                logger.LogInformation("    sysadmin user {0} created", name);
            }

            for (var i = 0; i < count; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                var name = $"restadmin{(i + 1):D3}";
                var tempResult = userFactory.Create(name, Role.SystemAdmin, $"{name}@gastromio.de", "Start2020!");
                if (tempResult.IsFailure)
                    return tempResult.Cast<bool>();

                var user = ((SuccessResult<User>) tempResult).Value;

                await userRepository.StoreAsync(user, cancellationToken);
                logger.LogInformation("    restadmin user {0} created", name);

                restAdminDict.Add(name, user);
            }

            logger.LogInformation("test users created");
            return SuccessResult<bool>.Create(true);
        }

        private async Task<Result<bool>> CreateCuisinesAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("creating test cuisines");
            
            var tempResult = cuisineFactory.Create("Chinesisch");
            if (tempResult.IsFailure)
                return tempResult.Cast<bool>();
            var cuisine = ((SuccessResult<Cuisine>) tempResult).Value;
            await cuisineRepository.StoreAsync(cuisine, cancellationToken);
            cuisines.Add(cuisine);

            tempResult = cuisineFactory.Create("Griechisch");
            if (tempResult.IsFailure)
                return tempResult.Cast<bool>();
            cuisine = ((SuccessResult<Cuisine>) tempResult).Value;
            await cuisineRepository.StoreAsync(cuisine, cancellationToken);
            cuisines.Add(cuisine);

            tempResult = cuisineFactory.Create("Italienisch");
            if (tempResult.IsFailure)
                return tempResult.Cast<bool>();
            cuisine = ((SuccessResult<Cuisine>) tempResult).Value;
            await cuisineRepository.StoreAsync(cuisine, cancellationToken);
            cuisines.Add(cuisine);

            tempResult = cuisineFactory.Create("Amerikanisch");
            if (tempResult.IsFailure)
                return tempResult.Cast<bool>();
            cuisine = ((SuccessResult<Cuisine>) tempResult).Value;
            await cuisineRepository.StoreAsync(cuisine, cancellationToken);
            cuisines.Add(cuisine);

            tempResult = cuisineFactory.Create("Mexikanisch");
            if (tempResult.IsFailure)
                return tempResult.Cast<bool>();
            cuisine = ((SuccessResult<Cuisine>) tempResult).Value;
            await cuisineRepository.StoreAsync(cuisine, cancellationToken);
            cuisines.Add(cuisine);

            tempResult = cuisineFactory.Create("Französisch");
            if (tempResult.IsFailure)
                return tempResult.Cast<bool>();
            cuisine = ((SuccessResult<Cuisine>) tempResult).Value;
            await cuisineRepository.StoreAsync(cuisine, cancellationToken);
            cuisines.Add(cuisine);

            tempResult = cuisineFactory.Create("Indisch");
            if (tempResult.IsFailure)
                return tempResult.Cast<bool>();
            cuisine = ((SuccessResult<Cuisine>) tempResult).Value;
            await cuisineRepository.StoreAsync(cuisine, cancellationToken);
            cuisines.Add(cuisine);

            tempResult = cuisineFactory.Create("Mediterran");
            if (tempResult.IsFailure)
                return tempResult.Cast<bool>();
            cuisine = ((SuccessResult<Cuisine>) tempResult).Value;
            await cuisineRepository.StoreAsync(cuisine, cancellationToken);
            cuisines.Add(cuisine);

            tempResult = cuisineFactory.Create("Japnisch");
            if (tempResult.IsFailure)
                return tempResult.Cast<bool>();
            cuisine = ((SuccessResult<Cuisine>) tempResult).Value;
            await cuisineRepository.StoreAsync(cuisine, cancellationToken);
            cuisines.Add(cuisine);

            tempResult = cuisineFactory.Create("Regional");
            if (tempResult.IsFailure)
                return tempResult.Cast<bool>();
            cuisine = ((SuccessResult<Cuisine>) tempResult).Value;
            await cuisineRepository.StoreAsync(cuisine, cancellationToken);
            cuisines.Add(cuisine);

            tempResult = cuisineFactory.Create("Deutsch (gut bürgerlich)");
            if (tempResult.IsFailure)
                return tempResult.Cast<bool>();
            cuisine = ((SuccessResult<Cuisine>) tempResult).Value;
            await cuisineRepository.StoreAsync(cuisine, cancellationToken);
            cuisines.Add(cuisine);

            logger.LogInformation("test cuisines created");
            return SuccessResult<bool>.Create(true);
        }

        private async Task<Result<bool>> CreatePaymentMethodsAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("creating test payment methods");

            var tempResult = paymentMethodFactory.Create("Bar", "Barzahlung");
            if (tempResult.IsFailure)
                return tempResult.Cast<bool>();
            var paymentMethod = ((SuccessResult<PaymentMethod>) tempResult).Value;
            await paymentMethodRepository.StoreAsync(paymentMethod, cancellationToken);
            paymentMethods.Add(paymentMethod);

            tempResult = paymentMethodFactory.Create("Visa", "Visakarte");
            if (tempResult.IsFailure)
                return tempResult.Cast<bool>();
            paymentMethod = ((SuccessResult<PaymentMethod>) tempResult).Value;
            await paymentMethodRepository.StoreAsync(paymentMethod, cancellationToken);
            paymentMethods.Add(paymentMethod);

            tempResult = paymentMethodFactory.Create("Mastercard", "Mastercard");
            if (tempResult.IsFailure)
                return tempResult.Cast<bool>();
            paymentMethod = ((SuccessResult<PaymentMethod>) tempResult).Value;
            await paymentMethodRepository.StoreAsync(paymentMethod, cancellationToken);
            paymentMethods.Add(paymentMethod);

            tempResult = paymentMethodFactory.Create("Paypal", "Paypal");
            if (tempResult.IsFailure)
                return tempResult.Cast<bool>();
            paymentMethod = ((SuccessResult<PaymentMethod>) tempResult).Value;
            await paymentMethodRepository.StoreAsync(paymentMethod, cancellationToken);
            paymentMethods.Add(paymentMethod);

            logger.LogInformation("test payment methods created");
            return SuccessResult<bool>.Create(true);
        }

        private async Task<Result<bool>> CreateRestaurantsAsync(int restCount, int dishCatCount, int dishCount, CancellationToken cancellationToken)
        {
            logger.LogInformation("creating test restaurants");

            for (var i = 0; i < restCount; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;
                var tempResult = await CreateRestaurantAsync(i, dishCatCount, dishCount, cancellationToken);
                if (tempResult.IsFailure)
                    return tempResult;
            }

            logger.LogInformation("test restaurants created");
            return SuccessResult<bool>.Create(true);
        }

        private async Task<Result<bool>> CreateRestaurantAsync(int index, int dishCatCount, int dishCount, CancellationToken cancellationToken)
        {
            var restaurantName = $"Restaurant {(index + 1):D3}";
            var restAdminName = $"restadmin{(index % restAdminDict.Count + 1):D3}";

            logger.LogInformation("    creating test restaurant {0}", restaurantName);
            
            var restaurantResult = restaurantFactory.CreateWithName(restaurantName);
            if (restaurantResult.IsFailure)
                return restaurantResult.Cast<bool>();
            var restaurant = ((SuccessResult<Restaurant>) restaurantResult).Value;

            var boolResult = restaurant.ChangeAddress(new Address("Musterstraße 1", "12345", "Musterstadt"));
            if (boolResult.IsFailure)
                return boolResult;

            boolResult = restaurant.ChangeContactDetails($"02871/1234-{(index + 1):D3}",
                $"http://www.restaurant{(index + 1):D3}.de", $"Impressum Restaurant {(index + 1):D3}",
                $"order@restaurant{(index + 1):D3}.de");
            if (boolResult.IsFailure)
                return boolResult;

            boolResult =
                restaurant.ChangeDeliveryData(5 + (decimal) index / 10, 4 + (decimal) index / 10);
            if (boolResult.IsFailure)
                return boolResult;

            restaurant.AddCuisine(cuisines[(index + 0) % cuisines.Count].Id);
            restaurant.AddCuisine(cuisines[(index + 1) % cuisines.Count].Id);

            restaurant.AddPaymentMethod(paymentMethods[(index + 0) % paymentMethods.Count].Id);
            restaurant.AddPaymentMethod(paymentMethods[(index + 1) % paymentMethods.Count].Id);

            for (var i = 0; i < 7; i++)
            {
                boolResult = restaurant.AddDeliveryTime(i, TimeSpan.FromHours(10 + (index % 4) * 0.5),
                    TimeSpan.FromHours(20 + (index % 4) * 0.5));
                if (boolResult.IsFailure)
                    return boolResult;
            }

            var restAdmin = restAdminDict[restAdminName];
            restaurant.AddAdministrator(restAdmin.Id);

            await restaurantRepository.StoreAsync(restaurant, cancellationToken);

            for (var catIndex = 0; catIndex < dishCatCount; catIndex++)
            {
                var dishCategoryName = $"Kategorie{(catIndex + 1):D2}";
                logger.LogInformation("        creating dish category {0}", dishCategoryName);

                var dishCategoryResult = dishCategoryFactory.Create(restaurant.Id, dishCategoryName);
                if (dishCategoryResult.IsFailure)
                    return dishCategoryResult.Cast<bool>();
                var dishCategory = ((SuccessResult<DishCategory>) dishCategoryResult).Value;
                await dishCategoryRepository.StoreAsync(dishCategory, cancellationToken);

                for (var dishIndex = 0; dishIndex < dishCount; dishIndex++)
                {
                    var dishName = $"Gericht{(dishIndex + 1):D2}";
                    
                    var variant = new DishVariant(Guid.NewGuid(), dishName,
                        5 + (decimal) dishIndex / 10, null);

                    var dishResult = dishFactory.Create(restaurant.Id, dishCategory.Id, dishName,
                        $"Beschreibung des Gerichts{(dishIndex + 1):D2}",
                        $"Produktinformation des Gerichts{(dishIndex + 1):D2}", new[] {variant});
                    if (dishResult.IsFailure)
                        return dishResult.Cast<bool>();
                    var dish = ((SuccessResult<Dish>) dishResult).Value;
                    await dishRepository.StoreAsync(dish, cancellationToken);
                }
                
                logger.LogInformation("        dish category {0} created", dishCategoryName);
            }

            logger.LogInformation("    test restaurant {0} created", restaurantName);
            return SuccessResult<bool>.Create(true);
        }
    }
}