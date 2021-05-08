using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Gastromio.Core.Application.Commands.AddOrChangeDishOfRestaurant;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Restaurants;
using Gastromio.Core.Domain.Model.Users;
using Gastromio.Domain.TestKit.Application.Ports.Persistence;
using Gastromio.Domain.TestKit.Domain.Model.Restaurants;
using Moq;
using Xunit;

namespace Gastromio.Domain.Tests.Application.Commands.AddOrChangeDishOfRestaurant
{
    public class AddOrChangeDishOfRestaurantCommandHandlerTests : CommandHandlerTestBase<
        AddOrChangeDishOfRestaurantCommandHandler,
        AddOrChangeDishOfRestaurantCommand, Guid>
    {
        private readonly Fixture fixture;

        public AddOrChangeDishOfRestaurantCommandHandlerTests()
        {
            fixture = new Fixture(Role.RestaurantAdmin);
        }

        [Fact]
        public async Task HandleAsync_RestaurantNotKnown_ReturnsFailure()
        {
            // Arrange
            fixture.SetupRandomDish();
            fixture.SetupRandomDishCategory();
            fixture.SetupRandomRestaurant(fixture.MinimumRole);
            fixture.SetupRestaurantRepositoryNotFindingRestaurant();

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulCommand();

            // Act
            Func<Task> act = async () => await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DomainException<RestaurantDoesNotExistFailure>>();
        }

        [Fact]
        public async Task HandleAsync_AllValid_AddsDishToRestaurantAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupForSuccessfulCommandExecution(fixture.MinimumRole);

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulCommand();

            // Act
            var result = await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsSuccess.Should().BeTrue();
                fixture.RestaurantRepositoryMock.VerifyStoreAsync(fixture.Restaurant, Times.Once);
            }
        }

        protected override
            CommandHandlerTestFixtureBase<AddOrChangeDishOfRestaurantCommandHandler, AddOrChangeDishOfRestaurantCommand,
                Guid> FixtureBase
        {
            get { return fixture; }
        }

        private sealed class Fixture : CommandHandlerTestFixtureBase<AddOrChangeDishOfRestaurantCommandHandler,
            AddOrChangeDishOfRestaurantCommand, Guid>
        {
            public Fixture(Role? minimumRole) : base(minimumRole)
            {
                RestaurantRepositoryMock = new RestaurantRepositoryMock(MockBehavior.Strict);
            }

            public RestaurantRepositoryMock RestaurantRepositoryMock { get; }

            public Restaurant Restaurant { get; private set; }

            public DishCategory DishCategory { get; private set; }

            public Dish Dish { get; private set; }

            public override AddOrChangeDishOfRestaurantCommandHandler CreateTestObject()
            {
                return new AddOrChangeDishOfRestaurantCommandHandler(RestaurantRepositoryMock.Object);
            }

            public override AddOrChangeDishOfRestaurantCommand CreateSuccessfulCommand()
            {
                return new AddOrChangeDishOfRestaurantCommand(
                    Restaurant.Id,
                    DishCategory.Id,
                    null,
                    Dish.Name,
                    Dish.Description,
                    Dish.ProductInfo,
                    Dish.OrderNo,
                    Dish.Variants
                );
            }

            public void SetupRandomRestaurant(Role? role)
            {
                var builder = new RestaurantBuilder();

                if (role == Role.RestaurantAdmin)
                {
                    builder = builder
                        .WithAdministrators(new HashSet<UserId> {UserId});
                }

                Restaurant = builder
                    .WithDishCategories(new []{DishCategory})
                    .WithValidConstrains()
                    .Create();
            }

            public void SetupRandomDishCategory()
            {
                DishCategory = new DishCategoryBuilder()
                    .WithOrderNo(0)
                    .WithValidConstrains()
                    .Create();
            }

            public void SetupRandomDish()
            {
                Dish = new DishBuilder()
                    .WithName("random-dish")
                    .WithOrderNo(0)
                    .WithValidConstrains()
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
                    .ReturnsAsync((Restaurant) null);
            }

            public void SetupRestaurantRepositoryStoringRestaurant()
            {
                RestaurantRepositoryMock.SetupStoreAsync(Restaurant)
                    .Returns(Task.CompletedTask);
            }

            public override void SetupForSuccessfulCommandExecution(Role? role)
            {
                SetupRandomDish();
                SetupRandomDishCategory();
                SetupRandomRestaurant(role);
                SetupRestaurantRepositoryFindingRestaurant();
                SetupRestaurantRepositoryStoringRestaurant();
            }
        }
    }
}
