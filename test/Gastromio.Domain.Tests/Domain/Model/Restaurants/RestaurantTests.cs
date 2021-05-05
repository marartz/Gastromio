using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Execution;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
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
            Action act = () => testObject.ChangeName(null, fixture.ChangedBy);

            // Assert
            act.Should().Throw<DomainException<RestaurantNameRequiredFailure>>();
        }

        [Fact]
        public void ChangeName_NameEmpty_ReturnsFailure()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            // Act
            Action act = () => testObject.ChangeName(string.Empty, fixture.ChangedBy);

            // Assert
            act.Should().Throw<DomainException<RestaurantNameRequiredFailure>>();
        }

        [Fact]
        public void ChangeName_NameLength101_ReturnsFailure()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            var name = RandomStringBuilder.BuildWithLength(101);

            // Act
            Action act = () => testObject.ChangeName(name, fixture.ChangedBy);

            // Assert
            act.Should().Throw<DomainException<RestaurantNameTooLongFailure>>();
        }

        [Fact]
        public void ChangeName_NameLength100_ChangesNameAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            var name = RandomStringBuilder.BuildWithLength(100);

            // Act
            testObject.ChangeName(name, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                testObject.Name.Should().Be(name);
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        [Fact]
        public void ChangeAddress_ChangesAddressAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            var address = new AddressBuilder()
                .WithValidConstrains()
                .Create();

            // Act
            testObject.ChangeAddress(address, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                testObject.Address.Should().BeEquivalentTo(address);
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        // [Fact]
        // public void AddRegularOpeningPeriod_DayNotKnown_AddsDayAndReturnsSuccess()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     fixture.SetupEmptyRegularOpeningDays();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var openingPeriod = new OpeningPeriodBuilder()
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     var result =
        //         testObject.AddRegularOpeningPeriod(0, openingPeriod, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsSuccess.Should().BeTrue();
        //         testObject.RegularOpeningDays.TryGetValue(0, out var openingDay).Should()
        //             .BeTrue();
        //         openingDay.Should().NotBeNull();
        //         openingDay?.DayOfWeek.Should().Be(0);
        //         openingDay?.OpeningPeriods.Should().BeEquivalentTo(openingPeriod);
        //         testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        //         testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
        //     }
        // }
        //
        // [Fact]
        // public void AddRegularOpeningPeriod_OtherDayKnown_AddsDayAndReturnsSuccess()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     fixture.SetupRegularOpeningDayNextMondayWithOneDefaultPeriod();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var openingPeriod = new OpeningPeriodBuilder()
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     var result =
        //         testObject.AddRegularOpeningPeriod(1, openingPeriod, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsSuccess.Should().BeTrue();
        //         testObject.RegularOpeningDays.TryGetValue(0, out var openingDayMonday)
        //             .Should().BeTrue();
        //         openingDayMonday.Should().NotBeNull();
        //         openingDayMonday?.DayOfWeek.Should().Be(0);
        //         openingDayMonday?.OpeningPeriods.Should().HaveCount(1);
        //         testObject.RegularOpeningDays.TryGetValue(1, out var openingDayTuesday)
        //             .Should().BeTrue();
        //         openingDayTuesday.Should().NotBeNull();
        //         openingDayTuesday?.DayOfWeek.Should().Be(1);
        //         openingDayTuesday?.OpeningPeriods.Should().BeEquivalentTo(openingPeriod);
        //         testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        //         testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
        //     }
        // }
        //
        // [Fact]
        // public void AddRegularOpeningPeriod_DayKnownAndPeriodOverlaps_ReturnsFailure()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     fixture.SetupRegularOpeningDayNextMondayWithOneDefaultPeriod();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var openingPeriod = new OpeningPeriodBuilder()
        //         .WithStart(TimeSpan.FromHours(6))
        //         .WithEnd(TimeSpan.FromHours(23))
        //         .Create();
        //
        //     // Act
        //     var result =
        //         testObject.AddRegularOpeningPeriod(0, openingPeriod, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsFailure.Should().BeTrue();
        //     }
        // }
        //
        // [Fact]
        // public void AddRegularOpeningPeriod_DayKnownAndPeriodDoesNotOverlap_AddsPeriodReturnsSuccess()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     fixture.SetupRegularOpeningDayNextMondayWithOneDefaultPeriod();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var openingPeriod = new OpeningPeriodBuilder()
        //         .WithStart(TimeSpan.FromHours(6))
        //         .WithEnd(TimeSpan.FromHours(7))
        //         .Create();
        //
        //     var expectedPeriods = testObject.RegularOpeningDays.First().Value.OpeningPeriods
        //         .ToList();
        //
        //     expectedPeriods.Add(openingPeriod);
        //
        //     // Act
        //     var result =
        //         testObject.AddRegularOpeningPeriod(0, openingPeriod, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsSuccess.Should().BeTrue();
        //         testObject.RegularOpeningDays.TryGetValue(0, out var openingDay).Should()
        //             .BeTrue();
        //         openingDay.Should().NotBeNull();
        //         openingDay?.DayOfWeek.Should().Be(0);
        //         openingDay?.OpeningPeriods.Should().BeEquivalentTo(expectedPeriods);
        //         testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        //         testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
        //     }
        // }
        //
        // [Fact]
        // public void RemoveRegularOpeningPeriod_DayNotKnown_DoesNothingAndReturnsSuccess()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     fixture.SetupEmptyRegularOpeningDays();
        //     var testObject = fixture.CreateTestObject();
        //
        //     // Act
        //     var result = testObject.RemoveRegularOpeningPeriod(0, TimeSpan.FromHours(8),
        //         fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsSuccess.Should().BeTrue();
        //         testObject.UpdatedOn.Should().NotBeCloseTo(DateTimeOffset.UtcNow, 1000);
        //         testObject.UpdatedBy.Should().NotBe(fixture.ChangedBy);
        //     }
        // }
        //
        // [Fact]
        // public void RemoveRegularOpeningPeriod_OtherDayKnownWithOnePeriod_DoesNothingAndReturnsSuccess()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     fixture.SetupEmptyRegularOpeningDays();
        //     var testObject = fixture.CreateTestObject();
        //
        //     // Act
        //     var result = testObject.RemoveRegularOpeningPeriod(1, TimeSpan.FromHours(8),
        //         fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsSuccess.Should().BeTrue();
        //         testObject.UpdatedOn.Should().NotBeCloseTo(DateTimeOffset.UtcNow, 1000);
        //         testObject.UpdatedBy.Should().NotBe(fixture.ChangedBy);
        //     }
        // }
        //
        // [Fact]
        // public void RemoveRegularOpeningPeriod_DayKnownWithOnePeriod_RemovesDayAndReturnsSuccess()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     fixture.SetupRegularOpeningDayNextMondayWithOneDefaultPeriod();
        //     var testObject = fixture.CreateTestObject();
        //
        //     // Act
        //     var result = testObject.RemoveRegularOpeningPeriod(0, TimeSpan.FromHours(16.5),
        //         fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsSuccess.Should().BeTrue();
        //         testObject.RegularOpeningDays.Should().HaveCount(0);
        //         testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        //         testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
        //     }
        // }
        //
        // [Fact]
        // public void RemoveRegularOpeningPeriod_DayKnownWithTwoPeriods_RemovesPeriodAndReturnsSuccess()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     fixture.SetupRegularOpeningDayNextMondayWithTwoPeriods();
        //     var testObject = fixture.CreateTestObject();
        //
        //     // Act
        //     var result = testObject.RemoveRegularOpeningPeriod(0, TimeSpan.FromHours(12),
        //         fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsSuccess.Should().BeTrue();
        //         testObject.RegularOpeningDays.TryGetValue(0, out var openingDay).Should()
        //             .BeTrue();
        //         openingDay?.DayOfWeek.Should().Be(0);
        //         openingDay?.OpeningPeriods.Should()
        //             .BeEquivalentTo(new OpeningPeriod(TimeSpan.FromHours(16), TimeSpan.FromHours(22)));
        //         testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        //         testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
        //     }
        // }
        //
        // [Fact]
        // public void AddDeviatingOpeningDay_DayNotKnown_AddsDayAndReturnsSuccess()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     fixture.SetupDeviatingDayDateOnMonday();
        //     fixture.SetupEmptyDeviatingOpeningDays();
        //     var testObject = fixture.CreateTestObject();
        //
        //     // Act
        //     var result = testObject.AddDeviatingOpeningDay(fixture.DeviatingDayDate, DeviatingOpeningDayStatus.Open,
        //         fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsSuccess.Should().BeTrue();
        //         testObject.DeviatingOpeningDays.TryGetValue(fixture.DeviatingDayDate, out var openingDay).Should()
        //             .BeTrue();
        //         openingDay?.Date.Should().Be(fixture.DeviatingDayDate);
        //         testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        //         testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
        //     }
        // }
        //
        // [Fact]
        // public void AddDeviatingOpeningDay_DayKnown_DoesNotChangeAnythingAndReturnsSuccess()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     fixture.SetupDeviatingDayDateOnMonday();
        //     fixture.SetupDeviatingOpeningDayWithOneDefaultPeriod();
        //     var testObject = fixture.CreateTestObject();
        //
        //     // Act
        //     var result = testObject.AddDeviatingOpeningDay(fixture.DeviatingDayDate, DeviatingOpeningDayStatus.Open,
        //         fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsSuccess.Should().BeTrue();
        //         testObject.DeviatingOpeningDays.TryGetValue(fixture.DeviatingDayDate, out var openingDay).Should()
        //             .BeTrue();
        //         openingDay?.Date.Should().Be(fixture.DeviatingDayDate);
        //         testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        //         testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
        //     }
        // }
        //
        // [Fact]
        // public void ChangeDeviatingOpeningDayStatus_DayNotKnown_ReturnsFailure()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     fixture.SetupDeviatingDayDateOnMonday();
        //     fixture.SetupEmptyDeviatingOpeningDays();
        //     var testObject = fixture.CreateTestObject();
        //
        //     // Act
        //     Action act = () => testObject.ChangeDeviatingOpeningDayStatus(fixture.DeviatingDayDate,
        //         DeviatingOpeningDayStatus.Open,
        //         fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsFailure.Should().BeTrue();
        //     }
        // }
        //
        // [Fact]
        // public void ChangeDeviatingOpeningDayStatus_DayKnownAndHasPeriods_ReturnsFailure()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     fixture.SetupDeviatingDayDateOnMonday();
        //     fixture.SetupDeviatingOpeningDayWithOneDefaultPeriod();
        //     var testObject = fixture.CreateTestObject();
        //
        //     // Act
        //     Action act = () => testObject.ChangeDeviatingOpeningDayStatus(fixture.DeviatingDayDate,
        //         DeviatingOpeningDayStatus.Open,
        //         fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsFailure.Should().BeTrue();
        //     }
        // }
        //
        // [Fact]
        // public void ChangeDeviatingOpeningDayStatus_DayKnown_ChangesStatusAndReturnsSuccess()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     fixture.SetupDeviatingDayDateOnMonday();
        //     fixture.SetupDeviatingOpeningDayWithoutPeriods();
        //     var testObject = fixture.CreateTestObject();
        //
        //     // Act
        //     var result = testObject.ChangeDeviatingOpeningDayStatus(fixture.DeviatingDayDate,
        //         DeviatingOpeningDayStatus.FullyBooked,
        //         fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsSuccess.Should().BeTrue();
        //         testObject.DeviatingOpeningDays.TryGetValue(fixture.DeviatingDayDate, out var openingDay).Should()
        //             .BeTrue();
        //         openingDay?.Date.Should().Be(fixture.DeviatingDayDate);
        //         openingDay?.Status.Should().Be(DeviatingOpeningDayStatus.FullyBooked);
        //         testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        //         testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
        //     }
        // }
        //
        // [Fact]
        // public void RemoveDeviatingOpeningDay_DayNotKnown_DoesNotChangeAnythingAndReturnsSuccess()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     fixture.SetupDeviatingDayDateOnMonday();
        //     fixture.SetupEmptyDeviatingOpeningDays();
        //     var testObject = fixture.CreateTestObject();
        //
        //     // Act
        //     var result = testObject.RemoveDeviatingOpeningDay(fixture.DeviatingDayDate, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsSuccess.Should().BeTrue();
        //         testObject.DeviatingOpeningDays.Should().BeEmpty();
        //         testObject.UpdatedOn.Should().NotBeCloseTo(DateTimeOffset.UtcNow, 1000);
        //         testObject.UpdatedBy.Should().NotBe(fixture.ChangedBy);
        //     }
        // }
        //
        // [Fact]
        // public void RemoveDeviatingOpeningDay_DayKnown_RemovesDayAndReturnsSuccess()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     fixture.SetupDeviatingDayDateOnMonday();
        //     fixture.SetupDeviatingOpeningDayWithOneDefaultPeriod();
        //     var testObject = fixture.CreateTestObject();
        //
        //     // Act
        //     var result = testObject.RemoveDeviatingOpeningDay(fixture.DeviatingDayDate, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsSuccess.Should().BeTrue();
        //         testObject.DeviatingOpeningDays.Should().BeEmpty();
        //         testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        //         testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
        //     }
        // }
        //
        // [Fact]
        // public void AddDeviatingOpeningPeriod_DayNotKnown_ReturnsFailure()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     fixture.SetupDeviatingDayDateOnMonday();
        //     fixture.SetupEmptyDeviatingOpeningDays();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var openingPeriod = new OpeningPeriodBuilder()
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     var result =
        //         testObject.AddDeviatingOpeningPeriod(fixture.DeviatingDayDate, openingPeriod, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsFailure.Should().BeTrue();
        //     }
        // }
        //
        // [Fact]
        // public void AddDeviatingOpeningPeriod_DayKnownAndPeriodOverlaps_ReturnsFailure()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     fixture.SetupDeviatingDayDateOnMonday();
        //     fixture.SetupDeviatingOpeningDayWithOneDefaultPeriod();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var openingPeriod = new OpeningPeriodBuilder()
        //         .WithStart(TimeSpan.FromHours(6))
        //         .WithEnd(TimeSpan.FromHours(23))
        //         .Create();
        //
        //     // Act
        //     var result =
        //         testObject.AddDeviatingOpeningPeriod(fixture.DeviatingDayDate, openingPeriod, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsFailure.Should().BeTrue();
        //     }
        // }
        //
        // [Fact]
        // public void AddDeviatingOpeningPeriod_DayKnownAndPeriodDoesNotOverlap_AddsPeriodReturnsSuccess()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     fixture.SetupDeviatingDayDateOnMonday();
        //     fixture.SetupDeviatingOpeningDayWithOneDefaultPeriod();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var openingPeriod = new OpeningPeriodBuilder()
        //         .WithStart(TimeSpan.FromHours(6))
        //         .WithEnd(TimeSpan.FromHours(7))
        //         .Create();
        //
        //     var expectedPeriods = testObject.DeviatingOpeningDays.First().Value.OpeningPeriods
        //         .ToList();
        //
        //     expectedPeriods.Add(openingPeriod);
        //
        //     // Act
        //     var result =
        //         testObject.AddDeviatingOpeningPeriod(fixture.DeviatingDayDate, openingPeriod, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsSuccess.Should().BeTrue();
        //         testObject.DeviatingOpeningDays.TryGetValue(fixture.DeviatingDayDate, out var openingDay).Should()
        //             .BeTrue();
        //         openingDay.Should().NotBeNull();
        //         openingDay?.Date.Should().Be(fixture.DeviatingDayDate);
        //         openingDay?.OpeningPeriods.Should().BeEquivalentTo(expectedPeriods);
        //         testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        //         testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
        //     }
        // }
        //
        // [Fact]
        // public void RemoveDeviatingOpeningPeriod_DayNotKnown_DoesNothingAndReturnsSuccess()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     fixture.SetupDeviatingDayDateOnMonday();
        //     fixture.SetupEmptyDeviatingOpeningDays();
        //     var testObject = fixture.CreateTestObject();
        //
        //     // Act
        //     var result = testObject.RemoveDeviatingOpeningPeriod(fixture.DeviatingDayDate, TimeSpan.FromHours(8),
        //         fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsSuccess.Should().BeTrue();
        //         testObject.UpdatedOn.Should().NotBeCloseTo(DateTimeOffset.UtcNow, 1000);
        //         testObject.UpdatedBy.Should().NotBe(fixture.ChangedBy);
        //     }
        // }
        //
        // [Fact]
        // public void RemoveDeviatingOpeningPeriod_OtherDayKnownWithOnePeriod_DoesNothingAndReturnsSuccess()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     fixture.SetupDeviatingDayDateOnMonday();
        //     fixture.SetupEmptyDeviatingOpeningDays();
        //     var testObject = fixture.CreateTestObject();
        //
        //     // Act
        //     var result = testObject.RemoveDeviatingOpeningPeriod(fixture.DeviatingDayDate.AddDays(1),
        //         TimeSpan.FromHours(8), fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsSuccess.Should().BeTrue();
        //         testObject.UpdatedOn.Should().NotBeCloseTo(DateTimeOffset.UtcNow, 1000);
        //         testObject.UpdatedBy.Should().NotBe(fixture.ChangedBy);
        //     }
        // }
        //
        // [Fact]
        // public void RemoveDeviatingOpeningPeriod_DayKnownWithOnePeriod_RemovesDayAndReturnsSuccess()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     fixture.SetupDeviatingDayDateOnMonday();
        //     fixture.SetupDeviatingOpeningDayWithOneDefaultPeriod();
        //     var testObject = fixture.CreateTestObject();
        //
        //     // Act
        //     var result = testObject.RemoveDeviatingOpeningPeriod(fixture.DeviatingDayDate, TimeSpan.FromHours(16.5),
        //         fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsSuccess.Should().BeTrue();
        //         testObject.DeviatingOpeningDays.TryGetValue(fixture.DeviatingDayDate, out var openingDay).Should()
        //             .BeTrue();
        //         openingDay.Should().NotBeNull();
        //         openingDay?.Status.Should().Be(DeviatingOpeningDayStatus.Closed);
        //         testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        //         testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
        //     }
        // }
        //
        // [Fact]
        // public void RemoveDeviatingOpeningPeriod_DayKnownWithTwoPeriods_RemovesPeriodAndReturnsSuccess()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     fixture.SetupDeviatingDayDateOnMonday();
        //     fixture.SetupDeviatingOpeningDayWithTwoPeriods();
        //     var testObject = fixture.CreateTestObject();
        //
        //     // Act
        //     var result = testObject.RemoveDeviatingOpeningPeriod(fixture.DeviatingDayDate, TimeSpan.FromHours(12),
        //         fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsSuccess.Should().BeTrue();
        //         testObject.DeviatingOpeningDays.TryGetValue(fixture.DeviatingDayDate, out var openingDay).Should()
        //             .BeTrue();
        //         openingDay?.Date.Should().Be(fixture.DeviatingDayDate);
        //         openingDay?.OpeningPeriods.Should()
        //             .BeEquivalentTo(new OpeningPeriod(TimeSpan.FromHours(16), TimeSpan.FromHours(22)));
        //         testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        //         testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
        //     }
        // }
        //
        // [Fact]
        // public void RemoveAllOpeningDays_RemovesAllOpeningDays()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     fixture.SetupRegularOpeningDayNextMondayWithTwoPeriods();
        //     fixture.SetupDeviatingDayDateOnMonday();
        //     fixture.SetupDeviatingOpeningDayWithTwoPeriods();
        //     var testObject = fixture.CreateTestObject();
        //
        //     // Act
        //     var result = testObject.RemoveAllOpeningDays(fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsSuccess.Should().BeTrue();
        //         testObject.RegularOpeningDays.Should().BeEmpty();
        //         testObject.DeviatingOpeningDays.Should().BeEmpty();
        //         testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        //         testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
        //     }
        // }

        [Fact]
        public void IsOrderPossible_OnlyPhone_ReturnsFalse()
        {
            // Arrange
            fixture.SetupOnlyPhoneOrderMode();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.IsOrderPossibleAt(DateTimeOffset.Now);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsOrderPossible_AtNextShift_NoRegular_NoDeviating_ReturnsFalse()
        {
            // Arrange
            fixture.SetupAtNextShiftOrderMode();
            fixture.SetupEmptyRegularOpeningDays();
            fixture.SetupEmptyDeviatingOpeningDays();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.IsOrderPossibleAt(DateTimeOffset.Now);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsOrderPossible_AtNextShift_OutsideEarlyRegular_OnDeviatingWithoutPeriods_ReturnsFalse()
        {
            // Arrange
            fixture.SetupAtNextShiftOrderMode();
            fixture.SetupRegularOpeningDayNextMondayWithOneEarlyPeriod();
            fixture.SetupDeviatingDayDateOnMonday();
            fixture.SetupDeviatingOpeningDayWithoutPeriods();
            var testObject = fixture.CreateTestObject();

            var orderDateTime = fixture.NextMonday.ToLocalDateTimeOffset().AddHours(18);

            // Act
            var result = testObject.IsOrderPossibleAt(orderDateTime);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsOrderPossible_AtNextShift_OutsideEarlyRegular_OnDeviatingJustBeforePeriodStart_ReturnsFalse()
        {
            // Arrange
            fixture.SetupAtNextShiftOrderMode();
            fixture.SetupRegularOpeningDayNextMondayWithOneEarlyPeriod();
            fixture.SetupDeviatingDayDateOnMonday();
            fixture.SetupDeviatingOpeningDayWithOneDefaultPeriod();
            var testObject = fixture.CreateTestObject();

            var orderDateTime = fixture.NextMonday.ToLocalDateTimeOffset().AddHours(16).AddMinutes(29);

            // Act
            var result = testObject.IsOrderPossibleAt(orderDateTime);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsOrderPossible_AtNextShift_OutsideEarlyRegular_OnDeviatingJustAfterPeriodEnd_ReturnsFalse()
        {
            // Arrange
            fixture.SetupAtNextShiftOrderMode();
            fixture.SetupRegularOpeningDayNextMondayWithOneEarlyPeriod();
            fixture.SetupDeviatingDayDateOnMonday();
            fixture.SetupDeviatingOpeningDayWithOneDefaultPeriod();
            var testObject = fixture.CreateTestObject();

            var orderDateTime = fixture.DeviatingDayDate.ToLocalDateTimeOffset().AddHours(22).AddMinutes(31);

            // Act
            var result = testObject.IsOrderPossibleAt(orderDateTime);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsOrderPossible_AtNextShift_InsideEarlyRegular_OnDeviatingDayButOutsidePeriod_ReturnsFalse()
        {
            // Arrange
            fixture.SetupAtNextShiftOrderMode();
            fixture.SetupRegularOpeningDayNextMondayWithOneEarlyPeriod();
            fixture.SetupDeviatingDayDateOnMonday();
            fixture.SetupDeviatingOpeningDayWithOneDefaultPeriod();
            var testObject = fixture.CreateTestObject();

            var orderDateTime = fixture.NextMonday.ToLocalDateTimeOffset().AddHours(6).AddMinutes(30);

            // Act
            var result = testObject.IsOrderPossibleAt(orderDateTime);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsOrderPossible_AtNextShift_JustBeforeRegularStart_NotOnDeviatingDay_ReturnsFalse()
        {
            // Arrange
            fixture.SetupAtNextShiftOrderMode();
            fixture.SetupRegularOpeningDayNextMondayWithOneEarlyPeriod();
            fixture.SetupDeviatingDayDateOnTuesday();
            fixture.SetupDeviatingOpeningDayWithOneDefaultPeriod();
            var testObject = fixture.CreateTestObject();

            var orderDateTime = fixture.NextMonday.ToLocalDateTimeOffset().AddHours(5).AddMinutes(59);

            // Act
            var result = testObject.IsOrderPossibleAt(orderDateTime);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsOrderPossible_AtNextShift_JustAfterRegularEnd_NotOnDeviatingDay_ReturnsFalse()
        {
            // Arrange
            fixture.SetupAtNextShiftOrderMode();
            fixture.SetupRegularOpeningDayNextMondayWithOneEarlyPeriod();
            fixture.SetupDeviatingDayDateOnTuesday();
            fixture.SetupDeviatingOpeningDayWithOneDefaultPeriod();
            var testObject = fixture.CreateTestObject();

            var orderDateTime = fixture.NextMonday.ToLocalDateTimeOffset().AddHours(7).AddMinutes(1);

            // Act
            var result = testObject.IsOrderPossibleAt(orderDateTime);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsOrderPossible_AtNextShift_NowClosed_OpenedInAnHour_ReturnsTrue()
        {
            // Arrange
            fixture.SetupAtNextShiftOrderMode();
            fixture.SetupRegularOpeningDayTodayWithPeriodInAnHourForRestOfDay();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.IsOrderPossibleAt(DateTimeOffset.Now.AddHours(1).AddMinutes(1));

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsOrderPossible_AtNextShift_NowOpenedFor2Min_OpenedInAnHour_ReturnsTrue()
        {
            // Arrange
            fixture.SetupAtNextShiftOrderMode();
            fixture.SetupRegularOpeningDayTodayWithPeriodNowFor2MinutesAndInAnHourForRestOfDay();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.IsOrderPossibleAt(DateTimeOffset.Now.AddHours(1).AddMinutes(1));

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsOrderPossible_AtNextShift_NowOpenedRestOfDay_ReturnsFalse()
        {
            // Arrange
            fixture.SetupAtNextShiftOrderMode();
            fixture.SetupRegularOpeningDayTodayWithPeriodNowForRestOfDay();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.IsOrderPossibleAt(DateTimeOffset.Now.AddHours(1).AddMinutes(1));

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsOrderPossible_Anytime_NoRegular_NoDeviating_ReturnsFalse()
        {
            // Arrange
            fixture.SetupAnytimeOrderMode();
            fixture.SetupEmptyRegularOpeningDays();
            fixture.SetupEmptyDeviatingOpeningDays();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.IsOrderPossibleAt(DateTimeOffset.Now);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsOrderPossible_Anytime_OutsideEarlyRegular_OnDeviatingWithoutPeriods_ReturnsFalse()
        {
            // Arrange
            fixture.SetupAnytimeOrderMode();
            fixture.SetupRegularOpeningDayNextMondayWithOneEarlyPeriod();
            fixture.SetupDeviatingDayDateOnMonday();
            fixture.SetupDeviatingOpeningDayWithoutPeriods();
            var testObject = fixture.CreateTestObject();

            var orderDateTime = fixture.NextMonday.ToLocalDateTimeOffset().AddHours(18);

            // Act
            var result = testObject.IsOrderPossibleAt(orderDateTime);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsOrderPossible_Anytime_OutsideEarlyRegular_OnDeviatingJustBeforePeriodStart_ReturnsFalse()
        {
            // Arrange
            fixture.SetupAnytimeOrderMode();
            fixture.SetupRegularOpeningDayNextMondayWithOneEarlyPeriod();
            fixture.SetupDeviatingDayDateOnMonday();
            fixture.SetupDeviatingOpeningDayWithOneDefaultPeriod();
            var testObject = fixture.CreateTestObject();

            var orderDateTime = fixture.NextMonday.ToLocalDateTimeOffset().AddHours(16).AddMinutes(29);

            // Act
            var result = testObject.IsOrderPossibleAt(orderDateTime);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsOrderPossible_Anytime_OutsideEarlyRegular_OnDeviatingAtPeriodStart_ReturnsTrue()
        {
            // Arrange
            fixture.SetupAnytimeOrderMode();
            fixture.SetupRegularOpeningDayNextMondayWithOneEarlyPeriod();
            fixture.SetupDeviatingDayDateOnMonday();
            fixture.SetupDeviatingOpeningDayWithOneDefaultPeriod();
            var testObject = fixture.CreateTestObject();

            var orderDateTime = fixture.NextMonday.ToLocalDateTimeOffset().AddHours(16).AddMinutes(30);

            // Act
            var result = testObject.IsOrderPossibleAt(orderDateTime);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsOrderPossible_Anytime_OutsideEarlyRegular_OnDeviatingAtPeriodEnd_ReturnsTrue()
        {
            // Arrange
            fixture.SetupAnytimeOrderMode();
            fixture.SetupRegularOpeningDayNextMondayWithOneEarlyPeriod();
            fixture.SetupDeviatingDayDateOnMonday();
            fixture.SetupDeviatingOpeningDayWithOneDefaultPeriod();
            var testObject = fixture.CreateTestObject();

            var orderDateTime = fixture.NextMonday.ToLocalDateTimeOffset().AddHours(22).AddMinutes(30);

            // Act
            var result = testObject.IsOrderPossibleAt(orderDateTime);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsOrderPossible_Anytime_OutsideEarlyRegular_OnDeviatingJustAfterPeriodEnd_ReturnsFalse()
        {
            // Arrange
            fixture.SetupAnytimeOrderMode();
            fixture.SetupRegularOpeningDayNextMondayWithOneEarlyPeriod();
            fixture.SetupDeviatingDayDateOnMonday();
            fixture.SetupDeviatingOpeningDayWithOneDefaultPeriod();
            var testObject = fixture.CreateTestObject();

            var orderDateTime = fixture.NextMonday.ToLocalDateTimeOffset().AddHours(22).AddMinutes(31);

            // Act
            var result = testObject.IsOrderPossibleAt(orderDateTime);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsOrderPossible_Anytime_InsideEarlyRegular_OnDeviatingDayButOutsidePeriod_ReturnsFalse()
        {
            // Arrange
            fixture.SetupAnytimeOrderMode();
            fixture.SetupRegularOpeningDayNextMondayWithOneEarlyPeriod();
            fixture.SetupDeviatingDayDateOnMonday();
            fixture.SetupDeviatingOpeningDayWithOneDefaultPeriod();
            var testObject = fixture.CreateTestObject();

            var orderDateTime = fixture.NextMonday.ToLocalDateTimeOffset().AddHours(6).AddMinutes(30);

            // Act
            var result = testObject.IsOrderPossibleAt(orderDateTime);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsOrderPossible_Anytime_JustBeforeRegularStart_NotOnDeviatingDay_ReturnsFalse()
        {
            // Arrange
            fixture.SetupAnytimeOrderMode();
            fixture.SetupRegularOpeningDayNextMondayWithOneEarlyPeriod();
            fixture.SetupDeviatingDayDateOnTuesday();
            fixture.SetupDeviatingOpeningDayWithOneDefaultPeriod();
            var testObject = fixture.CreateTestObject();

            var orderDateTime = fixture.NextMonday.ToLocalDateTimeOffset().AddHours(5).AddMinutes(59);

            // Act
            var result = testObject.IsOrderPossibleAt(orderDateTime);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsOrderPossible_Anytime_AtRegularStart_NotOnDeviatingDay_ReturnsTrue()
        {
            // Arrange
            fixture.SetupAnytimeOrderMode();
            fixture.SetupRegularOpeningDayNextMondayWithOneEarlyPeriod();
            fixture.SetupDeviatingDayDateOnTuesday();
            fixture.SetupDeviatingOpeningDayWithOneDefaultPeriod();
            var testObject = fixture.CreateTestObject();

            var orderDateTime = fixture.NextMonday.ToLocalDateTimeOffset().AddHours(6);

            // Act
            var result = testObject.IsOrderPossibleAt(orderDateTime);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsOrderPossible_Anytime_AtRegularEnd_NotOnDeviatingDay_ReturnsTrue()
        {
            // Arrange
            fixture.SetupAnytimeOrderMode();
            fixture.SetupRegularOpeningDayNextMondayWithOneEarlyPeriod();
            fixture.SetupDeviatingDayDateOnTuesday();
            fixture.SetupDeviatingOpeningDayWithOneDefaultPeriod();
            var testObject = fixture.CreateTestObject();

            var orderDateTime = fixture.NextMonday.ToLocalDateTimeOffset().AddHours(7);

            // Act
            var result = testObject.IsOrderPossibleAt(orderDateTime);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsOrderPossible_Anytime_JustAfterRegularEnd_NotOnDeviatingDay_ReturnsFalse()
        {
            // Arrange
            fixture.SetupAnytimeOrderMode();
            fixture.SetupRegularOpeningDayNextMondayWithOneEarlyPeriod();
            fixture.SetupDeviatingDayDateOnTuesday();
            fixture.SetupDeviatingOpeningDayWithOneDefaultPeriod();
            var testObject = fixture.CreateTestObject();

            var orderDateTime = fixture.NextMonday.ToLocalDateTimeOffset().AddHours(7).AddMinutes(1);

            // Act
            var result = testObject.IsOrderPossibleAt(orderDateTime);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void ChangeHygienicHandling_ChangesHygienicHandlingAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            var hygienicHandling = RandomStringBuilder.Build();

            // Act
            testObject.ChangeHygienicHandling(hygienicHandling, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                testObject.HygienicHandling.Should().BeEquivalentTo(hygienicHandling);
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        [Fact]
        public void AddCuisine_CuisineKnown_ReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupOneCuisine();
            var testObject = fixture.CreateTestObject();

            // Act
            testObject.AddCuisine(fixture.CuisineId, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                testObject.Cuisines.Should().BeEquivalentTo(fixture.Cuisines);
                testObject.UpdatedOn.Should().NotBeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().NotBe(fixture.ChangedBy);
            }
        }

        [Fact]
        public void AddCuisine_CuisineNotKnown_AddsCuisineAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupEmptyCuisines();
            var testObject = fixture.CreateTestObject();

            // Act
            testObject.AddCuisine(fixture.CuisineId, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                testObject.Cuisines.Should().BeEquivalentTo(fixture.CuisineId);
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        [Fact]
        public void RemoveCuisine_CuisineKnown_RemovesCuisineAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupOneCuisine();
            var testObject = fixture.CreateTestObject();

            // Act
            testObject.RemoveCuisine(fixture.CuisineId, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                testObject.Cuisines.Should().BeEmpty();
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        [Fact]
        public void RemoveCuisine_CuisineNotKnown_ReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupEmptyCuisines();
            var testObject = fixture.CreateTestObject();

            // Act
            testObject.RemoveCuisine(fixture.CuisineId, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                testObject.Cuisines.Should().BeEmpty();
                testObject.UpdatedOn.Should().NotBeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().NotBe(fixture.ChangedBy);
            }
        }

        [Fact]
        public void AddPaymentMethod_PaymentMethodKnown_ReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupOnePaymentMethod();
            var testObject = fixture.CreateTestObject();

            // Act
            testObject.AddPaymentMethod(fixture.PaymentMethodId, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                testObject.PaymentMethods.Should().BeEquivalentTo(fixture.PaymentMethods);
                testObject.UpdatedOn.Should().NotBeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().NotBe(fixture.ChangedBy);
            }
        }

        [Fact]
        public void AddPaymentMethod_PaymentMethodNotKnown_AddsPaymentMethodAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupEmptyPaymentMethods();
            var testObject = fixture.CreateTestObject();

            // Act
            testObject.AddPaymentMethod(fixture.PaymentMethodId, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                testObject.PaymentMethods.Should().BeEquivalentTo(fixture.PaymentMethodId);
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        [Fact]
        public void RemovePaymentMethod_PaymentMethodKnown_RemovesPaymentMethodAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupOnePaymentMethod();
            var testObject = fixture.CreateTestObject();

            // Act
            testObject.RemovePaymentMethod(fixture.PaymentMethodId, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                testObject.PaymentMethods.Should().BeEmpty();
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        [Fact]
        public void RemovePaymentMethod_PaymentMethodNotKnown_ReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupEmptyPaymentMethods();
            var testObject = fixture.CreateTestObject();

            // Act
            testObject.RemovePaymentMethod(fixture.PaymentMethodId, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                testObject.PaymentMethods.Should().BeEmpty();
                testObject.UpdatedOn.Should().NotBeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().NotBe(fixture.ChangedBy);
            }
        }

        [Fact]
        public void HasAdministrator_AdministratorKnown_ReturnsTrue()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupOneAdministrator();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.HasAdministrator(fixture.AdministratorId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void HasAdministrator_AdministratorNotKnown_ReturnsFalse()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupEmptyAdministrators();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.HasAdministrator(fixture.AdministratorId);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void AddAdministrator_AdministratorKnown_ReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupOneAdministrator();
            var testObject = fixture.CreateTestObject();

            // Act
            testObject.AddAdministrator(fixture.AdministratorId, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                testObject.Administrators.Should().BeEquivalentTo(fixture.Administrators);
                testObject.UpdatedOn.Should().NotBeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().NotBe(fixture.ChangedBy);
            }
        }

        [Fact]
        public void AddAdministrator_AdministratorNotKnown_AddsAdministratorAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupEmptyAdministrators();
            var testObject = fixture.CreateTestObject();

            // Act
            testObject.AddAdministrator(fixture.AdministratorId, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                testObject.Administrators.Should().BeEquivalentTo(fixture.AdministratorId);
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        [Fact]
        public void RemoveAdministrator_AdministratorKnown_RemovesAdministratorAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupOneAdministrator();
            var testObject = fixture.CreateTestObject();

            // Act
            testObject.RemoveAdministrator(fixture.AdministratorId, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                testObject.Administrators.Should().BeEmpty();
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        [Fact]
        public void RemoveAdministrator_AdministratorNotKnown_ReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupEmptyAdministrators();
            var testObject = fixture.CreateTestObject();

            // Act
            testObject.RemoveAdministrator(fixture.AdministratorId, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                testObject.Administrators.Should().BeEmpty();
                testObject.UpdatedOn.Should().NotBeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().NotBe(fixture.ChangedBy);
            }
        }

        [Fact]
        public void ChangeImportId_ChangesImportIdAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            var importId = RandomStringBuilder.Build();

            // Act
            testObject.ChangeImportId(importId, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                testObject.ImportId.Should().BeEquivalentTo(importId);
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        [Fact]
        public void Deactivate_Deactivated_DoesNotChangeAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupDeactivated();
            var testObject = fixture.CreateTestObject();

            // Act
            testObject.Deactivate(fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                testObject.IsActive.Should().BeFalse();
                testObject.UpdatedOn.Should().NotBeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().NotBe(fixture.ChangedBy);
            }
        }

        [Fact]
        public void Deactivate_Activated_DoesNotChangeAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupActivated();
            var testObject = fixture.CreateTestObject();

            // Act
            testObject.Deactivate(fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                testObject.IsActive.Should().BeFalse();
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        [Fact]
        public void Activate_Deactivated_DoesNotChangeAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupDeactivated();
            var testObject = fixture.CreateTestObject();

            // Act
            testObject.Activate(fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                testObject.IsActive.Should().BeTrue();
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        [Fact]
        public void Activate_Activated_DoesNotChangeAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupActivated();
            var testObject = fixture.CreateTestObject();

            // Act
            testObject.Activate(fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                testObject.IsActive.Should().BeTrue();
                testObject.UpdatedOn.Should().NotBeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().NotBe(fixture.ChangedBy);
            }
        }

        [Fact]
        public void DisableSupport_WithoutSupport_DoesNotChangeAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupWithoutSupport();
            var testObject = fixture.CreateTestObject();

            // Act
            testObject.DisableSupport(fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                testObject.NeedsSupport.Should().BeFalse();
                testObject.UpdatedOn.Should().NotBeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().NotBe(fixture.ChangedBy);
            }
        }

        [Fact]
        public void DisableSupport_WithSupport_DoesNotChangeAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupWithSupport();
            var testObject = fixture.CreateTestObject();

            // Act
            testObject.DisableSupport(fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                testObject.NeedsSupport.Should().BeFalse();
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        [Fact]
        public void EnableSupport_WithoutSupport_DoesNotChangeAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupWithoutSupport();
            var testObject = fixture.CreateTestObject();

            // Act
            testObject.EnableSupport(fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                testObject.NeedsSupport.Should().BeTrue();
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        [Fact]
        public void EnableSupport_WithSupport_DoesNotChangeAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupWithSupport();
            var testObject = fixture.CreateTestObject();

            // Act
            testObject.EnableSupport(fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                testObject.NeedsSupport.Should().BeTrue();
                testObject.UpdatedOn.Should().NotBeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().NotBe(fixture.ChangedBy);
            }
        }

        [Fact]
        public void ChangeSupportedOrderMode_ChangesSupportedOrderModeAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupAtNextShiftOrderMode();
            var testObject = fixture.CreateTestObject();

            // Act
            testObject.ChangeSupportedOrderMode(SupportedOrderMode.Anytime, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                testObject.SupportedOrderMode.Should().Be(SupportedOrderMode.Anytime);
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        private sealed class Fixture
        {
            public Fixture()
            {
                NextMonday = CalculateNextMonday();
                NextTuesday = NextMonday.AddDays(1);
                Today = Date.Today;
                TodayDayOfWeekIndex = CalculateDayOfWeekIndex(Today.DayOfWeek);
            }

            public Date NextMonday { get; }

            public Date NextTuesday { get; }

            public Date Today { get; }

            public int TodayDayOfWeekIndex { get; }

            public UserId ChangedBy { get; private set; }

            public SupportedOrderMode? SupportedOrderMode { get; private set; }

            public List<RegularOpeningDay> RegularOpeningDays { get; private set; }

            public Date DeviatingDayDate { get; private set; }

            public List<DeviatingOpeningDay> DeviatingOpeningDays { get; private set; }

            public CuisineId CuisineId { get; private set; }

            public HashSet<CuisineId> Cuisines { get; private set; }

            public PaymentMethodId PaymentMethodId { get; private set; }

            public HashSet<PaymentMethodId> PaymentMethods { get; private set; }

            public UserId AdministratorId { get; private set; }

            public HashSet<UserId> Administrators { get; private set; }

            public bool? IsActive { get; private set; }

            public bool? NeedsSupport { get; private set; }

            public ExternalMenu ExternalMenu { get; private set; }

            public List<ExternalMenu> ExternalMenus { get; private set; }

            public void SetupChangedBy()
            {
                ChangedBy = new UserIdBuilder().Create();
            }

            public void SetupOnlyPhoneOrderMode()
            {
                SupportedOrderMode = Core.Domain.Model.Restaurants.SupportedOrderMode.OnlyPhone;
            }

            public void SetupAtNextShiftOrderMode()
            {
                SupportedOrderMode = Core.Domain.Model.Restaurants.SupportedOrderMode.AtNextShift;
            }

            public void SetupAnytimeOrderMode()
            {
                SupportedOrderMode = Core.Domain.Model.Restaurants.SupportedOrderMode.Anytime;
            }

            public void SetupEmptyRegularOpeningDays()
            {
                RegularOpeningDays = new List<RegularOpeningDay>();
            }

            public void SetupRegularOpeningDayNextMondayWithOneDefaultPeriod()
            {
                RegularOpeningDays = new List<RegularOpeningDay>
                {
                    new RegularOpeningDayBuilder()
                        .WithDayOfWeek(CalculateDayOfWeekIndex(NextMonday.DayOfWeek))
                        .WithOpeningPeriods(new OpeningPeriodBuilder().WithValidConstrains().CreateMany(1))
                        .Create()
                };
            }

            public void SetupRegularOpeningDayNextMondayWithOneEarlyPeriod()
            {
                RegularOpeningDays = new List<RegularOpeningDay>
                {
                    new RegularOpeningDayBuilder()
                        .WithDayOfWeek(CalculateDayOfWeekIndex(NextMonday.DayOfWeek))
                        .WithOpeningPeriods(new []
                        {
                            new OpeningPeriod(TimeSpan.FromHours(6), TimeSpan.FromHours(7))
                        })
                        .Create()
                };
            }

            public void SetupRegularOpeningDayNextMondayWithTwoPeriods()
            {
                RegularOpeningDays = new List<RegularOpeningDay>
                {
                    new RegularOpeningDayBuilder()
                        .WithDayOfWeek(CalculateDayOfWeekIndex(NextMonday.DayOfWeek))
                        .WithOpeningPeriods(new[]
                        {
                            new OpeningPeriod(TimeSpan.FromHours(12), TimeSpan.FromHours(14)),
                            new OpeningPeriod(TimeSpan.FromHours(16), TimeSpan.FromHours(22))
                        })
                        .Create()
                };
            }

            public void SetupRegularOpeningDayTodayWithPeriodInAnHourForRestOfDay()
            {
                var currentTime = DateTimeOffset.Now.TimeOfDay.Add(TimeSpan.FromMinutes(-1));

                RegularOpeningDays = new List<RegularOpeningDay>
                {
                    new RegularOpeningDayBuilder()
                        .WithDayOfWeek(TodayDayOfWeekIndex)
                        .WithOpeningPeriods(new[]
                        {
                            new OpeningPeriod(currentTime.Add(TimeSpan.FromHours(1)), TimeSpan.FromHours(28))
                        })
                        .Create()
                };
            }

            public void SetupRegularOpeningDayTodayWithPeriodNowFor2MinutesAndInAnHourForRestOfDay()
            {
                var currentTime = DateTimeOffset.Now.TimeOfDay.Add(TimeSpan.FromMinutes(-1));

                RegularOpeningDays = new List<RegularOpeningDay>
                {
                    new RegularOpeningDayBuilder()
                        .WithDayOfWeek(TodayDayOfWeekIndex)
                        .WithOpeningPeriods(new[]
                        {
                            new OpeningPeriod(currentTime, currentTime.Add(TimeSpan.FromMinutes(2))),
                            new OpeningPeriod(currentTime.Add(TimeSpan.FromHours(1)), TimeSpan.FromHours(28))
                        })
                        .Create()
                };
            }

            public void SetupRegularOpeningDayTodayWithPeriodNowForRestOfDay()
            {
                var currentTime = DateTimeOffset.Now.TimeOfDay.Add(TimeSpan.FromMinutes(-1));

                RegularOpeningDays = new List<RegularOpeningDay>
                {
                    new RegularOpeningDayBuilder()
                        .WithDayOfWeek(TodayDayOfWeekIndex)
                        .WithOpeningPeriods(new[]
                        {
                            new OpeningPeriod(currentTime, TimeSpan.FromHours(28))
                        })
                        .Create()
                };
            }

            public void SetupEmptyDeviatingOpeningDays()
            {
                DeviatingOpeningDays = new List<DeviatingOpeningDay>();
            }

            public void SetupDeviatingDayDateOnMonday()
            {
                DeviatingDayDate = NextMonday;
            }

            public void SetupDeviatingDayDateOnTuesday()
            {
                DeviatingDayDate = NextTuesday;
            }

            public void SetupDeviatingOpeningDayWithoutPeriods()
            {
                DeviatingOpeningDays = new List<DeviatingOpeningDay>
                {
                    new DeviatingOpeningDayBuilder()
                        .WithDate(DeviatingDayDate)
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
                        .WithDate(DeviatingDayDate)
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
                            new OpeningPeriod(TimeSpan.FromHours(12), TimeSpan.FromHours(14)),
                            new OpeningPeriod(TimeSpan.FromHours(16), TimeSpan.FromHours(22))
                        })
                        .Create()
                };
            }

            public void SetupEmptyCuisines()
            {
                CuisineId = new CuisineId(Guid.NewGuid());
                Cuisines = new HashSet<CuisineId>();
            }

            public void SetupOneCuisine()
            {
                CuisineId = new CuisineId(Guid.NewGuid());
                Cuisines = new HashSet<CuisineId> {CuisineId};
            }

            public void SetupEmptyPaymentMethods()
            {
                PaymentMethodId = new PaymentMethodId(Guid.NewGuid());
                PaymentMethods = new HashSet<PaymentMethodId>();
            }

            public void SetupOnePaymentMethod()
            {
                PaymentMethodId = new PaymentMethodId(Guid.NewGuid());
                PaymentMethods = new HashSet<PaymentMethodId> {PaymentMethodId};
            }

            public void SetupEmptyAdministrators()
            {
                AdministratorId = new UserId(Guid.NewGuid());
                Administrators = new HashSet<UserId>();
            }

            public void SetupOneAdministrator()
            {
                AdministratorId = new UserId(Guid.NewGuid());
                Administrators = new HashSet<UserId> {AdministratorId};
            }

            public void SetupActivated()
            {
                IsActive = true;
            }

            public void SetupDeactivated()
            {
                IsActive = false;
            }

            public void SetupWithSupport()
            {
                NeedsSupport = true;
            }

            public void SetupWithoutSupport()
            {
                NeedsSupport = false;
            }

            public void SetupEmptyExternalMenus()
            {
                ExternalMenus = new List<ExternalMenu>();
            }

            public void SetupOneExternalMenu()
            {
                ExternalMenu = new ExternalMenuBuilder().Create();
                ExternalMenus = new List<ExternalMenu> {ExternalMenu};
            }

            public Restaurant CreateTestObject()
            {
                var builder = new RestaurantBuilder();

                if (SupportedOrderMode != null)
                    builder = builder.WithSupportedOrderMode(SupportedOrderMode.Value);

                if (RegularOpeningDays != null)
                    builder = builder.WithRegularOpeningDays(RegularOpeningDays);

                if (DeviatingOpeningDays != null)
                    builder = builder.WithDeviatingOpeningDays(DeviatingOpeningDays);

                if (Cuisines != null)
                    builder = builder.WithCuisines(Cuisines);

                if (PaymentMethods != null)
                    builder = builder.WithPaymentMethods(PaymentMethods);

                if (Administrators != null)
                    builder = builder.WithAdministrators(Administrators);

                if (IsActive.HasValue)
                    builder = builder.WithIsActive(IsActive.Value);

                if (NeedsSupport.HasValue)
                    builder = builder.WithNeedsSupport(NeedsSupport.Value);

                if (ExternalMenus != null)
                    builder = builder.WithExternalMenus(ExternalMenus);

                return builder
                    .WithValidConstrains()
                    .Create();
            }

            private static Date CalculateNextMonday()
            {
                var date = Date.Today.AddDays(1);
                while (date.DayOfWeek != DayOfWeek.Monday)
                    date = date.AddDays(1);
                return date;
            }

            private static int CalculateDayOfWeekIndex(DayOfWeek dayOfWeek)
            {
                var dayOfWeekIndex = ((int) dayOfWeek - 1) % 7;

                if (dayOfWeekIndex < 0)
                {
                    dayOfWeekIndex += 7;
                }

                return dayOfWeekIndex;
            }
        }
    }
}
