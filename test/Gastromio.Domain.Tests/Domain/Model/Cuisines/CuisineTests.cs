using System;
using FluentAssertions;
using FluentAssertions.Execution;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Cuisines;
using Gastromio.Core.Domain.Model.Users;
using Gastromio.Domain.TestKit.Common;
using Gastromio.Domain.TestKit.Domain.Model.Cuisines;
using Gastromio.Domain.TestKit.Domain.Model.Users;
using Xunit;

namespace Gastromio.Domain.Tests.Domain.Model.Cuisines
{
    public class CuisineTests
    {
        private readonly Fixture fixture;

        public CuisineTests()
        {
            fixture = new Fixture();
        }

        [Fact]
        public void ChangeName_NameNull_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupUpdatedBy();
            var testObject = fixture.CreateTestObject();

            // Act
            Action act = () => testObject.ChangeName(null, fixture.UpdatedBy);

            // Assert
            act.Should().Throw<DomainException<CuisineNameIsRequiredFailure>>();
        }

        [Fact]
        public void ChangeName_NameEmpty_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupUpdatedBy();
            var testObject = fixture.CreateTestObject();

            // Act
            Action act = () => testObject.ChangeName(string.Empty, fixture.UpdatedBy);

            // Assert
            act.Should().Throw<DomainException<CuisineNameIsRequiredFailure>>();
        }

        [Fact]
        public void ChangeName_NameLength101_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupUpdatedBy();
            var testObject = fixture.CreateTestObject();

            var name = RandomStringBuilder.BuildWithLength(101);

            // Act
            Action act = () => testObject.ChangeName(name, fixture.UpdatedBy);

            // Assert
            act.Should().Throw<DomainException<CuisineNameTooLongFailure>>();
        }

        [Fact]
        public void ChangeName_NameLength100_ChangesName()
        {
            // Arrange
            fixture.SetupUpdatedBy();
            var testObject = fixture.CreateTestObject();

            var name = RandomStringBuilder.BuildWithLength(100);

            // Act
            testObject.ChangeName(name, fixture.UpdatedBy);

            // Assert
            using (new AssertionScope())
            {
                testObject.Name.Should().Be(name);
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.UpdatedBy);
            }
        }

        private sealed class Fixture
        {
            public UserId UpdatedBy { get; private set; }

            public void SetupUpdatedBy()
            {
                UpdatedBy = new UserIdBuilder().Create();
            }

            public Cuisine CreateTestObject()
            {
                return new CuisineBuilder()
                    .WithValidConstrains()
                    .Create();
            }
        }
    }
}
