using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;

namespace OfflineMessaging.Infrastructure.Extensions.Cryptography
{
    public static class CryptographyExtensions
    {
        public static string Pbkdf2Hash(this string password, string salt)
        {
            string pbkdf2 = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Convert.FromBase64String(salt),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            var hashed = $"{pbkdf2}æ{salt}";

            return hashed;
        }

        public static bool ValidateHash(this string password, string hash)
        {
            if (!hash.Contains('æ'))
                return false;

            var salt = hash.Split('æ')[1];

            var hashedPassword = password.Pbkdf2Hash(salt);

            return hashedPassword == hash;
        }
    }
}
