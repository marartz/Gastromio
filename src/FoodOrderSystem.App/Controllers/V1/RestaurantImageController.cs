using System;
using System.Threading.Tasks;
using FoodOrderSystem.Core.Application.Queries;
using FoodOrderSystem.Core.Application.Queries.GetRestaurantImage;
using FoodOrderSystem.Core.Domain.Model.Restaurant;
using FoodOrderSystem.Core.Domain.Model.RestaurantImage;
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

        public RestaurantImageController(ILogger<OrderController> logger, IQueryDispatcher queryDispatcher)
        {
            this.logger = logger;
            this.queryDispatcher = queryDispatcher;
        }
        
        [Route("restaurants/{restaurantId}/images/{type}")]
        [HttpGet]
        public async Task<IActionResult> GetRestaurantImageAsync(Guid restaurantId, string type)
        {
            var query = new GetRestaurantImageQuery(new RestaurantId(restaurantId), type);
            var queryResult = await queryDispatcher.PostAsync<GetRestaurantImageQuery, RestaurantImage>(query, null);
            if (queryResult.IsFailure)
                return NotFound();

            var data = queryResult.Value.Data;
            var updatedOn = queryResult.Value.UpdatedOn;

            var fileContentResult = new FileContentResult(data, "image/jpeg")
            {
                LastModified = new DateTimeOffset(updatedOn),
            };

            return fileContentResult;
        }
    }
}