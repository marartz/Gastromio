using FoodOrderSystem.App.Models;
using FoodOrderSystem.Domain.Commands;
using FoodOrderSystem.Domain.Commands.ChangeRestaurantAddress;
using FoodOrderSystem.Domain.Commands.ChangeRestaurantContactDetails;
using FoodOrderSystem.Domain.Commands.ChangeRestaurantDeliveryData;
using FoodOrderSystem.Domain.Commands.ChangeRestaurantName;
using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;
using FoodOrderSystem.Domain.Queries;
using FoodOrderSystem.Domain.Queries.GetRestaurantById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FoodOrderSystem.App.Controllers.V1
{
    [Route("api/v1/restaurantadmin")]
    [ApiController]
    [Authorize()]
    public class RestaurantAdminController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IUserRepository userRepository;
        private readonly ICommandDispatcher commandDispatcher;
        private readonly IQueryDispatcher queryDispatcher;

        public RestaurantAdminController(ILogger<RestaurantAdminController> logger, IUserRepository userRepository, ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            this.logger = logger;
            this.userRepository = userRepository;
            this.commandDispatcher = commandDispatcher;
            this.queryDispatcher = queryDispatcher;
        }

        [Route("restaurants/{restaurantId}")]
        [HttpGet]
        public async Task<IActionResult> GetRestaurantAsync(Guid restaurantId)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var queryResult = await queryDispatcher.PostAsync(new GetRestaurantByIdQuery(new RestaurantId(restaurantId)), currentUser);
            switch (queryResult)
            {
                case UnauthorizedQueryResult _:
                    return Unauthorized();
                case ForbiddenQueryResult _:
                    return Forbid();
                case SuccessQueryResult<Restaurant> result:
                    return Ok(RestaurantModel.FromRestaurant(result.Value));
                default:
                    throw new InvalidOperationException("internal server error");
            }
        }

        [Route("restaurants/{restaurantId}/changename")]
        [HttpPost]
        public async Task<IActionResult> PostChangeNameAsync(Guid restaurantId, [FromBody] ChangeRestaurantNameModel changeRestaurantNameModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var commandResult = await commandDispatcher.PostAsync(new ChangeRestaurantNameCommand(new RestaurantId(restaurantId), changeRestaurantNameModel.Name), currentUser);
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

        [Route("restaurants/{restaurantId}/changeaddress")]
        [HttpPost]
        public async Task<IActionResult> PostChangeAddressAsync(Guid restaurantId, [FromBody] ChangeRestaurantAddressModel changeRestaurantAddressModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var address = new Address(
                changeRestaurantAddressModel.Line1,
                changeRestaurantAddressModel.Line2,
                changeRestaurantAddressModel.ZipCode,
                changeRestaurantAddressModel.City
            );

            var commandResult = await commandDispatcher.PostAsync(new ChangeRestaurantAddressCommand(new RestaurantId(restaurantId), address), currentUser);
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

        [Route("restaurants/{restaurantId}/changecontactdetails")]
        [HttpPost]
        public async Task<IActionResult> PostChangeContactDetailsAsync(Guid restaurantId, [FromBody] ChangeRestaurantContactDetailsModel changeRestaurantContactDetailsModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var commandResult = await commandDispatcher.PostAsync(
                new ChangeRestaurantContactDetailsCommand(new RestaurantId(restaurantId), changeRestaurantContactDetailsModel.Website, changeRestaurantContactDetailsModel.Imprint),
                currentUser
            );

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

        [Route("restaurants/{restaurantId}/changedeliverydata")]
        [HttpPost]
        public async Task<IActionResult> PostChangeDeliveryDataAsync(Guid restaurantId, [FromBody] ChangeRestaurantDeliveryDataModel changeRestaurantDeliveryDataModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var commandResult = await commandDispatcher.PostAsync(
                new ChangeRestaurantDeliveryDataCommand(new RestaurantId(restaurantId), changeRestaurantDeliveryDataModel.MinimumOrderValue, changeRestaurantDeliveryDataModel.DeliveryCosts),
                currentUser
            );

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

    }
}