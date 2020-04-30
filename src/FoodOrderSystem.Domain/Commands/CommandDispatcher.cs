using FoodOrderSystem.Domain.Model.User;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Commands
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider serviceProvider;
        private static readonly IDictionary<Type, Type> commandHandlerMapping = new Dictionary<Type, Type>();

        public static void Initialize(IServiceCollection services)
        {
            var CommandHandlerType = typeof(ICommandHandler<,>);
            var handlerTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.GetInterfaces().Any(intf => intf.IsGenericType && intf.GetGenericTypeDefinition() == CommandHandlerType));
            foreach (var handlerType in handlerTypes)
            {
                var CommandHandlerIntf = handlerType.GetInterfaces().First(intf => intf.IsGenericType && intf.GetGenericTypeDefinition() == CommandHandlerType);
                var CommandType = CommandHandlerIntf.GetGenericArguments()[0];
                commandHandlerMapping.Add(CommandType, handlerType);
                services.AddTransient(handlerType);
            }
        }

        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task<CommandResult<TResult>> PostAsync<TCommand, TResult>(TCommand Command, User currentUser, CancellationToken cancellationToken = default) where TCommand : ICommand<TResult>
        {
            var commandType = typeof(TCommand);

            if (!commandHandlerMapping.TryGetValue(commandType, out var handlerType))
                throw new InvalidOperationException($"could not find handler for command with type {commandType.FullName}");

            var handler = serviceProvider.GetService(handlerType) as ICommandHandler<TCommand, TResult>;

            return await handler.HandleAsync(Command, currentUser, cancellationToken);
        }
    }
}
