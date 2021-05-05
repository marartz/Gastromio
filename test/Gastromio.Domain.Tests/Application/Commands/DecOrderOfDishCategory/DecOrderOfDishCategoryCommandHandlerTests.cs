using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Gastromio.Core.Application.Commands.DecOrderOfDishCategory;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Restaurants;
using Gastromio.Core.Domain.Model.Users;
using Gastromio.Domain.TestKit.Application.Ports.Persistence;
using Gastromio.Domain.TestKit.Domain.Model.Restaurants;
using Moq;
using Xunit;

namespace Gastromio.Domain.Tests.Application.Commands.DecOrderOfDishCategory
{
    public class DecOrderOfDishCategoryCommandHandlerTests : CommandHandlerTestBase<
        DecOrderOfDishCategoryCommandHandler, DecOrderOfDishCategoryCommand, bool>
    {
        private readonly Fixture fixture;

        public DecOrderOfDishCategoryCommandHandlerTests()
        {
            fixture = new Fixture(Role.RestaurantAdmin);
        }

        [Fact]
        public async Task HandleAsync_DishNotKnown_ReturnsFailure()
        {
            // Arrange
            fixture.SetupRestaurantWithDishes(3);
            fixture.SetupCurrentDishCategory(1);
            fixture.SetupRestaurantRepositoryFindingRestaurant();

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulCommand();

            // Act
            Func<Task> act = async () => await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DomainException<RestaurantDoesNotExistFailure>>();
        }

        [Fact]
        public async Task HandleAsync_ThreeDishes_CurrentHasIndex0_ChangesNothingAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupRestaurantWithDishes(3);
            fixture.SetupCurrentDishCategory(0);
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
                fixture.DishCategories[0].OrderNo.Should().Be(0);
                fixture.DishCategories[1].OrderNo.Should().Be(1);
                fixture.DishCategories[2].OrderNo.Should().Be(2);
            }
        }

        [Fact]
        public async Task HandleAsync_ThreeDishes_CurrentHasIndex0_StoresRestaurant()
        {
            // Arrange
            fixture.SetupRestaurantWithDishes(3);
            fixture.SetupCurrentDishCategory(0);
            fixture.SetupRestaurantRepositoryFindingRestaurant();

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulCommand();

            // Act
            await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            fixture.RestaurantRepositoryMock.VerifyStoreAsync(fixture.Restaurant, Times.Once);
        }

        [Fact]
        public async Task HandleAsync_ThreeDishes_CurrentHasIndex1_ChangesDishCategoryOrderAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupRestaurantWithDishes(3);
            fixture.SetupCurrentDishCategory(1);
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
                fixture.DishCategories[0].OrderNo.Should().Be(1);
                fixture.DishCategories[1].OrderNo.Should().Be(0);
                fixture.DishCategories[2].OrderNo.Should().Be(2);
            }
        }

        [Fact]
        public async Task HandleAsync_ThreeDishes_CurrentHasIndex1_StoresRestaurant()
        {
            // Arrange
            fixture.SetupRestaurantWithDishes(3);
            fixture.SetupCurrentDishCategory(1);
            fixture.SetupRestaurantRepositoryFindingRestaurant();

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulCommand();

            // Act
            await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            fixture.RestaurantRepositoryMock.VerifyStoreAsync(fixture.Restaurant, Times.Once);
        }

        [Fact]
        public async Task HandleAsync_ThreeDishes_CurrentHasIndex2_ChangesDishCategoryOrderAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupRestaurantWithDishes(3);
            fixture.SetupCurrentDishCategory(2);
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
                fixture.DishCategories[0].OrderNo.Should().Be(0);
                fixture.DishCategories[1].OrderNo.Should().Be(2);
                fixture.DishCategories[2].OrderNo.Should().Be(1);
            }
        }

        [Fact]
        public async Task HandleAsync_ThreeDishes_CurrentHasIndex2_StoresRestaurant()
        {
            // Arrange
            fixture.SetupRestaurantWithDishes(3);
            fixture.SetupCurrentDishCategory(2);
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
            fixture.SetupCurrentDishCategory(0);
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
                fixture.DishCategories[0].OrderNo.Should().Be(0);
            }
        }

        [Fact]
        public async Task HandleAsync_OneDish_StoresRestaurant()
        {
            // Arrange
            fixture.SetupRestaurantWithDishes(1);
            fixture.SetupCurrentDishCategory(0);
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
            CommandHandlerTestFixtureBase<DecOrderOfDishCategoryCommandHandler, DecOrderOfDishCategoryCommand, bool> FixtureBase
        {
            get { return fixture; }
        }

        private sealed class Fixture : CommandHandlerTestFixtureBase<DecOrderOfDishCategoryCommandHandler,
            DecOrderOfDishCategoryCommand, bool>
        {
            public Fixture(Role? minimumRole) : base(minimumRole)
            {
                RestaurantRepositoryMock = new RestaurantRepositoryMock(MockBehavior.Strict);
            }

            public RestaurantRepositoryMock RestaurantRepositoryMock { get; }

            public List<DishCategory> DishCategories { get; private set; }

            public Restaurant Restaurant { get; private set; }

            public DishCategoryId CurrentDishCategoryId { get; private set; }

            public override DecOrderOfDishCategoryCommandHandler CreateTestObject()
            {
                return new DecOrderOfDishCategoryCommandHandler(
                    RestaurantRepositoryMock.Object
                );
            }

            public override DecOrderOfDishCategoryCommand CreateSuccessfulCommand()
            {
                return new DecOrderOfDishCategoryCommand(Restaurant.Id, CurrentDishCategoryId);
            }

            public void SetupRestaurantWithDishes(int count)
            {
                DishCategories = new List<DishCategory>();
                for (var i = 0; i < count; i++)
                {
                    DishCategories.Add(new DishCategoryBuilder()
                        .WithOrderNo(i)
                        .WithValidConstrains()
                        .Create());
                }

                Restaurant = new RestaurantBuilder()
                    .WithDishCategories(DishCategories)
                    .Create();
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
                SetupRestaurantWithDishes(3);
                SetupCurrentDishCategory(1);
                SetupRestaurantRepositoryFindingRestaurant();
                SetupRestaurantRepositoryStoringRestaurant();
            }
        }
    }
}
