using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;

namespace VacationManager.Domain.Tools
{
    /// <summary>
    /// Represente un outil pour le Hachage de mot de passe
    /// </summary>
    public class PasswordTool
    {
        public static string HashPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.UTF8.GetBytes("saltForPasswordHashing"),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashed;
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            byte[] salt = new byte[128 / 8];
            byte[] hashedBytes = Convert.FromBase64String(hashedPassword);

            // Utilisez le même sel et les mêmes paramètres de dérivation de clé pour vérifier le mot de passe
            string inputHashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.UTF8.GetBytes("saltForPasswordHashing"),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            // Comparez les hachages pour vérifier si le mot de passe est correct
            return inputHashed == hashedPassword;
        }

    }
}
