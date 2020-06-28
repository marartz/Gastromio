using FoodOrderSystem.App.Helper;
using FoodOrderSystem.App.Models;
using FoodOrderSystem.Domain.Commands;
using FoodOrderSystem.Domain.Commands.AddRestaurant;
using FoodOrderSystem.Domain.Commands.RemoveRestaurant;
using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;
using FoodOrderSystem.Domain.Queries;
using FoodOrderSystem.Domain.Queries.SysAdminSearchForRestaurants;
using FoodOrderSystem.Domain.Services;
using FoodOrderSystem.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FoodOrderSystem.Domain.Commands.ImportRestaurantData;
using Microsoft.AspNetCore.Http;

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
        private readonly IFailureMessageService failureMessageService;

        public RestaurantSysAdminController(ILogger<RestaurantSysAdminController> logger, IUserRepository userRepository,
            ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, IFailureMessageService failureMessageService)
        {
            this.logger = logger;
            this.userRepository = userRepository;
            this.commandDispatcher = commandDispatcher;
            this.queryDispatcher = queryDispatcher;
            this.failureMessageService = failureMessageService;
        }

        [Route("restaurants")]
        [HttpGet]
        public async Task<IActionResult> GetRestaurantsAsync(string search, int skip = 0, int take = -1)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var query = new SysAdminSearchForRestaurantsQuery(search, skip, take);

            var queryResult =
                await queryDispatcher
                    .PostAsync<SysAdminSearchForRestaurantsQuery, PagingViewModel<RestaurantViewModel>>(query,
                        currentUser);
            
            return ResultHelper.HandleResult(queryResult, failureMessageService);
        }

        [Route("restaurants")]
        [HttpPost]
        public async Task<IActionResult> PostRestaurantsAsync([FromBody] AddRestaurantModel addRestaurantModel)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var commandResult = await commandDispatcher.PostAsync<AddRestaurantCommand, RestaurantViewModel>(new AddRestaurantCommand(addRestaurantModel.Name), currentUser);
            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/import")]
        [HttpPost]
        public async Task<IActionResult> PostRestaurantImportAsync(string dryRun)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));
            
            var file = Request.Form.Files[0];

            await using var stream = file.OpenReadStream();
            
            var commandResult =
                await commandDispatcher.PostAsync<ImportRestaurantDataCommand, RestaurantImportLog>(
                    new ImportRestaurantDataCommand(stream, dryRun != null), currentUser);
            
            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }
        
        [Route("restaurants/{restaurantId}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteRestaurantAsync(Guid restaurantId)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var commandResult = await commandDispatcher.PostAsync<RemoveRestaurantCommand, bool>(new RemoveRestaurantCommand(new RestaurantId(restaurantId)), currentUser);
            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }
    }
}