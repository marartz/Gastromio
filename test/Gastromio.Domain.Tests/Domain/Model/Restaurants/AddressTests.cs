using System;
using FluentAssertions;
using FluentAssertions.Execution;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Restaurants;
using Gastromio.Domain.TestKit.Common;
using Xunit;

namespace Gastromio.Domain.Tests.Domain.Model.Restaurants
{
    public class AddressTests
    {
        private readonly Fixture fixture;

        public AddressTests()
        {
            fixture = new Fixture();
        }

        [Fact]
        public void Ctor_StreetNull_ReturnsFailure()
        {
            // Arrange
            fixture.SetupStreetNull();
            fixture.SetupValidZipCode();
            fixture.SetupValidCity();

            // Act
            Action act = () => fixture.CreateTestObject();

            // Assert
            act.Should().Throw<DomainException<RestaurantStreetRequiredFailure>>();
        }

        [Fact]
        public void Ctor_StreetEmpty_ReturnsFailure()
        {
            // Arrange
            fixture.SetupStreetEmpty();
            fixture.SetupValidZipCode();
            fixture.SetupValidCity();

            // Act
            Action act = () => fixture.CreateTestObject();

            // Assert
            act.Should().Throw<DomainException<RestaurantStreetRequiredFailure>>();
        }

        [Fact]
        public void Ctor_StreetLength101_ReturnsFailure()
        {
            // Arrange
            fixture.SetupStreetLength101();
            fixture.SetupValidZipCode();
            fixture.SetupValidCity();

            // Act
            Action act = () => fixture.CreateTestObject();

            // Assert
            act.Should().Throw<DomainException<RestaurantStreetTooLongFailure>>();
        }

        [Fact]
        public void Ctor_StreetInvalid_ReturnsFailure()
        {
            // Arrange
            fixture.SetupStreetInvalid();
            fixture.SetupValidZipCode();
            fixture.SetupValidCity();

            // Act
            Action act = () => fixture.CreateTestObject();

            // Assert
            act.Should().Throw<DomainException<RestaurantStreetInvalidFailure>>();
        }

        [Fact]
        public void Ctor_ZipCodeNull_ReturnsFailure()
        {
            // Arrange
            fixture.SetupValidStreet();
            fixture.SetupZipCodeNull();
            fixture.SetupValidCity();

            // Act
            Action act = () => fixture.CreateTestObject();

            // Assert
            act.Should().Throw<DomainException<RestaurantZipCodeRequiredFailure>>();
        }

        [Fact]
        public void Ctor_ZipCodeEmpty_ReturnsFailure()
        {
            // Arrange
            fixture.SetupValidStreet();
            fixture.SetupZipCodeEmpty();
            fixture.SetupValidCity();

            // Act
            Action act = () => fixture.CreateTestObject();

            // Assert
            act.Should().Throw<DomainException<RestaurantZipCodeRequiredFailure>>();
        }

        [Fact]
        public void Ctor_ZipCodeLength6_ReturnsFailure()
        {
            // Arrange
            fixture.SetupValidStreet();
            fixture.SetupZipCodeLength6();
            fixture.SetupValidCity();

            // Act
            Action act = () => fixture.CreateTestObject();

            // Assert
            act.Should().Throw<DomainException<RestaurantZipCodeInvalidFailure>>();
        }

        [Fact]
        public void Ctor_ZipCodeInvalid_ReturnsFailure()
        {
            // Arrange
            fixture.SetupValidStreet();
            fixture.SetupZipCodeInvalid();
            fixture.SetupValidCity();

            // Act
            Action act = () => fixture.CreateTestObject();

            // Assert
            act.Should().Throw<DomainException<RestaurantZipCodeInvalidFailure>>();
        }

        [Fact]
        public void Ctor_CityNull_ReturnsFailure()
        {
            // Arrange
            fixture.SetupValidStreet();
            fixture.SetupValidZipCode();
            fixture.SetupCityNull();

            // Act
            Action act = () => fixture.CreateTestObject();

            // Assert
            act.Should().Throw<DomainException<RestaurantCityRequiredFailure>>();
        }

        [Fact]
        public void Ctor_CityEmpty_ReturnsFailure()
        {
            // Arrange
            fixture.SetupValidStreet();
            fixture.SetupValidZipCode();
            fixture.SetupCityEmpty();

            // Act
            Action act = () => fixture.CreateTestObject();

            // Assert
            act.Should().Throw<DomainException<RestaurantCityRequiredFailure>>();
        }

        [Fact]
        public void Ctor_CityLength51_ReturnsFailure()
        {
            // Arrange
            fixture.SetupValidStreet();
            fixture.SetupValidZipCode();
            fixture.SetupCityLength51();

            // Act
            Action act = () => fixture.CreateTestObject();

            // Assert
            act.Should().Throw<DomainException<RestaurantCityTooLongFailure>>();
        }

        [Fact]
        public void Ctor_AllValid_ReturnsFailure()
        {
            // Arrange
            fixture.SetupValidStreet();
            fixture.SetupValidZipCode();
            fixture.SetupValidCity();

            // Act
            var result = fixture.CreateTestObject();

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Street.Should().Be(fixture.Street);
                result.ZipCode.Should().Be(fixture.ZipCode);
                result.City.Should().Be(fixture.City);
            }
        }

        private sealed class Fixture
        {
            public string Street { get; private set; }

            public string ZipCode { get; private set; }

            public string City { get; private set; }

            public void SetupStreetNull()
            {
                Street = null;
            }

            public void SetupStreetEmpty()
            {
                Street = string.Empty;
            }

            public void SetupStreetLength101()
            {
                Street = RandomStringBuilder.BuildWithLength(101, "ABCDEFGHIJKLM".ToCharArray());
            }

            public void SetupStreetInvalid()
            {
                Street = RandomStringBuilder.BuildWithLength(100, "ABCDEFGHIJKLM".ToCharArray());
            }

            public void SetupValidStreet()
            {
                Street = "Musterstraße 1";
            }

            public void SetupZipCodeNull()
            {
                ZipCode = null;
            }

            public void SetupZipCodeEmpty()
            {
                ZipCode = string.Empty;
            }

            public void SetupZipCodeLength6()
            {
                ZipCode = "123456";
            }

            public void SetupZipCodeInvalid()
            {
                ZipCode = "1234A";
            }

            public void SetupValidZipCode()
            {
                ZipCode = "12345";
            }

            public void SetupCityNull()
            {
                City = null;
            }

            public void SetupCityEmpty()
            {
                City = string.Empty;
            }

            public void SetupCityLength51()
            {
                City = RandomStringBuilder.BuildWithLength(51, "ABCDEFGHIJKLM".ToCharArray());
            }

            public void SetupValidCity()
            {
                City = "Musterstraße 1";
            }

            public Address CreateTestObject()
            {
                return new Address(
                    Street,
                    ZipCode,
                    City
                );
            }
        }
    }
}
