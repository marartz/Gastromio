using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gastromio.Core.Application.Ports.Persistence;
using Gastromio.Core.Domain.Model.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Gastromio.Core.Application.Commands
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider serviceProvider;
        private static readonly IDictionary<Type, Type> CommandHandlerMapping = new Dictionary<Type, Type>();

        public static void Initialize(IServiceCollection services)
        {
            InitializeHandlersOfType(services, typeof(ICommandHandler<>));
            InitializeHandlersOfType(services, typeof(ICommandHandler<,>));
        }

        private static void InitializeHandlersOfType(IServiceCollection services, Type commandHandlerType)
        {
            var handlerTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.GetInterfaces().Any(@interface =>
                    @interface.IsGenericType && @interface.GetGenericTypeDefinition() == commandHandlerType));
            foreach (var handlerType in handlerTypes)
            {
                var commandHandlerInterface = handlerType.GetInterfaces().First(@interface =>
                    @interface.IsGenericType && @interface.GetGenericTypeDefinition() == commandHandlerType);
                var commandType = commandHandlerInterface.GetGenericArguments()[0];
                CommandHandlerMapping.Add(commandType, handlerType);
                services.AddTransient(handlerType);
            }
        }

        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task PostAsync<TCommand>(TCommand command, UserId currentUserId, CancellationToken cancellationToken = default) where TCommand : ICommand
        {
            User currentUser = null;

            if (currentUserId != null)
            {
                var userRepository = serviceProvider.GetService<IUserRepository>();
                currentUser = await userRepository.FindByUserIdAsync(currentUserId, cancellationToken);
            }

            var commandType = typeof(TCommand);

            if (!CommandHandlerMapping.TryGetValue(commandType, out var handlerType))
                throw new InvalidOperationException($"could not find handler for command with type {commandType.FullName}");

            var handler = serviceProvider.GetService(handlerType) as ICommandHandler<TCommand>;
            if (handler == null)
                throw new InvalidOperationException(
                    $"handler of type {handlerType} could not be get from service provider");

            await handler.HandleAsync(command, currentUser, cancellationToken);
        }

        public async Task<TResult> PostAsync<TCommand, TResult>(TCommand command, UserId currentUserId, CancellationToken cancellationToken = default) where TCommand : ICommand<TResult>
        {
            User currentUser = null;

            if (currentUserId != null)
            {
                var userRepository = serviceProvider.GetService<IUserRepository>();
                currentUser = await userRepository.FindByUserIdAsync(currentUserId, cancellationToken);
            }

            var commandType = typeof(TCommand);

            if (!CommandHandlerMapping.TryGetValue(commandType, out var handlerType))
                throw new InvalidOperationException($"could not find handler for command with type {commandType.FullName}");

            var handler = serviceProvider.GetService(handlerType) as ICommandHandler<TCommand, TResult>;
            if (handler == null)
                throw new InvalidOperationException(
                    $"handler of type {handlerType} could not be get from service provider");

            return await handler.HandleAsync(command, currentUser, cancellationToken);
        }
    }
}
