using FoodOrderSystem.App.Helper;
using FoodOrderSystem.App.Models;
using FoodOrderSystem.Domain.Commands;
using FoodOrderSystem.Domain.Commands.AddCuisine;
using FoodOrderSystem.Domain.Commands.ChangeCuisine;
using FoodOrderSystem.Domain.Commands.RemoveCuisine;
using FoodOrderSystem.Domain.Model.Cuisine;
using FoodOrderSystem.Domain.Model.User;
using FoodOrderSystem.Domain.Queries;
using FoodOrderSystem.Domain.Queries.GetAllCuisines;
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
    public class CuisineAdminController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IUserRepository userRepository;
        private readonly ICommandDispatcher commandDispatcher;
        private readonly IQueryDispatcher queryDispatcher;

        public CuisineAdminController(ILogger<CuisineAdminController> logger, IUserRepository userRepository, ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            this.logger = logger;
            this.userRepository = userRepository;
            this.commandDispatcher = commandDispatcher;
            this.queryDispatcher = queryDispatcher;
        }

        [Route("cuisines")]
        [HttpGet]
        public async Task<IActionResult> GetCuisinesAsync()
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var queryResult = await queryDispatcher.PostAsync(new GetAllCuisinesQuery(), currentUser);
            switch (queryResult)
            {
                case UnauthorizedQueryResult _:
                    return Unauthorized();
                case ForbiddenQueryResult _:
                    return Forbid();
                case SuccessQueryResult<ICollection<Cuisine>> result:
                    var model = result.Value.Select(CuisineModel.FromCuisine).ToList();
                    return Ok(model);
                default:
                    throw new InvalidOperationException("internal server error");
            }
        }

        [Route("cuisines")]
        [HttpPost]
        public async Task<IActionResult> PostCuisinesAsync([FromBody] AddCuisineModel addCuisineModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var image = ImageHelper.ConvertFromImageUrl(addCuisineModel.Image);

            var commandResult = await commandDispatcher.PostAsync(new AddCuisineCommand(addCuisineModel.Name, image), currentUser);
            switch (commandResult)
            {
                case UnauthorizedCommandResult _:
                    return Unauthorized();
                case ForbiddenCommandResult _:
                    return Forbid();
                case FailureCommandResult result:
                    return BadRequest(result);
                case SuccessCommandResult<Cuisine> result:
                    return Ok(CuisineModel.FromCuisine(result.Value));
                default:
                    throw new InvalidOperationException("internal server error");
            }
        }

        [Route("cuisines/{cuisineId}/change")]
        [HttpPost]
        public async Task<IActionResult> PostChangeAsync(Guid cuisineId, [FromBody] ChangeCuisineModel changeCuisineModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var image = ImageHelper.ConvertFromImageUrl(changeCuisineModel.Image);

            var commandResult = await commandDispatcher.PostAsync(new ChangeCuisineCommand(new CuisineId(cuisineId), changeCuisineModel.Name, image), currentUser);
            switch (commandResult)
            {
                case UnauthorizedCommandResult _:
                    return Unauthorized();
                case ForbiddenCommandResult _:
                    return Forbid();
                case FailureCommandResult result:
                    return BadRequest(result);
                case SuccessCommandResult<Cuisine> result:
                    return Ok(CuisineModel.FromCuisine(result.Value));
                default:
                    throw new InvalidOperationException("internal server error");
            }
        }

        [Route("cuisines/{cuisineId}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteCuisineAsync(Guid cuisineId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var commandResult = await commandDispatcher.PostAsync(new RemoveCuisineCommand(new CuisineId(cuisineId)), currentUser);
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