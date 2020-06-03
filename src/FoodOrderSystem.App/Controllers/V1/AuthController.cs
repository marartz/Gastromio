using FoodOrderSystem.App.Helper;
using FoodOrderSystem.App.Models;
using FoodOrderSystem.Domain.Commands;
using FoodOrderSystem.Domain.Commands.Login;
using FoodOrderSystem.Domain.Model;
using FoodOrderSystem.Domain.Services;
using FoodOrderSystem.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrderSystem.App.Controllers.V1
{
    [Route("api/v1/auth")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IConfiguration config;
        private readonly ICommandDispatcher commandDispatcher;
        private readonly IFailureMessageService failureMessageService;

        public AuthController(ILogger<AuthController> logger, IConfiguration config, ICommandDispatcher commandDispatcher, IFailureMessageService failureMessageService)
        {
            this.logger = logger;
            this.config = config;
            this.commandDispatcher = commandDispatcher;
            this.failureMessageService = failureMessageService;
        }

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> PostLoginAsync([FromBody]LoginModel loginModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var commandResult = await commandDispatcher.PostAsync<LoginCommand, UserViewModel>(new LoginCommand(loginModel.Email, loginModel.Password), null);
            if (commandResult is SuccessResult<UserViewModel> successResult)
            {
                var tokenString = GenerateJSONWebToken(successResult.Value);
                return Ok(new { token = tokenString, user = successResult.Value });
            }

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("ping")]
        [HttpGet]
        public IActionResult Ping()
        {
            return Ok("pong");
        }

        private string GenerateJSONWebToken(UserViewModel user)
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
