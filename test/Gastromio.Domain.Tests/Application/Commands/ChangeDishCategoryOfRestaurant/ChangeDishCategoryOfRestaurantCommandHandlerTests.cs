using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Gastromio.Core.Application.Commands.ChangeDishCategoryOfRestaurant;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Restaurants;
using Gastromio.Core.Domain.Model.Users;
using Gastromio.Domain.TestKit.Application.Ports.Persistence;
using Gastromio.Domain.TestKit.Domain.Model.Restaurants;
using Moq;
using Xunit;

namespace Gastromio.Domain.Tests.Application.Commands.ChangeDishCategoryOfRestaurant
{
    public class ChangeDishCategoryOfRestaurantCommandHandlerTests : CommandHandlerTestBase<
        ChangeDishCategoryOfRestaurantCommandHandler, ChangeDishCategoryOfRestaurantCommand>
    {
        private readonly Fixture fixture;

        public ChangeDishCategoryOfRestaurantCommandHandlerTests()
        {
            fixture = new Fixture(Role.RestaurantAdmin);
        }

        [Fact]
        public async Task HandleAsync_RestaurantNotKnown_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupRandomDishCategory();
            fixture.SetupRandomRestaurantWithDishCategory(fixture.MinimumRole);
            fixture.SetupRestaurantRepositoryNotFindingRestaurant();

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulCommand();

            // Act
            Func<Task> act = async () => await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DomainException<RestaurantDoesNotExistFailure>>();
        }

        [Fact]
        public async Task HandleAsync_DishCategoryNotKnown_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupRandomDishCategory();
            fixture.SetupRandomRestaurantWithoutDishCategory(fixture.MinimumRole);
            fixture.SetupRestaurantRepositoryFindingRestaurant();

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulCommand();

            // Act
            Func<Task> act = async () => await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DomainException<DishCategoryDoesNotExistFailure>>();
        }

        [Fact]
        public async Task HandleAsync_AllValid_AddsDishCategoryToRestaurant()
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
                fixture.Restaurant.DishCategories.TryGetDishCategory(fixture.DishCategory.Id, out var dishCategory);
                dishCategory.Name.Should().Be("changed");
            }
        }

        protected override CommandHandlerTestFixtureBase<ChangeDishCategoryOfRestaurantCommandHandler,
            ChangeDishCategoryOfRestaurantCommand> FixtureBase
        {
            get { return fixture; }
        }

        private sealed class Fixture : CommandHandlerTestFixtureBase<ChangeDishCategoryOfRestaurantCommandHandler,
            ChangeDishCategoryOfRestaurantCommand>
        {
            public Fixture(Role? minimumRole) : base(minimumRole)
            {
                RestaurantRepositoryMock = new RestaurantRepositoryMock(MockBehavior.Strict);
            }

            public RestaurantRepositoryMock RestaurantRepositoryMock { get; }

            public Restaurant Restaurant { get; private set; }

            public DishCategory DishCategory { get; private set; }

            public override ChangeDishCategoryOfRestaurantCommandHandler CreateTestObject()
            {
                return new ChangeDishCategoryOfRestaurantCommandHandler(RestaurantRepositoryMock.Object);
            }

            public override ChangeDishCategoryOfRestaurantCommand CreateSuccessfulCommand()
            {
                return new ChangeDishCategoryOfRestaurantCommand(Restaurant.Id, DishCategory.Id, "changed");
            }

            public void SetupRandomRestaurantWithDishCategory(Role? role)
            {
                var builder = new RestaurantBuilder();

                if (role == Role.RestaurantAdmin)
                {
                    builder = builder
                        .WithAdministrators(new HashSet<UserId> {UserId});
                }

                Restaurant = builder
                    .WithDishCategories(new[] {DishCategory})
                    .WithValidConstrains()
                    .Create();
            }

            public void SetupRandomRestaurantWithoutDishCategory(Role? role)
            {
                var builder = new RestaurantBuilder();

                if (role == Role.RestaurantAdmin)
                {
                    builder = builder
                        .WithAdministrators(new HashSet<UserId> {UserId});
                }

                Restaurant = builder
                    .WithoutDishCategories()
                    .WithValidConstrains()
                    .Create();
            }

            public void SetupRandomDishCategory()
            {
                DishCategory = new DishCategoryBuilder()
                    .WithName("test")
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
                SetupRandomDishCategory();
                SetupRandomRestaurantWithDishCategory(role);
                SetupRestaurantRepositoryFindingRestaurant();
                SetupRestaurantRepositoryStoringRestaurant();
            }
        }
    }
}
