using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RandomNumbers.Utilities
{
    public class HashService : IHashService
    {
        private readonly byte[] salt;

        public HashService(string salt)
        {
            this.salt = Encoding.UTF8.GetBytes(salt);
        }

        public Task<string> HashText(string plainText)
        {
            using (var sha512 = new SHA512Managed())
            {
                var bytesOfText = Encoding.UTF8.GetBytes(plainText);

                var saltedValue = new byte[bytesOfText.Length + salt.Length];

                bytesOfText.CopyTo(saltedValue, 0);
                salt.CopyTo(saltedValue, bytesOfText.Length);

                var result = sha512.ComputeHash(saltedValue);

                return Task.FromResult(Convert.ToBase64String(result));
            }
        }
    }
}
