using FoodOrderSystem.Domain.Model;
using FoodOrderSystem.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

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
