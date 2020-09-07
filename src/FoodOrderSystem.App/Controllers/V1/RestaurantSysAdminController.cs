using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FoodOrderSystem.App.Helper;
using FoodOrderSystem.App.Models;
using FoodOrderSystem.Core.Application.Commands;
using FoodOrderSystem.Core.Application.Commands.AddRestaurant;
using FoodOrderSystem.Core.Application.Commands.DisableSupportForRestaurant;
using FoodOrderSystem.Core.Application.Commands.EnableSupportForRestaurant;
using FoodOrderSystem.Core.Application.Commands.ImportDishData;
using FoodOrderSystem.Core.Application.Commands.ImportRestaurantData;
using FoodOrderSystem.Core.Application.Commands.RemoveRestaurant;
using FoodOrderSystem.Core.Application.DTOs;
using FoodOrderSystem.Core.Application.Queries;
using FoodOrderSystem.Core.Application.Queries.SysAdminSearchForRestaurants;
using FoodOrderSystem.Core.Application.Services;
using FoodOrderSystem.Core.Domain.Model.Restaurant;
using FoodOrderSystem.Core.Domain.Model.User;
using FoodOrderSystem.Core.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FoodOrderSystem.App.Controllers.V1
{
    [Route("api/v1/systemadmin")]
    [ApiController]
    [Authorize()]
    public class RestaurantSysAdminController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly ICommandDispatcher commandDispatcher;
        private readonly IQueryDispatcher queryDispatcher;
        private readonly IFailureMessageService failureMessageService;

        public RestaurantSysAdminController(ILogger<RestaurantSysAdminController> logger,
            ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher,
            IFailureMessageService failureMessageService)
        {
            this.logger = logger;
            this.commandDispatcher = commandDispatcher;
            this.queryDispatcher = queryDispatcher;
            this.failureMessageService = failureMessageService;
        }

        [Route("restaurants")]
        [HttpGet]
        public async Task<IActionResult> GetRestaurantsAsync(string search, int skip = 0, int take = -1)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var query = new SysAdminSearchForRestaurantsQuery(search, skip, take);

            var queryResult =
                await queryDispatcher
                    .PostAsync<SysAdminSearchForRestaurantsQuery, PagingDTO<RestaurantDTO>>(query,
                        new UserId(currentUserId));

            return ResultHelper.HandleResult(queryResult, failureMessageService);
        }

        [Route("restaurants")]
        [HttpPost]
        public async Task<IActionResult> PostRestaurantsAsync([FromBody] AddRestaurantModel addRestaurantModel)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var commandResult =
                await commandDispatcher.PostAsync<AddRestaurantCommand, RestaurantDTO>(
                    new AddRestaurantCommand(addRestaurantModel.Name), new UserId(currentUserId));
            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/enablesupport")]
        [HttpPost]
        public async Task<IActionResult> PostEnableSupportAsync(Guid restaurantId)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var commandResult =
                await commandDispatcher.PostAsync<EnableSupportForRestaurantCommand, bool>(
                    new EnableSupportForRestaurantCommand(new RestaurantId(restaurantId)), new UserId(currentUserId));

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/disablesupport")]
        [HttpPost]
        public async Task<IActionResult> PostDisableSupportAsync(Guid restaurantId)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var commandResult =
                await commandDispatcher.PostAsync<DisableSupportForRestaurantCommand, bool>(
                    new DisableSupportForRestaurantCommand(new RestaurantId(restaurantId)), new UserId(currentUserId));

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteRestaurantAsync(Guid restaurantId)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var commandResult = await commandDispatcher.PostAsync<RemoveRestaurantCommand, bool>(
                new RemoveRestaurantCommand(new RestaurantId(restaurantId)), new UserId(currentUserId));
            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/import")]
        [HttpPost]
        public async Task<IActionResult> PostRestaurantImportAsync(string dryRun)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var file = Request.Form.Files[0];

            await using var stream = file.OpenReadStream();

            var commandResult =
                await commandDispatcher.PostAsync<ImportRestaurantDataCommand, ImportLog>(
                    new ImportRestaurantDataCommand(stream, dryRun != null), new UserId(currentUserId));

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("dishes/import")]
        [HttpPost]
        public async Task<IActionResult> PostDishImportAsync(string dryRun)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var file = Request.Form.Files[0];

            await using var stream = file.OpenReadStream();

            var commandResult =
                await commandDispatcher.PostAsync<ImportDishDataCommand, ImportLog>(
                    new ImportDishDataCommand(stream, dryRun != null), new UserId(currentUserId));

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }
   }
}