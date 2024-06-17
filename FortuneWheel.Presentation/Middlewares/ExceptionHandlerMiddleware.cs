using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace OnlineAuc.Middlewares
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
            var cultureFeature = context.Features.Get<IRequestCultureFeature>();
            var culture = cultureFeature?.RequestCulture.UICulture ?? CultureInfo.CurrentUICulture;

            var localizedMessage = CultureHelper.Exception(exception.Message, culture) ?? exception.Message;

            var queryString = $"?message={Uri.EscapeDataString(localizedMessage)}";

            context.Response.Redirect($"/Home/Error{queryString}");
            await Task.CompletedTask;
        }
    }
}
