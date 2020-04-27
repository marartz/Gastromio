using FoodOrderSystem.App.Models;
using FoodOrderSystem.Domain.Commands;
using FoodOrderSystem.Domain.Commands.Login;
using FoodOrderSystem.Domain.Model.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
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

        public AuthController(ILogger<AuthController> logger, IConfiguration config, ICommandDispatcher commandDispatcher)
        {
            this.logger = logger;
            this.config = config;
            this.commandDispatcher = commandDispatcher;
        }

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> PostLoginAsync([FromBody]LoginModel loginModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var commandResult = await commandDispatcher.PostAsync(new LoginCommand(loginModel.Username, loginModel.Password), null);
            switch (commandResult)
            {
                case UnauthorizedCommandResult _:
                    return Unauthorized();
                case ForbiddenCommandResult _:
                    return Forbid();
                case SuccessCommandResult<User> result:
                    var tokenString = GenerateJSONWebToken(result.Value);

                    var user = result.Value;
                    var userModel = new UserModel
                    {
                        Id = user.Id.Value,
                        Name = user.Name,
                        Role = user.Role.ToString()
                    };

                    return Ok(new { token = tokenString, user = userModel });
                default:
                    throw new InvalidOperationException("internal server error");
            }
        }

        [Route("ping")]
        [HttpGet]
        public IActionResult Ping()
        {
            return Ok("pong");
        }

        private string GenerateJSONWebToken(User user)
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
