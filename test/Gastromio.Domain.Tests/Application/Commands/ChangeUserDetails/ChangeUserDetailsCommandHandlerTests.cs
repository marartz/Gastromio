using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Gastromio.Core.Application.Commands.ChangeUserDetails;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Users;
using Gastromio.Domain.TestKit.Application.Ports.Persistence;
using Gastromio.Domain.TestKit.Domain.Model.Users;
using Moq;
using Xunit;

namespace Gastromio.Domain.Tests.Application.Commands.ChangeUserDetails
{
    public class ChangeUserDetailsCommandHandlerTests : CommandHandlerTestBase<ChangeUserDetailsCommandHandler,
        ChangeUserDetailsCommand>
    {
        private readonly Fixture fixture;

        public ChangeUserDetailsCommandHandlerTests()
        {
            fixture = new Fixture(Role.SystemAdmin);
        }

        [Fact]
        public async Task HandleAsync_UserNotKnown_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupUser();
            fixture.SetupNewRoleAndNewEmail();
            fixture.SetupUserRepositoryNotFindingUserById();

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulCommand();

            // Act
            Func<Task> act = async () => await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DomainException<UserDoesNotExistFailure>>();
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
                fixture.User.Role.Should().Be(fixture.NewRole);
                fixture.User.Email.Should().Be(fixture.NewEmail);
                fixture.UserRepositoryMock.VerifyStoreAsync(fixture.User, Times.Once);
            }
        }

        protected override
            CommandHandlerTestFixtureBase<ChangeUserDetailsCommandHandler, ChangeUserDetailsCommand> FixtureBase
        {
            get { return fixture; }
        }

        private sealed class Fixture : CommandHandlerTestFixtureBase<ChangeUserDetailsCommandHandler,
            ChangeUserDetailsCommand>
        {
            public Fixture(Role? minimumRole) : base(minimumRole)
            {
                UserRepositoryMock = new UserRepositoryMock(MockBehavior.Strict);
            }

            public User User { get; private set; }

            public UserRepositoryMock UserRepositoryMock { get; }

            public Role NewRole { get; private set; }

            public string NewEmail { get; private set; }

            public override ChangeUserDetailsCommandHandler CreateTestObject()
            {
                return new ChangeUserDetailsCommandHandler(
                    UserRepositoryMock.Object
                );
            }

            public override ChangeUserDetailsCommand CreateSuccessfulCommand()
            {
                return new ChangeUserDetailsCommand(User.Id, NewRole, NewEmail);
            }

            public void SetupUser()
            {
                User = new UserBuilder()
                    .WithRole(Role.Customer)
                    .WithEmail("test@test.de")
                    .Create();
            }

            public void SetupNewRoleAndNewEmail()
            {
                NewRole = Role.SystemAdmin;
                NewEmail = "changed@test.de";
            }

            public void SetupUserRepositoryFindingUserById()
            {
                UserRepositoryMock.SetupFindByUserIdAsync(User.Id)
                    .ReturnsAsync(User);
            }

            public void SetupUserRepositoryNotFindingUserById()
            {
                UserRepositoryMock.SetupFindByUserIdAsync(User.Id)
                    .ReturnsAsync((User) null);
            }

            public void SetupUserRepositoryStoringUser()
            {
                UserRepositoryMock.Setup(m => m.StoreAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);
            }

            public override void SetupForSuccessfulCommandExecution(Role? role)
            {
                SetupUser();
                SetupNewRoleAndNewEmail();
                SetupUserRepositoryFindingUserById();
                SetupUserRepositoryStoringUser();
            }
        }
    }
}
