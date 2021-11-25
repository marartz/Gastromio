using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Gastromio.Core.Application.Commands.AddDeviatingOpeningDayToRestaurant;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Restaurants;
using Gastromio.Core.Domain.Model.Users;
using Gastromio.Domain.TestKit.Application.Ports.Persistence;
using Gastromio.Domain.TestKit.Domain.Model.Restaurants;
using Moq;
using Xunit;

namespace Gastromio.Domain.Tests.Application.Commands.AddDeviatingOpeningDayToRestaurant
{
    public class AddDeviatingOpeningDayToRestaurantCommandHandlerTests : CommandHandlerTestBase<AddDeviatingOpeningDayToRestaurantCommandHandler,
        AddDeviatingOpeningDayToRestaurantCommand>
    {
        private readonly Fixture fixture;

        public AddDeviatingOpeningDayToRestaurantCommandHandlerTests()
        {
            fixture = new Fixture(Role.RestaurantAdmin);
        }

        [Fact]
        public async Task HandleAsync_RestaurantNotKnown_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupRandomRestaurant(fixture.MinimumRole);
            fixture.SetupRandomDeviatingOpeningDay();
            fixture.SetupRestaurantRepositoryNotFindingRestaurant();

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulCommand();

            // Act
            Func<Task> act = async () => await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DomainException<RestaurantDoesNotExistFailure>>();
        }

        [Fact]
        public async Task HandleAsync_AllValid_AddsDeviatingDays()
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
                fixture.Restaurant.DeviatingOpeningDays.Should().BeEquivalentTo(fixture.DeviatingOpeningDay);
                fixture.RestaurantRepositoryMock.VerifyStoreAsync(fixture.Restaurant, Times.Once);
            }
        }

        protected override
            CommandHandlerTestFixtureBase<AddDeviatingOpeningDayToRestaurantCommandHandler, AddDeviatingOpeningDayToRestaurantCommand> FixtureBase
        {
            get { return fixture; }
        }

        private sealed class Fixture : CommandHandlerTestFixtureBase<AddDeviatingOpeningDayToRestaurantCommandHandler,
            AddDeviatingOpeningDayToRestaurantCommand>
        {
            public Fixture(Role? minimumRole) : base(minimumRole)
            {
                RestaurantRepositoryMock = new RestaurantRepositoryMock(MockBehavior.Strict);
            }

            public RestaurantRepositoryMock RestaurantRepositoryMock { get; }

            public Restaurant Restaurant { get; private set; }

            public DeviatingOpeningDay DeviatingOpeningDay { get; private set; }

            public override AddDeviatingOpeningDayToRestaurantCommandHandler CreateTestObject()
            {
                return new AddDeviatingOpeningDayToRestaurantCommandHandler(
                    RestaurantRepositoryMock.Object
                );
            }

            public override AddDeviatingOpeningDayToRestaurantCommand CreateSuccessfulCommand()
            {
                return new AddDeviatingOpeningDayToRestaurantCommand(Restaurant.Id, DeviatingOpeningDay.Date,
                    DeviatingOpeningDay.Status);
            }

            public void SetupRandomRestaurant(Role? role)
            {
                var builder = new RestaurantBuilder()
                    .WithoutDeviatingOpeningDays();

                if (role == Role.RestaurantAdmin)
                {
                    builder = builder
                        .WithAdministrators(new HashSet<UserId> {UserId});
                }

                Restaurant = builder
                    .WithValidConstrains()
                    .Create();
            }

            public void SetupRandomDeviatingOpeningDay()
            {
                DeviatingOpeningDay = new DeviatingOpeningDayBuilder()
                    .WithoutOpeningPeriods()
                    .Create();
            }

            public void SetupRestaurantRepositoryFindingRestaurant()
            {
                RestaurantRepositoryMock.SetupFindByRestaurantIdAsync(Restaurant.Id)
                    .ReturnsAsync(Restaurant);
            }

            public void SetupRestaurantRepositoryNotFindingRestaurant()
            {
                RestaurantRepositoryMock.SetupFindByRestaurantIdAsync(Restaurant.Id)
                    .ReturnsAsync((Restaurant)null);
            }

            public void SetupRestaurantRepositoryStoringRestaurant()
            {
                RestaurantRepositoryMock.SetupStoreAsync(Restaurant)
                    .Returns(Task.CompletedTask);
            }

            public override void SetupForSuccessfulCommandExecution(Role? role)
            {
                SetupRandomRestaurant(role);
                SetupRandomDeviatingOpeningDay();
                SetupRestaurantRepositoryFindingRestaurant();
                SetupRestaurantRepositoryStoringRestaurant();
            }
        }
    }
}
