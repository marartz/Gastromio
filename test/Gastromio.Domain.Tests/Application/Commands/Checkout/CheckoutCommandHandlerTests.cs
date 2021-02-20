using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gastromio.Core.Application.Commands.Checkout;
using Gastromio.Core.Application.DTOs;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Model.DishCategories;
using Gastromio.Core.Domain.Model.Dishes;
using Gastromio.Core.Domain.Model.Orders;
using Gastromio.Core.Domain.Model.PaymentMethods;
using Gastromio.Core.Domain.Model.Restaurants;
using Gastromio.Core.Domain.Model.Users;
using Gastromio.Domain.TestKit.Application.Ports.Persistence;
using Gastromio.Domain.TestKit.Domain.Model.DishCategories;
using Gastromio.Domain.TestKit.Domain.Model.Dishes;
using Gastromio.Domain.TestKit.Domain.Model.PaymentMethods;
using Gastromio.Domain.TestKit.Domain.Model.Restaurants;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit.Abstractions;

namespace Gastromio.Domain.Tests.Application.Commands.Checkout
{
    public class CheckoutCommandHandlerTests : CommandHandlerTestBase<CheckoutCommandHandler, CheckoutCommand, OrderDTO>
    {
        private readonly Fixture fixture;

        public CheckoutCommandHandlerTests(ITestOutputHelper output)
        {
            fixture = new Fixture(output, null);
        }

        protected override CommandHandlerTestFixtureBase<CheckoutCommandHandler, CheckoutCommand, OrderDTO> FixtureBase
        {
            get { return fixture; }
        }

        private sealed class Fixture : CommandHandlerTestFixtureBase<CheckoutCommandHandler, CheckoutCommand, OrderDTO>
        {
            public Fixture(ITestOutputHelper output, Role? minimumRole) : base(minimumRole)
            {
                Logger = output.BuildLoggerFor<CheckoutCommandHandler>();
                RestaurantRepositoryMock = new RestaurantRepositoryMock(MockBehavior.Strict);
                DishRepositoryMock = new DishRepositoryMock(MockBehavior.Strict);
                PaymentMethodRepositoryMock = new PaymentMethodRepositoryMock(MockBehavior.Strict);
                OrderRepositoryMock = new OrderRepositoryMock(MockBehavior.Strict);
            }

            public ILogger<CheckoutCommandHandler> Logger { get; }

            public RestaurantRepositoryMock RestaurantRepositoryMock { get; }

            public DishRepositoryMock DishRepositoryMock { get; }

            public PaymentMethodRepositoryMock PaymentMethodRepositoryMock { get; }

            public OrderRepositoryMock OrderRepositoryMock { get; }

            public Restaurant Restaurant { get; private set; }

            public List<DishCategory> DishCategoriesOfRestaurant { get; private set; }

            public List<Dish> DishesOfRestaurant { get; private set; }

            public List<PaymentMethod> PaymentMethods { get; private set; }

            public DateTimeOffset ServiceTime { get; private set; }

            public List<CartDishInfoDTO> OrderedDishes { get; private set; }

            public CheckoutCommand SuccessfulCommand { get; private set; }

            public override CheckoutCommandHandler CreateTestObject()
            {
                return new CheckoutCommandHandler(
                    Logger,
                    RestaurantRepositoryMock.Object,
                    DishRepositoryMock.Object,
                    PaymentMethodRepositoryMock.Object,
                    OrderRepositoryMock.Object
                );
            }

            public override CheckoutCommand CreateSuccessfulCommand()
            {
                return SuccessfulCommand;
            }

            public void SetupRandomRestaurant()
            {
                var deliveryInfo = new DeliveryInfoBuilder()
                    .WithEnabled(true)
                    .WithMinimumOrderValue(0)
                    .WithMaximumOrderValue(200)
                    .WithValidConstrains()
                    .Create();

                var regularOpeningDays = new List<RegularOpeningDay>();
                for (var day = 0; day < 7; day++)
                {
                    regularOpeningDays.Add(new RegularOpeningDay(
                        day,
                        new []{new OpeningPeriod(TimeSpan.FromHours(16), TimeSpan.FromHours(22))}
                    ));
                }

                Restaurant = new RestaurantBuilder()
                    .WithIsActive(true)
                    .WithSupportedOrderMode(SupportedOrderMode.Anytime)
                    .WithRegularOpeningDays(regularOpeningDays)
                    .WithDeliveryInfo(deliveryInfo)
                    .WithValidConstrains()
                    .Create();
            }

