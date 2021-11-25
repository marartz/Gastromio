using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Gastromio.Core.Application.Commands.RemoveUser;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Restaurants;
using Gastromio.Core.Domain.Model.Users;
using Gastromio.Domain.TestKit.Application.Ports.Persistence;
using Gastromio.Domain.TestKit.Domain.Model.Restaurants;
using Gastromio.Domain.TestKit.Domain.Model.Users;
using Moq;
using Xunit;

namespace Gastromio.Domain.Tests.Application.Commands.RemoveUser
{
    public class
        RemoveUserCommandHandlerTests : CommandHandlerTestBase<RemoveUserCommandHandler, RemoveUserCommand>
    {
        private readonly Fixture fixture;

        public RemoveUserCommandHandlerTests()
        {
            fixture = new Fixture(Role.SystemAdmin);
        }

        [Fact]
        public async Task HandleAsync_UserIsAdminOfRestaurant_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupRandomUserToBeRemoved();
            fixture.SetupRandomRestaurantHavingUserAsAdmin();
            fixture.SetupRestaurantRepositoryFindingUser();

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulCommand();

            // Act
            Func<Task> act = async () => await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DomainException<UserIsRestaurantAdminFailure>>();
        }

        [Fact]
        public async Task HandleAsync_AllValid_CreatesUser()
        {
            // Arrange
            fixture.SetupForSuccessfulCommandExecution(fixture.MinimumRole);

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulCommand();

            // Act
            await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                fixture.UserRepositoryMock.VerifyRemoveAsync(fixture.UserToBeRemoved.Id, Times.Once);
            }
        }

        protected override CommandHandlerTestFixtureBase<RemoveUserCommandHandler, RemoveUserCommand> FixtureBase
        {
            get { return fixture; }
        }

        private sealed class Fixture : CommandHandlerTestFixtureBase<RemoveUserCommandHandler, RemoveUserCommand>
        {
            public Fixture(Role? minimumRole) : base(minimumRole)
            {
                UserRepositoryMock = new UserRepositoryMock(MockBehavior.Strict);
                RestaurantRepositoryMock = new RestaurantRepositoryMock(MockBehavior.Strict);
            }

            public User UserToBeRemoved { get; private set; }

            public Restaurant RestaurantHavingUserAsAdmin { get; private set; }

            public UserRepositoryMock UserRepositoryMock { get; }

            public RestaurantRepositoryMock RestaurantRepositoryMock { get; }

            public override RemoveUserCommandHandler CreateTestObject()
            {
                return new RemoveUserCommandHandler(
                    UserRepositoryMock.Object,
                    RestaurantRepositoryMock.Object
                );
            }

            public override RemoveUserCommand CreateSuccessfulCommand()
            {
                return new RemoveUserCommand(UserToBeRemoved.Id);
            }

            public void SetupRandomUserToBeRemoved()
            {
                UserToBeRemoved = new UserBuilder()
                    .WithEmail("max@mustermann.de")
                    .Create();
            }

            public void SetupRandomRestaurantHavingUserAsAdmin()
            {
                RestaurantHavingUserAsAdmin = new RestaurantBuilder()
                    .WithAdministrators(new HashSet<UserId> {UserToBeRemoved.Id})
                    .WithValidConstrains()
                    .Create();
            }

            public void SetupRestaurantRepositoryFindingUser()
            {
                RestaurantRepositoryMock.SetupFindByUserIdAsync(UserToBeRemoved.Id)
                    .ReturnsAsync(new[] {RestaurantHavingUserAsAdmin});
            }

            public void SetupRestaurantRepositoryNotFindingUser()
            {
                RestaurantRepositoryMock.SetupFindByUserIdAsync(UserToBeRemoved.Id)
                    .ReturnsAsync(Enumerable.Empty<Restaurant>());
            }

            public void SetupUserRepositoryRemovingUser()
            {
                UserRepositoryMock.SetupRemoveAsync(UserToBeRemoved.Id)
                    .Returns(Task.CompletedTask);
            }

            public override void SetupForSuccessfulCommandExecution(Role? role)
            {
                SetupRandomUserToBeRemoved();
                SetupRestaurantRepositoryNotFindingUser();
                SetupUserRepositoryRemovingUser();
            }
        }
    }
}
