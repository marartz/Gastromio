using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Gastromio.App.Models;
using Gastromio.Core.Application.Commands;
using Gastromio.Core.Application.Commands.AddUser;
using Gastromio.Core.Application.Commands.ChangeUserDetails;
using Gastromio.Core.Application.Commands.ChangeUserPassword;
using Gastromio.Core.Application.Commands.RemoveUser;
using Gastromio.Core.Application.DTOs;
using Gastromio.Core.Application.Queries;
using Gastromio.Core.Application.Queries.SearchForUsers;
using Gastromio.Core.Domain.Model.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gastromio.App.Controllers.V1
{
    [Route("api/v1/systemadmin")]
    [ApiController]
    [Authorize()]
    public class UserAdminController : ControllerBase
    {
        private readonly ICommandDispatcher commandDispatcher;
        private readonly IQueryDispatcher queryDispatcher;

        public UserAdminController(
            ICommandDispatcher commandDispatcher,
            IQueryDispatcher queryDispatcher
        )
        {
            this.commandDispatcher = commandDispatcher;
            this.queryDispatcher = queryDispatcher;
        }

        [Route("users")]
        [HttpGet]
        public async Task<IActionResult> GetUsersAsync(string search, int skip = 0, int take = -1)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var query = new SearchForUsersQuery(search, null, skip, take);

            var pagingDto = await queryDispatcher.PostAsync<SearchForUsersQuery, PagingDTO<UserDTO>>(
                query,
                new UserId(currentUserId)
            );

            return Ok(pagingDto);
        }

        [Route("users")]
        [HttpPost]
        public async Task<IActionResult> PostUsersAsync([FromBody] AddUserModel addUserModel)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var role = (Role) Enum.Parse(typeof(Role), addUserModel.Role);

            var userDto = await commandDispatcher.PostAsync<AddUserCommand, UserDTO>(
                new AddUserCommand(role, addUserModel.Email, addUserModel.Password),
                new UserId(currentUserId)
            );

            return Ok(userDto);
        }

        [Route("users/{userId}/changedetails")]
        [HttpPost]
        public async Task<IActionResult> PostChangeDetailsAsync(Guid userId,
            [FromBody] ChangeUserDetailsModel changeUserDetailsModel)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var role = (Role) Enum.Parse(typeof(Role), changeUserDetailsModel.Role);

            await commandDispatcher.PostAsync(
                new ChangeUserDetailsCommand(new UserId(userId), role, changeUserDetailsModel.Email),
                new UserId(currentUserId)
            );

            return Ok();
        }

        [Route("users/{userId}/changepassword")]
        [HttpPost]
        public async Task<IActionResult> PostChangePasswordAsync(Guid userId,
            [FromBody] ChangeUserPasswordModel changeUserPasswordModel)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            await commandDispatcher.PostAsync(
                new ChangeUserPasswordCommand(new UserId(userId), changeUserPasswordModel.Password),
                new UserId(currentUserId));

            return Ok();
        }

        [Route("users/{userId}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUserAsync(Guid userId)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            await commandDispatcher.PostAsync(
                new RemoveUserCommand(new UserId(userId)),
                new UserId(currentUserId)
            );

            return Ok();
        }
    }
}
