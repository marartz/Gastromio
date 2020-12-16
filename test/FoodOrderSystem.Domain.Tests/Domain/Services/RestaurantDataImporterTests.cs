using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using FoodOrderSystem.Core.Application.Ports.Persistence;
using FoodOrderSystem.Core.Application.Services;
using FoodOrderSystem.Core.Common;
using FoodOrderSystem.Core.Domain.Model.Cuisine;
using FoodOrderSystem.Core.Domain.Model.PaymentMethod;
using FoodOrderSystem.Core.Domain.Model.Restaurant;
using FoodOrderSystem.Core.Domain.Model.User;
using FoodOrderSystem.Core.Domain.Services;
using Moq;
using Xunit;

namespace FoodOrderSystem.Domain.Tests.Domain.Services
{
    public class RestaurantDataImporterTests
    {
        private readonly Mock<IFailureMessageService> failureMessageService = new Mock<IFailureMessageService>();
        private readonly Mock<IRestaurantRepository> restaurantRepository = new Mock<IRestaurantRepository>();
        private readonly Mock<ICuisineRepository> cuisineRepository = new Mock<ICuisineRepository>();
        private readonly Mock<IUserRepository> userRepository = new Mock<IUserRepository>();

        private readonly RestaurantDataImporter target;

        private readonly UserId mockUserId;
        private readonly RestaurantId mockRestaurantId;
        
        private readonly RestaurantRow mockRestaurantRow = new RestaurantRow
        {
            ImportId = "123",
            Name = "Musterrestaurant",
            Street = "Musterstraße 1",
            ZipCode = "12345",
            City = "Musterstadt",
            Phone = "012345/67890",
            Fax = "012345/67891",
            WebSite = "https://www.musterrestaurant.de",
            ResponsiblePerson = "Max Mustermann",
            OrderEmailAddress = "bestellung@musterrestaurant.de",
            OpeningHoursMonday = "10:00-14:00",
            OpeningHoursTuesday = "10:00-14:00",
            OpeningHoursWednesday = "10:00-14:00",
            OpeningHoursThursday = "10:00-14:00",
            OpeningHoursFriday = "10:00-14:00",
            OpeningHoursSaturday = "10:00-14:00",
            OpeningHoursSunday = "10:00-14:00",
            DeviatingOpeningHours = "24.12.2020: 10:00-14:00",
            OrderTypes = "Abholung, Lieferung",
            AverageTime = TimeSpan.FromMinutes(40),
            MinimumOrderValuePickup = 4,
            MinimumOrderValueDelivery = 5,
            DeliveryCosts = 3,
            HygienicHandling = "Hygienische Abwicklung",
            Cuisines = "Italienisch",
            PaymentMethods = "Bar, EC-Karte",
            AdministratorUserEmailAddress = "max@mustermann.de",
            IsActive = true,
            SupportedOrderMode = "Telefonisch",
            ExternalMenuName = "Tageskarte",
            ExternalMenuDescription = "Aktuelle Gerichte in der Tageskarte",
            ExternalMenuUrl = "https://www.musterrestaurant.de/tageskarte"
        };


