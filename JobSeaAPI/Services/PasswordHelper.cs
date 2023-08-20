using System.Security.Cryptography;
using System.Text;

namespace JobSeaAPI.Services
{
    public class PasswordHelper : IPasswordHelper
    {
        private readonly int _iterations;
        private HashAlgorithmName _hashAlgorithm;
        private int _keySize;
        PasswordHelper() 
        { 
            _iterations = 35000;
            _hashAlgorithm = HashAlgorithmName.SHA512;
            _keySize = 64;
        }
        public string HashPassword(string password, out byte[] salt)
        {
            salt = RandomNumberGenerator.GetBytes(_keySize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), salt, _iterations, _hashAlgorithm, _keySize);
            return Convert.ToHexString(hash);
        }

        public bool VerifyPassword(string password, string hash, byte[] salt)
        {
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, _iterations, _hashAlgorithm, _keySize);
            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
        }
    }
}
