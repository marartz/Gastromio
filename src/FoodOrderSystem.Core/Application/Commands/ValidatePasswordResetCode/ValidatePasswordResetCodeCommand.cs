using System;
using FoodOrderSystem.Core.Domain.Model.User;

namespace FoodOrderSystem.Core.Application.Commands.ValidatePasswordResetCode
{
    public class ValidatePasswordResetCodeCommand : ICommand<bool>
    {
        public ValidatePasswordResetCodeCommand(UserId userId, byte[] passwordResetCode)
        {
            UserId = userId;
            PasswordResetCode = passwordResetCode;
        }

        public UserId UserId { get; }
        public byte[] PasswordResetCode { get; }
    }
}