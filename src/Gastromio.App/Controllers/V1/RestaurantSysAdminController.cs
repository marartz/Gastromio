using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Gastromio.App.Models;
using Gastromio.Core.Application.Commands;
using Gastromio.Core.Application.Commands.ActivateRestaurant;
using Gastromio.Core.Application.Commands.AddAdminToRestaurant;
using Gastromio.Core.Application.Commands.AddCuisineToRestaurant;
using Gastromio.Core.Application.Commands.AddRestaurant;
using Gastromio.Core.Application.Commands.ChangeRestaurantName;
using Gastromio.Core.Application.Commands.DeactivateRestaurant;
using Gastromio.Core.Application.Commands.DisableSupportForRestaurant;
using Gastromio.Core.Application.Commands.EnableSupportForRestaurant;
using Gastromio.Core.Application.Commands.ImportDishData;
using Gastromio.Core.Application.Commands.ImportRestaurantData;
using Gastromio.Core.Application.Commands.RemoveAdminFromRestaurant;
using Gastromio.Core.Application.Commands.RemoveCuisineFromRestaurant;
using Gastromio.Core.Application.Commands.RemoveRestaurant;
using Gastromio.Core.Application.Commands.SetImportIdOfRestaurant;
using Gastromio.Core.Application.DTOs;
using Gastromio.Core.Application.Queries;
using Gastromio.Core.Application.Queries.SysAdminSearchForRestaurants;
using Gastromio.Core.Domain.Model.Cuisines;
using Gastromio.Core.Domain.Model.Restaurants;
using Gastromio.Core.Domain.Model.Users;
using Gastromio.Core.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gastromio.App.Controllers.V1
{
    [Route("api/v1/systemadmin")]
    [ApiController]
    [Authorize()]
    public class RestaurantSysAdminController : ControllerBase
    {
        private readonly ICommandDispatcher commandDispatcher;
        private readonly IQueryDispatcher queryDispatcher;

        public RestaurantSysAdminController(
            ICommandDispatcher commandDispatcher,
            IQueryDispatcher queryDispatcher
        )
        {
            this.commandDispatcher = commandDispatcher;
            this.queryDispatcher = queryDispatcher;
        }

        [Route("restaurants")]
        [HttpGet]
        public async Task<IActionResult> GetRestaurantsAsync(string search, int skip = 0, int take = -1)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var query = new SysAdminSearchForRestaurantsQuery(search, skip, take);

            var pagingDto =
                await queryDispatcher.PostAsync<SysAdminSearchForRestaurantsQuery, PagingDTO<RestaurantDTO>>(
                    query,
                    new UserId(currentUserId)
                );

            return Ok(pagingDto);
        }

        [Route("restaurants")]
        [HttpPost]
        public async Task<IActionResult> PostRestaurantsAsync([FromBody] AddRestaurantModel addRestaurantModel)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var restaurantDto = await commandDispatcher.PostAsync<AddRestaurantCommand, RestaurantDTO>(
                new AddRestaurantCommand(addRestaurantModel.Name),
                new UserId(currentUserId)
            );

            return Ok(restaurantDto);
        }

        [Route("restaurants/{restaurantId}/changename")]
        [HttpPost]
        public async Task<IActionResult> PostChangeNameAsync(Guid restaurantId,
            [FromBody] ChangeRestaurantNameModel changeRestaurantNameModel)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            await commandDispatcher.PostAsync(
                new ChangeRestaurantNameCommand(new RestaurantId(restaurantId), changeRestaurantNameModel.Name),
                new UserId(currentUserId)
            );

            return Ok();
        }

        [Route("restaurants/{restaurantId}/setimportid")]
        [HttpPost]
        public async Task<IActionResult> PostSetImportIdAsync(Guid restaurantId,
            [FromBody] SetImportIdOfRestaurantModel setImportIdOfRestaurantModel)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            await commandDispatcher.PostAsync(
                new SetImportIdOfRestaurantCommand(new RestaurantId(restaurantId),
                    setImportIdOfRestaurantModel.ImportId),
                new UserId(currentUserId)
            );

            return Ok();
        }

        [Route("restaurants/{restaurantId}/addcuisine")]
        [HttpPost]
        public async Task<IActionResult> PostAddCuisineAsync(Guid restaurantId,
            [FromBody] AddCuisineToRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            await commandDispatcher.PostAsync(
                new AddCuisineToRestaurantCommand(new RestaurantId(restaurantId), new CuisineId(model.CuisineId)),
                new UserId(currentUserId)
            );

            return Ok();
        }

        [Route("restaurants/{restaurantId}/removecuisine")]
        [HttpPost]
        public async Task<IActionResult> PostRemoveCuisineAsync(Guid restaurantId,
            [FromBody] RemoveCuisineFromRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            await commandDispatcher.PostAsync(
                new RemoveCuisineFromRestaurantCommand(new RestaurantId(restaurantId), new CuisineId(model.CuisineId)),
                new UserId(currentUserId)
            );

            return Ok();
        }

        [Route("restaurants/{restaurantId}/addadmin")]
        [HttpPost]
        public async Task<IActionResult> PostAddAdminAsync(Guid restaurantId,
            [FromBody] AddAdminToRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            await commandDispatcher.PostAsync(
                new AddAdminToRestaurantCommand(new RestaurantId(restaurantId), new UserId(model.UserId)),
                new UserId(currentUserId)
            );

            return Ok();
        }

        [Route("restaurants/{restaurantId}/removeadmin")]
        [HttpPost]
        public async Task<IActionResult> PostRemoveAdminAsync(Guid restaurantId,
            [FromBody] RemoveAdminFromRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            await commandDispatcher.PostAsync(
                new RemoveAdminFromRestaurantCommand(new RestaurantId(restaurantId), new UserId(model.UserId)),
                new UserId(currentUserId)
            );

            return Ok();
        }

        [Route("restaurants/{restaurantId}/activate")]
        [HttpPost]
        public async Task<IActionResult> PostActivateAsync(Guid restaurantId)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            await commandDispatcher.PostAsync(
                new ActivateRestaurantCommand(new RestaurantId(restaurantId)),
                new UserId(currentUserId)
            );

            return Ok();
        }

        [Route("restaurants/{restaurantId}/deactivate")]
        [HttpPost]
        public async Task<IActionResult> PostDeactivateAsync(Guid restaurantId)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            await commandDispatcher.PostAsync(
                new DeactivateRestaurantCommand(new RestaurantId(restaurantId)),
                new UserId(currentUserId)
            );

            return Ok();
        }

        [Route("restaurants/{restaurantId}/enablesupport")]
        [HttpPost]
        public async Task<IActionResult> PostEnableSupportAsync(Guid restaurantId)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            await commandDispatcher.PostAsync(
                new EnableSupportForRestaurantCommand(new RestaurantId(restaurantId)),
                new UserId(currentUserId)
            );

            return Ok();
        }

        [Route("restaurants/{restaurantId}/disablesupport")]
        [HttpPost]
        public async Task<IActionResult> PostDisableSupportAsync(Guid restaurantId)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            await commandDispatcher.PostAsync(
                new DisableSupportForRestaurantCommand(new RestaurantId(restaurantId)),
                new UserId(currentUserId)
            );

            return Ok();
        }

        [Route("restaurants/{restaurantId}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteRestaurantAsync(Guid restaurantId)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            await commandDispatcher.PostAsync(
                new RemoveRestaurantCommand(new RestaurantId(restaurantId)),
                new UserId(currentUserId)
            );

            return Ok();
        }

        [Route("restaurants/import")]
        [HttpPost]
        public async Task<IActionResult> PostRestaurantImportAsync(string dryRun)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var file = Request.Form.Files[0];

            await using var stream = file.OpenReadStream();

            var importLog = await commandDispatcher.PostAsync<ImportRestaurantDataCommand, ImportLog>(
                new ImportRestaurantDataCommand(stream, dryRun != null),
                new UserId(currentUserId)
            );

            return Ok(importLog);
        }

        [Route("dishes/import")]
        [HttpPost]
        public async Task<IActionResult> PostDishImportAsync(string dryRun)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var file = Request.Form.Files[0];

            await using var stream = file.OpenReadStream();

            var importLog = await commandDispatcher.PostAsync<ImportDishDataCommand, ImportLog>(
                new ImportDishDataCommand(stream, dryRun != null),
                new UserId(currentUserId)
            );

            return Ok(importLog);
        }
   }
}
