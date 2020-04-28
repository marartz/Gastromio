using System.Security.Cryptography;

namespace FoodOrderSystem.Domain.Model.User
{
    public class User
    {
        private const int SALT_BYTES = 24;
        private const int HASH_BYTES = 18;
        private const int PBKDF2_ITERATIONS = 64000;

        public User(UserId id, string name, Role role, byte[] passwordSalt, byte[] passwordHash)
        {
            Id = id;
            Name = name;
            Role = role;
            PasswordSalt = passwordSalt;
            PasswordHash = passwordHash;
        }

        public UserId Id { get; }
        public string Name { get; private set; }
        public Role Role { get; private set; }
        public string Email { get; private set; }
        public byte[] PasswordSalt { get; private set; }
        public byte[] PasswordHash { get; private set; }

        public void ChangeDetails(string name, Role role, string email)
        {
            Name = name;
            Role = role;
            Email = email;
        }

        public bool ValidatePassword(string password)
        {
            byte[] testPasswordHash = PBKDF2(password, PasswordSalt, PBKDF2_ITERATIONS, HASH_BYTES);
            return SlowEquals(PasswordHash, testPasswordHash);
        }

        public void ChangePassword(string password)
        {
            var newPasswordSalt = new byte[SALT_BYTES];
            using (var csprng = new RNGCryptoServiceProvider())
            {
                csprng.GetBytes(newPasswordSalt);
            }

            var newPasswordHash = PBKDF2(password, newPasswordSalt, PBKDF2_ITERATIONS, HASH_BYTES);

            PasswordSalt = newPasswordSalt;
            PasswordHash = newPasswordHash;
        }

        private static bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
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
