using System.Net;
using Burmalda.Entities;

namespace Burmalda.Routing;

public static class ExceptionHandlingMiddelware
{
    public static WebApplication UseBurmaldaExceptionHandling(this WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            try
            {
                await next.Invoke();
            }
            catch (BurmaldaException exception)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(
                new {
                    Code = exception.Code, 
                    Message = exception.Message
                });
            }
        });
        
        return app;
    }
}