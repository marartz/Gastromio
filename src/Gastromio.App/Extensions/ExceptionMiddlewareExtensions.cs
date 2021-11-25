using System;
using System.Net;
using Gastromio.Core.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Gastromio.App.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        var occuredException = contextFeature.Error;
                        Guid correlationId;
                        if (occuredException is DomainException domainException)
                        {
                            context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                            context.Response.ContentType = "application/json";
                            correlationId = domainException.CorrelationId;
                            await context.Response.WriteAsync(GetDomainExceptionResponseText(domainException));
                        }
                        else
                        {
                            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                            context.Response.ContentType = "text/plain";
                            correlationId = Guid.NewGuid();
                            await context.Response.WriteAsync(GetResponseText(correlationId));
                        }

                        logger.LogError(occuredException, $"Something went wrong! CorrelationId: {correlationId}");
                    }
                });
            });
        }

        private static string GetResponseText(Guid correlationId)
        {
            return $"Something went wrong! Please contact the IT and give them your correlationId '{correlationId}'.";
        }

        private static string GetDomainExceptionResponseText(DomainException occuredException)
        {
            return JsonConvert.SerializeObject(occuredException.Failure);
        }
    }
}