        private Restaurant storedRestaurant;
        private List<Cuisine> storedCuisines = new List<Cuisine>();
        private List<User> storedUsers = new List<User>();

        
        public RestaurantDataImporterTests()
        {
            mockUserId = new UserId(Guid.NewGuid());
            mockRestaurantId = new RestaurantId(Guid.NewGuid());

            failureMessageService
                .Setup(m => m.GetTranslatedMessage<bool>(It.IsAny<FailureResult<bool>>(),
                    It.IsAny<CultureInfo>()))
                .Returns((FailureResult<bool> failureResult, CultureInfo cultureInfo) =>
                {
                    var sb = new StringBuilder();
                    var first = true;
                    foreach (var (key, invariantErrors) in failureResult.Errors)
                    {
                        foreach (var invariantError in invariantErrors)
                        {
                            if (!first)
                                sb.Append(";");
                            sb.Append(invariantError.Code.ToString());
                            first = false;
                        }
                    }
                    return sb.ToString();
                });

            var restaurantFactory = new RestaurantFactory();

            restaurantRepository
                .Setup(m => m.StoreAsync(It.IsAny<Restaurant>(), It.IsAny<CancellationToken>()))
                .Callback((Restaurant restaurant, CancellationToken cancellationToken) =>
                {
                    storedRestaurant = restaurant;
                });
            
            var cuisineFactory = new CuisineFactory();

            cuisineRepository
                .Setup(m => m.StoreAsync(It.IsAny<Cuisine>(), It.IsAny<CancellationToken>()))
                .Callback((Cuisine cuisine, CancellationToken cancellationToken) =>
                {
                    storedCuisines.Add(cuisine);
                });

            var paymentMethodRepository = new PaymentMethodRepository();

            userRepository
                .Setup(m => m.StoreAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Callback((User user, CancellationToken cancellationToken) =>
                {
                    storedUsers.Add(user);
                });

            var userFactory = new UserFactory();
            
            target = new RestaurantDataImporter(
                failureMessageService.Object,
                restaurantFactory,
                restaurantRepository.Object,
                cuisineFactory,
                cuisineRepository.Object,
                paymentMethodRepository,
                userFactory,
                userRepository.Object
            );
        }

        [Fact]
        public async Task ImportRestaurantAsync_NoDryRun_NonExistent_AllValid_ImportsWithoutErrors()
        {
            // Arrange
            var log = new ImportLog();
            var rowIndex = 1;
 
            // Act
            await target.ImportRestaurantAsync(log, rowIndex, mockRestaurantRow, mockUserId, false);

            // Assert
            using (new AssertionScope())
            {
                log.Lines.Where(l => l.Type == ImportLogLineType.Error || l.Type == ImportLogLineType.Warning).Should()
                    .BeEmpty();
            }
        }
        
        [Fact]
        public async Task ImportRestaurantAsync_NoDryRun_NonExistent_ClosedDeviatingDay_ImportCorrectly()
        {
            // Arrange
            var log = new ImportLog();
            var rowIndex = 1;
            mockRestaurantRow.DeviatingOpeningHours = "24.12.2020: geschlossen";
            var date = new Date(2020, 12, 24);
 
            // Act
            await target.ImportRestaurantAsync(log, rowIndex, mockRestaurantRow, mockUserId, false);

            // Assert
            using (new AssertionScope())
            {
                storedRestaurant.Should().NotBeNull();
                storedRestaurant?.DeviatingOpeningPeriods.Should().NotBeNull();
                
                IReadOnlyCollection<DeviatingOpeningPeriod> deviatingOpeningPeriods = null;
                storedRestaurant?.DeviatingOpeningPeriods?.TryGetValue(date,
                    out deviatingOpeningPeriods);
                deviatingOpeningPeriods.Should().NotBeNull();
                deviatingOpeningPeriods.Should().BeEmpty();
            }
        }
        
        [Fact]
        public async Task ImportRestaurantAsync_NoDryRun_NonExistent_DeviatingDayWithOnePeriod_ImportCorrectly()
        {
            // Arrange
            var log = new ImportLog();
            var rowIndex = 1;
            mockRestaurantRow.DeviatingOpeningHours = "24.12.2020: 10:00-14:00";
            var date = new Date(2020, 12, 24);
 
            // Act
            await target.ImportRestaurantAsync(log, rowIndex, mockRestaurantRow, mockUserId, false);

            // Assert
            using (new AssertionScope())
            {
                storedRestaurant.Should().NotBeNull();
                storedRestaurant?.DeviatingOpeningPeriods.Should().NotBeNull();
                
                IReadOnlyCollection<DeviatingOpeningPeriod> deviatingOpeningPeriods = null;
                storedRestaurant?.DeviatingOpeningPeriods?.TryGetValue(date,
                    out deviatingOpeningPeriods);
                deviatingOpeningPeriods.Should().NotBeNull();
                deviatingOpeningPeriods.Should().BeEquivalentTo(
                    new DeviatingOpeningPeriod(date, TimeSpan.FromHours(10), TimeSpan.FromHours(14))
                );
            }
        }
        
        [Fact]
        public async Task ImportRestaurantAsync_NoDryRun_NonExistent_DeviatingDayWithTwoPeriods_ImportCorrectly()
        {
            // Arrange
            var log = new ImportLog();
            var rowIndex = 1;
            mockRestaurantRow.DeviatingOpeningHours = "24.12.2020: 10:00-14:00; 17:00-20:00";
            var date = new Date(2020, 12, 24);
 
            // Act
            await target.ImportRestaurantAsync(log, rowIndex, mockRestaurantRow, mockUserId, false);

            // Assert
            using (new AssertionScope())
            {
                storedRestaurant.Should().NotBeNull();
                storedRestaurant?.DeviatingOpeningPeriods.Should().NotBeNull();
                
                IReadOnlyCollection<DeviatingOpeningPeriod> deviatingOpeningPeriods = null;
                storedRestaurant?.DeviatingOpeningPeriods?.TryGetValue(date,
                    out deviatingOpeningPeriods);
                deviatingOpeningPeriods.Should().NotBeNull();
                deviatingOpeningPeriods.Should().BeEquivalentTo(
                    new DeviatingOpeningPeriod(date, TimeSpan.FromHours(10), TimeSpan.FromHours(14)),
                    new DeviatingOpeningPeriod(date, TimeSpan.FromHours(17), TimeSpan.FromHours(20))
                );
            }
        }
        
        [Fact]
        public async Task ImportRestaurantAsync_NoDryRun_NonExistent_TwoClosedDeviatingDays_ImportCorrectly()
        {
            // Arrange
            var log = new ImportLog();
            var rowIndex = 1;
            mockRestaurantRow.DeviatingOpeningHours = "24.12.2020: geschlossen | 25.12.2020: geschlossen";
            var date1 = new Date(2020, 12, 24);
            var date2 = new Date(2020, 12, 25);
 
            // Act
            await target.ImportRestaurantAsync(log, rowIndex, mockRestaurantRow, mockUserId, false);

            // Assert
            using (new AssertionScope())
            {
                storedRestaurant.Should().NotBeNull();
                storedRestaurant?.DeviatingOpeningPeriods.Should().NotBeNull();
                
                IReadOnlyCollection<DeviatingOpeningPeriod> deviatingOpeningPeriods = null;
                storedRestaurant?.DeviatingOpeningPeriods?.TryGetValue(date1,
                    out deviatingOpeningPeriods);
                deviatingOpeningPeriods.Should().NotBeNull();
                deviatingOpeningPeriods.Should().BeEmpty();
                
                deviatingOpeningPeriods = null;
                storedRestaurant?.DeviatingOpeningPeriods?.TryGetValue(date2,
                    out deviatingOpeningPeriods);
                deviatingOpeningPeriods.Should().NotBeNull();
                deviatingOpeningPeriods.Should().BeEmpty();
            }
        }
        
        [Fact]
        public async Task ImportRestaurantAsync_NoDryRun_NonExistent_TwoDeviatingDaysWithTwoPeriods_ImportCorrectly()
        {
            // Arrange
            var log = new ImportLog();
            var rowIndex = 1;
            mockRestaurantRow.DeviatingOpeningHours = "24.12.2020: 18:00-20:00; 22:00-02:00 | 25.12.2020: 18:00-20:00; 22:00-02:00";
            var date1 = new Date(2020, 12, 24);
            var date2 = new Date(2020, 12, 25);
 
            // Act
            await target.ImportRestaurantAsync(log, rowIndex, mockRestaurantRow, mockUserId, false);

            // Assert
            using (new AssertionScope())
            {
                storedRestaurant.Should().NotBeNull();
                storedRestaurant?.DeviatingOpeningPeriods.Should().NotBeNull();
                
                IReadOnlyCollection<DeviatingOpeningPeriod> deviatingOpeningPeriods = null;
                storedRestaurant?.DeviatingOpeningPeriods?.TryGetValue(date1,
                    out deviatingOpeningPeriods);
                deviatingOpeningPeriods.Should().NotBeNull();
                deviatingOpeningPeriods.Should().BeEquivalentTo(
                    new DeviatingOpeningPeriod(date1, TimeSpan.FromHours(18), TimeSpan.FromHours(20)),
                    new DeviatingOpeningPeriod(date1, TimeSpan.FromHours(22), TimeSpan.FromHours(26))
                );
               
                deviatingOpeningPeriods = null;
                storedRestaurant?.DeviatingOpeningPeriods?.TryGetValue(date2,
                    out deviatingOpeningPeriods);
                deviatingOpeningPeriods.Should().NotBeNull();
                deviatingOpeningPeriods.Should().BeEquivalentTo(
                    new DeviatingOpeningPeriod(date2, TimeSpan.FromHours(18), TimeSpan.FromHours(20)),
                    new DeviatingOpeningPeriod(date2, TimeSpan.FromHours(22), TimeSpan.FromHours(26))
                );
            }
        }
        
    }
}