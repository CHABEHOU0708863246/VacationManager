using System.ComponentModel.DataAnnotations;

namespace VacationManager.Domain.Configurations.Jwt
{
    /// <summary>
    /// Représente les paramètres de configuration nécessaires pour générer et valider des jetons JSON Web Token (JWT).
    /// </summary>
    public class JwtSettings
    {
        [Required]
        public string Secret { get; set; }

        [Required]
        public string Issuer { get; set; }

        [Required]
        public int ExpirationMinutes { get; set; }
    }
}
