using FortuneWheel.Application.Services.Auth;
using FortuneWheel.Data.DbContexts;
using FortuneWheel.Services;

namespace FortuneWheel
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
