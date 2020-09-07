using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Core.Application.DTOs;
using FoodOrderSystem.Core.Application.Ports.Persistence;
using FoodOrderSystem.Core.Common;
using FoodOrderSystem.Core.Domain.Model.Dish;
using FoodOrderSystem.Core.Domain.Model.Order;
using FoodOrderSystem.Core.Domain.Model.PaymentMethod;
using FoodOrderSystem.Core.Domain.Model.User;
using Microsoft.Extensions.Logging;

namespace FoodOrderSystem.Core.Application.Commands.Checkout
{
    public class CheckoutCommandHandler : ICommandHandler<CheckoutCommand, OrderDTO>
    {
        private readonly ILogger<CheckoutCommandHandler> logger;
        private readonly IRestaurantRepository restaurantRepository;
        private readonly IDishRepository dishRepository;
        private readonly IPaymentMethodRepository paymentMethodRepository;
        private readonly IOrderRepository orderRepository;

        public CheckoutCommandHandler(ILogger<CheckoutCommandHandler> logger,
            IRestaurantRepository restaurantRepository, IDishRepository dishRepository,
            IPaymentMethodRepository paymentMethodRepository, IOrderRepository orderRepository)
        {
            this.logger = logger;
            this.restaurantRepository = restaurantRepository;
            this.dishRepository = dishRepository;
            this.paymentMethodRepository = paymentMethodRepository;
            this.orderRepository = orderRepository;
        }

