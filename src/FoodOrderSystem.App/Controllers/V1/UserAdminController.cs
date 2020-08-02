using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FoodOrderSystem.App.Helper;
using FoodOrderSystem.App.Models;
using FoodOrderSystem.Core.Application.Commands;
using FoodOrderSystem.Core.Application.Commands.AddUser;
using FoodOrderSystem.Core.Application.Commands.ChangeUserDetails;
using FoodOrderSystem.Core.Application.Commands.ChangeUserPassword;
using FoodOrderSystem.Core.Application.Commands.RemoveUser;
using FoodOrderSystem.Core.Application.DTOs;
using FoodOrderSystem.Core.Application.Queries;
using FoodOrderSystem.Core.Application.Queries.SearchForUsers;
using FoodOrderSystem.Core.Application.Services;
using FoodOrderSystem.Core.Domain.Model.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FoodOrderSystem.App.Controllers.V1
{
    [Route("api/v1/systemadmin")]
    [ApiController]
    [Authorize()]
    public class UserAdminController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly ICommandDispatcher commandDispatcher;
        private readonly IQueryDispatcher queryDispatcher;
        private readonly IFailureMessageService failureMessageService;

        public UserAdminController(ILogger<UserAdminController> logger, ICommandDispatcher commandDispatcher,
            IQueryDispatcher queryDispatcher, IFailureMessageService failureMessageService)
        {
            this.logger = logger;
            this.commandDispatcher = commandDispatcher;
            this.queryDispatcher = queryDispatcher;
            this.failureMessageService = failureMessageService;
        }

        [Route("users")]
        [HttpGet]
        public async Task<IActionResult> GetUsersAsync(string search, int skip = 0, int take = -1)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var query = new SearchForUsersQuery(search, null, skip, take);

            var queryResult =
                await queryDispatcher
                    .PostAsync<SearchForUsersQuery, PagingDTO<UserDTO>>(query, new UserId(currentUserId));

            return ResultHelper.HandleResult(queryResult, failureMessageService);
        }

        [Route("users")]
        [HttpPost]
        public async Task<IActionResult> PostUsersAsync([FromBody] AddUserModel addUserModel)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var role = (Role) Enum.Parse(typeof(Role), addUserModel.Role);

            var commandResult = await commandDispatcher.PostAsync<AddUserCommand, UserDTO>(
                new AddUserCommand(role, addUserModel.Email, addUserModel.Password), new UserId(currentUserId));
            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("users/{userId}/changedetails")]
        [HttpPost]
        public async Task<IActionResult> PostChangeDetailsAsync(Guid userId,
            [FromBody] ChangeUserDetailsModel changeUserDetailsModel)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var role = (Role) Enum.Parse(typeof(Role), changeUserDetailsModel.Role);

            var commandResult = await commandDispatcher.PostAsync<ChangeUserDetailsCommand, bool>(
                new ChangeUserDetailsCommand(new UserId(userId), role, changeUserDetailsModel.Email),
                new UserId(currentUserId)
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("users/{userId}/changepassword")]
        [HttpPost]
        public async Task<IActionResult> PostChangePasswordAsync(Guid userId,
            [FromBody] ChangeUserPasswordModel changeUserPasswordModel)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var commandResult = await commandDispatcher.PostAsync<ChangeUserPasswordCommand, bool>(
                new ChangeUserPasswordCommand(new UserId(userId), changeUserPasswordModel.Password),
                new UserId(currentUserId));
            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("users/{userId}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUserAsync(Guid userId)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var commandResult =
                await commandDispatcher.PostAsync<RemoveUserCommand, bool>(new RemoveUserCommand(new UserId(userId)),
                    new UserId(currentUserId));
            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }
    }
}