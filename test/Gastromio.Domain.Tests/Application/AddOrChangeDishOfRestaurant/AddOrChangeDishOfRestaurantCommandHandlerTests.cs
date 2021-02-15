using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Gastromio.Core.Application.Commands.AddOrChangeDishOfRestaurant;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Model.DishCategories;
using Gastromio.Core.Domain.Model.Dishes;
using Gastromio.Core.Domain.Model.Restaurants;
using Gastromio.Core.Domain.Model.Users;
using Gastromio.Domain.TestKit.Domain.Model.DishCategories;
using Gastromio.Domain.TestKit.Domain.Model.Dishes;
using Gastromio.Domain.TestKit.Domain.Model.Restaurants;
using Moq;
using Xunit;

namespace Gastromio.Domain.Tests.Application.AddOrChangeDishOfRestaurant
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
            fixture.SetupRandomRestaurant(fixture.MinimumRole);
            fixture.SetupRandomDishCategory();
            fixture.SetupRandomDish();
            fixture.SetupRestaurantRepositoryNotFindingRestaurant();

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulCommand();

            // Act
            var result = await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsFailure.Should().BeTrue();
            }
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
                fixture.DishRepositoryMock.VerifyStoreAsync(fixture.Dish, Times.Once);
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
                DishCategoryRepositoryMock = new DishCategoryRepositoryMock(MockBehavior.Strict);
                DishRepositoryMock = new DishRepositoryMock(MockBehavior.Strict);
                DishFactoryMock = new DishFactoryMock(MockBehavior.Strict);
            }

            public RestaurantRepositoryMock RestaurantRepositoryMock { get; }

            public DishCategoryRepositoryMock DishCategoryRepositoryMock { get; }

            public DishRepositoryMock DishRepositoryMock { get; }

            public DishFactoryMock DishFactoryMock { get; }

            public Restaurant Restaurant { get; private set; }

            public DishCategory DishCategory { get; private set; }

            public Dish Dish { get; private set; }

            public override AddOrChangeDishOfRestaurantCommandHandler CreateTestObject()
            {
                return new AddOrChangeDishOfRestaurantCommandHandler(
                    RestaurantRepositoryMock.Object,
                    DishCategoryRepositoryMock.Object,
                    DishRepositoryMock.Object,
                    DishFactoryMock.Object
                );
            }

            public override AddOrChangeDishOfRestaurantCommand CreateSuccessfulCommand()
            {
                return new AddOrChangeDishOfRestaurantCommand(
                    Restaurant.Id,
                    Dish.CategoryId,
                    Guid.Empty,
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
                    .WithValidConstrains()
                    .Create();
            }

            public void SetupRandomDishCategory()
            {
                DishCategory = new DishCategoryBuilder()
                    .WithRestaurantId(Restaurant.Id)
                    .WithOrderNo(0)
                    .WithCreatedBy(UserId)
                    .WithCreatedOn(DateTimeOffset.Now)
                    .WithUpdatedBy(UserId)
                    .WithUpdatedOn(DateTimeOffset.Now)
                    .Create();
            }

            public void SetupRandomDish()
            {
                Dish = new DishBuilder()
                    .WithRestaurantId(Restaurant.Id)
                    .WithCategoryId(DishCategory.Id)
                    .WithOrderNo(0)
                    .WithCreatedBy(UserId)
                    .WithCreatedOn(DateTimeOffset.Now)
                    .WithUpdatedBy(UserId)
                    .WithUpdatedOn(DateTimeOffset.Now)
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

            public void SetupDishCategoryRepositoryFindingDishCategory()
            {
                DishCategoryRepositoryMock.SetupFindByRestaurantIdAsync(Restaurant.Id)
                    .ReturnsAsync(new[] {DishCategory});
            }

            public void SetupDishRepositoryFindingNoDishForDishCategory()
            {
                DishRepositoryMock.SetupFindByDishCategoryIdAsync(DishCategory.Id)
                    .ReturnsAsync(Enumerable.Empty<Dish>());
            }

            public void SetupDishRepositoryStoringDish()
            {
                DishRepositoryMock.SetupStoreAsync(Dish)
                    .Returns(Task.CompletedTask);
            }

            public void SetupDishFactoryCreatingDish()
            {
                DishFactoryMock
                    .SetupCreate(
                        Restaurant.Id,
                        DishCategory.Id,
                        Dish.Name,
                        Dish.Description,
                        Dish.ProductInfo,
                        Dish.OrderNo,
                        Dish.Variants,
                        DishCategory.CreatedBy
                    )
                    .Returns(SuccessResult<Dish>.Create(Dish));
            }

            public override void SetupForSuccessfulCommandExecution(Role? role)
            {
                SetupRandomRestaurant(role);
                SetupRandomDishCategory();
                SetupRandomDish();
                SetupRestaurantRepositoryFindingRestaurant();
                SetupDishCategoryRepositoryFindingDishCategory();
                SetupDishRepositoryFindingNoDishForDishCategory();
                SetupDishFactoryCreatingDish();
                SetupDishRepositoryStoringDish();
            }
        }
    }
}
