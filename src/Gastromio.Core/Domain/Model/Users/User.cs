using System;
using System.Security.Cryptography;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;

namespace Gastromio.Core.Domain.Model.Users
{
    public class User
    {
        private const int SALT_BYTES = 24;
        private const int HASH_BYTES = 18;
        private const int PBKDF2_ITERATIONS = 64000;

        public User(
            UserId id,
            Role role,
            string email,
            byte[] passwordSalt,
            byte[] passwordHash,
            byte[] passwordResetCode,
            DateTimeOffset? passwordResetExpiration,
            DateTimeOffset createdOn,
            UserId createdBy,
            DateTimeOffset updatedOn,
            UserId updatedBy
        )
        {
            ValidateEmail(email);

            Id = id;
            Role = role;
            Email = email;
            PasswordSalt = passwordSalt;
            PasswordHash = passwordHash;
            PasswordResetCode = passwordResetCode;
            PasswordResetExpiration = passwordResetExpiration;
            CreatedOn = createdOn;
            CreatedBy = createdBy;
            UpdatedOn = updatedOn;
            UpdatedBy = updatedBy;
        }

        public UserId Id { get; }

        public Role Role { get; private set; }

        public string Email { get; private set; }

        public byte[] PasswordSalt { get; private set; }

        public byte[] PasswordHash { get; private set; }

        public byte[] PasswordResetCode { get; private set; }

        public DateTimeOffset? PasswordResetExpiration { get; private set; }

        public DateTimeOffset CreatedOn { get; }

        public UserId CreatedBy { get; }

        public DateTimeOffset UpdatedOn { get; private set; }

        public UserId UpdatedBy { get; private set; }

        public void ChangeDetails(Role role, string email, UserId changedBy)
        {
            ValidateEmail(email);

            Role = role;
            Email = email;
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public bool ValidatePassword(string password)
        {
            var testPasswordHash = PBKDF2(password, PasswordSalt, PBKDF2_ITERATIONS, HASH_BYTES);
            var validationResult = SlowEquals(PasswordHash, testPasswordHash);
            return validationResult;
        }

        public void ChangePassword(string password, bool checkPasswordPolicy, UserId changedBy)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));

            if (checkPasswordPolicy && !Validators.IsValidPassword(password))
                throw DomainException.CreateFrom(new PasswordIsNotValidFailure());

            var newPasswordSalt = new byte[SALT_BYTES];
            using (var csprng = new RNGCryptoServiceProvider())
            {
                csprng.GetBytes(newPasswordSalt);
            }

            var newPasswordHash = PBKDF2(password, newPasswordSalt, PBKDF2_ITERATIONS, HASH_BYTES);

            PasswordSalt = newPasswordSalt;
            PasswordHash = newPasswordHash;
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = changedBy;
        }

        public void GeneratePasswordResetCode()
        {
            var resetCode = Guid.NewGuid();
            PasswordResetCode = resetCode.ToByteArray();
            PasswordResetExpiration = DateTimeOffset.UtcNow.AddMinutes(30);
        }

        public bool ValidatePasswordResetCode(byte[] resetCode)
        {
            if (resetCode == null)
                throw new ArgumentNullException(nameof(resetCode));
            return PasswordResetExpiration.HasValue && DateTimeOffset.UtcNow <= PasswordResetExpiration &&
                   SlowEquals(PasswordResetCode, resetCode);
        }

        public void ChangePasswordWithResetCode(byte[] resetCode, string password)
        {
            if (!ValidatePasswordResetCode(resetCode))
                throw DomainException.CreateFrom(new PasswordResetCodeIsInvalidFailure());
            ChangePassword(password, true, Id);
            PasswordResetCode = null;
            PasswordResetExpiration = null;
        }

        private static void ValidateEmail(string emailAddress)
        {
            if (string.IsNullOrEmpty(emailAddress))
                throw DomainException.CreateFrom(new UserEmailRequiredFailure());
            if (!Validators.IsValidEmailAddress(emailAddress))
                throw DomainException.CreateFrom(new UserEmailInvalidFailure());
        }

        private static bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint) a.Length ^ (uint) b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= (uint) (a[i] ^ b[i]);
            }

            return diff == 0;
        }

        private static byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt))
            {
                pbkdf2.IterationCount = iterations;
                return pbkdf2.GetBytes(outputBytes);
            }
        }
    }
}
