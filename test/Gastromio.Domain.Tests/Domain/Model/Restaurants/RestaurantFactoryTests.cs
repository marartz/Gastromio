using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Execution;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Model.Cuisines;
using Gastromio.Core.Domain.Model.PaymentMethods;
using Gastromio.Core.Domain.Model.Restaurants;
using Gastromio.Core.Domain.Model.Users;
using Gastromio.Domain.TestKit.Common;
using Gastromio.Domain.TestKit.Domain.Model.Restaurants;
using Gastromio.Domain.TestKit.Domain.Model.Users;
using Xunit;

namespace Gastromio.Domain.Tests.Domain.Model.Restaurants
{
    public class RestaurantFactoryTests
    {
        private readonly Fixture fixture;

        public RestaurantFactoryTests()
        {
            fixture = new Fixture();
        }

        [Fact]
        public void Create_ValidParameters_CreatesRestaurant()
        {
            // Arrange
            fixture.SetupValidParameters();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = CallCreate(testObject);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Name.Should().Be(fixture.Name);
                result.Address.Should().BeEquivalentTo(fixture.Address);
                result.ContactInfo.Should().BeEquivalentTo(fixture.ContactInfo);
                result.RegularOpeningDays.Should().BeEquivalentTo(fixture.RegularOpeningDays);
                result.DeviatingOpeningDays.Should().BeEquivalentTo(fixture.DeviatingOpeningDays);
                result.PickupInfo.Should().BeEquivalentTo(fixture.PickupInfo);
                result.DeliveryInfo.Should().BeEquivalentTo(fixture.DeliveryInfo);
                result.ReservationInfo.Should().BeEquivalentTo(fixture.ReservationInfo);
                result.HygienicHandling.Should().Be(fixture.HygienicHandling);
                result.Cuisines.Should().BeEquivalentTo(fixture.Cuisines);
                result.PaymentMethods.Should().BeEquivalentTo(fixture.PaymentMethods);
                result.Administrators.Should().BeEquivalentTo(fixture.Administrators);
                result.CreatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                result.CreatedBy.Should().Be(fixture.CreatedBy);
                result.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                result.UpdatedBy.Should().Be(fixture.CreatedBy);
            }
        }

        [Fact]
        public void Create_ValidParametersExceptName_ReturnsFailure()
        {
            // Arrange
            fixture.SetupValidParametersExceptName();
            var testObject = fixture.CreateTestObject();

            // Act
            Action act = () => CallCreate(testObject);

            // Assert
            act.Should().Throw<DomainException>();
        }

        [Fact]
        public void Create_ValidParametersExceptAddress_ReturnsFailure()
        {
            // Arrange
            fixture.SetupValidParametersExceptAddress();
            var testObject = fixture.CreateTestObject();

            // Act
            Action act = () => CallCreate(testObject);

            // Assert
            act.Should().Throw<DomainException>();
        }

        [Fact]
        public void Create_ValidParametersExceptContactInfo_ReturnsFailure()
        {
            // Arrange
            fixture.SetupValidParametersExceptContactInfo();
            var testObject = fixture.CreateTestObject();

            // Act
            Action act = () => CallCreate(testObject);

            // Assert
            act.Should().Throw<DomainException>();
        }

        [Fact]
        public void Create_ValidParametersExceptRegularOpeningDays_ReturnsFailure()
        {
            // Arrange
            fixture.SetupValidParametersExceptRegularOpeningDays();
            var testObject = fixture.CreateTestObject();

            // Act
            Action act = () => CallCreate(testObject);

            // Assert
            act.Should().Throw<DomainException>();
        }

        [Fact]
        public void Create_ValidParametersExceptDeviatingOpeningDays_ReturnsFailure()
        {
            // Arrange
            fixture.SetupValidParametersExceptDeviatingOpeningDays();
            var testObject = fixture.CreateTestObject();

            // Act
            Action act = () => CallCreate(testObject);

            // Assert
            act.Should().Throw<DomainException>();
        }

        [Fact]
        public void Create_ValidParametersExceptPickupInfo_ReturnsFailure()
        {
            // Arrange
            fixture.SetupValidParametersExceptPickupInfo();
            var testObject = fixture.CreateTestObject();

            // Act
            Action act = () => CallCreate(testObject);

            // Assert
            act.Should().Throw<DomainException>();
        }

        [Fact]
        public void Create_ValidParametersExceptDeliveryInfo_ReturnsFailure()
        {
            // Arrange
            fixture.SetupValidParametersExceptDeliveryInfo();
            var testObject = fixture.CreateTestObject();

            // Act
            Action act = () => CallCreate(testObject);

            // Assert
            act.Should().Throw<DomainException>();
        }

        private Restaurant CallCreate(IRestaurantFactory testObject)
        {
            return testObject.Create(
                fixture.Name,
                fixture.Address,
                fixture.ContactInfo,
                fixture.RegularOpeningDays,
                fixture.DeviatingOpeningDays,
                fixture.PickupInfo,
                fixture.DeliveryInfo,
                fixture.ReservationInfo,
                fixture.HygienicHandling,
                fixture.Cuisines,
                fixture.PaymentMethods,
                fixture.Administrators,
                fixture.CreatedBy
            );
        }

