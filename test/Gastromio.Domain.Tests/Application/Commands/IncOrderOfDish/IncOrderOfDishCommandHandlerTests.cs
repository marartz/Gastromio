using System;
using System.Collections.Generic;
using System.Linq;
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
        IncOrderOfDishCommandHandler, IncOrderOfDishCommand>
    {
        private readonly Fixture fixture;

        public IncOrderOfDishCommandHandlerTests()
        {
            fixture = new Fixture(Role.RestaurantAdmin);
        }

        [Fact]
        public async Task HandleAsync_DishNotKnown_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupRestaurantWithDishes(fixture.MinimumRole, 3);
            fixture.SetupCurrentDishToUnknown();
            fixture.SetupRestaurantRepositoryFindingRestaurant();

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulCommand();

            // Act
            Func<Task> act = async () => await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DomainException<DishDoesNotExistFailure>>();
        }

        [Fact]
        public async Task HandleAsync_ThreeDishes_CurrentHasIndex0_ChangesDishOrder()
        {
            // Arrange
            fixture.SetupRestaurantWithDishes(fixture.MinimumRole, 3);
            fixture.SetupCurrentDish(0);
            fixture.SetupRestaurantRepositoryFindingRestaurant();
            fixture.SetupRestaurantRepositoryStoringRestaurant();

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulCommand();

            // Act
            await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                fixture.Restaurant.DishCategories.TryGetDishCategory(fixture.DishCategory.Id, out var dishCategory);
                dishCategory.Should().NotBeNull();
                var dishes = dishCategory?.Dishes.OrderBy(dish => dish.OrderNo).ToList();
                dishes.Should().NotBeNull();
                dishes?[0].Id.Should().Be(fixture.Dishes[1].Id);
                dishes?[1].Id.Should().Be(fixture.Dishes[0].Id);
                dishes?[2].Id.Should().Be(fixture.Dishes[2].Id);
            }
        }

        [Fact]
        public async Task HandleAsync_ThreeDishes_CurrentHasIndex0_StoresRestaurant()
        {
            // Arrange
            fixture.SetupRestaurantWithDishes(fixture.MinimumRole, 3);
            fixture.SetupCurrentDish(0);
            fixture.SetupRestaurantRepositoryFindingRestaurant();
            fixture.SetupRestaurantRepositoryStoringRestaurant();

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulCommand();

            // Act
            await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            fixture.RestaurantRepositoryMock.VerifyStoreAsync(fixture.Restaurant, Times.Once);
        }

        [Fact]
        public async Task HandleAsync_ThreeDishes_CurrentHasIndex1_ChangesDishOrder()
        {
            // Arrange
            fixture.SetupRestaurantWithDishes(fixture.MinimumRole, 3);
            fixture.SetupCurrentDish(1);
            fixture.SetupRestaurantRepositoryFindingRestaurant();
            fixture.SetupRestaurantRepositoryStoringRestaurant();

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulCommand();

            // Act
            await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                fixture.Restaurant.DishCategories.TryGetDishCategory(fixture.DishCategory.Id, out var dishCategory);
                dishCategory.Should().NotBeNull();
                var dishes = dishCategory?.Dishes.OrderBy(dish => dish.OrderNo).ToList();
                dishes.Should().NotBeNull();
                dishes?[0].Id.Should().Be(fixture.Dishes[0].Id);
                dishes?[1].Id.Should().Be(fixture.Dishes[2].Id);
                dishes?[2].Id.Should().Be(fixture.Dishes[1].Id);
            }
        }

        [Fact]
        public async Task HandleAsync_ThreeDishes_CurrentHasIndex1_StoresRestaurant()
        {
            // Arrange
            fixture.SetupRestaurantWithDishes(fixture.MinimumRole, 3);
            fixture.SetupCurrentDish(1);
            fixture.SetupRestaurantRepositoryFindingRestaurant();
            fixture.SetupRestaurantRepositoryStoringRestaurant();

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulCommand();

            // Act
            await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            fixture.RestaurantRepositoryMock.VerifyStoreAsync(fixture.Restaurant, Times.Once);
        }

        [Fact]
        public async Task HandleAsync_ThreeDishes_CurrentHasIndex2_ChangesNothing()
        {
            // Arrange
            fixture.SetupRestaurantWithDishes(fixture.MinimumRole, 3);
            fixture.SetupCurrentDish(2);
            fixture.SetupRestaurantRepositoryFindingRestaurant();
            fixture.SetupRestaurantRepositoryStoringRestaurant();

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulCommand();

            // Act
            await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                fixture.Restaurant.DishCategories.TryGetDishCategory(fixture.DishCategory.Id, out var dishCategory);
                dishCategory.Should().NotBeNull();
                var dishes = dishCategory?.Dishes.OrderBy(dish => dish.OrderNo).ToList();
                dishes.Should().NotBeNull();
                dishes?[0].Id.Should().Be(fixture.Dishes[0].Id);
                dishes?[1].Id.Should().Be(fixture.Dishes[1].Id);
                dishes?[2].Id.Should().Be(fixture.Dishes[2].Id);
            }
        }

        [Fact]
        public async Task HandleAsync_ThreeDishes_CurrentHasIndex2_StoresRestaurant()
        {
            // Arrange
            fixture.SetupRestaurantWithDishes(fixture.MinimumRole, 3);
            fixture.SetupCurrentDish(2);
            fixture.SetupRestaurantRepositoryFindingRestaurant();
            fixture.SetupRestaurantRepositoryStoringRestaurant();

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulCommand();

            // Act
            await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            fixture.RestaurantRepositoryMock.VerifyStoreAsync(fixture.Restaurant, Times.Once);
        }

        [Fact]
        public async Task HandleAsync_OneDish_ChangesNothing()
        {
            // Arrange
            fixture.SetupRestaurantWithDishes(fixture.MinimumRole, 1);
            fixture.SetupCurrentDish(0);
            fixture.SetupRestaurantRepositoryFindingRestaurant();
            fixture.SetupRestaurantRepositoryStoringRestaurant();

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulCommand();

            // Act
            await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                fixture.Restaurant.DishCategories.TryGetDishCategory(fixture.DishCategory.Id, out var dishCategory);
                dishCategory.Should().NotBeNull();
                var dishes = dishCategory?.Dishes.OrderBy(dish => dish.OrderNo).ToList();
                dishes?[0].Id.Should().Be(fixture.Dishes[0].Id);
            }
        }

        [Fact]
        public async Task HandleAsync_OneDish_StoresRestaurant()
        {
            // Arrange
            fixture.SetupRestaurantWithDishes(fixture.MinimumRole, 1);
            fixture.SetupCurrentDish(0);
            fixture.SetupRestaurantRepositoryFindingRestaurant();
            fixture.SetupRestaurantRepositoryStoringRestaurant();

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulCommand();

            // Act
            await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            fixture.RestaurantRepositoryMock.VerifyStoreAsync(fixture.Restaurant, Times.Once);
        }

        protected override
            CommandHandlerTestFixtureBase<IncOrderOfDishCommandHandler, IncOrderOfDishCommand> FixtureBase
        {
            get { return fixture; }
        }

        private sealed class Fixture : CommandHandlerTestFixtureBase<IncOrderOfDishCommandHandler,
            IncOrderOfDishCommand>
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

            public void SetupRestaurantWithDishes(Role? role, int count)
            {
                Dishes = new List<Dish>();
                for (var i = 0; i < count; i++)
                {
                    Dishes.Add(new DishBuilder()
                        .WithName($"dish-{i + 1}")
                        .WithOrderNo(i)
                        .WithValidConstrains()
                        .Create());
                }

                DishCategory = new DishCategoryBuilder()
                    .WithDishes(Dishes)
                    .WithValidConstrains()
                    .Create();

                var restaurantBuilder = new RestaurantBuilder();

                if (role == Role.RestaurantAdmin)
                {
                    restaurantBuilder = restaurantBuilder
                        .WithAdministrators(new HashSet<UserId> {UserId});
                }

                Restaurant = restaurantBuilder
                    .WithDishCategories(new[] {DishCategory})
                    .WithValidConstrains()
                    .Create();
            }

            public void SetupCurrentDishToUnknown()
            {
                CurrentDishId = new DishId(Guid.NewGuid());
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
                SetupRestaurantWithDishes(MinimumRole, 3);
                SetupCurrentDish(1);
                SetupRestaurantRepositoryFindingRestaurant();
                SetupRestaurantRepositoryStoringRestaurant();
            }
        }
    }
}
