using System;
using System.Linq;
using System.Security.Cryptography;

namespace Apollo.Utilities
{
    public interface IPasswordHasher
    {
        string GenerateHash(string password);
        bool CheckHash(string hash, string password);
    }

    public class PasswordHasher : IPasswordHasher
    {
        private readonly IBase64Converter base64Converter;
        private const int SaltByteSize = 64;
        private const int HashByteSize = 128;
        private const int HasingIterationsCount = 4242;

        public PasswordHasher()
        {
            this.base64Converter = new Base64Converter();
        }

        public string GenerateHash(string password)
        {
            var salt = GenerateSalt();
            var hash = ComputeHash(password, salt);
            return hash;
        }

        public bool CheckHash(string hash, string password)
        {
            var hashBytes = this.base64Converter.Decode(hash);
            var salt = ExtractSalt(hashBytes);
            var checkHash = ComputeHash(password, salt);
            return hash == checkHash;
        }

        private static byte[] ExtractSalt(byte[] hashBytes)
        {
            var salt = new byte[SaltByteSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltByteSize);
            return salt;
        }

        private static byte[] GenerateSalt()
        {
            using (var saltGenerator = RandomNumberGenerator.Create())
            {
                var salt = new byte[SaltByteSize];
                saltGenerator.GetBytes(salt);
                return salt;
            }
        }

        private string ComputeHash(string password, byte[] salt)
        {
            using (var hashGenerator = new Rfc2898DeriveBytes(password, salt))
            {
                hashGenerator.IterationCount = HasingIterationsCount;
                var hashed = hashGenerator.GetBytes(HashByteSize);
                var fullarray = salt.Concat(hashed);
                return this.base64Converter.Encode(fullarray.ToArray());
            }
        }
    }
}
