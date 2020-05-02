using FoodOrderSystem.Domain.Model;
using FoodOrderSystem.Domain.Model.User;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Queries
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceProvider serviceProvider;
        private static readonly IDictionary<Type, Type> queryHandlerMapping = new Dictionary<Type, Type>();

        public static void Initialize(IServiceCollection services)
        {
            var queryHandlerType = typeof(IQueryHandler<,>);
            var handlerTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.GetInterfaces().Any(intf => intf.IsGenericType && intf.GetGenericTypeDefinition() == queryHandlerType));
            foreach (var handlerType in handlerTypes)
            {
                var queryHandlerIntf = handlerType.GetInterfaces().First(intf => intf.IsGenericType && intf.GetGenericTypeDefinition() == queryHandlerType);
                var queryType = queryHandlerIntf.GetGenericArguments()[0];
                queryHandlerMapping.Add(queryType, handlerType);
                services.AddTransient(handlerType);
            }
        }

        public QueryDispatcher(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task<Result<TResult>> PostAsync<TQuery, TResult>(TQuery query, User currentUser, CancellationToken cancellationToken = default) where TQuery : IQuery<TResult>
        {
            var queryType = typeof(TQuery);

            if (!queryHandlerMapping.TryGetValue(queryType, out var handlerType))
                throw new InvalidOperationException($"could not find handler for query with type {queryType.FullName}");

            var handler = serviceProvider.GetService(handlerType) as IQueryHandler<TQuery, TResult>;

            return await handler.HandleAsync(query, currentUser, cancellationToken);
        }
    }
}
