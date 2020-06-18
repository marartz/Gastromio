using System;
using System.Threading.Tasks;
using FoodOrderSystem.Domain.Commands;
using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Queries;
using FoodOrderSystem.Domain.Queries.GetRestaurantImage;
using FoodOrderSystem.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FoodOrderSystem.App.Controllers.V1
{
    [Route("api/v1")]
    [ApiController]
    public class RestaurantImageController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IQueryDispatcher queryDispatcher;
        private readonly ICommandDispatcher commandDispatcher;
        private readonly IFailureMessageService failureMessageService;

        public RestaurantImageController(ILogger<OrderController> logger, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher,
            IFailureMessageService failureMessageService)
        {
            this.logger = logger;
            this.queryDispatcher = queryDispatcher;
            this.commandDispatcher = commandDispatcher;
            this.failureMessageService = failureMessageService;
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
    }
}