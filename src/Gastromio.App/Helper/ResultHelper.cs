using Microsoft.AspNetCore.Mvc;
using System;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;

namespace Gastromio.App.Helper
{
    public static class ResultHelper
    {
        public static IActionResult HandleResult<TResult>(Result<TResult> result)
        {
            if (result is SuccessResult<TResult> successResult)
            {
                return new OkObjectResult(successResult.Value);
            }
            if (result is FailureResult<TResult> failureResult)
            {
                return failureResult.Failure switch
                {
                    SessionExpiredFailure _ => new ObjectResult(failureResult.Failure) {StatusCode = 401},
                    ForbiddenFailure _ => new ObjectResult(failureResult.Failure) {StatusCode = 403},
                    _ => new ObjectResult(failureResult.Failure) {StatusCode = 400}
                };
            }
            throw new InvalidOperationException("internal server error");
        }
    }
}