            public void SetupRandomDishCategories()
            {
                DishCategoriesOfRestaurant = new DishCategoryBuilder()
                    .WithRestaurantId(Restaurant.Id)
                    .WithValidConstrains()
                    .CreateMany(3).ToList();
            }

            public void SetupRandomDishes()
            {
                DishesOfRestaurant = new List<Dish>();
                foreach (var dishCategory in DishCategoriesOfRestaurant)
                {
                    var dishes = new DishBuilder()
                        .WithRestaurantId(Restaurant.Id)
                        .WithCategoryId(dishCategory.Id)
                        .WithVariants(new List<DishVariant>
                        {
                            new DishVariantBuilder()
                                .WithPrice(1.23m)
                                .WithValidConstrains()
                                .Create()
                        })
                        .WithValidConstrains()
                        .CreateMany(3).ToList();
                    DishesOfRestaurant.AddRange(dishes);
                }
            }

            public void SetupPaymentMethods()
            {
                PaymentMethods = new List<PaymentMethod>
                {
                    new PaymentMethod(PaymentMethodId.Cash, "Bar", "Sie bezahlen in bar.", null)
                };
            }

            public void SetupServiceTime()
            {
                ServiceTime = Date.Today.AddDays(1).ToUtcDateTimeOffset().AddHours(18);
            }

            public void SetupOrderedDishes()
            {
                OrderedDishes = new List<CartDishInfoDTO>();
                foreach (var dish in DishesOfRestaurant)
                {
                    OrderedDishes.Add(new CartDishInfoDTO(
                        Guid.NewGuid(),
                        dish.Id,
                        dish.Variants.First().VariantId,
                        1,
                        "Standard"
                    ));
                }
            }

            public void SetupSuccessfulCommand()
            {
                SuccessfulCommand = new CheckoutCommand(
                    "Max",
                    "Mustermann",
                    "Musterstraße 1",
                    "4. Stock",
                    "12345",
                    "Musterstadt",
                    "+491234567890",
                    "max@mustermann.de",
                    OrderType.Delivery,
                    Restaurant.Id.Value.ToString(),
                    OrderedDishes,
                    "Bitte schnell!",
                    PaymentMethodId.Cash,
                    ServiceTime
                );
            }

            public void SetupRestaurantRepositoryFindingRestaurantById()
            {
                RestaurantRepositoryMock.SetupFindByRestaurantIdAsync(Restaurant.Id)
                    .ReturnsAsync(Restaurant);
            }

            public void SetupDishRepositoryFindingDishesById()
            {
                foreach (var dish in DishesOfRestaurant)
                {
                    DishRepositoryMock.SetupFindByDishIdAsync(dish.Id)
                        .ReturnsAsync(dish);
                }
            }

            public void SetupPaymentMethodRepositoryFindingPaymentMethods()
            {
                foreach (var paymentMethod in PaymentMethods)
                {
                    PaymentMethodRepositoryMock.SetupFindByPaymentMethodIdAsync(paymentMethod.Id)
                        .ReturnsAsync(paymentMethod);
                }
            }

            public void SetupOrderRepositoryStoringOrder()
            {
                OrderRepositoryMock.SetupStoreAsync()
                    .Returns(Task.CompletedTask);
            }

            public override void SetupForSuccessfulCommandExecution(Role? role)
            {
                SetupRandomRestaurant();
                SetupRandomDishCategories();
                SetupRandomDishes();
                SetupPaymentMethods();
                SetupServiceTime();
                SetupOrderedDishes();
                SetupSuccessfulCommand();
                SetupRestaurantRepositoryFindingRestaurantById();
                SetupDishRepositoryFindingDishesById();
                SetupPaymentMethodRepositoryFindingPaymentMethods();
                SetupOrderRepositoryStoringOrder();
            }
        }
    }
}
