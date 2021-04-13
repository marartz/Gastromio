using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Gastromio.App.Models;
using Gastromio.Core.Application.Commands;
using Gastromio.Core.Application.Commands.ChangePassword;
using Gastromio.Core.Application.Commands.ChangePasswordWithResetCode;
using Gastromio.Core.Application.Commands.Login;
using Gastromio.Core.Application.Commands.RequestPasswordChange;
using Gastromio.Core.Application.Commands.ValidatePasswordResetCode;
using Gastromio.Core.Application.DTOs;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Gastromio.App.Controllers.V1
{
    [Route("api/v1/auth")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IConfiguration config;
        private readonly ICommandDispatcher commandDispatcher;
        private readonly IMemoryCache memoryCache;

        public AuthController(
            ILogger<AuthController> logger,
            IConfiguration config,
            ICommandDispatcher commandDispatcher,
            IMemoryCache memoryCache
        )
        {
            this.logger = logger;
            this.config = config;
            this.commandDispatcher = commandDispatcher;
            this.memoryCache = memoryCache;
        }

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> PostLoginAsync([FromBody]LoginModel loginModel)
        {
            var cacheKey = GetFailureCountCacheKey(Request.Host.Value);
            if (memoryCache.TryGetValue<int>(cacheKey, out var delay))
            {
                logger.LogInformation($"Waiting {delay} ms");
                await Task.Delay(delay);
            }

            try
            {
                var user = await commandDispatcher.PostAsync<LoginCommand, UserDTO>(
                    new LoginCommand(loginModel.Email, loginModel.Password),
                    null
                );
                memoryCache.Remove(cacheKey);

                var tokenString = GenerateJSONWebToken(user);

                return Ok(new {token = tokenString, user});
            }
            catch (DomainException<WrongCredentialsFailure>)
            {
                if (delay == 0)
                {
                    delay = 100;
                }
                else
                {
                    delay = (int)(delay * 1.2);
                    if (delay > 1000)
                    {
                        delay = 1000;
                    }
                }

                memoryCache.Set(cacheKey, delay);

                throw;
            }
        }

        [AllowAnonymous]
        [Route("requestpasswordchange")]
        [HttpPost]
        public async Task<IActionResult> PostRequestPasswordChangeAsync(
            [FromBody] RequestPasswordChangeModel requestPasswordChangeModel)
        {
            await commandDispatcher.PostAsync(
                new RequestPasswordChangeCommand(requestPasswordChangeModel.UserEmail),
                null
            );

            return Ok();
        }

        [AllowAnonymous]
        [Route("validatepasswordresetcode")]
        [HttpPost]
        public async Task<IActionResult> PostValidatePasswordResetCodeAsync(
            [FromBody] ValidatePasswordResetCodeModel validatePasswordResetCodeModel)
        {
            byte[] passwordResetCode;

            try
            {
                passwordResetCode = Convert.FromBase64String(validatePasswordResetCodeModel.PasswordResetCode);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            await commandDispatcher.PostAsync(
                new ValidatePasswordResetCodeCommand(new UserId(validatePasswordResetCodeModel.UserId),
                    passwordResetCode),
                null
            );

            return Ok();
        }

        [AllowAnonymous]
        [Route("changepasswordwithresetcode")]
        [HttpPost]
        public async Task<IActionResult> PostChangePasswordWithResetCodeAsync(
            [FromBody] ChangePasswordWithResetCodeModel changePasswordWithResetCodeModel)
        {
            byte[] passwordResetCode;

            try
            {
                passwordResetCode = Convert.FromBase64String(changePasswordWithResetCodeModel.PasswordResetCode);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            await commandDispatcher.PostAsync(
                new ChangePasswordWithResetCodeCommand(new UserId(changePasswordWithResetCodeModel.UserId),
                    passwordResetCode, changePasswordWithResetCodeModel.Password),
                null
            );
                    passwordResetCode, changePasswordWithResetCodeModel.Password), null);

            return commandResult is SuccessResult<bool>
                ? Ok()
                : ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("changepassword")]
        [HttpPost]
        public async Task<IActionResult> PostChangePasswordAsync(
            [FromBody] ChangeUserPasswordModel changeUserPasswordModel)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var curUserId = new UserId(currentUserId);

            var commandResult = await commandDispatcher.PostAsync<ChangePasswordCommand, bool>(
                new ChangePasswordCommand(changeUserPasswordModel.Password), curUserId);

            return Ok();
        }

        [Route("ping")]
        [HttpGet]
        public IActionResult Ping()
        {
            return Ok("pong");
        }

        private string GetFailureCountCacheKey(string userHostAddress)
        {
            return $"FailureCount:{userHostAddress}";
        }

        private string GenerateJSONWebToken(UserDTO user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                config["Jwt:Issuer"],
                config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
