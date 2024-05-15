using FortuneWheel.Middlewares;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using WheelOfFortune.Data.DbContexts;
using WheelOfFortune.Middlewares;

namespace WheelOfFortune;

public class Program
{
    public static void Main(string[] args)
    {
        var rootPath = Directory.GetParent(
            Directory.GetParent(
            Directory.GetParent(
                AppContext.BaseDirectory).FullName).FullName).FullName;

        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();
        ConfigureApp(app);

        app.Run();

    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddLocalization(options =>
        {
            options.ResourcesPath = "Resources";
        });
        
        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.AddSupportedUICultures("en-US", "uk-UA", "fr-FR", "de-DE");
            options.FallBackToParentCultures = true;

            var acceptLanguageProvider = options.RequestCultureProviders
                .FirstOrDefault(p => p.GetType() == typeof(AcceptLanguageHeaderRequestCultureProvider));

            if (acceptLanguageProvider != null)
            {
                options.RequestCultureProviders.Remove(acceptLanguageProvider);
            }
        });

        services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
        });
        services.AddControllersWithViews().AddRazorRuntimeCompilation();
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Access/Login";
                options.ExpireTimeSpan = TimeSpan.FromHours(12);
                
            });

        services.AddServices();
        services.AddRazorPages().AddViewLocalization();
        services.AddScoped<RequestLocalizationCookiesMiddleware>();
    }

    private static void ConfigureApp(WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseSession();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<ExceptionHandlerMiddleware>();
        app.UseRequestLocalization();
        app.UseRequestLocalizationCookies();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Auth}/{action=Login}");
        });
    }
}
