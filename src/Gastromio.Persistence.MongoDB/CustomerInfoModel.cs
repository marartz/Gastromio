namespace Gastromio.Persistence.MongoDB
{
    public class CustomerInfoModel
    {
        public string GivenName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string AddAddressInfo { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}