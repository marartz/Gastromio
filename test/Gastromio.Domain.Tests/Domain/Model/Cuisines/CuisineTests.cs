using System;
using FluentAssertions;
using FluentAssertions.Execution;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Cuisines;
using Gastromio.Core.Domain.Model.Users;
using Gastromio.Domain.TestKit.Common;
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
        public void Ctor_NameNull_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupNameNull();

            // Act
            Action act = () => fixture.CreateTestObject();

            // Assert
            act.Should().Throw<DomainException<CuisineNameIsRequiredFailure>>();
        }

        [Fact]
        public void Ctor_NameEmpty_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupNameEmpty();

            // Act
            Action act = () => fixture.CreateTestObject();

            // Assert
            act.Should().Throw<DomainException<CuisineNameIsRequiredFailure>>();
        }

        [Fact]
        public void Ctor_NameLength101_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupNameLength101();

            // Act
            Action act = () => fixture.CreateTestObject();

            // Assert
            act.Should().Throw<DomainException<CuisineNameTooLongFailure>>();
        }

        [Fact]
        public void Ctor_NameLength100_ChangesName()
        {
            // Arrange
            fixture.SetupNameLength100();

            // Act
            var testObject = fixture.CreateTestObject();

            // Assert
            using (new AssertionScope())
            {
                testObject.Name.Should().Be(fixture.Name);
            }
        }

        [Fact]
        public void ChangeName_NameNull_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupNameLength100();
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
            fixture.SetupNameLength100();
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
            fixture.SetupNameLength100();
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
            fixture.SetupNameLength100();
            fixture.SetupUpdatedBy();
            var testObject = fixture.CreateTestObject();

            var name = RandomStringBuilder.BuildWithLength(100);

            // Act
            testObject.ChangeName(name, fixture.UpdatedBy);

            // Assert
            using (new AssertionScope())
            {
                testObject.Name.Should().Be(name);
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
                testObject.UpdatedBy.Should().Be(fixture.UpdatedBy);
            }
        }

        private sealed class Fixture
        {
            public string Name { get; private set; }

            public UserId UpdatedBy { get; private set; }

            public void SetupNameNull()
            {
                Name = null;
            }

            public void SetupNameEmpty()
            {
                Name = string.Empty;
            }

            public void SetupNameLength101()
            {
                Name = RandomStringBuilder.BuildWithLength(101);
            }

            public void SetupNameLength100()
            {
                Name = RandomStringBuilder.BuildWithLength(100);
            }

            public void SetupUpdatedBy()
            {
                UpdatedBy = new UserIdBuilder().Create();
            }

            public Cuisine CreateTestObject()
            {
                var userId = new UserId(Guid.NewGuid());

                return new Cuisine(
                    new CuisineId(Guid.NewGuid()),
                    Name,
                    DateTimeOffset.UtcNow,
                    userId,
                    DateTimeOffset.UtcNow,
                    userId
                );
            }
        }
    }
}
