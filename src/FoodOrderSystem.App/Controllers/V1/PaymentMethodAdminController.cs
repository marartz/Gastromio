using FoodOrderSystem.App.Helper;
using FoodOrderSystem.App.Models;
using FoodOrderSystem.Domain.Commands;
using FoodOrderSystem.Domain.Commands.AddPaymentMethod;
using FoodOrderSystem.Domain.Commands.ChangePaymentMethod;
using FoodOrderSystem.Domain.Commands.RemovePaymentMethod;
using FoodOrderSystem.Domain.Model.PaymentMethod;
using FoodOrderSystem.Domain.Model.User;
using FoodOrderSystem.Domain.Queries;
using FoodOrderSystem.Domain.Queries.GetAllPaymentMethods;
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
    public class PaymentMethodAdminController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IUserRepository userRepository;
        private readonly ICommandDispatcher commandDispatcher;
        private readonly IQueryDispatcher queryDispatcher;
        private readonly IFailureMessageService failureMessageService;

        public PaymentMethodAdminController(ILogger<PaymentMethodAdminController> logger, IUserRepository userRepository,
            ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, IFailureMessageService failureMessageService)
        {
            this.logger = logger;
            this.userRepository = userRepository;
            this.commandDispatcher = commandDispatcher;
            this.queryDispatcher = queryDispatcher;
            this.failureMessageService = failureMessageService;
        }

        [Route("paymentMethods")]
        [HttpGet]
        public async Task<IActionResult> GetPaymentMethodsAsync()
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var queryResult = await queryDispatcher.PostAsync<GetAllPaymentMethodsQuery, ICollection<PaymentMethodViewModel>>(new GetAllPaymentMethodsQuery(), currentUser);
            return ResultHelper.HandleResult(queryResult, failureMessageService);
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

            var commandResult = await commandDispatcher.PostAsync<AddPaymentMethodCommand, PaymentMethodViewModel>(new AddPaymentMethodCommand(addPaymentMethodModel.Name, addPaymentMethodModel.Description), currentUser);
            return ResultHelper.HandleResult(commandResult, failureMessageService);
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

            var commandResult = await commandDispatcher.PostAsync<ChangePaymentMethodCommand, bool>(new ChangePaymentMethodCommand(new PaymentMethodId(paymentMethodId), changePaymentMethodModel.Name, changePaymentMethodModel.Description), currentUser);
            return ResultHelper.HandleResult(commandResult, failureMessageService);
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

            var commandResult = await commandDispatcher.PostAsync<RemovePaymentMethodCommand, bool>(new RemovePaymentMethodCommand(new PaymentMethodId(paymentMethodId)), currentUser);
            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }
    }
}