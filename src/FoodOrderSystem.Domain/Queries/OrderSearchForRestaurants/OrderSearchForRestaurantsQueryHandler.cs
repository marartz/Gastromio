using FoodOrderSystem.Domain.Model.PaymentMethod;
using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;
using FoodOrderSystem.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Queries.OrderSearchForRestaurants
{
    public class OrderSearchForRestaurantsQueryHandler : IQueryHandler<OrderSearchForRestaurantsQuery, ICollection<RestaurantViewModel>>
    {
        private readonly IRestaurantRepository restaurantRepository;
        private readonly IPaymentMethodRepository paymentMethodRepository;

        public OrderSearchForRestaurantsQueryHandler(IRestaurantRepository restaurantRepository, IPaymentMethodRepository paymentMethodRepository)
        {
            this.restaurantRepository = restaurantRepository;
            this.paymentMethodRepository = paymentMethodRepository;
        }

        public async Task<QueryResult<ICollection<RestaurantViewModel>>> HandleAsync(OrderSearchForRestaurantsQuery query, User currentUser, CancellationToken cancellationToken = default)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var paymentMethods = (await paymentMethodRepository.FindAllAsync(cancellationToken))
                .ToDictionary(en => en.Id.Value, PaymentMethodViewModel.FromPaymentMethod);

            var restaurants = await restaurantRepository.SearchAsync(query.SearchPhrase, cancellationToken);

            return new SuccessQueryResult<ICollection<RestaurantViewModel>>(restaurants
                .Select(en => RestaurantViewModel.FromRestaurant(en, paymentMethods)).ToList());
        }
    }
}
