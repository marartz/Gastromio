using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Gastromio.Core.Application.Commands.AddRestaurant;
using Gastromio.Core.Application.DTOs;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Restaurants;
using Gastromio.Core.Domain.Model.Users;
using Gastromio.Domain.TestKit.Application.Ports.Persistence;
using Gastromio.Domain.TestKit.Domain.Model.Cuisines;
using Gastromio.Domain.TestKit.Domain.Model.PaymentMethods;
using Gastromio.Domain.TestKit.Domain.Model.Restaurants;
using Gastromio.Domain.TestKit.Domain.Model.Users;
using Moq;
using Xunit;

namespace Gastromio.Domain.Tests.Application.Commands.AddRestaurant
{
    public class AddRestaurantCommandHandlerTests : CommandHandlerTestBase<AddRestaurantCommandHandler,
        AddRestaurantCommand, RestaurantDTO>
    {
        private readonly Fixture fixture;

        public AddRestaurantCommandHandlerTests()
        {
            fixture = new Fixture(Role.SystemAdmin);
        }

        [Fact]
        public async Task HandleAsync_RestaurantCreationWithFailure_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupRandomRestaurantToCreate();
            fixture.SetupRestaurantRepositoryNotFindingRestaurantByName();
            fixture.SetupCuisineRepositoryFindingCuisines();
            fixture.SetupPaymentMethodRepositoryFindingPaymentMethods();
            fixture.SetupRestaurantFactoryNotCreatingRestaurant();

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulCommand();

            // Act
            Func<Task> act = async () => await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DomainException<RestaurantNameRequiredFailure>>();
        }

        [Fact]
        public async Task HandleAsync_RestaurantAlreadyKnown_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupRandomRestaurantToCreate();
            fixture.SetupRestaurantRepositoryFindingRestaurantByName();

            var testObject = fixture.CreateTestObject();
            var command = fixture.CreateSuccessfulCommand();

            // Act
            Func<Task> act = async () => await testObject.HandleAsync(command, fixture.UserWithMinimumRole, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DomainException<RestaurantAlreadyExistsFailure>>();
        }

