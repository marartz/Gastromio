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
            var expectedVariants = new List<DishVariant>
            {
                new DishVariant(new DishVariantId(Guid.NewGuid()), "Unit-Test", 5)
            };

            var command = fixture.CreateSuccessfulEditCommand(expectedName, expectedDescription, expectedInfo, expectedOrderNo, expectedVariants);

            // Act
            var result = await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.Should().Be(fixture.Dish.Id.Value);
                fixture.Restaurant.DishCategories.TryGetDish(fixture.Dish.Id, out var _, out var dish);
                dish.Should().BeEquivalentTo(new
                {
                    Name = expectedName,
                    Description = expectedDescription,
                    ProductInfo = expectedInfo,
                    OrderNo = expectedOrderNo,
                    Variants = expectedVariants
                }, opt => opt.ExcludingMissingMembers());
                fixture.RestaurantRepositoryMock.VerifyStoreAsync(fixture.Restaurant, Times.Once);
            }
        }

        [Fact]
        public async Task HandleAsync_ChangeDishName_ShouldFailWithoutGivenName()
        {
            // Arrange
            fixture.SetupForSuccessfulCommandExecution(fixture.MinimumRole);

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulEditCommand(string.Empty);

            // Act
            Func<Task> act = async () => await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DomainException<DishNameRequiredFailure>>();
        }

        [Fact]
        public async Task HandleAsync_ChangeDishName_ShouldFailWithToManyCharacters()
        {
            // Arrange
            fixture.SetupForSuccessfulCommandExecution(fixture.MinimumRole);

            var testObject = fixture.CreateTestObject();
            var expectedChange = new string('*', 101);
            var command = fixture.CreateSuccessfulEditCommand(expectedChange);

            // Act
            Func<Task> act = async () => await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DomainException<DishNameTooLongFailure>>();
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
            Func<Task> act = async () => await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DomainException<DishDescriptionTooLongFailure>>();
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
            Func<Task> act = async () => await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DomainException<DishProductInfoTooLongFailure>>();
        }

        [Fact]
        public async Task HandleAsync_ChangeDishOrderNumber_ShouldFailWithNegativeNumber()
        {
            // Arrange
            fixture.SetupForSuccessfulCommandExecution(fixture.MinimumRole);

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulEditCommand(null, null, null, -1);

            // Act
            Func<Task> act = async () => await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DomainException<DishInvalidOrderNoFailure>>();
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
            Func<Task> act = async () => await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            await act.Should().ThrowAsync<NullReferenceException>();
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
                    Dish.Id,
                    Dish.Name,
                    Dish.Description,
                    Dish.ProductInfo,
                    Dish.OrderNo,
                    Dish.Variants
                );
            }

            public AddOrChangeDishOfRestaurantCommand CreateSuccessfulEditCommand(string name = null, string desc = null, string info = null, int? order = null, IEnumerable<DishVariant> variants = null)
            {
                return new AddOrChangeDishOfRestaurantCommand(
                    Restaurant.Id,
                    DishCategory.Id,
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
