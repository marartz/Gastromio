namespace FoodOrderSystem.Domain.Model.Restaurant
{
    public class Address
    {
        public Address(string line1, string line2, string zipCode, string city)
        {
            Line1 = line1;
            Line2 = line2;
            ZipCode = zipCode;
            City = city;
        }

        public string Line1 { get; }
        public string Line2 { get; }
        public string ZipCode { get; }
        public string City { get; }
    }
}
