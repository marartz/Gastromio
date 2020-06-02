using System;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace FoodOrderSystem.Domain.Model
{
    public static class Validators
    {
        public static bool IsValidEmailAddress(string value)
        {
            try
            {
                var m = new MailAddress(value);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static bool IsValidStreet(string value)
        {
            return StreetRegex.IsMatch(value);
        }

        public static bool IsValidZipCode(string value)
        {
            return ZipCodeRegex.IsMatch(value);
        }

        public static bool IsValidPhoneNumber(string value)
        {
            return PhoneNumberRegex.IsMatch(value);
        }

        public static bool IsValidWebsite(string value)
        {
            return WebsiteRegex.IsMatch(value);
        }

        public static bool IsValidPassword(string value)
        {
            return PasswordRegex.IsMatch(value);
        }

        private static readonly Regex StreetRegex = new Regex(@"^(([a-zA-ZäöüÄÖÜ]\D*)\s+\d+?\s*.*)$");

        private static readonly Regex ZipCodeRegex =
            new Regex(@"^((0(1\d\d[1-9])|([2-9]\d\d\d))|(?(?=^(^9{5}))|[1-9]\d{4}))$");

        private static readonly Regex PhoneNumberRegex =
            new Regex(
                @"^(((((((00|\+)49[ \-/]?)|0)[1-9][0-9]{1,4})[ \-/]?)|((((00|\+)49\()|\(0)[1-9][0-9]{1,4}\)[ \-/]?))[0-9]{1,7}([ \-/]?[0-9]{1,5})?)$");

        private static readonly Regex WebsiteRegex =
            new Regex(
                @"^(https?:\/\/){0,1}(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)$");

        private static readonly Regex PasswordRegex =
            new Regex(
                @"(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&]).{6,}");
    }
}