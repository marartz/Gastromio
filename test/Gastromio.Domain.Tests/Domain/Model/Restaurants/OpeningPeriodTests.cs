using System;
using FluentAssertions;
using FluentAssertions.Execution;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Restaurants;
using Gastromio.Domain.TestKit.Domain.Model.Restaurants;
using Xunit;

namespace Gastromio.Domain.Tests.Domain.Model.Restaurants
{
    public class OpeningPeriodTests
    {
        private readonly Fixture fixture;

        public OpeningPeriodTests()
        {
            fixture = new Fixture();
        }

        [Fact]
        public void Ctor_StartBeforeEarliestOpeningTime_ThrowsDomainException()
        {
            // Arrange
            fixture.Start = TimeSpan.FromHours(OpeningPeriod.EarliestOpeningTime - 1);
            fixture.End = TimeSpan.FromHours(OpeningPeriod.EarliestOpeningTime + 1);

            // Act
            Action act = () => fixture.CreateTestObject();

            // Assert
            act.Should().Throw<DomainException<RestaurantOpeningPeriodBeginsTooEarlyFailure>>();
        }

        [Fact]
        public void Ctor_StartBeforeEnd_ThrowsDomainException()
        {
            // Arrange
            fixture.Start = TimeSpan.FromHours(OpeningPeriod.EarliestOpeningTime + 2);
            fixture.End = TimeSpan.FromHours(OpeningPeriod.EarliestOpeningTime + 1);

            // Act
            Action act = () => fixture.CreateTestObject();

            // Assert
            act.Should().Throw<DomainException<RestaurantOpeningPeriodEndsBeforeStartFailure>>();
        }

        [Fact]
        public void Ctor_EndBeforeEarliestOpeningTime_Adds24Hours()
        {
            // Arrange
            fixture.Start = TimeSpan.FromHours(OpeningPeriod.EarliestOpeningTime + 2);
            fixture.End = TimeSpan.FromHours(OpeningPeriod.EarliestOpeningTime - 1);

            // Act
            var testObject = fixture.CreateTestObject();

            // Assert
            using (new AssertionScope())
            {
                testObject.Start.Should().Be(fixture.Start);
                testObject.End.Should().Be(fixture.End.Add(TimeSpan.FromHours(24)));
            }
        }

        [Fact]
        public void Ctor_EndAfterEarliestOpeningTime_ReturnsWithCorrectValues()
        {
            // Arrange
            fixture.Start = TimeSpan.FromHours(OpeningPeriod.EarliestOpeningTime + 2);
            fixture.End = TimeSpan.FromHours(OpeningPeriod.EarliestOpeningTime + 3);

            // Act
            var testObject = fixture.CreateTestObject();

            // Assert
            using (new AssertionScope())
            {
                testObject.Start.Should().Be(fixture.Start);
                testObject.End.Should().Be(fixture.End);
            }
        }

        private sealed class Fixture
        {
            public TimeSpan Start { get; set; }

            public TimeSpan End { get; set; }

            public OpeningPeriod CreateTestObject()
            {
                return new OpeningPeriod(Start, End);
            }
        }
    }
}
