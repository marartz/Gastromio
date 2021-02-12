using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using Gastromio.Core.Domain.Model.Restaurants;
using Gastromio.Core.Domain.Model.Users;
using Gastromio.Domain.TestKit.Common;
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
                .WithValidConstrains()
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
                .WithValidConstrains()
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
                .WithValidConstrains()
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
        public void ChangeAddress_StreetInvalid_ReturnsFailure()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            var invalidStreet = RandomStringBuilder.BuildWithLength(100, "ABCDEFGHIJKLM".ToCharArray());

            var address = new AddressBuilder()
                .WithStreet(invalidStreet)
                .WithValidConstrains()
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
        public void ChangeAddress_ZipCodeNull_ReturnsFailure()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            var address = new AddressBuilder()
                .WithZipCode(null)
                .WithValidConstrains()
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
        public void ChangeAddress_ZipCodeEmpty_ReturnsFailure()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            var address = new AddressBuilder()
                .WithZipCode("")
                .WithValidConstrains()
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
        public void ChangeAddress_ZipCodeLength6_ReturnsFailure()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            var address = new AddressBuilder()
                .WithZipCode("123456")
                .WithValidConstrains()
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
        public void ChangeAddress_ZipCodeInvalid_ReturnsFailure()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            var address = new AddressBuilder()
                .WithZipCode("1234A")
                .WithValidConstrains()
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
        public void ChangeAddress_AllValid_ChangesAddressAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            var address = new AddressBuilder()
                .WithValidConstrains()
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

        [Fact]
        public void ChangeContactInfo_PhoneNull_ReturnsFailure()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            var contactInfo = new ContactInfoBuilder()
                .WithPhone(null)
                .WithValidConstrains()
                .Create();

            // Act
            var result = testObject.ChangeContactInfo(contactInfo, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsFailure.Should().BeTrue();
            }
        }

        [Fact]
        public void ChangeContactInfo_PhoneEmpty_ReturnsFailure()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            var contactInfo = new ContactInfoBuilder()
                .WithPhone("")
                .WithValidConstrains()
                .Create();

            // Act
            var result = testObject.ChangeContactInfo(contactInfo, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsFailure.Should().BeTrue();
            }
        }

        [Fact]
        public void ChangeContactInfo_PhoneInvalid_ReturnsFailure()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            var contactInfo = new ContactInfoBuilder()
                .WithPhone("abcd")
                .WithValidConstrains()
                .Create();

            // Act
            var result = testObject.ChangeContactInfo(contactInfo, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsFailure.Should().BeTrue();
            }
        }

        [Fact]
        public void ChangeContactInfo_FaxNull_ChangesContactInfoAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            var contactInfo = new ContactInfoBuilder()
                .WithFax(null)
                .WithValidConstrains()
                .Create();

            // Act
            var result = testObject.ChangeContactInfo(contactInfo, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsSuccess.Should().BeTrue();
                testObject.ContactInfo.Should().BeEquivalentTo(contactInfo);
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        [Fact]
        public void ChangeContactInfo_FaxEmpty_ChangesContactInfoAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            var contactInfo = new ContactInfoBuilder()
                .WithFax("")
                .WithValidConstrains()
                .Create();

            // Act
            var result = testObject.ChangeContactInfo(contactInfo, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsSuccess.Should().BeTrue();
                testObject.ContactInfo.Should().BeEquivalentTo(contactInfo);
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        [Fact]
        public void ChangeContactInfo_FaxInvalid_ReturnsFailure()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            var contactInfo = new ContactInfoBuilder()
                .WithFax("abcd")
                .WithValidConstrains()
                .Create();

            // Act
            var result = testObject.ChangeContactInfo(contactInfo, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsFailure.Should().BeTrue();
            }
        }

        [Fact]
        public void ChangeContactInfo_WebSiteNull_ChangesContactInfoAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            var contactInfo = new ContactInfoBuilder()
                .WithWebSite(null)
                .WithValidConstrains()
                .Create();

            // Act
            var result = testObject.ChangeContactInfo(contactInfo, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsSuccess.Should().BeTrue();
                testObject.ContactInfo.Should().BeEquivalentTo(contactInfo);
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        [Fact]
        public void ChangeContactInfo_WebSiteEmpty_ChangesContactInfoAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            var contactInfo = new ContactInfoBuilder()
                .WithWebSite("")
                .WithValidConstrains()
                .Create();

            // Act
            var result = testObject.ChangeContactInfo(contactInfo, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsSuccess.Should().BeTrue();
                testObject.ContactInfo.Should().BeEquivalentTo(contactInfo);
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        [Fact]
        public void ChangeContactInfo_WebSiteInvalid_ReturnsFailure()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            var contactInfo = new ContactInfoBuilder()
                .WithWebSite("abcd")
                .WithValidConstrains()
                .Create();

            // Act
            var result = testObject.ChangeContactInfo(contactInfo, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsFailure.Should().BeTrue();
            }
        }

        [Fact]
        public void ChangeContactInfo_EmailAddressNull_ReturnsFailure()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            var contactInfo = new ContactInfoBuilder()
                .WithEmailAddress(null)
                .WithValidConstrains()
                .Create();

            // Act
            var result = testObject.ChangeContactInfo(contactInfo, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsFailure.Should().BeTrue();
            }
        }

        [Fact]
        public void ChangeContactInfo_EmailAddressEmpty_ReturnsFailure()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            var contactInfo = new ContactInfoBuilder()
                .WithEmailAddress("")
                .WithValidConstrains()
                .Create();

            // Act
            var result = testObject.ChangeContactInfo(contactInfo, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsFailure.Should().BeTrue();
            }
        }

        [Fact]
        public void ChangeContactInfo_EmailAddressInvalid_ReturnsFailure()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            var contactInfo = new ContactInfoBuilder()
                .WithEmailAddress("abcd")
                .WithValidConstrains()
                .Create();

            // Act
            var result = testObject.ChangeContactInfo(contactInfo, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsFailure.Should().BeTrue();
            }
        }

        [Fact]
        public void ChangeContactInfo_MobileNull_ChangesContactInfoAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            var contactInfo = new ContactInfoBuilder()
                .WithMobile(null)
                .WithValidConstrains()
                .Create();

            // Act
            var result = testObject.ChangeContactInfo(contactInfo, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsSuccess.Should().BeTrue();
                testObject.ContactInfo.Should().BeEquivalentTo(contactInfo);
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        [Fact]
        public void ChangeContactInfo_MobileEmpty_ChangesContactInfoAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            var contactInfo = new ContactInfoBuilder()
                .WithMobile("")
                .WithValidConstrains()
                .Create();

            // Act
            var result = testObject.ChangeContactInfo(contactInfo, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsSuccess.Should().BeTrue();
                testObject.ContactInfo.Should().BeEquivalentTo(contactInfo);
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        [Fact]
        public void ChangeContactInfo_MobileInvalid_ReturnsFailure()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            var contactInfo = new ContactInfoBuilder()
                .WithMobile("abcd")
                .WithValidConstrains()
                .Create();

            // Act
            var result = testObject.ChangeContactInfo(contactInfo, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsFailure.Should().BeTrue();
            }
        }

        [Fact]
        public void ChangeContactInfo_AllValid_ChangesContactInfoAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            var contactInfo = new ContactInfoBuilder()
                .WithValidConstrains()
                .Create();

            // Act
            var result = testObject.ChangeContactInfo(contactInfo, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsSuccess.Should().BeTrue();
                testObject.ContactInfo.Should().BeEquivalentTo(contactInfo);
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        [Fact]
        public void AddRegularOpeningPeriod_DayNotKnown_AddsDayAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupEmptyRegularOpeningDays();
            var testObject = fixture.CreateTestObject();

            var openingPeriod = new OpeningPeriodBuilder()
                .WithValidConstrains()
                .Create();

            // Act
            var result = testObject.AddRegularOpeningPeriod(0, openingPeriod, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsSuccess.Should().BeTrue();
                var openingDay = testObject.RegularOpeningDays.First().Value;
                openingDay.DayOfWeek.Should().Be(0);
                openingDay.OpeningPeriods.Should().BeEquivalentTo(openingPeriod);
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        private sealed class Fixture
        {
            public UserId ChangedBy { get; private set; }

            public List<RegularOpeningDay> RegularOpeningDays { get; private set; }

            public void SetupChangedBy()
            {
                ChangedBy = new UserIdBuilder().Create();
            }

            public void SetupEmptyRegularOpeningDays()
            {
                RegularOpeningDays = new List<RegularOpeningDay>();
            }

            public Restaurant CreateTestObject()
            {
                var builder = new RestaurantBuilder();

                if (RegularOpeningDays != null)
                    builder = builder.WithRegularOpeningDays(RegularOpeningDays);

                return builder
                    .WithValidConstrains()
                    .Create();
            }
        }
    }
}
