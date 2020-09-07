using System;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace FoodOrderSystem.Core.Domain.Model
{
    internal static class Validators
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

        private static readonly Regex StreetRegex = new Regex(@"^(([a-zA-ZäöüÄÖÜß]\D*)\s+\d+?\s*.*)$");

        private static readonly Regex ZipCodeRegex =
            new Regex(@"^([0]{1}[1-9]{1}|[1-9]{1}[0-9]{1})[0-9]{3}$");

        private static readonly Regex PhoneNumberRegex =
            new Regex(
                @"^(((((((00|\+)49[ \-/]?)|0)[1-9][0-9]{1,4})[ \-/]?)|((((00|\+)49\()|\(0)[1-9][0-9]{1,4}\)[ \-/]?))[0-9]{1,7}([ \-/]?[0-9]{1,5})?)$");

        private static readonly Regex WebsiteRegex =
            new Regex(
                @"^(https?:\/\/){0,1}(www\.)?[-a-zäöüA-ZÄÖÜ0-9@:%._\+~#=]{1,256}\.[a-zäöüA-ZÄÖÜ0-9()]{1,6}\b([-a-zäöüA-ZÄÖÜ0-9()@:%_\+.~#?&//=]*)$");

        private static readonly Regex PasswordRegex =
            new Regex(
                @"(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&]).{6,}");
    }
}