        public async Task<Result<OrderDTO>> HandleAsync(CheckoutCommand command, User currentUser,
            CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var restaurant =
                await restaurantRepository.FindByRestaurantIdAsync(command.RestaurantId, cancellationToken);
            if (restaurant == null)
                return FailureResult<OrderDTO>.Create(FailureResultCode.OrderIsInvalid);
            if (!restaurant.IsActive)
                return FailureResult<OrderDTO>.Create(FailureResultCode.OrderIsInvalid);
            var restaurantInfo =
                $"{restaurant.Name} ({restaurant.Address?.Street}, {restaurant.Address?.ZipCode} {restaurant.Address?.City})";

            switch (command.OrderType)
            {
                case OrderType.Pickup:
                    if (restaurant.PickupInfo == null || !restaurant.PickupInfo.Enabled)
                        return FailureResult<OrderDTO>.Create(FailureResultCode.OrderIsInvalid);
                    break;
                case OrderType.Delivery:
                    if (restaurant.DeliveryInfo == null || !restaurant.DeliveryInfo.Enabled)
                        return FailureResult<OrderDTO>.Create(FailureResultCode.OrderIsInvalid);
                    break;
                case OrderType.Reservation:
                    if (restaurant.ReservationInfo == null || !restaurant.ReservationInfo.Enabled)
                        return FailureResult<OrderDTO>.Create(FailureResultCode.OrderIsInvalid);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            decimal totalPrice = 0;

            var dishDict = new Dictionary<Guid, Dish>();
            var variantDict = new Dictionary<Guid, DishVariant>();
            foreach (var cartDish in command.CartDishes)
            {
                var dish = await dishRepository.FindByDishIdAsync(cartDish.DishId, cancellationToken);
                if (dish == null)
                    return FailureResult<OrderDTO>.Create(FailureResultCode.OrderIsInvalid);
                dishDict.Add(cartDish.ItemId, dish);
                if (dish.RestaurantId != command.RestaurantId)
                    return FailureResult<OrderDTO>.Create(FailureResultCode.OrderIsInvalid);
                var variant = dish.Variants.FirstOrDefault(en => en.VariantId == cartDish.VariantId);
                if (variant == null)
                    return FailureResult<OrderDTO>.Create(FailureResultCode.OrderIsInvalid);
                variantDict.Add(cartDish.ItemId, variant);
                if (!string.IsNullOrWhiteSpace(cartDish.Remarks))
                {
                    var remarks = cartDish.Remarks.Trim();
                    if (remarks.Length > 1000)
                        return FailureResult<OrderDTO>.Create(FailureResultCode.OrderIsInvalid);
                }

                if (cartDish.Count <= 0)
                    return FailureResult<OrderDTO>.Create(FailureResultCode.OrderIsInvalid);
                if (cartDish.Count > 100)
                    return FailureResult<OrderDTO>.Create(FailureResultCode.OrderIsInvalid);

                totalPrice += cartDish.Count * variant.Price;
            }

            if (command.OrderType == OrderType.Pickup && restaurant.PickupInfo.MinimumOrderValue.HasValue &&
                totalPrice < restaurant.PickupInfo.MinimumOrderValue.Value)
            {
                return FailureResult<OrderDTO>.Create(FailureResultCode.OrderIsInvalid);
            }
            
            if (command.OrderType == OrderType.Pickup && restaurant.PickupInfo.MaximumOrderValue.HasValue &&
                totalPrice > restaurant.PickupInfo.MaximumOrderValue.Value)
            {
                return FailureResult<OrderDTO>.Create(FailureResultCode.OrderIsInvalid);
            }
            
            if (command.OrderType == OrderType.Delivery && restaurant.DeliveryInfo.MinimumOrderValue.HasValue &&
                totalPrice < restaurant.DeliveryInfo.MinimumOrderValue.Value)
            {
                return FailureResult<OrderDTO>.Create(FailureResultCode.OrderIsInvalid);
            }
            
            if (command.OrderType == OrderType.Delivery && restaurant.DeliveryInfo.MaximumOrderValue.HasValue &&
                totalPrice > restaurant.DeliveryInfo.MaximumOrderValue.Value)
            {
                return FailureResult<OrderDTO>.Create(FailureResultCode.OrderIsInvalid);
            }
            
            switch (command.OrderType)
            {
                case OrderType.Pickup:
                    if (restaurant.PickupInfo == null || !restaurant.PickupInfo.Enabled)
                        return FailureResult<OrderDTO>.Create(FailureResultCode.OrderIsInvalid);
                    break;
                case OrderType.Delivery:
                    if (restaurant.DeliveryInfo == null || !restaurant.DeliveryInfo.Enabled)
                        return FailureResult<OrderDTO>.Create(FailureResultCode.OrderIsInvalid);
                    break;
                case OrderType.Reservation:
                    if (restaurant.ReservationInfo == null || !restaurant.ReservationInfo.Enabled)
                        return FailureResult<OrderDTO>.Create(FailureResultCode.OrderIsInvalid);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            var paymentMethod =
                await paymentMethodRepository.FindByPaymentMethodIdAsync(command.PaymentMethodId, cancellationToken);
            if (paymentMethod == null)
                return FailureResult<OrderDTO>.Create(FailureResultCode.OrderIsInvalid);

            decimal costs = 0;
            if (command.OrderType == OrderType.Delivery && restaurant.DeliveryInfo.Costs.HasValue)
            {
                costs = restaurant.DeliveryInfo.Costs.Value;
            }

            totalPrice += costs;

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
                    restaurant.Name,
                    restaurantInfo,
                    restaurant.ContactInfo.Phone,
                    restaurant.ContactInfo.EmailAddress,
                    restaurant.NeedsSupport,
                    command.CartDishes?.Select(en => new OrderedDishInfo(
                        en.ItemId,
                        en.DishId,
                        $"{dishDict[en.ItemId].Name} ({dishDict[en.ItemId].Description})",
                        en.VariantId,
                        dishDict[en.ItemId].Variants.Count > 1 ? variantDict[en.ItemId].Name : "",
                        variantDict[en.ItemId].Price,
                        en.Count,
                        en.Remarks
                    )).ToList()
                ),
                command.Comments,
                command.PaymentMethodId,
                paymentMethod.Name,
                paymentMethod.Description,
                costs,
                totalPrice
            );

            var result = order.Validate();
            if (result.IsFailure)
                return result.Cast<OrderDTO>();

            await orderRepository.StoreAsync(order, cancellationToken);
            
            var viewModel = new OrderDTO(order, paymentMethod);
            return SuccessResult<OrderDTO>.Create(viewModel);
        }
    }
}