using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FoodOrderSystem.Persistence.SQLite
{
    public static class Initializer
    {
        public static void ConfigureServices(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<SystemDbContext>(options =>
            {
                options.EnableSensitiveDataLogging();
                options.UseLazyLoadingProxies();
                options.UseSqlite(connectionString, b =>
                {
                    b.MigrationsAssembly("FoodOrderSystem.Persistence.SQLite");
                });
            });
        }
    }
}
