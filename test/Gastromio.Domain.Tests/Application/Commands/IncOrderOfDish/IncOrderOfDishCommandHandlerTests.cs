using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Gastromio.Core.Application.Commands.IncOrderOfDish;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Restaurants;
using Gastromio.Core.Domain.Model.Users;
using Gastromio.Domain.TestKit.Application.Ports.Persistence;
using Gastromio.Domain.TestKit.Domain.Model.Restaurants;
using Moq;
using Xunit;

namespace Gastromio.Domain.Tests.Application.Commands.IncOrderOfDish
{
    public class IncOrderOfDishCommandHandlerTests : CommandHandlerTestBase<
        IncOrderOfDishCommandHandler, IncOrderOfDishCommand, bool>
    {
        private readonly Fixture fixture;

        public IncOrderOfDishCommandHandlerTests()
        {
            fixture = new Fixture(Role.RestaurantAdmin);
        }

        [Fact]
        public async Task HandleAsync_DishNotKnown_ReturnsFailure()
        {
            // Arrange
            fixture.SetupRestaurantWithDishes(3);
            fixture.SetupCurrentDish(1);
            fixture.SetupRestaurantRepositoryFindingRestaurant();

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulCommand();

            // Act
            Func<Task> act = async () => await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DomainException<RestaurantDoesNotExistFailure>>();
        }

        [Fact]
        public async Task HandleAsync_ThreeDishes_CurrentHasIndex0_ChangesDishOrderAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupRestaurantWithDishes(3);
            fixture.SetupCurrentDish(0);
            fixture.SetupRestaurantRepositoryFindingRestaurant();

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulCommand();

            // Act
            var result = await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsSuccess.Should().BeTrue();
                fixture.Dishes[0].OrderNo.Should().Be(1);
                fixture.Dishes[1].OrderNo.Should().Be(0);
                fixture.Dishes[2].OrderNo.Should().Be(2);
            }
        }

        [Fact]
        public async Task HandleAsync_ThreeDishes_CurrentHasIndex0_StoresRestaurant()
        {
            // Arrange
            fixture.SetupRestaurantWithDishes(3);
            fixture.SetupCurrentDish(0);
            fixture.SetupRestaurantRepositoryFindingRestaurant();

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulCommand();

            // Act
            await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            fixture.RestaurantRepositoryMock.VerifyStoreAsync(fixture.Restaurant, Times.Once);
        }

        [Fact]
        public async Task HandleAsync_ThreeDishes_CurrentHasIndex1_ChangesDishOrderAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupRestaurantWithDishes(3);
            fixture.SetupCurrentDish(1);
            fixture.SetupRestaurantRepositoryFindingRestaurant();
            fixture.SetupRestaurantRepositoryStoringRestaurant();

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulCommand();

            // Act
            var result = await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsSuccess.Should().BeTrue();
                fixture.Dishes[0].OrderNo.Should().Be(0);
                fixture.Dishes[1].OrderNo.Should().Be(2);
                fixture.Dishes[2].OrderNo.Should().Be(1);
            }
        }

        [Fact]
        public async Task HandleAsync_ThreeDishes_CurrentHasIndex1_StoresRestaurant()
        {
            // Arrange
            fixture.SetupRestaurantWithDishes(3);
            fixture.SetupCurrentDish(1);
            fixture.SetupRestaurantRepositoryFindingRestaurant();

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulCommand();

            // Act
            await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            fixture.RestaurantRepositoryMock.VerifyStoreAsync(fixture.Restaurant, Times.Once);
        }

        [Fact]
        public async Task HandleAsync_ThreeDishes_CurrentHasIndex2_ChangesNothingAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupRestaurantWithDishes(3);
            fixture.SetupCurrentDish(2);
            fixture.SetupRestaurantRepositoryFindingRestaurant();
            fixture.SetupRestaurantRepositoryStoringRestaurant();

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulCommand();

            // Act
            var result = await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsSuccess.Should().BeTrue();
                fixture.Dishes[0].OrderNo.Should().Be(0);
                fixture.Dishes[1].OrderNo.Should().Be(1);
                fixture.Dishes[2].OrderNo.Should().Be(2);
            }
        }

        [Fact]
        public async Task HandleAsync_ThreeDishes_CurrentHasIndex2_StoresRestaurant()
        {
            // Arrange
            fixture.SetupRestaurantWithDishes(3);
            fixture.SetupCurrentDish(2);
            fixture.SetupRestaurantRepositoryFindingRestaurant();

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulCommand();

            // Act
            await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            fixture.RestaurantRepositoryMock.VerifyStoreAsync(fixture.Restaurant, Times.Once);
        }

        [Fact]
        public async Task HandleAsync_OneDish_ChangesNothingAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupRestaurantWithDishes(1);
            fixture.SetupCurrentDish(0);
            fixture.SetupRestaurantRepositoryFindingRestaurant();
            fixture.SetupRestaurantRepositoryStoringRestaurant();

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulCommand();

            // Act
            var result = await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsSuccess.Should().BeTrue();
                fixture.Dishes[0].OrderNo.Should().Be(0);
            }
        }

        [Fact]
        public async Task HandleAsync_OneDish_StoresRestaurant()
        {
            // Arrange
            fixture.SetupRestaurantWithDishes(1);
            fixture.SetupCurrentDish(0);
            fixture.SetupRestaurantRepositoryFindingRestaurant();
            fixture.SetupRestaurantRepositoryStoringRestaurant();

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulCommand();

            // Act
            var result = await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            fixture.RestaurantRepositoryMock.VerifyStoreAsync(fixture.Restaurant, Times.Once);
        }

        protected override
            CommandHandlerTestFixtureBase<IncOrderOfDishCommandHandler, IncOrderOfDishCommand, bool> FixtureBase
        {
            get { return fixture; }
        }

        private sealed class Fixture : CommandHandlerTestFixtureBase<IncOrderOfDishCommandHandler,
            IncOrderOfDishCommand, bool>
        {
            public Fixture(Role? minimumRole) : base(minimumRole)
            {
                RestaurantRepositoryMock = new RestaurantRepositoryMock(MockBehavior.Strict);
            }

            public RestaurantRepositoryMock RestaurantRepositoryMock { get; }

            public List<Dish> Dishes { get; private set; }

            public DishCategory DishCategory { get; private set; }

            public Restaurant Restaurant { get; private set; }

            public DishId CurrentDishId { get; private set; }

            public override IncOrderOfDishCommandHandler CreateTestObject()
            {
                return new IncOrderOfDishCommandHandler(
                    RestaurantRepositoryMock.Object
                );
            }

            public override IncOrderOfDishCommand CreateSuccessfulCommand()
            {
                return new IncOrderOfDishCommand(Restaurant.Id, DishCategory.Id, CurrentDishId);
            }

            public void SetupRestaurantWithDishes(int count)
            {
                Dishes = new List<Dish>();
                for (var i = 0; i < count; i++)
                {
                    Dishes.Add(new DishBuilder()
                        .WithOrderNo(i)
                        .WithValidConstrains()
                        .Create());
                }

                DishCategory = new DishCategoryBuilder()
                    .WithDishes(Dishes)
                    .Create();

                Restaurant = new RestaurantBuilder()
                    .WithDishCategories(new[] {DishCategory})
                    .Create();
            }

            public void SetupCurrentDish(int index)
            {
                CurrentDishId = Dishes[index].Id;
            }

            public void SetupRestaurantRepositoryFindingRestaurant()
            {
                RestaurantRepositoryMock.SetupFindByRestaurantIdAsync(Restaurant.Id)
                    .ReturnsAsync(Restaurant);
            }

            public void SetupRestaurantRepositoryStoringRestaurant()
            {
                RestaurantRepositoryMock.SetupStoreAsync(Restaurant)
                    .Returns(Task.CompletedTask);
            }

            public override void SetupForSuccessfulCommandExecution(Role? role)
            {
                SetupRestaurantWithDishes(3);
                SetupCurrentDish(1);
                SetupRestaurantRepositoryFindingRestaurant();
                SetupRestaurantRepositoryStoringRestaurant();
            }
        }
    }
}
