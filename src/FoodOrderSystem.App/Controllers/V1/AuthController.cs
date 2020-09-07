using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FoodOrderSystem.App.Helper;
using FoodOrderSystem.App.Models;
using FoodOrderSystem.Core.Application.Commands;
using FoodOrderSystem.Core.Application.Commands.Login;
using FoodOrderSystem.Core.Application.DTOs;
using FoodOrderSystem.Core.Application.Services;
using FoodOrderSystem.Core.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

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
        private readonly IMemoryCache memoryCache;

        public AuthController(
            ILogger<AuthController> logger,
            IConfiguration config,
            ICommandDispatcher commandDispatcher,
            IFailureMessageService failureMessageService,
            IMemoryCache memoryCache
        )
        {
            this.logger = logger;
            this.config = config;
            this.commandDispatcher = commandDispatcher;
            this.failureMessageService = failureMessageService;
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
            
            var commandResult = await commandDispatcher.PostAsync<LoginCommand, UserDTO>(new LoginCommand(loginModel.Email, loginModel.Password), null);
            if (commandResult is SuccessResult<UserDTO> successResult)
            {
                memoryCache.Remove(cacheKey);
                
                var tokenString = GenerateJSONWebToken(successResult.Value);
                return Ok(new { token = tokenString, user = successResult.Value });
            }
            else
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
            }

            return ResultHelper.HandleResult(commandResult, failureMessageService);
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
