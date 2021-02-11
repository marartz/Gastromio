using Gastromio.Core.Domain.Model.Restaurants;
using Gastromio.Domain.TestKit.Common;

namespace Gastromio.Domain.TestKit.Domain.Model.Restaurants
{
    public class AddressBuilder : TestObjectBuilderBase<Address>
    {
        protected override void AddDefaultConstraints()
        {
            WithLengthConstrainedStringConstructorArgumentFor("street", 0, 100);
            WithLengthConstrainedStringConstructorArgumentFor("zipCode", 5, 5, "0123456789".ToCharArray());
            WithLengthConstrainedStringConstructorArgumentFor("city", 0, 50);
        }

        public AddressBuilder WithStreet(string street)
        {
            WithConstantConstructorArgumentFor("street", street);
            return this;
        }

        public AddressBuilder WithZipCode(string zipCode)
        {
            WithConstantConstructorArgumentFor("zipCode", zipCode);
            return this;
        }

        public AddressBuilder WithCity(string city)
        {
            WithConstantConstructorArgumentFor("city", city);
            return this;
        }

    }
}
