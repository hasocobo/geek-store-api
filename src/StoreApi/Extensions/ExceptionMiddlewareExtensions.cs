using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using StoreApi.Entities.Exceptions;

namespace StoreApi.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this WebApplication app, ILogger logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    context.Response.StatusCode = contextFeature.Error switch
                    {
                       NotFoundException => StatusCodes.Status404NotFound,
                       _ => StatusCodes.Status500InternalServerError
                    };
                    context.Response.ContentType = "application/json";

                    if (contextFeature != null)
                    {
                        logger.LogError("Something went wrong {message}", contextFeature.Error);

                        await context.Response.WriteAsync(
                            new ErrorDetails()
                            {
                                StatusCode = context.Response.StatusCode,
                                Message = contextFeature.Error.Message
                            }.ToString()
                        );
                    }
                });
            });
        }
    }
}
