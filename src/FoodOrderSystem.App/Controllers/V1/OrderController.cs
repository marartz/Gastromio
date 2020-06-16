﻿using FoodOrderSystem.App.Helper;
using FoodOrderSystem.Domain.Commands;
using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Queries;
using FoodOrderSystem.Domain.Queries.GetDishesOfRestaurant;
using FoodOrderSystem.Domain.Queries.GetRestaurantById;
using FoodOrderSystem.Domain.Queries.OrderSearchForRestaurants;
using FoodOrderSystem.Domain.Services;
using FoodOrderSystem.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodOrderSystem.App.Models;
using FoodOrderSystem.Domain.Commands.Checkout;
using FoodOrderSystem.Domain.Model.Dish;
using FoodOrderSystem.Domain.Model.Order;
using FoodOrderSystem.Domain.Model.PaymentMethod;
using FoodOrderSystem.Domain.Queries.GetRestaurantImage;

namespace FoodOrderSystem.App.Controllers.V1
{
    [Route("api/v1/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IQueryDispatcher queryDispatcher;
        private readonly ICommandDispatcher commandDispatcher;
        private readonly IFailureMessageService failureMessageService;

        public OrderController(ILogger<OrderController> logger, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher,
            IFailureMessageService failureMessageService)
        {
            this.logger = logger;
            this.queryDispatcher = queryDispatcher;
            this.commandDispatcher = commandDispatcher;
            this.failureMessageService = failureMessageService;
        }

        [Route("restaurants")]
        [HttpGet]
        public async Task<IActionResult> SearchForRestaurantAsync(string search, string orderType)
        {
            OrderType? orderTypeEnum = string.IsNullOrWhiteSpace(orderType) ? (OrderType?)null : ConvertOrderType(orderType); 
            
            var queryResult =
                await queryDispatcher.PostAsync<OrderSearchForRestaurantsQuery, ICollection<RestaurantViewModel>>(
                    new OrderSearchForRestaurantsQuery(search, orderTypeEnum), null);
            return ResultHelper.HandleResult(queryResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}")]
        [HttpGet]
        public async Task<IActionResult> GetRestaurantAsync(Guid restaurantId)
        {
            var queryResult = await queryDispatcher.PostAsync<GetRestaurantByIdQuery, RestaurantViewModel>(new GetRestaurantByIdQuery(new RestaurantId(restaurantId)), null);
            return ResultHelper.HandleResult(queryResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/images/{type}")]
        [HttpGet]
        public async Task<IActionResult> GetRestaurantImageAsync(Guid restaurantId, string type)
        {
            var query = new GetRestaurantImageQuery(new RestaurantId(restaurantId), type);
            var queryResult = await queryDispatcher.PostAsync<GetRestaurantImageQuery, byte[]>(query, null);
            if (queryResult.IsFailure)
                return NotFound();
            return File(queryResult.Value, "image/jpeg");
        }

        [Route("restaurants/{restaurantId}/dishes")]
        [HttpGet]
        public async Task<IActionResult> GetDishesOfRestaurantAsync(Guid restaurantId)
        {
            var queryResult = await queryDispatcher.PostAsync<GetDishesOfRestaurantQuery, ICollection<DishCategoryViewModel>>(
                new GetDishesOfRestaurantQuery(new RestaurantId(restaurantId)),
                null
            );
            return ResultHelper.HandleResult(queryResult, failureMessageService);
        }

        [Route("checkout")]
        [HttpPost]
        public async Task<IActionResult> PostOrder([FromBody] CheckoutModel checkoutModel)
        {
            var command = new CheckoutCommand(
                checkoutModel.GivenName,
                checkoutModel.LastName,
                checkoutModel.Street,
                checkoutModel.AddAddressInfo,
                checkoutModel.ZipCode,
                checkoutModel.City,
                checkoutModel.Phone,
                checkoutModel.Email,
                ConvertOrderType(checkoutModel.OrderType),
                new RestaurantId(checkoutModel.RestaurantId),
                checkoutModel.CartDishes?.Select(en => new Domain.Commands.Checkout.CartDishInfo(
                    en.ItemId,
                    new DishId(en.DishId),
                    en.VariantId,
                    en.Count,
                    en.Remarks
                )).ToList(),
                checkoutModel.Comments,
                new PaymentMethodId(checkoutModel.PaymentMethodId)
            );
           
            var commandResult = await commandDispatcher.PostAsync<CheckoutCommand, OrderViewModel>(command, null);
            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        private static OrderType ConvertOrderType(string orderType)
        {
            switch (orderType)
            {
                case "pickup":
                    return OrderType.Pickup;
                case "delivery":
                    return OrderType.Delivery;
                case "reservation":
                    return OrderType.Reservation;
                default:
                    throw new InvalidOperationException($"unknown order type: {orderType}");
            }
        }
    }
}
