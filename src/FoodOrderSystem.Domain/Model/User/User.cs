using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace FoodOrderSystem.Domain.Model.User
{
    public class User
    {
        private const int SALT_BYTES = 24;
        private const int HASH_BYTES = 18;
        private const int PBKDF2_ITERATIONS = 64000;
        
        private static Regex passwordPolicyRegex = new Regex(@"(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&]).{6,}");

        public User(
            UserId id,
            DateTime createdOn,
            UserId createdBy,
            DateTime updatedOn,
            UserId updatedBy
        )
        {
            Id = id;
            CreatedOn = createdOn;
            CreatedBy = createdBy;
            UpdatedOn = updatedOn;
            UpdatedBy = updatedBy;
        }

        public User(
            UserId id,
            Role role,
            string email,
            byte[] passwordSalt,
            byte[] passwordHash,
            DateTime createdOn,
            UserId createdBy,
            DateTime updatedOn,
            UserId updatedBy
        )
        {
            Id = id;
            Role = role;
            Email = email;
            PasswordSalt = passwordSalt;
            PasswordHash = passwordHash;
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

        public DateTime CreatedOn { get; }

        public UserId CreatedBy { get; }

        public DateTime UpdatedOn { get; private set; }

        public UserId UpdatedBy { get; private set; }

        public Result<bool> ChangeDetails(Role role, string email, UserId changedBy)
        {
            Role = role;
            Email = email;
            UpdatedOn = DateTime.UtcNow;
            UpdatedBy = changedBy;

            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> ValidatePassword(string password)
        {
            byte[] testPasswordHash = PBKDF2(password, PasswordSalt, PBKDF2_ITERATIONS, HASH_BYTES);
            var validationResult = SlowEquals(PasswordHash, testPasswordHash);
            return SuccessResult<bool>.Create(validationResult);
        }

        public Result<bool> ChangePassword(string password, bool checkPasswordPolicy, UserId changedBy)
        {
            if (checkPasswordPolicy && !passwordPolicyRegex.IsMatch(password))
                return FailureResult<bool>.Create(FailureResultCode.PasswordIsNotValid);
            
            var newPasswordSalt = new byte[SALT_BYTES];
            using (var csprng = new RNGCryptoServiceProvider())
            {
                csprng.GetBytes(newPasswordSalt);
            }

            var newPasswordHash = PBKDF2(password, newPasswordSalt, PBKDF2_ITERATIONS, HASH_BYTES);

            PasswordSalt = newPasswordSalt;
            PasswordHash = newPasswordHash;
            UpdatedOn = DateTime.UtcNow;
            UpdatedBy = changedBy;

            return SuccessResult<bool>.Create(true);
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