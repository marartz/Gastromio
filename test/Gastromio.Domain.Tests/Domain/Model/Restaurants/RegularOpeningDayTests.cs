using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Restaurants;
using Gastromio.Domain.TestKit.Domain.Model.Restaurants;
using Xunit;

namespace Gastromio.Domain.Tests.Domain.Model.Restaurants
{
    public class RegularOpeningDayTests
    {
        private readonly Fixture fixture;

        public RegularOpeningDayTests()
        {
            fixture = new Fixture();
        }

        [Fact]
        public void FindPeriodAtTime_WithoutPeriods_ReturnsNull()
        {
            // Arrange
            fixture.SetupWithoutPeriods();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.FindPeriodAtTime(TimeSpan.Zero);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void FindPeriodAtTime_LaterPeriod_ReturnsNull()
        {
            // Arrange
            fixture.SetupWithOnePeriod();
            var testObject = fixture.CreateTestObject();

            var time = fixture.OpeningPeriods.First().Start.Add(TimeSpan.FromMinutes(-1));

            // Act
            var result = testObject.FindPeriodAtTime(time);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void FindPeriodAtTime_EarlierPeriod_ReturnsNull()
        {
            // Arrange
            fixture.SetupWithOnePeriod();
            var testObject = fixture.CreateTestObject();

            var time = fixture.OpeningPeriods.First().End.Add(TimeSpan.FromMinutes(1));

            // Act
            var result = testObject.FindPeriodAtTime(time);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void FindPeriodAtTime_MatchingPeriodAtStart_ReturnsPeriod()
        {
            // Arrange
            fixture.SetupWithOnePeriod();
            var testObject = fixture.CreateTestObject();

            var time = fixture.OpeningPeriods.First().Start;

            // Act
            var result = testObject.FindPeriodAtTime(time);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Should().BeEquivalentTo(fixture.OpeningPeriods.First());
            }
        }

        [Fact]
        public void FindPeriodAtTime_MatchingPeriodAtEnd_ReturnsPeriod()
        {
            // Arrange
            fixture.SetupWithOnePeriod();
            var testObject = fixture.CreateTestObject();

            var time = fixture.OpeningPeriods.First().End;

            // Act
            var result = testObject.FindPeriodAtTime(time);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Should().BeEquivalentTo(fixture.OpeningPeriods.First());
            }
        }

        [Fact]
        public void AddPeriod_SamePeriodAlreadyPresent_ChangesNothing()
        {
            // Arrange
            fixture.SetupWithOnePeriod();
            var testObject = fixture.CreateTestObject();

            var curPeriod = fixture.OpeningPeriods.First();

            var period = new OpeningPeriodBuilder()
                .WithStart(curPeriod.Start)
                .WithEnd(curPeriod.End)
                .Create();

            // Act
            var result = testObject.AddPeriod(period);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.OpeningPeriods.Should().BeEquivalentTo(new [] { curPeriod });
            }
        }

        [Fact]
        public void AddPeriod_NewEndEqualsCurrentStart_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupWithOnePeriod();
            var testObject = fixture.CreateTestObject();

            var curPeriod = fixture.OpeningPeriods.First();

            var period = new OpeningPeriodBuilder()
                .WithStart(curPeriod.Start.Add(TimeSpan.FromHours(-1)))
                .WithEnd(curPeriod.Start)
                .Create();

            // Act
            Action act = () => testObject.AddPeriod(period);

            // Assert
            act.Should().Throw<DomainException<RestaurantOpeningPeriodIntersectsFailure>>();
        }

        [Fact]
        public void AddPeriod_NewEndIsOverlappingIntoCurrent_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupWithOnePeriod();
            var testObject = fixture.CreateTestObject();

            var curPeriod = fixture.OpeningPeriods.First();

            var period = new OpeningPeriodBuilder()
                .WithStart(curPeriod.Start.Add(TimeSpan.FromHours(-1)))
                .WithEnd(curPeriod.Start.Add(TimeSpan.FromHours(+1)))
                .Create();

            // Act
            Action act = () => testObject.AddPeriod(period);

