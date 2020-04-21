using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FoodOrderSystem.Persistence.InMemory
{
    public static class Initializer
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SystemDbContext>(options =>
            {
                options.EnableSensitiveDataLogging();
                options.UseLazyLoadingProxies();
                options.UseInMemoryDatabase("FoodOrderSystem");
            });
        }
    }
}
