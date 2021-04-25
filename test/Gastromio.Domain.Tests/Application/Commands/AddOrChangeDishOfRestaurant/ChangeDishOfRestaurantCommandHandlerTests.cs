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
using Gastromio.Domain.TestKit.Application.Ports.Persistence;
using Gastromio.Domain.TestKit.Domain.Model.DishCategories;
using Gastromio.Domain.TestKit.Domain.Model.Dishes;
using Gastromio.Domain.TestKit.Domain.Model.Restaurants;
using Moq;
using Xunit;

namespace Gastromio.Domain.Tests.Application.Commands.AddOrChangeDishOfRestaurant
{
    public class ChangeDishOfRestaurantCommandHandlerTests : CommandHandlerTestBase<
        AddOrChangeDishOfRestaurantCommandHandler,
        AddOrChangeDishOfRestaurantCommand, Guid>
    {
        private readonly Fixture fixture;

        public ChangeDishOfRestaurantCommandHandlerTests()
        {
            fixture = new Fixture(Role.RestaurantAdmin);
        }

        [Fact]
        public async Task HandleAsync_AllValid_EditDishAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupForSuccessfulCommandExecution(fixture.MinimumRole);

            var testObject = fixture.CreateTestObject();
            var expectedName = "Unit-Test";
            var expectedDescription = "Changed in Unit-Test";
            var expectedInfo = "CommandHandlerTest";
            var expectedOrderNo = 2;
            var expectedVariants = new List<DishVariant> { new DishVariant(Guid.NewGuid(), "Unit-Test", 5) };
            var command = fixture.CreateSuccessfulEditCommand(expectedName, expectedDescription, expectedInfo, expectedOrderNo, expectedVariants);

            // Act
            var result = await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsSuccess.Should().BeTrue();
                fixture.Dish.Should().BeEquivalentTo(new
                {
                    Name = expectedName,
                    Description = expectedDescription,
                    ProductInfo = expectedInfo,
                    OrderNo = expectedOrderNo,
                    Variants = expectedVariants
                }, opt => opt.ExcludingMissingMembers());
            }
        }

        [Fact]
        public async Task HandleAsync_ChangeDishName_ShouldFailWithouGiventName()
        {
            // Arrange
            fixture.SetupForSuccessfulCommandExecution(fixture.MinimumRole);

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulEditCommand(string.Empty);

            // Act
            var result = await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            AssertFailure(result, FailureResultCode.DishNameRequired);
        }

        [Fact]
        public async Task HandleAsync_ChangeDishName_ShouldFailWithToManyCharacters()
        {
            // Arrange
            fixture.SetupForSuccessfulCommandExecution(fixture.MinimumRole);

            var testObject = fixture.CreateTestObject();
            var expectedChange = new string('*', 41);
            var command = fixture.CreateSuccessfulEditCommand(expectedChange);

            // Act
            var result = await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            AssertFailure(result, FailureResultCode.DishNameTooLong);
        }

        [Fact]
        public async Task HandleAsync_ChangeDishDescription_ShouldFailWithToManyCharacters()
        {
            // Arrange
            fixture.SetupForSuccessfulCommandExecution(fixture.MinimumRole);

            var testObject = fixture.CreateTestObject();
            var expectedChange = new string('*', 501);
            var command = fixture.CreateSuccessfulEditCommand(null, expectedChange);

            // Act
            var result = await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            AssertFailure(result, FailureResultCode.DishDescriptionTooLong);
        }

        [Fact]
        public async Task HandleAsync_ChangeDishInfo_ShouldFailWithToManyCharacters()
        {
            // Arrange
            fixture.SetupForSuccessfulCommandExecution(fixture.MinimumRole);

            var testObject = fixture.CreateTestObject();
            var expectedChange = new string('*', 501);
            var command = fixture.CreateSuccessfulEditCommand(null, null, expectedChange);

            // Act
            var result = await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            AssertFailure(result, FailureResultCode.DishProductInfoTooLong);
        }

        [Fact]
        public async Task HandleAsync_ChangeDishOrderNumber_ShouldFailWithNegativeNumber()
        {
            // Arrange
            fixture.SetupForSuccessfulCommandExecution(fixture.MinimumRole);

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulEditCommand(null, null, null, -1);

            // Act
            var result = await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            AssertFailure(result, FailureResultCode.DishInvalidOrderNo);
        }

        [Fact]
        public async Task HandleAsync_ChangeDishVariant_ShouldThrowWithoutVariants()
        {
            // Arrange
            fixture.SetupForSuccessfulCommandExecution(fixture.MinimumRole);

            var testObject = fixture.CreateTestObject();
            var expectedChange = new List<DishVariant> { null };
            var command = fixture.CreateSuccessfulEditCommand(null, null, null, null, expectedChange);

            // Act
            var result = await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            AssertFailure(result, FailureResultCode.DishVariantNameTooLong);
        }

        [Fact]
        public async Task HandleAsync_ChangeDishVariant_ShouldFailVariantNameTooLong()
        {
            // Arrange
            fixture.SetupForSuccessfulCommandExecution(fixture.MinimumRole);

            var testObject = fixture.CreateTestObject();
            var expectedChange = new List<DishVariant> { new DishVariant(Guid.NewGuid(), new string('*', 41), 5) };
            var command = fixture.CreateSuccessfulEditCommand(null, null, null, null, expectedChange);

            // Act
            var result = await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            AssertFailure(result, FailureResultCode.DishVariantNameTooLong);
        }

        [Fact]
        public async Task HandleAsync_ChangeDishVariant_ShouldFailWithNegativePrice()
        {
            // Arrange
            fixture.SetupForSuccessfulCommandExecution(fixture.MinimumRole);

            var testObject = fixture.CreateTestObject();
            var expectedChange = new List<DishVariant> { new DishVariant(Guid.NewGuid(), "Unit Test", -1) };
            var command = fixture.CreateSuccessfulEditCommand(null, null, null, null, expectedChange);

            // Act
            var result = await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            AssertFailure(result, FailureResultCode.DishVariantPriceIsNegativeOrZero);
        }

        [Fact]
        public async Task HandleAsync_ChangeDishVariant_ShouldFailPriceTooBig()
        {
            // Arrange
            fixture.SetupForSuccessfulCommandExecution(fixture.MinimumRole);

            var testObject = fixture.CreateTestObject();
            var expectedChange = new List<DishVariant> { new DishVariant(Guid.NewGuid(), "Unit Test", 201) };
            var command = fixture.CreateSuccessfulEditCommand(null, null, null, null, expectedChange);

            // Act
            var result = await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            AssertFailure(result, FailureResultCode.DishVariantPriceIsTooBig);
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

            public AddOrChangeDishOfRestaurantCommand CreateSuccessfulEditCommand(string name = null, string desc = null, string info = null, int? order = null, IEnumerable<DishVariant> variants = null)
            {
                return new AddOrChangeDishOfRestaurantCommand(
                    Restaurant.Id,
                    Dish.CategoryId,
                    Dish.Id,
                    name ?? Dish.Name,
                    desc ?? Dish.Description,
                    info ?? Dish.ProductInfo,
                    order.HasValue ? order.Value : Dish.OrderNo,
                    variants ?? Dish.Variants
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
                var variants = new DishVariantBuilder().WithValidConstrains().CreateMany(3);

                Dish = new DishBuilder()
                    .WithRestaurantId(Restaurant.Id)
                    .WithCategoryId(DishCategory.Id)
                    .WithOrderNo(0)
                    .WithCreatedBy(UserId)
                    .WithCreatedOn(DateTimeOffset.Now)
                    .WithUpdatedBy(UserId)
                    .WithUpdatedOn(DateTimeOffset.Now)
                    .WithVariants(variants)
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

            public void SetupDishRepositoryFindingDishForDishCategory()
            {
                DishRepositoryMock.SetupFindByDishCategoryIdAsync(DishCategory.Id)
                    .ReturnsAsync(new List<Dish> { Dish });
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
            public override AddOrChangeDishOfRestaurantCommand CreateSuccessfulCommand()
            {
                return new AddOrChangeDishOfRestaurantCommand(
                   Restaurant.Id,
                   Dish.CategoryId,
                   Dish.Id,
                   Dish.Name,
                   Dish.Description,
                   Dish.ProductInfo,
                   Dish.OrderNo,
                   Dish.Variants
               );
            }

            public override void SetupForSuccessfulCommandExecution(Role? role)
            {
                SetupRandomRestaurant(role);
                SetupRandomDishCategory();
                SetupRandomDish();
                SetupRestaurantRepositoryFindingRestaurant();
                SetupDishCategoryRepositoryFindingDishCategory();
                SetupDishRepositoryFindingDishForDishCategory();
                SetupDishFactoryCreatingDish();
                SetupDishRepositoryStoringDish();
            }

        }
    }
}