            // Assert
            act.Should().Throw<DomainException<RestaurantOpeningPeriodIntersectsFailure>>();
        }

        [Fact]
        public void AddPeriod_NewStartIsOverlappingIntoCurrent_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupWithOnePeriod();
            var testObject = fixture.CreateTestObject();

            var curPeriod = fixture.OpeningPeriods.First();

            var period = new OpeningPeriodBuilder()
                .WithStart(curPeriod.End.Add(TimeSpan.FromHours(-1)))
                .WithEnd(curPeriod.End.Add(TimeSpan.FromHours(+1)))
                .Create();

            // Act
            Action act = () => testObject.AddPeriod(period);

            // Assert
            act.Should().Throw<DomainException<RestaurantOpeningPeriodIntersectsFailure>>();
        }

        [Fact]
        public void AddPeriod_NewStartEqualsCurrentEnd_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupWithOnePeriod();
            var testObject = fixture.CreateTestObject();

            var curPeriod = fixture.OpeningPeriods.First();

            var period = new OpeningPeriodBuilder()
                .WithStart(curPeriod.End)
                .WithEnd(curPeriod.End.Add(TimeSpan.FromHours(+1)))
                .Create();

            // Act
            Action act = () => testObject.AddPeriod(period);

            // Assert
            act.Should().Throw<DomainException<RestaurantOpeningPeriodIntersectsFailure>>();
        }

        [Fact]
        public void AddPeriod_NewStartEqualsCurStartButShorter_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupWithOnePeriod();
            var testObject = fixture.CreateTestObject();

            var curPeriod = fixture.OpeningPeriods.First();

            var period = new OpeningPeriodBuilder()
                .WithStart(curPeriod.Start)
                .WithEnd(curPeriod.End.Add(TimeSpan.FromHours(-1)))
                .Create();

            // Act
            Action act = () => testObject.AddPeriod(period);

            // Assert
            act.Should().Throw<DomainException<RestaurantOpeningPeriodIntersectsFailure>>();
        }

        [Fact]
        public void AddPeriod_NewIsCompletelyInCurrent_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupWithOnePeriod();
            var testObject = fixture.CreateTestObject();

            var curPeriod = fixture.OpeningPeriods.First();

            var period = new OpeningPeriodBuilder()
                .WithStart(curPeriod.Start.Add(TimeSpan.FromHours(+1)))
                .WithEnd(curPeriod.End.Add(TimeSpan.FromHours(-1)))
                .Create();

            // Act
            Action act = () => testObject.AddPeriod(period);

            // Assert
            act.Should().Throw<DomainException<RestaurantOpeningPeriodIntersectsFailure>>();
        }

        [Fact]
        public void AddPeriod_NewEndEqualsCurEndButShorter_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupWithOnePeriod();
            var testObject = fixture.CreateTestObject();

            var curPeriod = fixture.OpeningPeriods.First();

            var period = new OpeningPeriodBuilder()
                .WithStart(curPeriod.Start.Add(TimeSpan.FromHours(+1)))
                .WithEnd(curPeriod.End)
                .Create();

            // Act
            Action act = () => testObject.AddPeriod(period);

            // Assert
            act.Should().Throw<DomainException<RestaurantOpeningPeriodIntersectsFailure>>();
        }

        [Fact]
        public void AddPeriod_CurrentIsCompletelyInNew_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupWithOnePeriod();
            var testObject = fixture.CreateTestObject();

            var curPeriod = fixture.OpeningPeriods.First();

            var period = new OpeningPeriodBuilder()
                .WithStart(curPeriod.Start.Add(TimeSpan.FromHours(-1)))
                .WithEnd(curPeriod.End.Add(TimeSpan.FromHours(+1)))
                .Create();

            // Act
            Action act = () => testObject.AddPeriod(period);

            // Assert
            act.Should().Throw<DomainException<RestaurantOpeningPeriodIntersectsFailure>>();
        }

        [Fact]
        public void AddPeriod_WithoutPeriods_AddsPeriod()
        {
            // Arrange
            fixture.SetupWithoutPeriods();
            var testObject = fixture.CreateTestObject();

            var period = new OpeningPeriodBuilder()
                .WithStart(TimeSpan.FromHours(8))
                .WithEnd(TimeSpan.FromHours(16))
                .Create();

            // Act
            var result = testObject.AddPeriod(period);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.OpeningPeriods.Should().BeEquivalentTo(new [] { period });
            }
        }

        [Fact]
        public void RemovePeriod_WithoutPeriods_DoesNotChangeAnything()
        {
            // Arrange
            fixture.SetupWithoutPeriods();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.RemovePeriod(TimeSpan.FromHours(8));

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.OpeningPeriods.Should().BeEmpty();
            }
        }

        [Fact]
        public void RemovePeriod_WithNonMatchingPeriods_DoesNotChangeAnything()
        {
            // Arrange
            fixture.SetupWithOnePeriod();
            var testObject = fixture.CreateTestObject();

            var curPeriod = fixture.OpeningPeriods.First();

            // Act
            var result = testObject.RemovePeriod(curPeriod.Start.Add(TimeSpan.FromHours(1)));

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.OpeningPeriods.Should().BeEquivalentTo(new [] { curPeriod });
            }
        }

        [Fact]
        public void RemovePeriod_WithMatchingPeriod_RemovesPeriod()
        {
            // Arrange
            fixture.SetupWithOnePeriod();
            var testObject = fixture.CreateTestObject();

            var curPeriod = fixture.OpeningPeriods.First();

            // Act
            var result = testObject.RemovePeriod(curPeriod.Start);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.OpeningPeriods.Should().BeEmpty();
            }
        }

        private sealed class Fixture
        {
            public IList<OpeningPeriod> OpeningPeriods { get; private set; }

            public RegularOpeningDay CreateTestObject()
            {
                return new RegularOpeningDayBuilder()
                    .WithDayOfWeek(2)
                    .WithOpeningPeriods(OpeningPeriods)
                    .Create();
            }

            public void SetupWithoutPeriods()
            {
                OpeningPeriods = new List<OpeningPeriod>();
            }

            public void SetupWithOnePeriod()
            {
                var start = TimeSpan.FromHours(8);
                var end = TimeSpan.FromHours(16);

                OpeningPeriods = new OpeningPeriodBuilder()
                    .WithStart(start)
                    .WithEnd(end)
                    .CreateMany(1)
                    .ToList();
            }
        }
    }
}
