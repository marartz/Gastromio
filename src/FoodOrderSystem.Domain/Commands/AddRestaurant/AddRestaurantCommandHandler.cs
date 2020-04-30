using FoodOrderSystem.Domain.Model.PaymentMethod;
using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;
using FoodOrderSystem.Domain.ViewModels;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Commands.AddRestaurant
{
    public class AddRestaurantCommandHandler : ICommandHandler<AddRestaurantCommand, RestaurantViewModel>
    {
        private readonly IRestaurantFactory restaurantFactory;
        private readonly IRestaurantRepository restaurantRepository;
        private readonly IPaymentMethodRepository paymentMethodRepository;

        public AddRestaurantCommandHandler(IRestaurantFactory restaurantFactory, IRestaurantRepository restaurantRepository, IPaymentMethodRepository paymentMethodRepository)
        {
            this.restaurantFactory = restaurantFactory;
            this.restaurantRepository = restaurantRepository;
            this.paymentMethodRepository = paymentMethodRepository;
        }

        public async Task<CommandResult<RestaurantViewModel>> HandleAsync(AddRestaurantCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                return new UnauthorizedCommandResult<RestaurantViewModel>();

            if (currentUser.Role < Role.SystemAdmin)
                return new ForbiddenCommandResult<RestaurantViewModel>();

            var paymentMethods = (await paymentMethodRepository.FindAllAsync(cancellationToken))
                .ToDictionary(en => en.Id.Value, PaymentMethodViewModel.FromPaymentMethod);

            var restaurant = restaurantFactory.CreateWithName(command.Name);
            await restaurantRepository.StoreAsync(restaurant, cancellationToken);

            return new SuccessCommandResult<RestaurantViewModel>(RestaurantViewModel.FromRestaurant(restaurant, paymentMethods));
        }
    }
}
