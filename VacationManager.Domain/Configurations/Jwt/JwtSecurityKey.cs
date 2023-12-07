

using Microsoft.IdentityModel.Tokens;

namespace VacationManager.Domain.Configurations.Jwt
{
    /// <summary>
    /// Représente les paramètres de configuration nécessaires pour générer l'aspect de sécurité
    /// </summary>
    public static class JwtSecurityKey
    {
        public static SymmetricSecurityKey Create(string secret)
        {
            // Vérifie si la chaîne secrète est null. Si c'est le cas, elle lance une exception.
            if (secret == null)
            {
                throw new ArgumentNullException(nameof(secret));
            }

            // Vérifie si la longueur de la chaîne secrète est un multiple de 4. Si ce n'est pas le cas, elle lance une exception.
            if (secret.Length % 4 != 0)
            {
                throw new ArgumentException("Longueur secrète non valide. Il doit s’agir d’une chaîne Base64 valide.");
            }

            // Convertit la chaîne secrète en une clé de sécurité symétrique.
            var key = new SymmetricSecurityKey(Convert.FromBase64String(secret));
            return key;
        }
    }
}
