using System;
using FluentAssertions;
using FluentAssertions.Execution;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Restaurants;
using Gastromio.Core.Domain.Model.Users;
using Gastromio.Domain.TestKit.Common;
using Gastromio.Domain.TestKit.Domain.Model.Restaurants;
using Gastromio.Domain.TestKit.Domain.Model.Users;
using Xunit;

namespace Gastromio.Domain.Tests.Domain.Model.Restaurants
{
    public class DishCategoryTests
    {
        private readonly Fixture fixture;

        public DishCategoryTests()
        {
            fixture = new Fixture();
        }

        [Fact]
        public void ChangeName_NameNull_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            // Act
            Action act = () => testObject.ChangeName(null);

            // Assert
            act.Should().Throw<DomainException<DishCategoryNameRequiredFailure>>();
        }

        [Fact]
        public void ChangeName_NameEmpty_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            // Act
            Action act = () => testObject.ChangeName(string.Empty);

            // Assert
            act.Should().Throw<DomainException<DishCategoryNameRequiredFailure>>();
        }

        [Fact]
        public void ChangeName_NameLength101_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            var name = RandomStringBuilder.BuildWithLength(101);

            // Act
            Action act = () => testObject.ChangeName(name);

            // Assert
            act.Should().Throw<DomainException<DishCategoryNameTooLongFailure>>();
        }

        [Fact]
        public void ChangeName_NameLength100_ChangesName()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            var name = RandomStringBuilder.BuildWithLength(100);

            // Act
            var result = testObject.ChangeName(name);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Name.Should().Be(name);
            }
        }

        [Fact]
        public void ChangeOrderNo_OrderNoNegative_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            // Act
            Action act = () => testObject.ChangeOrderNo(-1);

            // Assert
            act.Should().Throw<DomainException<DishCategoryInvalidOrderNoFailure>>();
        }

        [Fact]
        public void ChangeOrderNo_OrderNoOne_ChangesOrderNo()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.ChangeOrderNo(1);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.OrderNo.Should().Be(1);
            }
        }

        private sealed class Fixture
        {
            public UserId ChangedBy { get; private set; }

            public void SetupChangedBy()
            {
                ChangedBy = new UserIdBuilder().Create();
            }

            public DishCategory CreateTestObject()
            {
                return new DishCategoryBuilder()
                    .WithValidConstrains()
                    .Create();
            }
        }
    }
}
