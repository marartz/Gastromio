using FoodOrderSystem.Domain.Commands;
using FoodOrderSystem.Domain.Queries;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FoodOrderSystem.App.Helper
{
    public static class ResultHelper
    {
        public static IActionResult HandleCommandResult<TResult>(CommandResult<TResult> commandResult)
        {
            switch (commandResult)
            {
                case UnauthorizedCommandResult<TResult> _:
                    return new UnauthorizedResult();
                case ForbiddenCommandResult<TResult> _:
                    return new ForbidResult();
                case FailureCommandResult<TResult> _:
                    return new BadRequestResult();
                case SuccessCommandResult<TResult> result:
                    return new OkObjectResult(result.Value);
                default:
                    throw new InvalidOperationException("internal server error");
            }
        }

        public static IActionResult HandleQueryResult<TResult>(QueryResult<TResult> queryResult)
        {
            switch (queryResult)
            {
                case UnauthorizedQueryResult<TResult> _:
                    return new UnauthorizedResult();
                case ForbiddenQueryResult<TResult> _:
                    return new ForbidResult();
                case FailureCommandResult<TResult> _:
                    return new BadRequestResult();
                case SuccessQueryResult<TResult> result:
                    return new OkObjectResult(result.Value);
                default:
                    throw new InvalidOperationException("internal server error");
            }
        }
    }
}
