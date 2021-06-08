using System;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Restaurants;
using Gastromio.Domain.TestKit.Common;
using Gastromio.Domain.TestKit.Domain.Model.Restaurants;
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
        public void Ctor_NameNull_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupNameWithNull();
            fixture.SetupDescriptionWithValidString();
            fixture.SetupProductInfoWithValidString();
            fixture.SetupOrderNoWithValidValue();
            fixture.SetupRandomVariants();

            // Act
            // ReSharper disable once ObjectCreationAsStatement
            Action act = () => new Dish(
                new DishId(Guid.NewGuid()),
                fixture.Name,
                fixture.Description,
                fixture.ProductInfo,
                fixture.OrderNo,
                fixture.Variants
            );

            // Assert
            act.Should().Throw<DomainException<DishNameRequiredFailure>>();
        }

        [Fact]
        public void Ctor_NameEmpty_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupNameWithEmptyString();
            fixture.SetupDescriptionWithValidString();
            fixture.SetupProductInfoWithValidString();
            fixture.SetupOrderNoWithValidValue();
            fixture.SetupRandomVariants();

            // Act
            // ReSharper disable once ObjectCreationAsStatement
            Action act = () => new Dish(
                new DishId(Guid.NewGuid()),
                fixture.Name,
                fixture.Description,
                fixture.ProductInfo,
                fixture.OrderNo,
                fixture.Variants
            );

            // Assert
            act.Should().Throw<DomainException<DishNameRequiredFailure>>();
        }

        [Fact]
        public void Ctor_NameTooLong_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupNameWithTooLongString();
            fixture.SetupDescriptionWithValidString();
            fixture.SetupProductInfoWithValidString();
            fixture.SetupOrderNoWithValidValue();
            fixture.SetupRandomVariants();

            // Act
            // ReSharper disable once ObjectCreationAsStatement
            Action act = () => new Dish(
                new DishId(Guid.NewGuid()),
                fixture.Name,
                fixture.Description,
                fixture.ProductInfo,
                fixture.OrderNo,
                fixture.Variants
            );

            // Assert
            act.Should().Throw<DomainException<DishNameTooLongFailure>>();
        }

        [Fact]
        public void Ctor_DescriptionTooLong_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupNameWithValidString();
            fixture.SetupDescriptionWithTooLongString();
            fixture.SetupProductInfoWithValidString();
            fixture.SetupOrderNoWithValidValue();
            fixture.SetupRandomVariants();

            // Act
            // ReSharper disable once ObjectCreationAsStatement
            Action act = () => new Dish(
                new DishId(Guid.NewGuid()),
                fixture.Name,
                fixture.Description,
                fixture.ProductInfo,
                fixture.OrderNo,
                fixture.Variants
            );

            // Assert
            act.Should().Throw<DomainException<DishDescriptionTooLongFailure>>();
        }

        [Fact]
        public void Ctor_ProductInfoTooLong_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupNameWithValidString();
            fixture.SetupDescriptionWithValidString();
            fixture.SetupProductInfoWithTooLongString();
            fixture.SetupOrderNoWithValidValue();
            fixture.SetupRandomVariants();

            // Act
            // ReSharper disable once ObjectCreationAsStatement
            Action act = () => new Dish(
                new DishId(Guid.NewGuid()),
                fixture.Name,
                fixture.Description,
                fixture.ProductInfo,
                fixture.OrderNo,
                fixture.Variants
            );

            // Assert
            act.Should().Throw<DomainException<DishProductInfoTooLongFailure>>();
        }

        [Fact]
        public void Ctor_OrderNoNegative_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupNameWithValidString();
            fixture.SetupDescriptionWithValidString();
            fixture.SetupProductInfoWithValidString();
            fixture.SetupOrderNoWithNegativeValue();
            fixture.SetupRandomVariants();

            // Act
            // ReSharper disable once ObjectCreationAsStatement
            Action act = () => new Dish(
                new DishId(Guid.NewGuid()),
                fixture.Name,
                fixture.Description,
                fixture.ProductInfo,
                fixture.OrderNo,
                fixture.Variants
            );

            // Assert
            act.Should().Throw<DomainException<DishInvalidOrderNoFailure>>();
        }

        [Fact]
        public void Ctor_AllValid_CreatesInstance()
        {
            // Arrange
            fixture.SetupNameWithValidString();
            fixture.SetupDescriptionWithValidString();
            fixture.SetupProductInfoWithValidString();
            fixture.SetupOrderNoWithValidValue();
            fixture.SetupRandomVariants();

            // Act
            var dish = new Dish(
                new DishId(Guid.NewGuid()),
                fixture.Name,
                fixture.Description,
                fixture.ProductInfo,
                fixture.OrderNo,
                fixture.Variants
            );

            // Assert
            dish.Should().NotBeNull();
        }

        [Fact]
        public void ChangeName_NameNull_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupNameWithValidString();
            fixture.SetupDescriptionWithValidString();
            fixture.SetupProductInfoWithValidString();
            fixture.SetupOrderNoWithValidValue();
            fixture.SetupRandomVariants();
            var testObject = fixture.CreateTestObject();

            // Act
            Action act = () => testObject.ChangeName(null);

            // Assert
            act.Should().Throw<DomainException<DishNameRequiredFailure>>();
        }

        [Fact]
        public void ChangeName_NameEmpty_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupNameWithValidString();
            fixture.SetupDescriptionWithValidString();
            fixture.SetupProductInfoWithValidString();
            fixture.SetupOrderNoWithValidValue();
            fixture.SetupRandomVariants();
            var testObject = fixture.CreateTestObject();

            // Act
            Action act = () => testObject.ChangeName(string.Empty);

            // Assert
            act.Should().Throw<DomainException<DishNameRequiredFailure>>();
        }

        [Fact]
        public void ChangeName_NameLength101_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupNameWithValidString();
            fixture.SetupDescriptionWithValidString();
            fixture.SetupProductInfoWithValidString();
            fixture.SetupOrderNoWithValidValue();
            fixture.SetupRandomVariants();
            var testObject = fixture.CreateTestObject();

            var name = RandomStringBuilder.BuildWithLength(101);

            // Act
            Action act = () => testObject.ChangeName(name);

            // Assert
            act.Should().Throw<DomainException<DishNameTooLongFailure>>();
        }

        [Fact]
        public void ChangeName_NameLength100_ChangesName()
        {
            // Arrange
            fixture.SetupNameWithValidString();
            fixture.SetupDescriptionWithValidString();
            fixture.SetupProductInfoWithValidString();
            fixture.SetupOrderNoWithValidValue();
            fixture.SetupRandomVariants();
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
        public void ChangeDescription_DescriptionNull_ReturnsChangedDish()
        {
            // Arrange
            fixture.SetupNameWithValidString();
            fixture.SetupDescriptionWithValidString();
            fixture.SetupProductInfoWithValidString();
            fixture.SetupOrderNoWithValidValue();
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
        public void ChangeDescription_DescriptionEmpty_ReturnsChangedDish()
        {
            // Arrange
            fixture.SetupNameWithValidString();
            fixture.SetupDescriptionWithValidString();
            fixture.SetupProductInfoWithValidString();
            fixture.SetupOrderNoWithValidValue();
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
        public void ChangeDescription_DescriptionLength501_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupNameWithValidString();
            fixture.SetupDescriptionWithValidString();
            fixture.SetupProductInfoWithValidString();
            fixture.SetupOrderNoWithValidValue();
            fixture.SetupRandomVariants();
            var testObject = fixture.CreateTestObject();

            var description = RandomStringBuilder.BuildWithLength(501);

            // Act
            Action act = () => testObject.ChangeDescription(description);

            // Assert
            act.Should().Throw<DomainException<DishDescriptionTooLongFailure>>();
        }

        [Fact]
        public void ChangeDescription_DescriptionLength500_ChangesDescription()
        {
            // Arrange
            fixture.SetupNameWithValidString();
            fixture.SetupDescriptionWithValidString();
            fixture.SetupProductInfoWithValidString();
            fixture.SetupOrderNoWithValidValue();
            fixture.SetupRandomVariants();
            var testObject = fixture.CreateTestObject();

            var description = RandomStringBuilder.BuildWithLength(500);

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
        public void ChangeProductInfo_ProductInfoNull_ReturnsChangedDish()
        {
            // Arrange
            fixture.SetupNameWithValidString();
            fixture.SetupDescriptionWithValidString();
            fixture.SetupProductInfoWithValidString();
            fixture.SetupOrderNoWithValidValue();
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
        public void ChangeProductInfo_ProductInfoEmpty_ReturnsChangedDish()
        {
            // Arrange
            fixture.SetupNameWithValidString();
            fixture.SetupDescriptionWithValidString();
            fixture.SetupProductInfoWithValidString();
            fixture.SetupOrderNoWithValidValue();
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
        public void ChangeProductInfo_ProductInfoLength501_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupNameWithValidString();
            fixture.SetupDescriptionWithValidString();
            fixture.SetupProductInfoWithValidString();
            fixture.SetupOrderNoWithValidValue();
            fixture.SetupRandomVariants();
            var testObject = fixture.CreateTestObject();

            var productInfo = RandomStringBuilder.BuildWithLength(501);

            // Act
            Action act = () => testObject.ChangeProductInfo(productInfo);

            // Assert
            act.Should().Throw<DomainException<DishProductInfoTooLongFailure>>();
        }

        [Fact]
        public void ChangeProductInfo_ProductInfoLength500_ChangesProductInfo()
        {
            // Arrange
            fixture.SetupNameWithValidString();
            fixture.SetupDescriptionWithValidString();
            fixture.SetupProductInfoWithValidString();
            fixture.SetupOrderNoWithValidValue();
            fixture.SetupRandomVariants();
            var testObject = fixture.CreateTestObject();

            var productInfo = RandomStringBuilder.BuildWithLength(500);

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
        public void ChangeOrderNo_OrderNoNegative_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupNameWithValidString();
            fixture.SetupDescriptionWithValidString();
            fixture.SetupProductInfoWithValidString();
            fixture.SetupOrderNoWithValidValue();
            fixture.SetupRandomVariants();
            var testObject = fixture.CreateTestObject();

            // Act
            Action act = () => testObject.ChangeOrderNo(-1);

            // Assert
            act.Should().Throw<DomainException<DishInvalidOrderNoFailure>>();
        }

        [Fact]
        public void ChangeOrderNo_OrderNoOne_ChangesOrderNo()
        {
            // Arrange
            fixture.SetupNameWithValidString();
            fixture.SetupDescriptionWithValidString();
            fixture.SetupProductInfoWithValidString();
            fixture.SetupOrderNoWithValidValue();
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
            fixture.SetupNameWithValidString();
            fixture.SetupDescriptionWithValidString();
            fixture.SetupProductInfoWithValidString();
            fixture.SetupOrderNoWithValidValue();
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
        public void AddDishVariant_ValidParameters_AddsVariant()
        {
            // Arrange
            fixture.SetupNameWithValidString();
            fixture.SetupDescriptionWithValidString();
            fixture.SetupProductInfoWithValidString();
            fixture.SetupOrderNoWithValidValue();
            fixture.SetupRandomVariants();
            var testObject = fixture.CreateTestObject();

            var variant = new DishVariantBuilder()
                .WithValidConstrains()
                .Create();

            // Act
            var result = testObject.AddDishVariant(variant);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Variants.Any(v => v.Id == variant.Id).Should().BeTrue();
            }
        }

        [Fact]
        public void RemoveVariant_VariantDoesNotExist_DoesNothing()
        {
            // Arrange
            fixture.SetupNameWithValidString();
            fixture.SetupDescriptionWithValidString();
            fixture.SetupProductInfoWithValidString();
            fixture.SetupOrderNoWithValidValue();
            fixture.SetupRandomVariants();
            var testObject = fixture.CreateTestObject();

            var variantId = new DishVariantId(Guid.NewGuid());

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
        public void RemoveVariant_VariantExists_RemovesVariants()
        {
            // Arrange
            fixture.SetupNameWithValidString();
            fixture.SetupDescriptionWithValidString();
            fixture.SetupProductInfoWithValidString();
            fixture.SetupOrderNoWithValidValue();
            fixture.SetupRandomVariants();
            var testObject = fixture.CreateTestObject();

            var variantId = fixture.Variants.First().Id;

            // Act
            var result = testObject.RemoveDishVariant(variantId);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Variants.Any(v => v.Id == variantId).Should().BeFalse();
            }
        }

        [Fact]
        public void ReplaceVariants_NewVariantsNull_RemovesAllVariants()
        {
            // Arrange
            fixture.SetupNameWithValidString();
            fixture.SetupDescriptionWithValidString();
            fixture.SetupProductInfoWithValidString();
            fixture.SetupOrderNoWithValidValue();
            fixture.SetupRandomVariants();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.ReplaceDishVariants(null);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Variants.Should().BeNull();
            }
        }

        [Fact]
        public void ReplaceVariant_VariantExists_ReplacesVariants()
        {
            // Arrange
            fixture.SetupNameWithValidString();
            fixture.SetupDescriptionWithValidString();
            fixture.SetupProductInfoWithValidString();
            fixture.SetupOrderNoWithValidValue();
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
                result.Variants.Should().BeEquivalentTo(newVariants);
            }
        }

        private sealed class Fixture
        {
            public string Name { get; private set; }

            public string Description { get; private set; }

            public string ProductInfo { get; private set; }

            public int OrderNo { get; private set; }

            public DishVariants Variants { get; private set; }

            public void SetupNameWithNull()
            {
                Name = null;
            }

            public void SetupNameWithEmptyString()
            {
                Name = string.Empty;
            }

            public void SetupNameWithValidString()
            {
                Name = RandomStringBuilder.BuildWithLength(100);
            }

            public void SetupNameWithTooLongString()
            {
                Name = RandomStringBuilder.BuildWithLength(101);
            }

            public void SetupDescriptionWithValidString()
            {
                Description = RandomStringBuilder.BuildWithLength(500);
            }

            public void SetupDescriptionWithTooLongString()
            {
                Description = RandomStringBuilder.BuildWithLength(501);
            }

            public void SetupProductInfoWithValidString()
            {
                ProductInfo = RandomStringBuilder.BuildWithLength(500);
            }

            public void SetupProductInfoWithTooLongString()
            {
                ProductInfo = RandomStringBuilder.BuildWithLength(501);
            }

            public void SetupOrderNoWithNegativeValue()
            {
                OrderNo = -1;
            }

            public void SetupOrderNoWithValidValue()
            {
                OrderNo = 0;
            }

            public void SetupRandomVariants()
            {
                Variants = new DishVariants(
                    new DishVariantBuilder()
                        .WithValidConstrains()
                        .CreateMany(3)
                );
            }

            public Dish CreateTestObject()
            {
                return new DishBuilder()
                    .WithName(Name)
                    .WithDescription(Description)
                    .WithProductInfo(ProductInfo)
                    .WithOrderNo(OrderNo)
                    .WithVariants(Variants)
                    .Create();
            }
        }
    }
}
