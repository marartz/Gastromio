using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using Gastromio.Core.Common;
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
                testObject.RegularOpeningDays.TryGetValue(0, out var openingDay).Should().BeTrue();
                openingDay.Should().NotBeNull();
                openingDay?.DayOfWeek.Should().Be(0);
                openingDay?.OpeningPeriods.Should().BeEquivalentTo(openingPeriod);
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        [Fact]
        public void AddRegularOpeningPeriod_OtherDayKnown_AddsDayAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRegularOpeningDayOnMondayWithOneDefaultPeriod();
            var testObject = fixture.CreateTestObject();

            var openingPeriod = new OpeningPeriodBuilder()
                .WithValidConstrains()
                .Create();

            // Act
            var result = testObject.AddRegularOpeningPeriod(1, openingPeriod, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsSuccess.Should().BeTrue();
                testObject.RegularOpeningDays.TryGetValue(0, out var openingDayMonday).Should().BeTrue();
                openingDayMonday.Should().NotBeNull();
                openingDayMonday?.DayOfWeek.Should().Be(0);
                openingDayMonday?.OpeningPeriods.Should().HaveCount(1);
                testObject.RegularOpeningDays.TryGetValue(1, out var openingDayTuesday).Should().BeTrue();
                openingDayTuesday.Should().NotBeNull();
                openingDayTuesday?.DayOfWeek.Should().Be(1);
                openingDayTuesday?.OpeningPeriods.Should().BeEquivalentTo(openingPeriod);
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        [Fact]
        public void AddRegularOpeningPeriod_DayKnownAndPeriodOverlaps_ReturnsFailure()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRegularOpeningDayOnMondayWithOneDefaultPeriod();
            var testObject = fixture.CreateTestObject();

            var openingPeriod = new OpeningPeriodBuilder()
                .WithStart(TimeSpan.FromHours(6))
                .WithEnd(TimeSpan.FromHours(23))
                .Create();

            // Act
            var result = testObject.AddRegularOpeningPeriod(0, openingPeriod, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsFailure.Should().BeTrue();
            }
        }

        [Fact]
        public void AddRegularOpeningPeriod_DayKnownAndPeriodDoesNotOverlap_AddsPeriodReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRegularOpeningDayOnMondayWithOneDefaultPeriod();
            var testObject = fixture.CreateTestObject();

            var openingPeriod = new OpeningPeriodBuilder()
                .WithStart(TimeSpan.FromHours(6))
                .WithEnd(TimeSpan.FromHours(7))
                .Create();

            var expectedPeriods = testObject.RegularOpeningDays.First().Value.OpeningPeriods
                .ToList();

            expectedPeriods.Add(openingPeriod);

            // Act
            var result = testObject.AddRegularOpeningPeriod(0, openingPeriod, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsSuccess.Should().BeTrue();
                testObject.RegularOpeningDays.TryGetValue(0, out var openingDay).Should().BeTrue();
                openingDay.Should().NotBeNull();
                openingDay?.DayOfWeek.Should().Be(0);
                openingDay?.OpeningPeriods.Should().BeEquivalentTo(expectedPeriods);
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        [Fact]
        public void RemoveRegularOpeningPeriod_DayNotKnown_DoesNothingAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupEmptyRegularOpeningDays();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.RemoveRegularOpeningPeriod(0, TimeSpan.FromHours(8), fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsSuccess.Should().BeTrue();
                testObject.UpdatedOn.Should().NotBeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().NotBe(fixture.ChangedBy);
            }
        }

        [Fact]
        public void RemoveRegularOpeningPeriod_OtherDayKnownWithOnePeriod_DoesNothingAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupEmptyRegularOpeningDays();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.RemoveRegularOpeningPeriod(1, TimeSpan.FromHours(8), fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsSuccess.Should().BeTrue();
                testObject.UpdatedOn.Should().NotBeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().NotBe(fixture.ChangedBy);
            }
        }

        [Fact]
        public void RemoveRegularOpeningPeriod_DayKnownWithOnePeriod_RemovesDayAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRegularOpeningDayOnMondayWithOneDefaultPeriod();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.RemoveRegularOpeningPeriod(0, TimeSpan.FromHours(16.5), fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsSuccess.Should().BeTrue();
                testObject.RegularOpeningDays.Should().HaveCount(0);
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        [Fact]
        public void RemoveRegularOpeningPeriod_DayKnownWithTwoPeriods_RemovesPeriodAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRegularOpeningDayOnMondayWithTwoPeriods();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.RemoveRegularOpeningPeriod(0, TimeSpan.FromHours(12), fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsSuccess.Should().BeTrue();
                testObject.RegularOpeningDays.TryGetValue(0, out var openingDay).Should().BeTrue();
                openingDay?.DayOfWeek.Should().Be(0);
                openingDay?.OpeningPeriods.Should()
                    .BeEquivalentTo(new OpeningPeriod(TimeSpan.FromHours(16), TimeSpan.FromHours(22)));
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        [Fact]
        public void AddDeviatingOpeningDay_DayNotKnown_AddsDayAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupDeviatingDayDate();
            fixture.SetupEmptyDeviatingOpeningDays();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.AddDeviatingOpeningDay(fixture.DeviatingDayDate, DeviatingOpeningDayStatus.Open,
                fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsSuccess.Should().BeTrue();
                testObject.DeviatingOpeningDays.TryGetValue(fixture.DeviatingDayDate, out var openingDay).Should()
                    .BeTrue();
                openingDay?.Date.Should().Be(fixture.DeviatingDayDate);
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        [Fact]
        public void AddDeviatingOpeningDay_DayKnown_DoesNotChangeAnythingAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupDeviatingDayDate();
            fixture.SetupDeviatingOpeningDayWithOneDefaultPeriod();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.AddDeviatingOpeningDay(fixture.DeviatingDayDate, DeviatingOpeningDayStatus.Open,
                fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsSuccess.Should().BeTrue();
                testObject.DeviatingOpeningDays.TryGetValue(fixture.DeviatingDayDate, out var openingDay).Should()
                    .BeTrue();
                openingDay?.Date.Should().Be(fixture.DeviatingDayDate);
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        [Fact]
        public void ChangeDeviatingOpeningDayStatus_DayNotKnown_ReturnsFailure()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupDeviatingDayDate();
            fixture.SetupEmptyDeviatingOpeningDays();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.ChangeDeviatingOpeningDayStatus(fixture.DeviatingDayDate,
                DeviatingOpeningDayStatus.Open,
                fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsFailure.Should().BeTrue();
            }
        }

        [Fact]
        public void ChangeDeviatingOpeningDayStatus_DayKnownAndHasPeriods_ReturnsFailure()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupDeviatingDayDate();
            fixture.SetupDeviatingOpeningDayWithOneDefaultPeriod();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.ChangeDeviatingOpeningDayStatus(fixture.DeviatingDayDate,
                DeviatingOpeningDayStatus.Open,
                fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsFailure.Should().BeTrue();
            }
        }

        [Fact]
        public void ChangeDeviatingOpeningDayStatus_DayKnown_ChangesStatusAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupDeviatingDayDate();
            fixture.SetupDeviatingOpeningDayWithoutPeriods();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.ChangeDeviatingOpeningDayStatus(fixture.DeviatingDayDate,
                DeviatingOpeningDayStatus.FullyBooked,
                fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsSuccess.Should().BeTrue();
                testObject.DeviatingOpeningDays.TryGetValue(fixture.DeviatingDayDate, out var openingDay).Should()
                    .BeTrue();
                openingDay?.Date.Should().Be(fixture.DeviatingDayDate);
                openingDay?.Status.Should().Be(DeviatingOpeningDayStatus.FullyBooked);
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        [Fact]
        public void RemoveDeviatingOpeningDay_DayNotKnown_DoesNotChangeAnythingAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupDeviatingDayDate();
            fixture.SetupEmptyDeviatingOpeningDays();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.RemoveDeviatingOpeningDay(fixture.DeviatingDayDate, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsSuccess.Should().BeTrue();
                testObject.DeviatingOpeningDays.Should().BeEmpty();
                testObject.UpdatedOn.Should().NotBeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().NotBe(fixture.ChangedBy);
            }
        }

        [Fact]
        public void RemoveDeviatingOpeningDay_DayKnown_RemovesDayAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupDeviatingDayDate();
            fixture.SetupDeviatingOpeningDayWithOneDefaultPeriod();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.RemoveDeviatingOpeningDay(fixture.DeviatingDayDate, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsSuccess.Should().BeTrue();
                testObject.DeviatingOpeningDays.Should().BeEmpty();
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        [Fact]
        public void AddDeviatingOpeningPeriod_DayNotKnown_ReturnsFailure()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupDeviatingDayDate();
            fixture.SetupEmptyDeviatingOpeningDays();
            var testObject = fixture.CreateTestObject();

            var openingPeriod = new OpeningPeriodBuilder()
                .WithValidConstrains()
                .Create();

            // Act
            var result =
                testObject.AddDeviatingOpeningPeriod(fixture.DeviatingDayDate, openingPeriod, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsFailure.Should().BeTrue();
            }
        }

        [Fact]
        public void AddDeviatingOpeningPeriod_DayKnownAndPeriodOverlaps_ReturnsFailure()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupDeviatingDayDate();
            fixture.SetupDeviatingOpeningDayWithOneDefaultPeriod();
            var testObject = fixture.CreateTestObject();

            var openingPeriod = new OpeningPeriodBuilder()
                .WithStart(TimeSpan.FromHours(6))
                .WithEnd(TimeSpan.FromHours(23))
                .Create();

            // Act
            var result =
                testObject.AddDeviatingOpeningPeriod(fixture.DeviatingDayDate, openingPeriod, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsFailure.Should().BeTrue();
            }
        }

        [Fact]
        public void AddDeviatingOpeningPeriod_DayKnownAndPeriodDoesNotOverlap_AddsPeriodReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupDeviatingDayDate();
            fixture.SetupDeviatingOpeningDayWithOneDefaultPeriod();
            var testObject = fixture.CreateTestObject();

            var openingPeriod = new OpeningPeriodBuilder()
                .WithStart(TimeSpan.FromHours(6))
                .WithEnd(TimeSpan.FromHours(7))
                .Create();

            var expectedPeriods = testObject.DeviatingOpeningDays.First().Value.OpeningPeriods
                .ToList();

            expectedPeriods.Add(openingPeriod);

            // Act
            var result =
                testObject.AddDeviatingOpeningPeriod(fixture.DeviatingDayDate, openingPeriod, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsSuccess.Should().BeTrue();
                testObject.DeviatingOpeningDays.TryGetValue(fixture.DeviatingDayDate, out var openingDay).Should()
                    .BeTrue();
                openingDay.Should().NotBeNull();
                openingDay?.Date.Should().Be(fixture.DeviatingDayDate);
                openingDay?.OpeningPeriods.Should().BeEquivalentTo(expectedPeriods);
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        [Fact]
        public void RemoveDeviatingOpeningPeriod_DayNotKnown_DoesNothingAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupDeviatingDayDate();
            fixture.SetupEmptyDeviatingOpeningDays();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.RemoveDeviatingOpeningPeriod(fixture.DeviatingDayDate, TimeSpan.FromHours(8),
                fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsSuccess.Should().BeTrue();
                testObject.UpdatedOn.Should().NotBeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().NotBe(fixture.ChangedBy);
            }
        }

        [Fact]
        public void RemoveDeviatingOpeningPeriod_OtherDayKnownWithOnePeriod_DoesNothingAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupDeviatingDayDate();
            fixture.SetupEmptyDeviatingOpeningDays();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.RemoveDeviatingOpeningPeriod(fixture.DeviatingDayDate.AddDays(1),
                TimeSpan.FromHours(8), fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsSuccess.Should().BeTrue();
                testObject.UpdatedOn.Should().NotBeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().NotBe(fixture.ChangedBy);
            }
        }

        [Fact]
        public void RemoveDeviatingOpeningPeriod_DayKnownWithOnePeriod_RemovesDayAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupDeviatingDayDate();
            fixture.SetupDeviatingOpeningDayWithOneDefaultPeriod();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.RemoveDeviatingOpeningPeriod(fixture.DeviatingDayDate, TimeSpan.FromHours(16.5),
                fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsSuccess.Should().BeTrue();
                testObject.DeviatingOpeningDays.TryGetValue(fixture.DeviatingDayDate, out var openingDay).Should()
                    .BeTrue();
                openingDay.Should().NotBeNull();
                openingDay?.Status.Should().Be(DeviatingOpeningDayStatus.Closed);
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        [Fact]
        public void RemoveDeviatingOpeningPeriod_DayKnownWithTwoPeriods_RemovesPeriodAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupDeviatingDayDate();
            fixture.SetupDeviatingOpeningDayWithTwoPeriods();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.RemoveDeviatingOpeningPeriod(fixture.DeviatingDayDate, TimeSpan.FromHours(12),
                fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsSuccess.Should().BeTrue();
                testObject.DeviatingOpeningDays.TryGetValue(fixture.DeviatingDayDate, out var openingDay).Should()
                    .BeTrue();
                openingDay?.Date.Should().Be(fixture.DeviatingDayDate);
                openingDay?.OpeningPeriods.Should()
                    .BeEquivalentTo(new OpeningPeriod(TimeSpan.FromHours(16), TimeSpan.FromHours(22)));
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        [Fact]
        public void RemoveAllOpeningDays_RemovesAllOpeningDays()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRegularOpeningDayOnMondayWithTwoPeriods();
            fixture.SetupDeviatingDayDate();
            fixture.SetupDeviatingOpeningDayWithTwoPeriods();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.RemoveAllOpeningDays(fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.IsSuccess.Should().BeTrue();
                testObject.RegularOpeningDays.Should().BeEmpty();
                testObject.DeviatingOpeningDays.Should().BeEmpty();
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        private sealed class Fixture
        {
            public UserId ChangedBy { get; private set; }

            public List<RegularOpeningDay> RegularOpeningDays { get; private set; }

            public Date DeviatingDayDate { get; private set; }

            public List<DeviatingOpeningDay> DeviatingOpeningDays { get; private set; }

            public void SetupChangedBy()
            {
                ChangedBy = new UserIdBuilder().Create();
            }

            public void SetupEmptyRegularOpeningDays()
            {
                RegularOpeningDays = new List<RegularOpeningDay>();
            }

            public void SetupRegularOpeningDayOnMondayWithOneDefaultPeriod()
            {
                RegularOpeningDays = new List<RegularOpeningDay>
                {
                    new RegularOpeningDayBuilder()
                        .WithDayOfWeek(0)
                        .WithOpeningPeriods(new OpeningPeriodBuilder().WithValidConstrains().CreateMany(1))
                        .Create()
                };
            }

            public void SetupRegularOpeningDayOnMondayWithTwoPeriods()
            {
                RegularOpeningDays = new List<RegularOpeningDay>
                {
                    new RegularOpeningDayBuilder()
                        .WithDayOfWeek(0)
                        .WithOpeningPeriods(new[]
                        {
                            new OpeningPeriodBuilder()
                                .WithStart(TimeSpan.FromHours(12))
                                .WithEnd(TimeSpan.FromHours(14))
                                .Create(),
                            new OpeningPeriodBuilder()
                                .WithStart(TimeSpan.FromHours(16))
                                .WithEnd(TimeSpan.FromHours(22))
                                .Create()
                        })
                        .Create()
                };
            }

            public void SetupEmptyDeviatingOpeningDays()
            {
                DeviatingOpeningDays = new List<DeviatingOpeningDay>();
            }

            public void SetupDeviatingDayDate()
            {
                DeviatingDayDate = new Date(2021, 1, 1);
            }

            public void SetupDeviatingOpeningDayWithoutPeriods()
            {
                DeviatingOpeningDays = new List<DeviatingOpeningDay>
                {
                    new DeviatingOpeningDayBuilder()
                        .WithDate(new Date(2021, 1, 1))
                        .WithStatus(DeviatingOpeningDayStatus.Open)
                        .WithoutOpeningPeriods()
                        .Create()
                };
            }

            public void SetupDeviatingOpeningDayWithOneDefaultPeriod()
            {
                DeviatingOpeningDays = new List<DeviatingOpeningDay>
                {
                    new DeviatingOpeningDayBuilder()
                        .WithDate(new Date(2021, 1, 1))
                        .WithStatus(DeviatingOpeningDayStatus.Open)
                        .WithOpeningPeriods(new OpeningPeriodBuilder().WithValidConstrains().CreateMany(1))
                        .Create()
                };
            }

            public void SetupDeviatingOpeningDayWithTwoPeriods()
            {
                DeviatingOpeningDays = new List<DeviatingOpeningDay>
                {
                    new DeviatingOpeningDayBuilder()
                        .WithDate(DeviatingDayDate)
                        .WithOpeningPeriods(new[]
                        {
                            new OpeningPeriodBuilder()
                                .WithStart(TimeSpan.FromHours(12))
                                .WithEnd(TimeSpan.FromHours(14))
                                .Create(),
                            new OpeningPeriodBuilder()
                                .WithStart(TimeSpan.FromHours(16))
                                .WithEnd(TimeSpan.FromHours(22))
                                .Create()
                        })
                        .Create()
                };
            }

            public Restaurant CreateTestObject()
            {
                var builder = new RestaurantBuilder();

                if (RegularOpeningDays != null)
                    builder = builder.WithRegularOpeningDays(RegularOpeningDays);

                if (DeviatingOpeningDays != null)
                    builder = builder.WithDeviatingOpeningDays(DeviatingOpeningDays);

                return builder
                    .WithValidConstrains()
                    .Create();
            }
        }
    }
}
