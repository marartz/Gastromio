using System.Linq;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;

namespace Gastromio.Core.Domain.Model.Restaurants
{
    public class Address
    {
        public Address(string street, string zipCode, string city)
        {
            ValidateStreet(street);
            ValidateZipCode(zipCode);
            ValidateCity(city);

            Street = street;
            ZipCode = zipCode;
            City = city;
        }

        public string Street { get; }

        public string ZipCode { get; }

        public string City { get; }

        private static void ValidateStreet(string street)
        {
            if (string.IsNullOrEmpty(street))
                throw DomainException.CreateFrom(new RestaurantStreetRequiredFailure());
            if (street.Length > 100)
                throw DomainException.CreateFrom(new RestaurantStreetTooLongFailure());
            if (!Validators.IsValidStreet(street))
                throw DomainException.CreateFrom(new RestaurantStreetInvalidFailure());
        }

        private static void ValidateZipCode(string zipCode)
        {
            if (string.IsNullOrEmpty(zipCode))
                throw DomainException.CreateFrom(new RestaurantZipCodeRequiredFailure());
            if (zipCode.Length != 5 || zipCode.Any(en => !char.IsDigit(en)))
                throw DomainException.CreateFrom(new RestaurantZipCodeInvalidFailure());
            if (!Validators.IsValidZipCode(zipCode))
                throw DomainException.CreateFrom(new RestaurantZipCodeInvalidFailure());
        }

        private static void ValidateCity(string city)
        {
            if (string.IsNullOrEmpty(city))
                throw DomainException.CreateFrom(new RestaurantCityRequiredFailure());
            if (city.Length > 50)
                throw DomainException.CreateFrom(new RestaurantCityTooLongFailure());
        }
    }
}

