using FoodOrderSystem.Domain.Model;
using FoodOrderSystem.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FoodOrderSystem.App.Helper
{
    public static class ResultHelper
    {
        public static IActionResult HandleResult<TResult>(Result<TResult> result, IFailureMessageService failureMessageService)
        {
            switch (result)
            {
                case SuccessResult<TResult> successResult:
                    return new OkObjectResult(successResult.Value);
                case FailureResult<TResult> failureResult:
                    switch (failureResult.Code)
                    {
                        case FailureResultCode.Unauthorized:
                            return new UnauthorizedResult();
                        case FailureResultCode.Forbidden:
                            return new ForbidResult();
                        default:
                            var message = failureMessageService.GetMessageFromResult(failureResult);
                            return new BadRequestObjectResult(message);
                    }
                default:
                    throw new InvalidOperationException("internal server error");
            }
        }
    }
}
