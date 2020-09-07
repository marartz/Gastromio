using FoodOrderSystem.Core.Application.Ports.Template;
using Microsoft.Extensions.DependencyInjection;

namespace FoodOrderSystem.Template.DotLiquid
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDotLiquid(this IServiceCollection services)
        {
            services.AddTransient<ITemplateService, TemplateService>();
        }
    }
}