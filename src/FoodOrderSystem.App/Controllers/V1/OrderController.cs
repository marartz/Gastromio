using FoodOrderSystem.App.Models;
using FoodOrderSystem.Domain.Commands;
using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Queries;
using FoodOrderSystem.Domain.Queries.OrderSearchForRestaurants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public OrderController(ILogger<OrderController> logger, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            this.logger = logger;
            this.queryDispatcher = queryDispatcher;
            this.commandDispatcher = commandDispatcher;
        }

        [Route("restaurants")]
        [HttpGet]
        public async Task<IActionResult> SearchForRestaurantAsync(string search)
        {
            var queryResult = await queryDispatcher.PostAsync(new OrderSearchForRestaurantsQuery(search), null);
            switch (queryResult)
            {
                case UnauthorizedQueryResult _:
                    return Unauthorized();
                case ForbiddenQueryResult _:
                    return Forbid();
                case SuccessQueryResult<ICollection<Restaurant>> result:
                    var model = result.Value.Select(RestaurantModel.FromRestaurant).ToList();
                    return Ok(model);
                default:
                    throw new InvalidOperationException("internal server error");
            }
        }
    }
}
