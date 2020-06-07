using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.ViewModels
{
    public class ContactInfoViewModel
    {
        public string Phone { get; set; }
        
        public string Fax { get; set; }
        
        public string WebSite { get; set; }
        
        public string ResponsiblePerson { get; set; }
        
        public string EmailAddress { get; set; }
        
        public static ContactInfoViewModel FromContactInfo(ContactInfo contactInfo)
        {
            return new ContactInfoViewModel
            {
                Phone = contactInfo.Phone,
                Fax = contactInfo.Fax,
                WebSite = contactInfo.WebSite,
                ResponsiblePerson = contactInfo.ResponsiblePerson,
                EmailAddress = contactInfo.EmailAddress
            };
        }
    }
}