using Auth.WebAPI.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Auth.WebAPI.Utils
{
    public static class ExceptionHandling
    {

        public static IApplicationBuilder HandleExceptions(this IApplicationBuilder app)
            => app.UseExceptionHandler(handler =>
            {
                handler.Run(async context =>
                {
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var exception = exceptionHandlerPathFeature?.Error;
                    var response = context.Response;

                    if (exception is BadRequestException ex)
                    {
                        response.StatusCode = StatusCodes.Status400BadRequest;
                        await response.WriteAsJsonAsync(ex.Messages);
                    }
                });
            });

    }
}
