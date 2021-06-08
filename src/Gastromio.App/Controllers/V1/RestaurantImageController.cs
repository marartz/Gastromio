using System;
using System.Threading.Tasks;
using Gastromio.Core.Application.Queries;
using Gastromio.Core.Application.Queries.GetRestaurantImage;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.RestaurantImages;
using Gastromio.Core.Domain.Model.Restaurants;
using Microsoft.AspNetCore.Mvc;

namespace Gastromio.App.Controllers.V1
{
    [Route("api/v1")]
    [ApiController]
    public class RestaurantImageController : ControllerBase
    {
        private readonly IQueryDispatcher queryDispatcher;

        public RestaurantImageController(IQueryDispatcher queryDispatcher)
        {
            this.queryDispatcher = queryDispatcher;
        }

        [Route("restaurants/{restaurantId}/images/{type}")]
        [HttpGet]
        public async Task<IActionResult> GetRestaurantImageAsync(Guid restaurantId, string type)
        {
            try
            {
                var query = new GetRestaurantImageQuery(new RestaurantId(restaurantId), type);
                var restaurantImage = await queryDispatcher.PostAsync<GetRestaurantImageQuery, RestaurantImage>(query, null);

                var data = restaurantImage.Data;
                var updatedOn = restaurantImage.UpdatedOn;

                var fileContentResult = new FileContentResult(data, "image/jpeg")
                {
                    LastModified = updatedOn
                };

                return fileContentResult;
            }
            catch (DomainException<RestaurantImageNotValidFailure>)
            {
                return NotFound();
            }
        }
    }
}
