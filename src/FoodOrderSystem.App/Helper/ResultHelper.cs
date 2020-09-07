using Microsoft.AspNetCore.Mvc;
using System;
using FoodOrderSystem.Core.Application.Services;
using FoodOrderSystem.Core.Common;

namespace FoodOrderSystem.App.Helper
{
    public static class ResultHelper
    {
        public static IActionResult HandleResult<TResult>(Result<TResult> result, IFailureMessageService failureMessageService)
        {
            if (result is SuccessResult<TResult> successResult)
            {
                return new OkObjectResult(successResult.Value);
            }
            if (result is FailureResult<TResult> failureResult)
            {
                var errorDTO = new FailureResultDTO(failureMessageService.GetTranslatedMessages<TResult>(failureResult.Errors));
                return new ObjectResult(errorDTO) { StatusCode = failureResult.StatusCode };
            }
            throw new InvalidOperationException("internal server error");
        }
    }
}
