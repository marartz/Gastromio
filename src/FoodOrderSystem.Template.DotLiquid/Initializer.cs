using FoodOrderSystem.Domain.Adapters.Template;
using Microsoft.Extensions.DependencyInjection;

namespace FoodOrderSystem.Template.DotLiquid
{
    public static class Initializer
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ITemplateService, TemplateService>();
        }
    }
}