using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Domain.Model;
using FoodOrderSystem.Domain.Model.Dish;
using FoodOrderSystem.Domain.Model.Order;
using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;
using Microsoft.Extensions.Logging;

namespace FoodOrderSystem.Domain.Commands.Checkout
{
    public class CheckoutCommandHandler : ICommandHandler<CheckoutCommand, OrderId>
    {
        private readonly ILogger<CheckoutCommandHandler> logger;
        private readonly IRestaurantRepository restaurantRepository;
        private readonly IDishRepository dishRepository;
        private readonly IOrderRepository orderRepository;

        public CheckoutCommandHandler(ILogger<CheckoutCommandHandler> logger,
            IRestaurantRepository restaurantRepository, IDishRepository dishRepository,
            IOrderRepository orderRepository)
        {
            this.logger = logger;
            this.restaurantRepository = restaurantRepository;
            this.dishRepository = dishRepository;
            this.orderRepository = orderRepository;
        }

        public async Task<Result<OrderId>> HandleAsync(CheckoutCommand command, User currentUser,
            CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));
            
            var restaurant =
                await restaurantRepository.FindByRestaurantIdAsync(command.RestaurantId, cancellationToken);
            if (restaurant == null)
                return FailureResult<OrderId>.Create(FailureResultCode.OrderIsInvalid);
            var restaurantInfo =
                $"{restaurant.Name} ({restaurant.Address?.Street}, {restaurant.Address?.ZipCode} {restaurant.Address?.City})";

            var dishDict = new Dictionary<Guid, Dish>();
            var variantDict = new Dictionary<Guid, DishVariant>();
            foreach (var orderedDish in command.OrderedDishes)
            {
                var dish = await dishRepository.FindByDishIdAsync(orderedDish.DishId, cancellationToken);
                if (dish == null)
                    return FailureResult<OrderId>.Create(FailureResultCode.OrderIsInvalid);
                dishDict.Add(orderedDish.ItemId, dish);
                if (dish.RestaurantId != command.RestaurantId)
                    return FailureResult<OrderId>.Create(FailureResultCode.OrderIsInvalid);
                var variant = dish.Variants.FirstOrDefault(en => en.VariantId == orderedDish.VariantId);
                if (variant == null)
                    return FailureResult<OrderId>.Create(FailureResultCode.OrderIsInvalid);
                variantDict.Add(orderedDish.ItemId, variant);
                if (!string.IsNullOrWhiteSpace(orderedDish.Remarks))
                {
                    var remarks = orderedDish.Remarks.Trim();
                    if (remarks.Length > 1000)
                        return FailureResult<OrderId>.Create(FailureResultCode.OrderIsInvalid);
                }
                if (orderedDish.Count <= 0)
                    return FailureResult<OrderId>.Create(FailureResultCode.OrderIsInvalid);
                if (orderedDish.Count > 100)
                    return FailureResult<OrderId>.Create(FailureResultCode.OrderIsInvalid);
            }
            
            var order = new Order(
                new OrderId(Guid.NewGuid()),
                new CustomerInfo(
                    command.GivenName,
                    command.LastName,
                    command.Street,
                    command.AddAddressInfo,
                    command.ZipCode,
                    command.City,
                    command.Phone,
                    command.Email
                ),
                new CartInfo(
                    command.OrderType,
                    command.RestaurantId,
                    restaurantInfo,
                    command.OrderedDishes?.Select(en => new Model.Order.OrderedDishInfo(
                        en.ItemId,
                        en.DishId,
                        $"{dishDict[en.ItemId].Name} ({dishDict[en.ItemId].Description})",
                        en.VariantId,
                        variantDict[en.ItemId].Name,
                        variantDict[en.ItemId].Price,
                        en.Count,
                        en.Remarks
                    )).ToList()
                ),
                command.Comments
            );

            var result = order.Validate();
            if (result.IsFailure)
                return result.Cast<OrderId>();


            await orderRepository.StoreAsync(order, cancellationToken);
            
            return SuccessResult<OrderId>.Create(order.Id);
        }
    }
}