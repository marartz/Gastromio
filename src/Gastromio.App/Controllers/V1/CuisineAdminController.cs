using Gastromio.App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Gastromio.Core.Application.Commands;
using Gastromio.Core.Application.Commands.AddCuisine;
using Gastromio.Core.Application.Commands.ChangeCuisine;
using Gastromio.Core.Application.Commands.RemoveCuisine;
using Gastromio.Core.Application.DTOs;
using Gastromio.Core.Application.Queries;
using Gastromio.Core.Application.Queries.GetAllCuisines;
using Gastromio.Core.Domain.Model.Cuisines;
using Gastromio.Core.Domain.Model.Users;

namespace Gastromio.App.Controllers.V1
{
    [Route("api/v1/systemadmin")]
    [ApiController]
    [Authorize()]
    public class CuisineAdminController : ControllerBase
    {
        private readonly ICommandDispatcher commandDispatcher;
        private readonly IQueryDispatcher queryDispatcher;

        public CuisineAdminController(
            ICommandDispatcher commandDispatcher,
            IQueryDispatcher queryDispatcher
        )
        {
            this.commandDispatcher = commandDispatcher;
            this.queryDispatcher = queryDispatcher;
        }

        [Route("cuisines")]
        [HttpGet]
        public async Task<IActionResult> GetCuisinesAsync()
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var cuisineDtos = await queryDispatcher.PostAsync<GetAllCuisinesQuery, ICollection<CuisineDTO>>(
                new GetAllCuisinesQuery(),
                new UserId(currentUserId)
            );

            return Ok(cuisineDtos);
        }

        [Route("cuisines")]
        [HttpPost]
        public async Task<IActionResult> PostCuisinesAsync([FromBody] AddCuisineModel addCuisineModel)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var cuisineDto = await commandDispatcher.PostAsync<AddCuisineCommand, CuisineDTO>(
                new AddCuisineCommand(addCuisineModel.Name),
                new UserId(currentUserId)
            );

            return Ok(cuisineDto);
        }

        [Route("cuisines/{cuisineId}/change")]
        [HttpPost]
        public async Task<IActionResult> PostChangeAsync(Guid cuisineId,
            [FromBody] ChangeCuisineModel changeCuisineModel)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            await commandDispatcher.PostAsync(
                new ChangeCuisineCommand(new CuisineId(cuisineId), changeCuisineModel.Name),
                new UserId(currentUserId)
            );

            return Ok();
        }

        [Route("cuisines/{cuisineId}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteCuisineAsync(Guid cuisineId)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            await commandDispatcher.PostAsync(
                new RemoveCuisineCommand(new CuisineId(cuisineId)),
                new UserId(currentUserId)
            );

            return Ok();
        }
    }
}
