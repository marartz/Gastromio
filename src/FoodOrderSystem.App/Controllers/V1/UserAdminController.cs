using FoodOrderSystem.App.Helper;
using FoodOrderSystem.App.Models;
using FoodOrderSystem.Domain.Commands;
using FoodOrderSystem.Domain.Commands.AddUser;
using FoodOrderSystem.Domain.Commands.ChangeUserDetails;
using FoodOrderSystem.Domain.Commands.ChangeUserPassword;
using FoodOrderSystem.Domain.Commands.RemoveUser;
using FoodOrderSystem.Domain.Model.User;
using FoodOrderSystem.Domain.Queries;
using FoodOrderSystem.Domain.Queries.SearchForUsers;
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

namespace FoodOrderSystem.App.Controllers.V1
{
    [Route("api/v1/systemadmin")]
    [ApiController]
    [Authorize()]
    public class UserAdminController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IUserRepository userRepository;
        private readonly ICommandDispatcher commandDispatcher;
        private readonly IQueryDispatcher queryDispatcher;
        private readonly IFailureMessageService failureMessageService;

        public UserAdminController(ILogger<UserAdminController> logger, IUserRepository userRepository, ICommandDispatcher commandDispatcher,
            IQueryDispatcher queryDispatcher, IFailureMessageService failureMessageService)
        {
            this.logger = logger;
            this.userRepository = userRepository;
            this.commandDispatcher = commandDispatcher;
            this.queryDispatcher = queryDispatcher;
            this.failureMessageService = failureMessageService;
        }

        [Route("users")]
        [HttpGet]
        public async Task<IActionResult> GetUsersAsync(string search)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var queryResult = await queryDispatcher.PostAsync<SearchForUsersQuery, ICollection<UserViewModel>>(new SearchForUsersQuery(search), currentUser);
            return ResultHelper.HandleResult(queryResult, failureMessageService);
        }

        [Route("users")]
        [HttpPost]
        public async Task<IActionResult> PostUsersAsync([FromBody] AddUserModel addUserModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var role = (Role)Enum.Parse(typeof(Role), addUserModel.Role);

            var commandResult = await commandDispatcher.PostAsync<AddUserCommand, UserViewModel>(new AddUserCommand(role, addUserModel.Email, addUserModel.Password), currentUser);
            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("users/{userId}/changedetails")]
        [HttpPost]
        public async Task<IActionResult> PostChangeDetailsAsync(Guid userId, [FromBody] ChangeUserDetailsModel changeUserDetailsModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var role = (Role)Enum.Parse(typeof(Role), changeUserDetailsModel.Role);

            var commandResult = await commandDispatcher.PostAsync<ChangeUserDetailsCommand, bool>(
                new ChangeUserDetailsCommand(new UserId(userId), role, changeUserDetailsModel.Email),
                currentUser
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("users/{userId}/changepassword")]
        [HttpPost]
        public async Task<IActionResult> PostChangePasswordAsync(Guid userId, [FromBody] ChangeUserPasswordModel changeUserPasswordModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var commandResult = await commandDispatcher.PostAsync<ChangeUserPasswordCommand, bool>(new ChangeUserPasswordCommand(new UserId(userId), changeUserPasswordModel.Password), currentUser);
            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("users/{userId}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUserAsync(Guid userId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var commandResult = await commandDispatcher.PostAsync<RemoveUserCommand, bool>(new RemoveUserCommand(new UserId(userId)), currentUser);
            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }
    }
}