using System.Globalization;

namespace FortuneWheel.Middlewares
{
    public class CultureMiddleware
    {
        private readonly RequestDelegate _next;

        public CultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string culture = context.Request.Cookies[""];
            string[] languages = culture?.Split(',');
            string primaryLanguage = languages?.FirstOrDefault()?.Trim();

            if (string.IsNullOrEmpty(primaryLanguage))
                primaryLanguage = "uk-UA";

            CultureInfo.CurrentUICulture = new CultureInfo(primaryLanguage);

            await _next(context);
        }

    }
}
