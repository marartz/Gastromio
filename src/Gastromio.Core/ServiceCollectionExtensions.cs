using Gastromio.Core.Application.Commands;
using Gastromio.Core.Application.Queries;
using Gastromio.Core.Domain.Model.PaymentMethods;
using Gastromio.Core.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Gastromio.Core
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCore(this IServiceCollection services)
        {
            // Register model classes
            services.AddTransient<IPaymentMethodRepository, PaymentMethodRepository>();

            // Register command classes
            services.AddTransient<ICommandDispatcher, CommandDispatcher>();
            CommandDispatcher.Initialize(services);

            // Register query classes
            services.AddTransient<IQueryDispatcher, QueryDispatcher>();
            QueryDispatcher.Initialize(services);

            // Import
            services.AddTransient<IRestaurantDataImporter, RestaurantDataImporter>();
            services.AddTransient<IDishDataImporter, DishDataImporter>();
        }
    }
}
