using System;
using System.Security.Cryptography;

namespace OfflineMessaging.Infrastructure.Helpers
{
    public class CryptographyHelpers
    {
        public static string CreateSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
                return Convert.ToBase64String(salt);
            }
        }
    }
}
