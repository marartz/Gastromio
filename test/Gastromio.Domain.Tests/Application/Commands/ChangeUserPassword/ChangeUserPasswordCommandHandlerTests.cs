using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Gastromio.Core.Application.Commands.ChangeUserPassword;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Users;
using Gastromio.Domain.TestKit.Application.Ports.Persistence;
using Gastromio.Domain.TestKit.Domain.Model.Users;
using Moq;
using Xunit;

namespace Gastromio.Domain.Tests.Application.Commands.ChangeUserPassword
{
    public class ChangeUserPasswordCommandHandlerTests : CommandHandlerTestBase<ChangeUserPasswordCommandHandler,
        ChangeUserPasswordCommand>
    {
        private readonly Fixture fixture;

        public ChangeUserPasswordCommandHandlerTests()
        {
            fixture = new Fixture(Role.SystemAdmin);
        }

        [Fact]
        public async Task HandleAsync_UserNotKnown_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupUser();
            fixture.SetupNewPassword();
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
                var validationResult = fixture.User.ValidatePassword("Start2020!!!");
                validationResult.Should().BeTrue();
                fixture.UserRepositoryMock.VerifyStoreAsync(fixture.User, Times.Once);
            }
        }

        protected override
            CommandHandlerTestFixtureBase<ChangeUserPasswordCommandHandler, ChangeUserPasswordCommand> FixtureBase
        {
            get { return fixture; }
        }

        private sealed class Fixture : CommandHandlerTestFixtureBase<ChangeUserPasswordCommandHandler,
            ChangeUserPasswordCommand>
        {
            public Fixture(Role? minimumRole) : base(minimumRole)
            {
                UserRepositoryMock = new UserRepositoryMock(MockBehavior.Strict);
            }

            public User User { get; private set; }

            public UserRepositoryMock UserRepositoryMock { get; }

            public string NewPassword { get; private set; }

            public override ChangeUserPasswordCommandHandler CreateTestObject()
            {
                return new ChangeUserPasswordCommandHandler(
                    UserRepositoryMock.Object
                );
            }

            public override ChangeUserPasswordCommand CreateSuccessfulCommand()
            {
                return new ChangeUserPasswordCommand(User.Id, NewPassword);
            }

            public void SetupUser()
            {
                User = new UserBuilder()
                    .WithRole(Role.Customer)
                    .WithEmail("test@test.de")
                    .Create();
            }

            public void SetupNewPassword()
            {
                NewPassword = "Start2020!!!";
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
                SetupNewPassword();
                SetupUserRepositoryFindingUserById();
                SetupUserRepositoryStoringUser();
            }
        }
    }
}
