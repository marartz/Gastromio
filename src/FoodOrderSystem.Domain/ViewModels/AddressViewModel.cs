using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.ViewModels
{
    public class AddressViewModel
    {
        public AddressViewModel(
            string street,
            string zipCode,
            string city
        )
        {
            Street = street;
            ZipCode = zipCode;
            City = city;
        }

        public string Street { get; }

        public string ZipCode { get; }

        public string City { get; }

        public static AddressViewModel FromAddress(Address address)
        {
            return new AddressViewModel(address.Street, address.ZipCode, address.City);
        }
    }
}
