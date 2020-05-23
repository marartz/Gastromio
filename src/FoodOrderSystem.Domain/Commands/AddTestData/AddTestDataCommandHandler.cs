using FoodOrderSystem.Domain.Model;
using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Domain.Model.Cuisine;
using FoodOrderSystem.Domain.Model.Dish;
using FoodOrderSystem.Domain.Model.DishCategory;
using FoodOrderSystem.Domain.Model.PaymentMethod;

namespace FoodOrderSystem.Domain.Commands.AddTestData
{
    public class AddTestDataCommandHandler : ICommandHandler<AddTestDataCommand, bool>
    {
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
        
        private Dictionary<string, User> restAdminDict;

        public AddTestDataCommandHandler(
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

        public async Task<Result<bool>> HandleAsync(AddTestDataCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                return FailureResult<bool>.Unauthorized();

            if (currentUser.Role < Role.SystemAdmin)
                return FailureResult<bool>.Forbidden();

            var tempResult = await CreateUsersAsync(cancellationToken);
            if (tempResult.IsFailure)
                return tempResult;

            tempResult = await CreateCuisinesAsync(cancellationToken);
            if (tempResult.IsFailure)
                return tempResult;

            tempResult = await CreatePaymentMethodsAsync(cancellationToken);
            if (tempResult.IsFailure)
                return tempResult;

            tempResult = await CreateRestaurantsAsync(cancellationToken);
            if (tempResult.IsFailure)
                return tempResult;

            return SuccessResult<bool>.Create(true);
        }

        private async Task<Result<bool>> CreateUsersAsync(CancellationToken cancellationToken)
        {
            for (var i = 0; i < 100; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                var name = $"sysadmin{(i + 1):D3}";
                var tempResult = userFactory.Create(name, Role.SystemAdmin, $"{name}@gastromio.de", "Start2020!");
                if (tempResult.IsFailure)
                    return tempResult.Cast<bool>();

                var user = ((SuccessResult<User>) tempResult).Value;

                await userRepository.StoreAsync(user, cancellationToken);
            }

            restAdminDict = new Dictionary<string, User>();

            for (var i = 0; i < 100; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                var name = $"restadmin{(i + 1):D3}";
                var tempResult = userFactory.Create(name, Role.SystemAdmin, $"{name}@gastromio.de", "Start2020!");
                if (tempResult.IsFailure)
                    return tempResult.Cast<bool>();

                var user = ((SuccessResult<User>) tempResult).Value;

                await userRepository.StoreAsync(user, cancellationToken);

                restAdminDict.Add(name, user);
            }

            return SuccessResult<bool>.Create(true);
        }

        private async Task<Result<bool>> CreateCuisinesAsync(CancellationToken cancellationToken)
        {
            var tempResult = cuisineFactory.Create("Chinesisch");
            if (tempResult.IsFailure)
                return tempResult.Cast<bool>();
            var cuisine = ((SuccessResult<Cuisine>) tempResult).Value;
            await cuisineRepository.StoreAsync(cuisine, cancellationToken);

            tempResult = cuisineFactory.Create("Griechisch");
            if (tempResult.IsFailure)
                return tempResult.Cast<bool>();
            cuisine = ((SuccessResult<Cuisine>) tempResult).Value;
            await cuisineRepository.StoreAsync(cuisine, cancellationToken);

            tempResult = cuisineFactory.Create("Italienisch");
            if (tempResult.IsFailure)
                return tempResult.Cast<bool>();
            cuisine = ((SuccessResult<Cuisine>) tempResult).Value;
            await cuisineRepository.StoreAsync(cuisine, cancellationToken);

            tempResult = cuisineFactory.Create("Amerikanisch");
            if (tempResult.IsFailure)
                return tempResult.Cast<bool>();
            cuisine = ((SuccessResult<Cuisine>) tempResult).Value;
            await cuisineRepository.StoreAsync(cuisine, cancellationToken);

            tempResult = cuisineFactory.Create("Mexikanisch");
            if (tempResult.IsFailure)
                return tempResult.Cast<bool>();
            cuisine = ((SuccessResult<Cuisine>) tempResult).Value;
            await cuisineRepository.StoreAsync(cuisine, cancellationToken);

            tempResult = cuisineFactory.Create("Französisch");
            if (tempResult.IsFailure)
                return tempResult.Cast<bool>();
            cuisine = ((SuccessResult<Cuisine>) tempResult).Value;
            await cuisineRepository.StoreAsync(cuisine, cancellationToken);

            tempResult = cuisineFactory.Create("Indisch");
            if (tempResult.IsFailure)
                return tempResult.Cast<bool>();
            cuisine = ((SuccessResult<Cuisine>) tempResult).Value;
            await cuisineRepository.StoreAsync(cuisine, cancellationToken);

            tempResult = cuisineFactory.Create("Mediterran");
            if (tempResult.IsFailure)
                return tempResult.Cast<bool>();
            cuisine = ((SuccessResult<Cuisine>) tempResult).Value;
            await cuisineRepository.StoreAsync(cuisine, cancellationToken);

            tempResult = cuisineFactory.Create("Japnisch");
            if (tempResult.IsFailure)
                return tempResult.Cast<bool>();
            cuisine = ((SuccessResult<Cuisine>) tempResult).Value;
            await cuisineRepository.StoreAsync(cuisine, cancellationToken);

            tempResult = cuisineFactory.Create("Regional");
            if (tempResult.IsFailure)
                return tempResult.Cast<bool>();
            cuisine = ((SuccessResult<Cuisine>) tempResult).Value;
            await cuisineRepository.StoreAsync(cuisine, cancellationToken);

            tempResult = cuisineFactory.Create("Deutsch (gut bürgerlich)");
            if (tempResult.IsFailure)
                return tempResult.Cast<bool>();
            cuisine = ((SuccessResult<Cuisine>) tempResult).Value;
            await cuisineRepository.StoreAsync(cuisine, cancellationToken);

            return SuccessResult<bool>.Create(true);
        }

        private async Task<Result<bool>> CreatePaymentMethodsAsync(CancellationToken cancellationToken)
        {
            var tempResult = paymentMethodFactory.Create("Bar", "Barzahlung");
            if (tempResult.IsFailure)
                return tempResult.Cast<bool>();
            var paymentMethod = ((SuccessResult<PaymentMethod>) tempResult).Value;
            await paymentMethodRepository.StoreAsync(paymentMethod, cancellationToken);

            tempResult = paymentMethodFactory.Create("Visa", "Visakarte");
            if (tempResult.IsFailure)
                return tempResult.Cast<bool>();
            paymentMethod = ((SuccessResult<PaymentMethod>) tempResult).Value;
            await paymentMethodRepository.StoreAsync(paymentMethod, cancellationToken);

            tempResult = paymentMethodFactory.Create("Mastercard", "Mastercard");
            if (tempResult.IsFailure)
                return tempResult.Cast<bool>();
            paymentMethod = ((SuccessResult<PaymentMethod>) tempResult).Value;
            await paymentMethodRepository.StoreAsync(paymentMethod, cancellationToken);

            tempResult = paymentMethodFactory.Create("Paypal", "Paypal");
            if (tempResult.IsFailure)
                return tempResult.Cast<bool>();
            paymentMethod = ((SuccessResult<PaymentMethod>) tempResult).Value;
            await paymentMethodRepository.StoreAsync(paymentMethod, cancellationToken);

            return SuccessResult<bool>.Create(true);
        }
        
        private async Task<Result<bool>> CreateRestaurantsAsync(CancellationToken cancellationToken)
        {
            for (var i = 0; i < restAdminDict.Count; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;
                var tempResult = await CreateRestaurantAsync(i, cancellationToken);
                if (tempResult.IsFailure)
                    return tempResult;
            }

            return SuccessResult<bool>.Create(true);
        }

        private async Task<Result<bool>> CreateRestaurantAsync(int index, CancellationToken cancellationToken)
        {
            var restaurantName = $"Restaurant {(index + 1):D3}";
            var restAdminName = $"restadmin{(index + 1):D3}";

            var tempResult = restaurantFactory.CreateWithName(restaurantName);
            if (tempResult.IsFailure)
                return tempResult.Cast<bool>();
            var restaurant = ((SuccessResult<Restaurant>) tempResult).Value;
            await restaurantRepository.StoreAsync(restaurant, cancellationToken);

            return SuccessResult<bool>.Create(true);
        }
    }
}
