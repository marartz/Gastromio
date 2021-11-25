using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Gastromio.Core.Application.Commands.IncOrderOfDishCategory;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Restaurants;
using Gastromio.Core.Domain.Model.Users;
using Gastromio.Domain.TestKit.Application.Ports.Persistence;
using Gastromio.Domain.TestKit.Domain.Model.Restaurants;
using Moq;
using Xunit;

namespace Gastromio.Domain.Tests.Application.Commands.IncOrderOfDishCategory
{
    public class IncOrderOfDishCategoryCommandHandlerTests : CommandHandlerTestBase<
        IncOrderOfDishCategoryCommandHandler, IncOrderOfDishCategoryCommand>
    {
        private readonly Fixture fixture;

        public IncOrderOfDishCategoryCommandHandlerTests()
        {
            fixture = new Fixture(Role.RestaurantAdmin);
        }

        [Fact]
        public async Task HandleAsync_DishNotKnown_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupRestaurantWithDishes(fixture.MinimumRole, 3);
            fixture.SetupCurrentDishCategoryToUnknown();
            fixture.SetupRestaurantRepositoryFindingRestaurant();

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulCommand();

            // Act
            Func<Task> act = async () => await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DomainException<DishCategoryDoesNotExistFailure>>();
        }

        [Fact]
        public async Task HandleAsync_ThreeDishes_CurrentHasIndex0_ChangesDishCategoryOrder()
        {
            // Arrange
            fixture.SetupRestaurantWithDishes(fixture.MinimumRole, 3);
            fixture.SetupCurrentDishCategory(0);
            fixture.SetupRestaurantRepositoryFindingRestaurant();
            fixture.SetupRestaurantRepositoryStoringRestaurant();

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulCommand();

            // Act
            await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                var dishCategories = fixture.Restaurant.DishCategories.OrderBy(dishCategory => dishCategory.OrderNo)
                    .ToList();
                dishCategories.Should().NotBeNull();
                dishCategories[0].Id.Should().Be(fixture.DishCategories[1].Id);
                dishCategories[1].Id.Should().Be(fixture.DishCategories[0].Id);
                dishCategories[2].Id.Should().Be(fixture.DishCategories[2].Id);
            }
        }

        [Fact]
        public async Task HandleAsync_ThreeDishes_CurrentHasIndex0_StoresRestaurant()
        {
            // Arrange
            fixture.SetupRestaurantWithDishes(fixture.MinimumRole, 3);
            fixture.SetupCurrentDishCategory(0);
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
        public async Task HandleAsync_ThreeDishes_CurrentHasIndex1_ChangesDishCategoryOrder()
        {
            // Arrange
            fixture.SetupRestaurantWithDishes(fixture.MinimumRole, 3);
            fixture.SetupCurrentDishCategory(1);
            fixture.SetupRestaurantRepositoryFindingRestaurant();
            fixture.SetupRestaurantRepositoryStoringRestaurant();

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulCommand();

            // Act
            await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                var dishCategories = fixture.Restaurant.DishCategories.OrderBy(dishCategory => dishCategory.OrderNo)
                    .ToList();
                dishCategories.Should().NotBeNull();
                dishCategories[0].Id.Should().Be(fixture.DishCategories[0].Id);
                dishCategories[1].Id.Should().Be(fixture.DishCategories[2].Id);
                dishCategories[2].Id.Should().Be(fixture.DishCategories[1].Id);
            }
        }

        [Fact]
        public async Task HandleAsync_ThreeDishes_CurrentHasIndex1_StoresRestaurant()
        {
            // Arrange
            fixture.SetupRestaurantWithDishes(fixture.MinimumRole, 3);
            fixture.SetupCurrentDishCategory(1);
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
            fixture.SetupCurrentDishCategory(2);
            fixture.SetupRestaurantRepositoryFindingRestaurant();
            fixture.SetupRestaurantRepositoryStoringRestaurant();

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulCommand();

            // Act
            await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                var dishCategories = fixture.Restaurant.DishCategories.OrderBy(dishCategory => dishCategory.OrderNo)
                    .ToList();
                dishCategories.Should().NotBeNull();
                dishCategories[0].Id.Should().Be(fixture.DishCategories[0].Id);
                dishCategories[1].Id.Should().Be(fixture.DishCategories[1].Id);
                dishCategories[2].Id.Should().Be(fixture.DishCategories[2].Id);
            }
        }

        [Fact]
        public async Task HandleAsync_ThreeDishes_CurrentHasIndex2_StoresRestaurant()
        {
            // Arrange
            fixture.SetupRestaurantWithDishes(fixture.MinimumRole, 3);
            fixture.SetupCurrentDishCategory(2);
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
            fixture.SetupCurrentDishCategory(0);
            fixture.SetupRestaurantRepositoryFindingRestaurant();
            fixture.SetupRestaurantRepositoryStoringRestaurant();

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulCommand();

            // Act
            await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                var dishCategories = fixture.Restaurant.DishCategories.OrderBy(dishCategory => dishCategory.OrderNo)
                    .ToList();
                dishCategories.Should().NotBeNull();
                dishCategories[0].Id.Should().Be(fixture.DishCategories[0].Id);
            }
        }

        [Fact]
        public async Task HandleAsync_OneDish_StoresRestaurant()
        {
            // Arrange
            fixture.SetupRestaurantWithDishes(fixture.MinimumRole, 1);
            fixture.SetupCurrentDishCategory(0);
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
            CommandHandlerTestFixtureBase<IncOrderOfDishCategoryCommandHandler, IncOrderOfDishCategoryCommand> FixtureBase
        {
            get { return fixture; }
        }

        private sealed class Fixture : CommandHandlerTestFixtureBase<IncOrderOfDishCategoryCommandHandler,
            IncOrderOfDishCategoryCommand>
        {
            public Fixture(Role? minimumRole) : base(minimumRole)
            {
                RestaurantRepositoryMock = new RestaurantRepositoryMock(MockBehavior.Strict);
            }

            public RestaurantRepositoryMock RestaurantRepositoryMock { get; }

            public List<DishCategory> DishCategories { get; private set; }

            public Restaurant Restaurant { get; private set; }

            public DishCategoryId CurrentDishCategoryId { get; private set; }

            public override IncOrderOfDishCategoryCommandHandler CreateTestObject()
            {
                return new IncOrderOfDishCategoryCommandHandler(
                    RestaurantRepositoryMock.Object
                );
            }

            public override IncOrderOfDishCategoryCommand CreateSuccessfulCommand()
            {
                return new IncOrderOfDishCategoryCommand(Restaurant.Id, CurrentDishCategoryId);
            }

            public void SetupRestaurantWithDishes(Role? role, int count)
            {
                DishCategories = new List<DishCategory>();
                for (var i = 0; i < count; i++)
                {
                    DishCategories.Add(new DishCategoryBuilder()
                        .WithName($"dish-category-{i + 1}")
                        .WithOrderNo(i)
                        .WithValidConstrains()
                        .Create());
                }

                var restaurantBuilder = new RestaurantBuilder();

                if (role == Role.RestaurantAdmin)
                {
                    restaurantBuilder = restaurantBuilder
                        .WithAdministrators(new HashSet<UserId> {UserId});
                }

                Restaurant = restaurantBuilder
                    .WithDishCategories(DishCategories)
                    .WithValidConstrains()
                    .Create();
            }

            public void SetupCurrentDishCategoryToUnknown()
            {
                CurrentDishCategoryId = new DishCategoryId(Guid.NewGuid());
            }

            public void SetupCurrentDishCategory(int index)
            {
                CurrentDishCategoryId = DishCategories[index].Id;
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
                SetupCurrentDishCategory(1);
                SetupRestaurantRepositoryFindingRestaurant();
                SetupRestaurantRepositoryStoringRestaurant();
            }
        }
    }
}
