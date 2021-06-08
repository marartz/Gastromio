using System;
using FluentAssertions;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Restaurants;
using Gastromio.Domain.TestKit.Common;
using Xunit;

namespace Gastromio.Domain.Tests.Domain.Model.Restaurants
{
    public class DishVariantTests
    {
        private readonly Fixture fixture;

        public DishVariantTests()
        {
            fixture = new Fixture();
        }

        [Fact]
        public void Ctor_NameTooLong_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupNameWithTooLongString();
            fixture.SetupPriceWithValidValue();

            // Act
            // ReSharper disable once ObjectCreationAsStatement
            Action act = () => new DishVariant(
                new DishVariantId(Guid.NewGuid()),
                fixture.Name,
                fixture.Price
            );

            // Assert
            act.Should().Throw<DomainException<DishVariantNameTooLongFailure>>();
        }

        [Fact]
        public void Ctor_PriceNegative_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupNameWithValidString();
            fixture.SetupPriceWithNegativeValue();

            // Act
            // ReSharper disable once ObjectCreationAsStatement
            Action act = () => new DishVariant(
                new DishVariantId(Guid.NewGuid()),
                fixture.Name,
                fixture.Price
            );

            // Assert
            act.Should().Throw<DomainException<DishVariantPriceIsNegativeOrZeroFailure>>();
        }

        [Fact]
        public void Ctor_PriceZero_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupNameWithValidString();
            fixture.SetupPriceWithZeroValue();

            // Act
            // ReSharper disable once ObjectCreationAsStatement
            Action act = () => new DishVariant(
                new DishVariantId(Guid.NewGuid()),
                fixture.Name,
                fixture.Price
            );

            // Assert
            act.Should().Throw<DomainException<DishVariantPriceIsNegativeOrZeroFailure>>();
        }

        [Fact]
        public void Ctor_PriceTooBig_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupNameWithValidString();
            fixture.SetupPriceWithTooBigValue();

            // Act
            // ReSharper disable once ObjectCreationAsStatement
            Action act = () => new DishVariant(
                new DishVariantId(Guid.NewGuid()),
                fixture.Name,
                fixture.Price
            );

            // Assert
            act.Should().Throw<DomainException<DishVariantPriceIsTooBigFailure>>();
        }

        [Fact]
        public void Ctor_AllValid_NameNull_CreatesInstance()
        {
            // Arrange
            fixture.SetupNameWithNull();
            fixture.SetupPriceWithValidValue();

            // Act
            // ReSharper disable once ObjectCreationAsStatement
            var dishVariant = new DishVariant(
                new DishVariantId(Guid.NewGuid()),
                fixture.Name,
                fixture.Price
            );

            // Assert
            dishVariant.Should().NotBeNull();
        }

        [Fact]
        public void Ctor_AllValid_NameValid_CreatesInstance()
        {
            // Arrange
            fixture.SetupNameWithValidString();
            fixture.SetupPriceWithValidValue();

            // Act
            // ReSharper disable once ObjectCreationAsStatement
            var dishVariant = new DishVariant(
                new DishVariantId(Guid.NewGuid()),
                fixture.Name,
                fixture.Price
            );

            // Assert
            dishVariant.Should().NotBeNull();
        }

        private sealed class Fixture
        {
            public string Name { get; private set; }

            public decimal Price { get; private set; }

            public void SetupNameWithNull()
            {
                Name = null;
            }

            public void SetupNameWithValidString()
            {
                Name = RandomStringBuilder.BuildWithLength(40);
            }

            public void SetupNameWithTooLongString()
            {
                Name = RandomStringBuilder.BuildWithLength(41);
            }

            public void SetupPriceWithNegativeValue()
            {
                Price = -0.01m;
            }

            public void SetupPriceWithZeroValue()
            {
                Price = 0m;
            }

            public void SetupPriceWithValidValue()
            {
                Price = 200m;
            }

            public void SetupPriceWithTooBigValue()
            {
                Price = 200.01m;
            }
        }
    }
}
