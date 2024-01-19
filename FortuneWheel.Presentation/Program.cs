using FortuneWheel.Application.Services.Auth;
using FortuneWheel.Data.DbContexts;
using FortuneWheel.Middlewares;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace FortuneWheel;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();
        ConfigureApp(app);

        app.Run();

    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.AddSession();
        services.AddControllersWithViews().AddRazorRuntimeCompilation();
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Access/Login";
                options.ExpireTimeSpan = TimeSpan.FromHours(12);
                
            });

        services.AddScoped<IDbContext>(provider => provider.GetService<ApplicationDbContext>());
        services.AddTransient<IAuthService, AuthService>();
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

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Auth}/{action=Login}/{id?}");
        });
    }
}
