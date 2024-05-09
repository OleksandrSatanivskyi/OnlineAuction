using WheelOfFortune.Application.Services.Auth;
using WheelOfFortune.Data.DbContexts;
using WheelOfFortune.Services;

namespace WheelOfFortune
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IDbContext>(provider => provider.GetService<ApplicationDbContext>());
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IWheelService, WheelService>();

            return services;
        }
    }
}
