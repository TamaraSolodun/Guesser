using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BusinessLayer_Guesser.Managers;
using DataLayer_Guesser;

namespace BusinessLayer_Guesser
{
    public static class Extensions
    {
        public static void AddBusinessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataLayer(configuration);

            services.AddTransient<UserManager>();
            services.AddTransient<GameManager>();
        }
    }
}
