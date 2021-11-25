using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Gastromio.Core.Application.Commands.ChangeCuisine;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Cuisines;
using Gastromio.Core.Domain.Model.Users;
using Gastromio.Domain.TestKit.Application.Ports.Persistence;
using Gastromio.Domain.TestKit.Domain.Model.Cuisines;
using Moq;
using Xunit;

namespace Gastromio.Domain.Tests.Application.Commands.ChangeCuisine
{
    public class ChangeCuisineCommandHandlerTests : CommandHandlerTestBase<ChangeCuisineCommandHandler,
        ChangeCuisineCommand>
    {
        private readonly Fixture fixture;

        public ChangeCuisineCommandHandlerTests()
        {
            fixture = new Fixture(Role.SystemAdmin);
        }

        [Fact]
        public async Task HandleAsync_CuisineNotKnown_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupRandomCuisine();
            fixture.SetupCuisineRepositoryNotFindingCuisineById();

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulCommand();

            // Act
            Func<Task> act = async () => await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DomainException<CuisineDoesNotExistFailure>>();
        }

        [Fact]
        public async Task HandleAsync_AllValid_StoresCuisine()
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
                fixture.CuisineRepositoryMock.VerifyStoreAsync(fixture.Cuisine, Times.Once);
            }
        }

        protected override
            CommandHandlerTestFixtureBase<ChangeCuisineCommandHandler, ChangeCuisineCommand> FixtureBase
        {
            get { return fixture; }
        }

        private sealed class Fixture : CommandHandlerTestFixtureBase<ChangeCuisineCommandHandler,
            ChangeCuisineCommand>
        {
            public Fixture(Role? minimumRole) : base(minimumRole)
            {
                CuisineRepositoryMock = new CuisineRepositoryMock(MockBehavior.Strict);
            }

            public Cuisine Cuisine { get; private set; }

            public CuisineRepositoryMock CuisineRepositoryMock { get; }

            public override ChangeCuisineCommandHandler CreateTestObject()
            {
                return new ChangeCuisineCommandHandler(
                    CuisineRepositoryMock.Object
                );
            }

            public override ChangeCuisineCommand CreateSuccessfulCommand()
            {
                return new ChangeCuisineCommand(Cuisine.Id, "test");
            }

            public void SetupRandomCuisine()
            {
                Cuisine = new CuisineBuilder()
                    .WithValidConstrains()
                    .Create();
            }

            public void SetupCuisineRepositoryFindingCuisineById()
            {
                CuisineRepositoryMock.SetupFindByCuisineIdAsync(Cuisine.Id)
                    .ReturnsAsync(Cuisine);
            }

            public void SetupCuisineRepositoryNotFindingCuisineById()
            {
                CuisineRepositoryMock.SetupFindByCuisineIdAsync(Cuisine.Id)
                    .ReturnsAsync((Cuisine)null);
            }

            public void SetupCuisineRepositoryStoringCuisine()
            {
                CuisineRepositoryMock.Setup(m => m.StoreAsync(Cuisine, It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);
            }

            public override void SetupForSuccessfulCommandExecution(Role? role)
            {
                SetupRandomCuisine();
                SetupCuisineRepositoryFindingCuisineById();
                SetupCuisineRepositoryStoringCuisine();
            }
        }
    }
}
