using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;

namespace Gastromio.Core.Domain.Model.Restaurants
{
    public class ContactInfo
    {
        public ContactInfo(string phone, string fax, string webSite, string responsiblePerson, string emailAddress,
            string mobile, bool orderNotificationByMobile)
        {
            ValidatePhone(phone);
            ValidateFax(fax);
            ValidateWebSite(webSite);
            ValidateResponsiblePerson(responsiblePerson);
            ValidateEmailAddress(emailAddress);
            ValidateMobile(mobile);

            Phone = phone;
            Fax = fax;
            WebSite = webSite;
            ResponsiblePerson = responsiblePerson;
            EmailAddress = emailAddress;
            Mobile = mobile;
            OrderNotificationByMobile = orderNotificationByMobile;
        }

        public string Phone { get; }

        public string Fax { get; }

        public string WebSite { get; }

        public string ResponsiblePerson { get; }

        public string EmailAddress { get; }

        public string Mobile { get; }

        public bool OrderNotificationByMobile { get; }

        private static void ValidatePhone(string phone)
        {
            if (string.IsNullOrEmpty(phone))
                throw DomainException.CreateFrom(new RestaurantPhoneRequiredFailure());
            if (!Validators.IsValidPhoneNumber(phone))
                throw DomainException.CreateFrom(new RestaurantPhoneInvalidFailure(phone));
        }

        private static void ValidateFax(string fax)
        {
            if (!string.IsNullOrEmpty(fax) && !Validators.IsValidPhoneNumber(fax))
                throw DomainException.CreateFrom(new RestaurantFaxInvalidFailure(fax));
        }

        private static void ValidateWebSite(string webSite)
        {
            if (!string.IsNullOrEmpty(webSite) && !Validators.IsValidWebsite(webSite))
                throw DomainException.CreateFrom(new RestaurantWebSiteInvalidFailure(webSite));
        }

        private static void ValidateResponsiblePerson(string responsiblePerson)
        {
            if (string.IsNullOrEmpty(responsiblePerson))
                throw DomainException.CreateFrom(new RestaurantResponsiblePersonRequiredFailure());
        }

        private static void ValidateEmailAddress(string emailAddress)
        {
            if (string.IsNullOrEmpty(emailAddress))
                throw DomainException.CreateFrom(new RestaurantEmailRequiredFailure());
            if (!Validators.IsValidEmailAddress(emailAddress))
                throw DomainException.CreateFrom(new RestaurantEmailInvalidFailure(emailAddress));
        }

        private static void ValidateMobile(string mobile)
        {
            if (!string.IsNullOrEmpty(mobile) && !Validators.IsValidPhoneNumber(mobile))
                throw DomainException.CreateFrom(new RestaurantMobileInvalidFailure(mobile));
        }
    }
}
