using FoodOrderSystem.App.Models;
using FoodOrderSystem.Domain.Commands;
using FoodOrderSystem.Domain.Commands.AddPaymentMethod;
using FoodOrderSystem.Domain.Commands.ChangePaymentMethod;
using FoodOrderSystem.Domain.Commands.RemovePaymentMethod;
using FoodOrderSystem.Domain.Model.PaymentMethod;
using FoodOrderSystem.Domain.Model.User;
using FoodOrderSystem.Domain.Queries;
using FoodOrderSystem.Domain.Queries.GetAllPaymentMethods;
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
    public class PaymentMethodAdminController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IUserRepository userRepository;
        private readonly ICommandDispatcher commandDispatcher;
        private readonly IQueryDispatcher queryDispatcher;

        public PaymentMethodAdminController(ILogger<PaymentMethodAdminController> logger, IUserRepository userRepository, ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            this.logger = logger;
            this.userRepository = userRepository;
            this.commandDispatcher = commandDispatcher;
            this.queryDispatcher = queryDispatcher;
        }

        [Route("paymentMethods")]
        [HttpGet]
        public async Task<IActionResult> GetPaymentMethodsAsync()
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var queryResult = await queryDispatcher.PostAsync(new GetAllPaymentMethodsQuery(), currentUser);
            switch (queryResult)
            {
                case UnauthorizedQueryResult _:
                    return Unauthorized();
                case ForbiddenQueryResult _:
                    return Forbid();
                case SuccessQueryResult<ICollection<PaymentMethod>> result:
                    var model = result.Value.Select(en => new PaymentMethodModel
                    {
                        Id = en.Id.Value,
                        Name = en.Name,
                        Description = en.Description
                    }).ToList();
                    return Ok(model);
                default:
                    throw new InvalidOperationException("internal server error");
            }
        }

        [Route("paymentMethods")]
        [HttpPost]
        public async Task<IActionResult> PostPaymentMethodsAsync([FromBody] AddPaymentMethodModel addPaymentMethodModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var commandResult = await commandDispatcher.PostAsync(new AddPaymentMethodCommand(addPaymentMethodModel.Name, addPaymentMethodModel.Description), currentUser);
            switch (commandResult)
            {
                case UnauthorizedCommandResult _:
                    return Unauthorized();
                case ForbiddenCommandResult _:
                    return Forbid();
                case FailureCommandResult result:
                    return BadRequest(result);
                case SuccessCommandResult<PaymentMethod> result:
                    var model = new PaymentMethodModel
                    {
                        Id = result.Value.Id.Value,
                        Name = result.Value.Name,
                        Description = result.Value.Description
                    };
                    return Ok(model);
                default:
                    throw new InvalidOperationException("internal server error");
            }
        }

        [Route("paymentMethods/{paymentMethodId}/change")]
        [HttpPost]
        public async Task<IActionResult> PostChangeAsync(Guid paymentMethodId, [FromBody] ChangePaymentMethodModel changePaymentMethodModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var commandResult = await commandDispatcher.PostAsync(new ChangePaymentMethodCommand(new PaymentMethodId(paymentMethodId), changePaymentMethodModel.Name, changePaymentMethodModel.Description), currentUser);
            switch (commandResult)
            {
                case UnauthorizedCommandResult _:
                    return Unauthorized();
                case ForbiddenCommandResult _:
                    return Forbid();
                case FailureCommandResult result:
                    return BadRequest(result);
                case SuccessCommandResult<PaymentMethod> result:
                    var model = new PaymentMethodModel
                    {
                        Id = result.Value.Id.Value,
                        Name = result.Value.Name,
                        Description = result.Value.Description
                    };
                    return Ok(model);
                default:
                    throw new InvalidOperationException("internal server error");
            }
        }

        [Route("paymentMethods/{paymentMethodId}")]
        [HttpDelete]
        public async Task<IActionResult> DeletePaymentMethodAsync(Guid paymentMethodId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var commandResult = await commandDispatcher.PostAsync(new RemovePaymentMethodCommand(new PaymentMethodId(paymentMethodId)), currentUser);
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