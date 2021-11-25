﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gastromio.App.Models;
using Gastromio.Core.Application.Commands;
using Gastromio.Core.Application.Commands.Checkout;
using Gastromio.Core.Application.DTOs;
using Gastromio.Core.Application.Queries;
using Gastromio.Core.Application.Queries.GetAllCuisines;
using Gastromio.Core.Application.Queries.GetRestaurantById;
using Gastromio.Core.Application.Queries.OrderSearchForRestaurants;
using Gastromio.Core.Domain.Model.Cuisines;
using Gastromio.Core.Domain.Model.Orders;
using Gastromio.Core.Domain.Model.PaymentMethods;
using Gastromio.Core.Domain.Model.Restaurants;
using Microsoft.AspNetCore.Mvc;

namespace Gastromio.App.Controllers.V1
{
    [Route("api/v1/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IQueryDispatcher queryDispatcher;
        private readonly ICommandDispatcher commandDispatcher;

        public OrderController(
            IQueryDispatcher queryDispatcher,
            ICommandDispatcher commandDispatcher
        )
        {
            this.queryDispatcher = queryDispatcher;
            this.commandDispatcher = commandDispatcher;
        }

        [Route("cuisines")]
        [HttpGet]
        public async Task<IActionResult> GetAllCuisines()
        {
            var cuisineDtos = await queryDispatcher.PostAsync<GetAllCuisinesQuery, ICollection<CuisineDTO>>(
                new GetAllCuisinesQuery(),
                null
            );

            return Ok(cuisineDtos);
        }

        [Route("restaurants")]
        [HttpGet]
        public async Task<IActionResult> SearchForRestaurantAsync(string search, string orderType, Guid cuisineId, string openingHour)
        {
            var orderTypeEnum = string.IsNullOrWhiteSpace(orderType) ? (OrderType?)null : ConvertOrderType(orderType);

            var tempCuisineId = cuisineId != Guid.Empty ? new CuisineId(cuisineId) : null;

            DateTimeOffset? openingHourDateTime = null;
            if (!string.IsNullOrWhiteSpace(openingHour))
            {
                if (DateTimeOffset.TryParse(openingHour, out var tempOpeningHour))
                {
                    openingHourDateTime = tempOpeningHour;
                }
            }

            var restaurantDtos =
                await queryDispatcher.PostAsync<OrderSearchForRestaurantsQuery, ICollection<RestaurantDTO>>(
                    new OrderSearchForRestaurantsQuery(search, orderTypeEnum, tempCuisineId, openingHourDateTime),
                    null
                );

            return Ok(restaurantDtos);
        }

        [Route("restaurants/{restaurant}")]
        [HttpGet]
        public async Task<IActionResult> GetRestaurantAsync(string restaurant)
        {
            var restaurantDto = await queryDispatcher.PostAsync<GetRestaurantByIdQuery, RestaurantDTO>(
                new GetRestaurantByIdQuery(restaurant, true),
                null
            );

            return Ok(restaurantDto);
        }

        [Route("checkout")]
        [HttpPost]
        public async Task<IActionResult> PostOrder([FromBody] CheckoutModel checkoutModel)
        {
            if (checkoutModel.ServiceTime.HasValue)
            {
                checkoutModel.ServiceTime = checkoutModel.ServiceTime.Value.ToLocalTime();
            }

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
                checkoutModel.RestaurantId,
                checkoutModel.CartDishes?.Select(en => new CartDishInfoDTO(
                    en.ItemId,
                    new DishId(en.DishId),
                    new DishVariantId(en.VariantId),
                    en.Count,
                    en.Remarks
                )).ToList(),
                checkoutModel.Comments,
                new PaymentMethodId(checkoutModel.PaymentMethodId),
                checkoutModel.ServiceTime
            );

            var orderDto = await commandDispatcher.PostAsync<CheckoutCommand, OrderDTO>(
                command,
                null
            );

            return Ok(orderDto);
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