        [Fact]
        public async Task HandleAsync_AllValid_CreatesRestaurant()
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
                fixture.RestaurantRepositoryMock.VerifyStoreAsync(fixture.Restaurant, Times.Once);
            }
        }

        protected override
            CommandHandlerTestFixtureBase<AddRestaurantCommandHandler, AddRestaurantCommand, RestaurantDTO> FixtureBase
        {
            get { return fixture; }
        }

        private sealed class Fixture : CommandHandlerTestFixtureBase<AddRestaurantCommandHandler,
            AddRestaurantCommand, RestaurantDTO>
        {
            public Fixture(Role? minimumRole) : base(minimumRole)
            {
                RestaurantFactoryMock = new RestaurantFactoryMock(MockBehavior.Strict);
                RestaurantRepositoryMock = new RestaurantRepositoryMock(MockBehavior.Strict);
                RestaurantImageRepositoryMock = new RestaurantImageRepositoryMock(MockBehavior.Strict);
                CuisineRepositoryMock = new CuisineRepositoryMock(MockBehavior.Strict);
                PaymentMethodRepositoryMock = new PaymentMethodRepositoryMock(MockBehavior.Strict);
                UserRepositoryMock = new UserRepositoryMock(MockBehavior.Strict);
            }

            public Restaurant Restaurant { get; private set; }

            public RestaurantFactoryMock RestaurantFactoryMock { get; }

            public RestaurantRepositoryMock RestaurantRepositoryMock { get; }

            public RestaurantImageRepositoryMock RestaurantImageRepositoryMock { get; }

            public CuisineRepositoryMock CuisineRepositoryMock { get; }

            public PaymentMethodRepositoryMock PaymentMethodRepositoryMock { get; }

            public UserRepositoryMock UserRepositoryMock { get; }

            public override AddRestaurantCommandHandler CreateTestObject()
            {
                return new AddRestaurantCommandHandler(
                    RestaurantFactoryMock.Object,
                    RestaurantRepositoryMock.Object,
                    RestaurantImageRepositoryMock.Object,
                    CuisineRepositoryMock.Object,
                    PaymentMethodRepositoryMock.Object,
                    UserRepositoryMock.Object
                );
            }

            public override AddRestaurantCommand CreateSuccessfulCommand()
            {
                return new AddRestaurantCommand("test");
            }

            public void SetupRandomRestaurantToCreate()
            {
                Restaurant = new RestaurantBuilder()
                    .WithName("test")
                    .WithValidConstrains()
                    .Create();
            }

            public void SetupRestaurantRepositoryFindingRestaurantByName()
            {
                var restaurant = new RestaurantBuilder()
                    .WithName("test")
                    .WithValidConstrains()
                    .Create();

                RestaurantRepositoryMock.SetupFindByRestaurantNameAsync("test")
                    .ReturnsAsync(new[] {restaurant});
            }

            public void SetupRestaurantRepositoryNotFindingRestaurantByName()
            {
                RestaurantRepositoryMock.SetupFindByRestaurantNameAsync("test")
                    .ReturnsAsync(Enumerable.Empty<Restaurant>());
            }

            public void SetupCuisineRepositoryFindingCuisines()
            {
                var cuisines = Restaurant.Cuisines.Select(
                    cuisineId => new CuisineBuilder().WithId(cuisineId).Create()
                );
                CuisineRepositoryMock.SetupFindAllAsync()
                    .ReturnsAsync(cuisines);
            }

            public void SetupPaymentMethodRepositoryFindingPaymentMethods()
            {
                var paymentMethods = Restaurant.PaymentMethods.Select(
                    paymentMethodId => new PaymentMethodBuilder().WithId(paymentMethodId).Create()
                );
                PaymentMethodRepositoryMock.SetupFindAllAsync()
                    .ReturnsAsync(paymentMethods);
            }

            public void SetupRestaurantRepositoryStoringRestaurant()
            {
                RestaurantRepositoryMock.Setup(m => m.StoreAsync(It.IsAny<Restaurant>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);
            }

            public void SetupRestaurantFactoryCreatingRestaurant()
            {
                RestaurantFactoryMock.SetupCreateWithName("test", UserWithMinimumRole.Id)
                    .Returns(Restaurant);
            }

            public void SetupRestaurantFactoryNotCreatingRestaurant()
            {
                RestaurantFactoryMock.SetupCreateWithName("test", UserWithMinimumRole.Id)
                    .Throws(DomainException.CreateFrom(new RestaurantNameRequiredFailure()));
            }

            public void SetupUserRepositoryFindingAdministrators()
            {
                var userIds = Restaurant.Administrators
                    .Union(new[] {Restaurant.CreatedBy, Restaurant.UpdatedBy})
                    .ToList();

                var users = userIds.Select(
                    userId => new UserBuilder()
                        .WithId(userId)
                        .WithEmail($"{userId}@gastromio.de")
                        .Create()
                );

                UserRepositoryMock.SetupFindByUserIdsAsync(userIds)
                    .ReturnsAsync(users);
            }

            public void SetupRestaurantImageRepositoryFindingTypes()
            {
                RestaurantImageRepositoryMock.SetupFindTypesByRestaurantIdsAsync(new[] {Restaurant.Id})
                    .ReturnsAsync(new Dictionary<RestaurantId, IEnumerable<string>>());
            }

            public override void SetupForSuccessfulCommandExecution(Role? role)
            {
                SetupRandomRestaurantToCreate();
                SetupRestaurantRepositoryNotFindingRestaurantByName();
                SetupCuisineRepositoryFindingCuisines();
                SetupPaymentMethodRepositoryFindingPaymentMethods();
                SetupRestaurantFactoryCreatingRestaurant();
                SetupRestaurantRepositoryStoringRestaurant();
                SetupUserRepositoryFindingAdministrators();
                SetupRestaurantImageRepositoryFindingTypes();
            }
        }
    }
}
