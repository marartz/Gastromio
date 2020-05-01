using FoodOrderSystem.Domain.Model.PaymentMethod;
using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;
using FoodOrderSystem.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Queries.SysAdminSearchForRestaurants
{
    public class SysAdminSearchForRestaurantsQueryHandler : IQueryHandler<SysAdminSearchForRestaurantsQuery, ICollection<RestaurantViewModel>>
    {
        private readonly IRestaurantRepository restaurantRepository;
        private readonly IPaymentMethodRepository paymentMethodRepository;
        private readonly IUserRepository userRepository;

        public SysAdminSearchForRestaurantsQueryHandler(IRestaurantRepository restaurantRepository, IPaymentMethodRepository paymentMethodRepository, IUserRepository userRepository)
        {
            this.restaurantRepository = restaurantRepository;
            this.paymentMethodRepository = paymentMethodRepository;
            this.userRepository = userRepository;
        }

        public async Task<QueryResult<ICollection<RestaurantViewModel>>> HandleAsync(SysAdminSearchForRestaurantsQuery query, User currentUser, CancellationToken cancellationToken = default)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (currentUser == null)
                return new UnauthorizedQueryResult<ICollection<RestaurantViewModel>>();

            if (currentUser.Role < Role.SystemAdmin)
                return new ForbiddenQueryResult<ICollection<RestaurantViewModel>>();

            var paymentMethods = (await paymentMethodRepository.FindAllAsync(cancellationToken))
                .ToDictionary(en => en.Id.Value, PaymentMethodViewModel.FromPaymentMethod);

            var restaurants = await restaurantRepository.SearchAsync(query.SearchPhrase, cancellationToken);

            return new SuccessQueryResult<ICollection<RestaurantViewModel>>(restaurants
                .Select(en => RestaurantViewModel.FromRestaurant(en, paymentMethods, userRepository)).ToList());
        }
    }
}
