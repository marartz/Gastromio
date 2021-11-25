using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Gastromio.Core.Application.Commands;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Model.Users;
using Xunit;

namespace Gastromio.Domain.Tests.Application
{
    public abstract class CommandHandlerTestBase<THandler, TCommand>
        where THandler : ICommandHandler<TCommand> where TCommand : ICommand
    {
        protected abstract CommandHandlerTestFixtureBase<THandler, TCommand> FixtureBase
        {
            get;
        }

        [Fact]
        public async Task HandleAsync_CommandNull_ThrowsArgumentNullException()
        {
            // Arrange
            var testObject = FixtureBase.CreateTestObject();

            // Act
            Func<Task> act = async () =>
                await testObject.HandleAsync(default, FixtureBase.UserWithMinimumRole, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task HandleAsync_NotAuthorized_ChecksAuthorizationCorrectly()
        {
            // Arrange
            FixtureBase.SetupForSuccessfulCommandExecution(null);
            var testObject = FixtureBase.CreateTestObject();

            // Act
            Func<Task> act = async () => await testObject.HandleAsync(FixtureBase.CreateSuccessfulCommand(),
                null, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                if (FixtureBase.MinimumRole.HasValue)
                {
                    await act.Should().ThrowAsync<DomainException>();
                }
                else
                {
                    await act.Should().NotThrowAsync();
                }
            }
        }

        [Fact]
        public async Task HandleAsync_AuthorizedAsCustomer_ChecksAuthorizationCorrectly()
        {
            // Arrange
            FixtureBase.SetupForSuccessfulCommandExecution(Role.Customer);
            var testObject = FixtureBase.CreateTestObject();

            // Act
            Func<Task> act = async () => await testObject.HandleAsync(FixtureBase.CreateSuccessfulCommand(),
                FixtureBase.UserWithCustomerRole, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                if (FixtureBase.MinimumRole.HasValue && FixtureBase.MinimumRole.Value > Role.Customer)
                {
                    await act.Should().ThrowAsync<DomainException>();
                }
                else
                {
                    await act.Should().NotThrowAsync();
                }
            }
        }

        [Fact]
        public async Task HandleAsync_AuthorizedAsRestaurantAdmin_ChecksAuthorizationCorrectly()
        {
            // Arrange
            FixtureBase.SetupForSuccessfulCommandExecution(Role.RestaurantAdmin);
            var testObject = FixtureBase.CreateTestObject();

            // Act
            Func<Task> act = async () => await testObject.HandleAsync(FixtureBase.CreateSuccessfulCommand(),
                FixtureBase.UserWithRestaurantAdminRole, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                if (FixtureBase.MinimumRole.HasValue && FixtureBase.MinimumRole.Value > Role.RestaurantAdmin)
                {
                    await act.Should().ThrowAsync<DomainException>();
                }
                else
                {
                    await act.Should().NotThrowAsync();
                }
            }
        }

        [Fact]
        public async Task HandleAsync_AuthorizedAsSystemAdmin_ChecksAuthorizationCorrectly()
        {
            // Arrange
            FixtureBase.SetupForSuccessfulCommandExecution(Role.SystemAdmin);
            var testObject = FixtureBase.CreateTestObject();

            // Act
            Func<Task> act = async () => await testObject.HandleAsync(FixtureBase.CreateSuccessfulCommand(),
                FixtureBase.UserWithSystemAdminRole, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                if (FixtureBase.MinimumRole.HasValue && FixtureBase.MinimumRole.Value > Role.SystemAdmin)
                {
                    await act.Should().ThrowAsync<DomainException>();
                }
                else
                {
                    await act.Should().NotThrowAsync();
                }
            }
        }
    }

    public abstract class CommandHandlerTestBase<THandler, TCommand, TResult>
        where THandler : ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
    {
        protected abstract CommandHandlerTestFixtureBase<THandler, TCommand, TResult> FixtureBase
        {
            get;
        }

        [Fact]
        public async Task HandleAsync_CommandNull_ThrowsArgumentNullException()
        {
            // Arrange
            var testObject = FixtureBase.CreateTestObject();

            // Act
            Func<Task> act = async () =>
                await testObject.HandleAsync(default, FixtureBase.UserWithMinimumRole, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task HandleAsync_NotAuthorized_ChecksAuthorizationCorrectly()
        {
            // Arrange
            FixtureBase.SetupForSuccessfulCommandExecution(null);
            var testObject = FixtureBase.CreateTestObject();

            // Act
            Func<Task> act = async () => await testObject.HandleAsync(FixtureBase.CreateSuccessfulCommand(),
                null, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                if (FixtureBase.MinimumRole.HasValue)
                {
                    await act.Should().ThrowAsync<DomainException>();
                }
                else
                {
                    await act.Should().NotThrowAsync();
                }
            }
        }

        [Fact]
        public async Task HandleAsync_AuthorizedAsCustomer_ChecksAuthorizationCorrectly()
        {
            // Arrange
            FixtureBase.SetupForSuccessfulCommandExecution(Role.Customer);
            var testObject = FixtureBase.CreateTestObject();

            // Act
            Func<Task> act = async () => await testObject.HandleAsync(FixtureBase.CreateSuccessfulCommand(),
                FixtureBase.UserWithCustomerRole, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                if (FixtureBase.MinimumRole.HasValue && FixtureBase.MinimumRole.Value > Role.Customer)
                {
                    await act.Should().ThrowAsync<DomainException>();
                }
                else
                {
                    await act.Should().NotThrowAsync();
                }
            }
        }

        [Fact]
        public async Task HandleAsync_AuthorizedAsRestaurantAdmin_ChecksAuthorizationCorrectly()
        {
            // Arrange
            FixtureBase.SetupForSuccessfulCommandExecution(Role.RestaurantAdmin);
            var testObject = FixtureBase.CreateTestObject();

            // Act
            Func<Task> act = async () => await testObject.HandleAsync(FixtureBase.CreateSuccessfulCommand(),
                FixtureBase.UserWithRestaurantAdminRole, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                if (FixtureBase.MinimumRole.HasValue && FixtureBase.MinimumRole.Value > Role.RestaurantAdmin)
                {
                    await act.Should().ThrowAsync<DomainException>();
                }
                else
                {
                    await act.Should().NotThrowAsync();
                }
            }
        }

        [Fact]
        public async Task HandleAsync_AuthorizedAsSystemAdmin_ChecksAuthorizationCorrectly()
        {
            // Arrange
            FixtureBase.SetupForSuccessfulCommandExecution(Role.SystemAdmin);
            var testObject = FixtureBase.CreateTestObject();

            // Act
            Func<Task> act = async () => await testObject.HandleAsync(FixtureBase.CreateSuccessfulCommand(),
                FixtureBase.UserWithSystemAdminRole, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                if (FixtureBase.MinimumRole.HasValue && FixtureBase.MinimumRole.Value > Role.SystemAdmin)
                {
                    await act.Should().ThrowAsync<DomainException>();
                }
                else
                {
                    await act.Should().NotThrowAsync();
                }
            }
        }
    }
}
