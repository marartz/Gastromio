namespace FoodOrderSystem.Domain.Model.Restaurant
{
    public class ContactInfo
    {
        public ContactInfo(string phone, string fax, string webSite, string responsiblePerson, string emailAddress)
        {
            Phone = phone;
            Fax = fax;
            WebSite = webSite;
            ResponsiblePerson = responsiblePerson;
            EmailAddress = emailAddress;
        }
        
        public string Phone { get; }
        public string Fax { get; }
        public string WebSite { get; }
        public string ResponsiblePerson { get; }
        public string EmailAddress { get; }
    }
}