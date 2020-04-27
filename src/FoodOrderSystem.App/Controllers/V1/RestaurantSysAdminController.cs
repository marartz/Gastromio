using FoodOrderSystem.App.Models;
using FoodOrderSystem.Domain.Commands;
using FoodOrderSystem.Domain.Commands.AddRestaurant;
using FoodOrderSystem.Domain.Commands.RemoveRestaurant;
using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;
using FoodOrderSystem.Domain.Queries;
using FoodOrderSystem.Domain.Queries.SysAdminSearchForRestaurants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FoodOrderSystem.App.Controllers.V1
{
    [Route("api/v1/systemadmin")]
    [ApiController]
    [Authorize()]
    public class RestaurantSysAdminController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IUserRepository userRepository;
        private readonly ICommandDispatcher commandDispatcher;
        private readonly IQueryDispatcher queryDispatcher;

        public RestaurantSysAdminController(ILogger<RestaurantSysAdminController> logger, IUserRepository userRepository, ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            this.logger = logger;
            this.userRepository = userRepository;
            this.commandDispatcher = commandDispatcher;
            this.queryDispatcher = queryDispatcher;
        }

        [Route("restaurants")]
        [HttpGet]
        public async Task<IActionResult> GetRestaurantsAsync(string search)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var queryResult = await queryDispatcher.PostAsync(new SysAdminSearchForRestaurantsQuery(search), currentUser);
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

        [Route("restaurants")]
        [HttpPost]
        public async Task<IActionResult> PostRestaurantsAsync([FromBody] AddRestaurantModel addRestaurantModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var commandResult = await commandDispatcher.PostAsync(new AddRestaurantCommand(addRestaurantModel.Name), currentUser);
            switch (commandResult)
            {
                case UnauthorizedCommandResult _:
                    return Unauthorized();
                case ForbiddenCommandResult _:
                    return Forbid();
                case FailureCommandResult result:
                    return BadRequest(result);
                case SuccessCommandResult<Restaurant> result:
                    return Ok(RestaurantModel.FromRestaurant(result.Value));
                default:
                    throw new InvalidOperationException("internal server error");
            }
        }

        [Route("restaurants/{restaurantId}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteRestaurantAsync(Guid restaurantId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var commandResult = await commandDispatcher.PostAsync(new RemoveRestaurantCommand(new RestaurantId(restaurantId)), currentUser);
            switch (commandResult)
            {
                case UnauthorizedCommandResult _:
                    return Unauthorized();
                case ForbiddenCommandResult _:
                    return Forbid();
                case FailureCommandResult result:
                    return BadRequest(result);
                case SuccessCommandResult result:
                    return Ok();
                default:
                    throw new InvalidOperationException("internal server error");
            }
        }
    }
}