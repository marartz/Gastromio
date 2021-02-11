using System;
using FluentAssertions;
using FluentAssertions.Execution;
using Gastromio.Core.Domain.Model.Restaurants;
using Gastromio.Core.Domain.Model.Users;
using Gastromio.Domain.TestKit.Common;
using Gastromio.Domain.TestKit.Domain.Model.Cuisines;
using Gastromio.Domain.TestKit.Domain.Model.Restaurants;
using Gastromio.Domain.TestKit.Domain.Model.Users;
using Xunit;

namespace Gastromio.Domain.Tests.Domain.Model.Restaurants
{
    public class RestaurantTests
    {
        private readonly Fixture fixture;

        public RestaurantTests()
        {
            fixture = new Fixture();
        }

        [Fact]
        public void Test()
        {
            var cuisines1 = new CuisineBuilder()
                .CreateMany(3);

            var cuisines2 = new CuisineBuilder()
                .WithName("test")
                .CreateMany(3);

            var openingPeriod1 = new OpeningPeriodBuilder()
                .Create();

            var openingPeriod2 = new OpeningPeriodBuilder()
                .WithStart(TimeSpan.FromHours(8))
                .Create();

            var openingPeriod3 = new OpeningPeriodBuilder()
                .WithEnd(TimeSpan.FromHours(22))
                .Create();

            var openingPeriod4 = new OpeningPeriodBuilder()
                .WithStart(TimeSpan.FromHours(8))
                .WithEnd(TimeSpan.FromHours(22))
                .Create();
        }

        [Fact]
        public void ChangeName_NameNull_ReturnsFailure()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.ChangeName(null, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsFailure.Should().BeTrue();
            }
        }

        [Fact]
        public void ChangeName_NameEmpty_ReturnsFailure()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.ChangeName(string.Empty, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsFailure.Should().BeTrue();
            }
        }

        [Fact]
        public void ChangeName_NameLength101_ReturnsFailure()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            var name = RandomStringBuilder.BuildWithLength(101);

            // Act
            var result = testObject.ChangeName(name, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsFailure.Should().BeTrue();
            }
        }

        [Fact]
        public void ChangeName_NameLength100_ChangesNameAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            var name = RandomStringBuilder.BuildWithLength(100);

            // Act
            var result = testObject.ChangeName(name, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsSuccess.Should().BeTrue();
                testObject.Name.Should().Be(name);
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        [Fact]
        public void ChangeAddress_StreetNull_ReturnsFailure()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            var address = new AddressBuilder()
                .WithStreet(null)
                .Create();

            // Act
            var result = testObject.ChangeAddress(address, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsFailure.Should().BeTrue();
            }
        }

        [Fact]
        public void ChangeAddress_StreetEmpty_ReturnsFailure()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            var address = new AddressBuilder()
                .WithStreet("")
                .Create();

            // Act
            var result = testObject.ChangeAddress(address, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsFailure.Should().BeTrue();
            }
        }

        [Fact]
        public void ChangeAddress_StreetLength101_ReturnsFailure()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            var randomStreet = RandomStringBuilder.BuildWithLength(80, "ABCDEFGHIJKLM".ToCharArray());
            var randomHouseNumber = RandomStringBuilder.BuildWithLength(20, "0123456789".ToCharArray());

            var address = new AddressBuilder()
                .WithStreet($"{randomStreet} {randomHouseNumber}")
                .Create();

            // Act
            var result = testObject.ChangeAddress(address, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsFailure.Should().BeTrue();
            }
        }

        [Fact]
        public void ChangeAddress_StreetLength100_ChangesNameAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            var randomStreet = RandomStringBuilder.BuildWithLength(80, "ABCDEFGHIJKLM".ToCharArray());
            var randomHouseNumber = RandomStringBuilder.BuildWithLength(19, "0123456789".ToCharArray());

            var address = new AddressBuilder()
                .WithStreet($"{randomStreet} {randomHouseNumber}")
                .Create();

            // Act
            var result = testObject.ChangeAddress(address, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsSuccess.Should().BeTrue();
                testObject.Address.Should().BeEquivalentTo(address);
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        private sealed class Fixture
        {
            public UserId ChangedBy { get; private set; }

            public void SetupChangedBy()
            {
                ChangedBy = new UserIdBuilder().Create();
            }

            public Restaurant CreateTestObject()
            {
                return new RestaurantBuilder()
                    .Create();
            }
        }
    }
}
