using FoodOrderSystem.App.Helper;
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
using System.Threading.Tasks;

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
        public async Task<IActionResult> SearchForRestaurantAsync(string search)
        {
            var queryResult = await queryDispatcher.PostAsync<OrderSearchForRestaurantsQuery, ICollection<RestaurantViewModel>>(new OrderSearchForRestaurantsQuery(search), null);
            return ResultHelper.HandleResult(queryResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}")]
        [HttpGet]
        public async Task<IActionResult> GetRestaurantAsync(Guid restaurantId)
        {
            var queryResult = await queryDispatcher.PostAsync<GetRestaurantByIdQuery, RestaurantViewModel>(new GetRestaurantByIdQuery(new RestaurantId(restaurantId)), null);
            return ResultHelper.HandleResult(queryResult, failureMessageService);
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

    }
}
