using Microsoft.AspNetCore.Authorization;

namespace VacationManager.Domain.Configurations.OAuth
{
    /// <summary>
    /// Cette exigence vérifie si scopeLa réclamation émise par votre locataire d'Auth0 est présente. Si le scopela demande existe, l'exigence vérifie si scopeLa réclamation contient le champ d'application demandé.
    /// </summary>
    public class HasScopeRequirement : IAuthorizationRequirement
    {
        public string Issuer { get; }
        public string Scope { get; }

        public HasScopeRequirement(string scope, string issuer)
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
            Issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
        }
    }
}
