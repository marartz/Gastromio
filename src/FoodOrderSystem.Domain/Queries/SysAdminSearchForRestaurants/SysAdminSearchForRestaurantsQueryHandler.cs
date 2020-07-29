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

namespace FoodOrderSystem.Domain.Queries.SysAdminSearchForRestaurants
{
    public class SysAdminSearchForRestaurantsQueryHandler : IQueryHandler<SysAdminSearchForRestaurantsQuery, PagingViewModel<RestaurantViewModel>>
    {
        private readonly IRestaurantRepository restaurantRepository;
        private readonly IRestaurantImageRepository restaurantImageRepository;
        private readonly ICuisineRepository cuisineRepository;
        private readonly IPaymentMethodRepository paymentMethodRepository;
        private readonly IUserRepository userRepository;

        public SysAdminSearchForRestaurantsQueryHandler(
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

        public async Task<Result<PagingViewModel<RestaurantViewModel>>> HandleAsync(SysAdminSearchForRestaurantsQuery query, User currentUser, CancellationToken cancellationToken = default)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (currentUser == null)
                return FailureResult<PagingViewModel<RestaurantViewModel>>.Unauthorized();

            if (currentUser.Role < Role.SystemAdmin)
                return FailureResult<PagingViewModel<RestaurantViewModel>>.Forbidden();

            var cuisines = (await cuisineRepository.FindAllAsync(cancellationToken))
                .ToDictionary(en => en.Id.Value, CuisineViewModel.FromCuisine);

            var paymentMethods = (await paymentMethodRepository.FindAllAsync(cancellationToken))
                .ToDictionary(en => en.Id.Value, PaymentMethodViewModel.FromPaymentMethod);

            var (total, items) = await restaurantRepository.SearchPagedAsync(query.SearchPhrase, null, null, null, null,
                query.Skip, query.Take, cancellationToken);

            var pagingViewModel = new PagingViewModel<RestaurantViewModel>((int) total, query.Skip, query.Take,
                items.Select(en =>
                    RestaurantViewModel.FromRestaurant(en, cuisines, paymentMethods, userRepository,
                        restaurantImageRepository)).ToList());

            return SuccessResult<PagingViewModel<RestaurantViewModel>>.Create(pagingViewModel);
        }
    }
}
