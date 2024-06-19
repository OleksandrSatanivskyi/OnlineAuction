using Microsoft.Extensions.Localization;
using OnlineAuc.Application.Services.Auth;
using OnlineAuc.Data.DbContexts;
using OnlineAuc.Services;

namespace OnlineAuc
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IStringLocalizerFactory, ResourceManagerStringLocalizerFactory>();
            services.AddTransient(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));
            services.AddScoped<IDbContext>(provider => provider.GetService<ApplicationDbContext>());
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IAuctionService, AuctionService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IGuestService, GuestService>();

            return services;
        }
    }
}
