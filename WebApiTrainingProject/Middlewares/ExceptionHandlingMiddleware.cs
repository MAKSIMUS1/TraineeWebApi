using FluentValidation;
using System.Net;
using System.Text.Json;

namespace WebApiTrainingProject.Middlewares
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";

                var errors = ex.Errors.Select(e => e.ErrorMessage).ToList();

                var body = JsonSerializer.Serialize(new
                {
                    errors = errors
                });

                await context.Response.WriteAsync(body);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var body = JsonSerializer.Serialize(new
                {
                    error = "Internal Server Error",
                    details = ex.Message
                });

                await context.Response.WriteAsync(body);
            }
        }
    }
}