        private sealed class Fixture
        {
            public string Name { get; private set; }

            public Address Address { get; private set; }

            public ContactInfo ContactInfo { get; private set; }

            public RegularOpeningDays RegularOpeningDays { get; private set; }

            public DeviatingOpeningDays DeviatingOpeningDays { get; private set; }

            public PickupInfo PickupInfo { get; private set; }

            public DeliveryInfo DeliveryInfo { get; private set; }

            public ReservationInfo ReservationInfo { get; private set; }

            public string HygienicHandling { get; private set; }

            public HashSet<CuisineId> Cuisines { get; private set; }

            public HashSet<PaymentMethodId> PaymentMethods { get; private set; }

            public HashSet<UserId> Administrators { get; private set; }

            public UserId CreatedBy { get; private set; }

            public void SetupValidParameters()
            {
                Name = RandomStringBuilder.BuildWithLength(20);
                Address = new AddressBuilder()
                    .WithValidConstrains()
                    .Create();
                ContactInfo = new ContactInfoBuilder()
                    .WithValidConstrains()
                    .Create();
                RegularOpeningDays = new RegularOpeningDays(
                    new RegularOpeningDayBuilder()
                        .WithDayOfWeek(0)
                        .WithOpeningPeriods(new[]
                        {
                            new OpeningPeriod(TimeSpan.FromHours(16), TimeSpan.FromHours(22))
                        })
                        .CreateMany(1)
                );
                DeviatingOpeningDays = new DeviatingOpeningDays(
                    new DeviatingOpeningDayBuilder()
                        .WithDate(Date.Today.AddDays(1))
                        .WithStatus(DeviatingOpeningDayStatus.Open)
                        .WithOpeningPeriods(new[]
                        {
                            new OpeningPeriod(TimeSpan.FromHours(16), TimeSpan.FromHours(22))
                        })
                        .CreateMany(1)
                );
                PickupInfo = new PickupInfoBuilder()
                    .WithValidConstrains()
                    .Create();
                DeliveryInfo = new DeliveryInfoBuilder()
                    .WithValidConstrains()
                    .Create();
                ReservationInfo = new ReservationInfoBuilder()
                    .Create();
                HygienicHandling = RandomStringBuilder.BuildWithLength(20);
                Cuisines = new HashSet<CuisineId> {new CuisineId(Guid.NewGuid())};
                PaymentMethods = new HashSet<PaymentMethodId> {new PaymentMethodId(Guid.NewGuid())};
                Administrators = new HashSet<UserId> {new UserId(Guid.NewGuid())};
                CreatedBy = new UserIdBuilder().Create();
            }

            public void SetupValidParametersExceptName()
            {
                SetupValidParameters();
                Name = RandomStringBuilder.BuildWithLength(101);
            }

            public void SetupValidParametersExceptAddress()
            {
                SetupValidParameters();
                Address = new AddressBuilder()
                    .WithZipCode("abcde")
                    .WithValidConstrains()
                    .Create();
            }

            public void SetupValidParametersExceptContactInfo()
            {
                SetupValidParameters();
                ContactInfo = new ContactInfoBuilder()
                    .WithPhone("abcdef")
                    .WithValidConstrains()
                    .Create();
            }

            public void SetupValidParametersExceptRegularOpeningDays()
            {
                SetupValidParameters();
                RegularOpeningDays = new RegularOpeningDays(new[]
                {
                    new RegularOpeningDayBuilder()
                        .WithDayOfWeek(0)
                        .WithOpeningPeriods(new[]
                        {
                            new OpeningPeriod(TimeSpan.FromHours(2), TimeSpan.FromHours(3))
                        })
                        .Create()
                });
            }

            public void SetupValidParametersExceptDeviatingOpeningDays()
            {
                SetupValidParameters();
                DeviatingOpeningDays = new DeviatingOpeningDays(new[]
                {
                    new DeviatingOpeningDayBuilder()
                        .WithDate(Date.Today.AddDays(1))
                        .WithStatus(DeviatingOpeningDayStatus.Open)
                        .WithOpeningPeriods(new[]
                        {
                            new OpeningPeriod(TimeSpan.FromHours(2), TimeSpan.FromHours(3))
                        })
                        .Create()
                });
            }

            public void SetupValidParametersExceptPickupInfo()
            {
                SetupValidParameters();
                PickupInfo = new PickupInfoBuilder()
                    .WithAverageTime(1)
                    .WithValidConstrains()
                    .Create();
            }

            public void SetupValidParametersExceptDeliveryInfo()
            {
                SetupValidParameters();
                DeliveryInfo = new DeliveryInfoBuilder()
                    .WithAverageTime(1)
                    .WithValidConstrains()
                    .Create();
            }

            public RestaurantFactory CreateTestObject()
            {
                return new RestaurantFactory();
            }
        }
    }
}
