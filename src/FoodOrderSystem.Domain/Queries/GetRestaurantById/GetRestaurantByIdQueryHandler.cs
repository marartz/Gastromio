using FoodOrderSystem.Domain.Model;
using FoodOrderSystem.Domain.Model.Cuisine;
using FoodOrderSystem.Domain.Model.PaymentMethod;
using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;
using FoodOrderSystem.Domain.ViewModels;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Domain.Model.RestaurantImage;

namespace FoodOrderSystem.Domain.Queries.GetRestaurantById
{
    public class GetRestaurantByIdQueryHandler : IQueryHandler<GetRestaurantByIdQuery, RestaurantViewModel>
    {
        private readonly IRestaurantRepository restaurantRepository;
        private readonly IRestaurantImageRepository restaurantImageRepository;
        private readonly ICuisineRepository cuisineRepository;
        private readonly IPaymentMethodRepository paymentMethodRepository;
        private readonly IUserRepository userRepository;

        public GetRestaurantByIdQueryHandler(
            IRestaurantRepository restaurantRepository,
            IRestaurantImageRepository restaurantImageRepository,
            ICuisineRepository cuisineRepository,
            IPaymentMethodRepository paymentMethodRepository,
            IUserRepository userRepository
        )
        {
            this.restaurantRepository = restaurantRepository;
            this.restaurantImageRepository = restaurantImageRepository;
            this.cuisineRepository = cuisineRepository;
            this.paymentMethodRepository = paymentMethodRepository;
            this.userRepository = userRepository;
        }

        public async Task<Result<RestaurantViewModel>> HandleAsync(GetRestaurantByIdQuery query, User currentUser, CancellationToken cancellationToken = default)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var cuisines = (await cuisineRepository.FindAllAsync(cancellationToken))
                .ToDictionary(en => en.Id.Value, CuisineViewModel.FromCuisine);

            var paymentMethods = (await paymentMethodRepository.FindAllAsync(cancellationToken))
                .ToDictionary(en => en.Id.Value, PaymentMethodViewModel.FromPaymentMethod);

            var restaurant = await restaurantRepository.FindByRestaurantIdAsync(query.RestaurantId);
            if (restaurant == null)
                return FailureResult<RestaurantViewModel>.Create(FailureResultCode.RestaurantDoesNotExist);

            return SuccessResult<RestaurantViewModel>.Create(RestaurantViewModel.FromRestaurant(restaurant, cuisines,
                paymentMethods, userRepository, restaurantImageRepository));
        }
    }
}
