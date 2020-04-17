using System;
using System.Security.Cryptography;
using System.Text;

namespace SchoolBridge.Helpers.Managers
{
    public class PasswordHandler
    {
        public static string CreatePasswordHash(string pwd)
        {
            return CreatePasswordHash(pwd, CreateSalt());
        }

        public static string CreatePasswordHash(string pwd, string salt)
        {
            string saltAndPwd = String.Concat(pwd, salt);
            string hashedPwd = GetHashString(saltAndPwd);
            var saltPosition = 5;
            hashedPwd = hashedPwd.Insert(saltPosition, salt);
            return hashedPwd;
        }

        public static bool Validate(string password, string passwordHash)
        {
            var saltPosition = 5;
            var saltSize = 10;
            var salt = passwordHash.Substring(saltPosition, saltSize);
            var hashedPassword = CreatePasswordHash(password, salt);
            return hashedPassword == passwordHash;
        }

        private static string CreateSalt()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[20];
            rng.GetBytes(buff);
            var saltSize = 10;
            string salt = Convert.ToBase64String(buff);
            if (salt.Length > saltSize)
            {
                salt = salt.Substring(0, saltSize);
                return salt.ToUpper();
            }

            var saltChar = '^';
            salt = salt.PadRight(saltSize, saltChar);
            return salt.ToUpper();
        }

        private static string GetHashString(string password)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(password))
                sb.Append(b.ToString("X2"));
            return sb.ToString();
        }

        private static byte[] GetHash(string password)
        {
            SHA384 sha = new SHA384CryptoServiceProvider();
            return sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}