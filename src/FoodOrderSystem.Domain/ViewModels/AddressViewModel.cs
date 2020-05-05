using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.ViewModels
{
    public class AddressViewModel
    {
        public string Street { get; set; }

        public string ZipCode { get; set; }

        public string City { get; set; }

        public static AddressViewModel FromAddress(Address address)
        {
            return new AddressViewModel
            {
                Street = address.Street,
                ZipCode = address.ZipCode,
                City = address.City
            };
        }
    }
}
