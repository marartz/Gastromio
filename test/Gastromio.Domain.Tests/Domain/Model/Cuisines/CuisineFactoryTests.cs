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
    public class CuisineFactoryTests
    {
        private readonly Fixture fixture;

        public CuisineFactoryTests()
        {
            fixture = new Fixture();
        }

        [Fact]
        public void Create_ValidParameters_CreatesCuisine()
        {
            // Arrange
            fixture.SetupValidParameters();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.Create(fixture.Name, fixture.CreatedBy);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.Should().BeOfType<Cuisine>();
                result?.Id.Value.Should().NotBeEmpty();
                result?.Name.Should().Be(fixture.Name);
                result?.CreatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                result?.CreatedBy.Should().Be(fixture.CreatedBy);
                result?.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                result?.UpdatedBy.Should().Be(fixture.CreatedBy);
            }
        }

        [Fact]
        public void Create_InvalidParameters_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupInvalidParameters();
            var testObject = fixture.CreateTestObject();

            // Act
            Action act = () => testObject.Create(fixture.Name, fixture.CreatedBy);

            // Assert
            act.Should().Throw<DomainException<CuisineNameTooLongFailure>>();
        }

        private sealed class Fixture
        {
            public string Name { get; private set; }

            public UserId CreatedBy { get; private set; }

            public void SetupValidParameters()
            {
                Name = RandomStringBuilder.BuildWithLength(100);
                CreatedBy = new UserIdBuilder().Create();
            }

            public void SetupInvalidParameters()
            {
                Name = RandomStringBuilder.BuildWithLength(101);
                CreatedBy = new UserIdBuilder().Create();
            }

            public CuisineFactory CreateTestObject()
            {
                return new CuisineFactory();
            }
        }
    }
}
