using System.ComponentModel.DataAnnotations;
using System.Net;

namespace WheelOfFortune.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var queryString = $"?message={exception.Message}&route={context.Request.Path}";

            context.Response.Redirect($"/Home/Error{queryString}");
        }
    }
}
