using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace TaskManagementApi.Helpers
{
    public static class PasswordHasher
    {
        public static void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = Convert.ToBase64String(hmac.Key);
                passwordHash = Convert.ToBase64String(
                    KeyDerivation.Pbkdf2(
                        password: password,
                        salt: hmac.Key,
                        prf: KeyDerivationPrf.HMACSHA512,
                        iterationCount: 10000,
                        numBytesRequested: 256 / 8));
            }
        }

        public static bool VerifyPasswordHash(string password, string storedHash, string storedSalt)
        {
            byte[] saltBytes = Convert.FromBase64String(storedSalt);

            string computedHash = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: saltBytes,
                    prf: KeyDerivationPrf.HMACSHA512,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));

            return computedHash == storedHash;
        }
    }
}