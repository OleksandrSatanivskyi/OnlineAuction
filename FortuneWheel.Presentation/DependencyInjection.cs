using FortuneWheel.Services;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Localization;
using WheelOfFortune.Application.Services.Auth;
using WheelOfFortune.Data.DbContexts;
using WheelOfFortune.Services;

namespace WheelOfFortune
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IStringLocalizerFactory, ResourceManagerStringLocalizerFactory>();
            services.AddTransient(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));
            services.AddScoped<IDbContext>(provider => provider.GetService<ApplicationDbContext>());
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IWheelService, WheelService>();
            services.AddTransient<IAccountService, AccountService>();


            return services;
        }
    }
}
