using DataLayer_Guesser.RepositoryImplementations;
using DataLayer_Guesser.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataLayer_Guesser
{
    public static class Extensions
    {
        public static void AddDataLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<GuesserDBContext>(options => options.UseNpgsql(configuration.GetSection("PostgreSQL:ConnectionString").Value));
            services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
            services.AddScoped(typeof(IGameRepository), typeof(GameRepository));

            using (var serviceScope = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<GuesserDBContext>())
                {
                    context.Database.Migrate();
                }
            }

        }
    }
}
