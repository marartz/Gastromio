using System;

namespace FoodOrderSystem.App.Models
{
    public class ChangePasswordWithResetCodeModel
    {
        public Guid UserId { get; set; }

        public string PasswordResetCode { get; set; }

        public string Password { get; set; }
    }
}