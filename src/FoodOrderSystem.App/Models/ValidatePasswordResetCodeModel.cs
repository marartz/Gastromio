using System;

namespace FoodOrderSystem.App.Models
{
    public class ValidatePasswordResetCodeModel
    {
        public Guid UserId { get; set; }

        public string PasswordResetCode { get; set; }
    }
}