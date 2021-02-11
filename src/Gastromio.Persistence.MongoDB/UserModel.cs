using System;

namespace Gastromio.Persistence.MongoDB
{
    public class UserModel
    {
        public Guid Id { get; set; }

        public string Role { get; set; }

        public string Email { get; set; }

        public byte[] PasswordSalt { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordResetCode { get; set; }

        public DateTimeOffset? PasswordResetExpiration { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTimeOffset UpdatedOn { get; set; }

        public Guid UpdatedBy { get; set; }
    }
}
