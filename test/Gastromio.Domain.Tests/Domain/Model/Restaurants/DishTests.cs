using System;
using System.Collections.Generic;
using System.Linq;
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
    public class DishTests
    {
        private readonly Fixture fixture;

        public DishTests()
        {
            fixture = new Fixture();
        }

        [Fact]
        public void ChangeName_NameNull_ReturnsFailure()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomVariants();
            var testObject = fixture.CreateTestObject();

            // Act
            Action act = () => testObject.ChangeName(null);

            // Assert
            act.Should().Throw<DomainException<DishNameRequiredFailure>>();
        }

        [Fact]
        public void ChangeName_NameEmpty_ReturnsFailure()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomVariants();
            var testObject = fixture.CreateTestObject();

            // Act
            Action act = () => testObject.ChangeName(string.Empty);

            // Assert
            act.Should().Throw<DomainException<DishNameRequiredFailure>>();
        }

        [Fact]
        public void ChangeName_NameLength41_ReturnsFailure()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomVariants();
            var testObject = fixture.CreateTestObject();

            var name = RandomStringBuilder.BuildWithLength(41);

            // Act
            Action act = () => testObject.ChangeName(name);

            // Assert
            act.Should().Throw<DomainException<DishNameTooLongFailure>>();
        }

        [Fact]
        public void ChangeName_NameLength40_ChangesNameAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomVariants();
            var testObject = fixture.CreateTestObject();

            var name = RandomStringBuilder.BuildWithLength(40);

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
        public void ChangeDescription_DescriptionNull_ReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomVariants();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.ChangeDescription(null);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Description.Should().BeNull();
            }
        }

        [Fact]
        public void ChangeDescription_DescriptionEmpty_ReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomVariants();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.ChangeDescription(string.Empty);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Description.Should().BeEmpty();
            }
        }

        [Fact]
        public void ChangeDescription_DescriptionLength201_ReturnsFailure()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomVariants();
            var testObject = fixture.CreateTestObject();

            var description = RandomStringBuilder.BuildWithLength(201);

            // Act
            Action act = () => testObject.ChangeDescription(description);

            // Assert
            act.Should().Throw<DomainException<DishDescriptionTooLongFailure>>();
        }

        [Fact]
        public void ChangeDescription_DescriptionLength200_ChangesDescriptionAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomVariants();
            var testObject = fixture.CreateTestObject();

            var description = RandomStringBuilder.BuildWithLength(200);

            // Act
            var result = testObject.ChangeDescription(description);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Description.Should().Be(description);
            }
        }

        [Fact]
        public void ChangeProductInfo_ProductInfoNull_ReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomVariants();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.ChangeProductInfo(null);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.ProductInfo.Should().BeNull();
            }
        }

        [Fact]
        public void ChangeProductInfo_ProductInfoEmpty_ReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomVariants();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.ChangeProductInfo(string.Empty);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.ProductInfo.Should().BeEmpty();
            }
        }

        [Fact]
        public void ChangeProductInfo_ProductInfoLength201_ReturnsFailure()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomVariants();
            var testObject = fixture.CreateTestObject();

            var productInfo = RandomStringBuilder.BuildWithLength(201);

            // Act
            Action act = () => testObject.ChangeProductInfo(productInfo);

            // Assert
            act.Should().Throw<DomainException<DishProductInfoTooLongFailure>>();
        }

        [Fact]
        public void ChangeProductInfo_ProductInfoLength200_ChangesProductInfoAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomVariants();
            var testObject = fixture.CreateTestObject();

            var productInfo = RandomStringBuilder.BuildWithLength(200);

            // Act
            var result = testObject.ChangeProductInfo(productInfo);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.ProductInfo.Should().Be(productInfo);
            }
        }

        [Fact]
        public void ChangeOrderNo_OrderNoNegative_ReturnsFailure()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomVariants();
            var testObject = fixture.CreateTestObject();

            // Act
            Action act = () => testObject.ChangeOrderNo(-1);

            // Assert
            act.Should().Throw<DomainException<DishInvalidOrderNoFailure>>();
        }

        [Fact]
        public void ChangeOrderNo_OrderNoOne_ChangesOrderNoAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomVariants();
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

        [Fact]
        public void AddDishVariant_VariantAlreadyExists_ThrowsException()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomVariants();
            var testObject = fixture.CreateTestObject();

            var variant = new DishVariantBuilder()
                .WithId(fixture.Variants.First().Id)
                .WithValidConstrains()
                .Create();

            // Act
            Action act = () => testObject.AddDishVariant(variant);

            // Assert
            act.Should().Throw<DomainException<DishVariantAlreadyExistsFailure>>();
        }

        [Fact]
        public void AddDishVariant_ValidParameters_AddsVariantAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomVariants();
            var testObject = fixture.CreateTestObject();

            var variant = new DishVariantBuilder()
                .WithId(fixture.Variants.First().Id)
                .WithValidConstrains()
                .Create();

            // Act
            var result = testObject.AddDishVariant(variant);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                testObject.Variants.Any(v => v.Id == variant.Id).Should().BeTrue();
            }
        }

        [Fact]
        public void RemoveVariant_VariantDoesNotExist_ThrowsException()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomVariants();
            var testObject = fixture.CreateTestObject();

            var variantId = new DishVariantId(Guid.NewGuid());

            // Act
            Action act = () => testObject.RemoveDishVariant(variantId);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void RemoveVariant_VariantExists_RemovesVariantsAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomVariants();
            var testObject = fixture.CreateTestObject();

            var variantId = fixture.Variants.First().Id;

            // Act
            var result = testObject.RemoveDishVariant(variantId);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                testObject.Variants.Any(v => v.Id == variantId).Should().BeFalse();
            }
        }

        [Fact]
        public void ReplaceVariants_NewVariantsNull_RemovesAllVariantsAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomVariants();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.ReplaceDishVariants(null);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                testObject.Variants.Should().BeEmpty();
            }
        }

        [Fact]
        public void ReplaceVariant_VariantExists_ReplacesVariantsAndReturnsSuccess()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomVariants();
            var testObject = fixture.CreateTestObject();

            var newVariants = new DishVariantBuilder()
                .WithValidConstrains()
                .CreateMany(2)
                .ToList();

            // Act
            var result = testObject.ReplaceDishVariants(new DishVariants(newVariants));

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                testObject.Variants.Should().BeEquivalentTo(newVariants);
            }
        }

        private sealed class Fixture
        {
            public UserId ChangedBy { get; private set; }

            public IList<DishVariant> Variants { get; private set; }

            public void SetupChangedBy()
            {
                ChangedBy = new UserIdBuilder().Create();
            }

            public void SetupRandomVariants()
            {
                Variants = new DishVariantBuilder()
                    .WithValidConstrains()
                    .CreateMany(3)
                    .ToList();
            }

            public Dish CreateTestObject()
            {
                return new DishBuilder()
                    .WithVariants(Variants)
                    .WithValidConstrains()
                    .Create();
            }
        }
    }
}
