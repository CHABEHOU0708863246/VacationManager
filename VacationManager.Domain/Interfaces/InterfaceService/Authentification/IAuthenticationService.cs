

using VacationManager.Domain.Models.Authentification;

namespace VacationManager.Domain.Interfaces.InterfaceService.Authentification
{
    public interface IAuthenticationService
    {
        // Méthode pour authentifier un utilisateur prend en paramètre une requête d'authentification contenant les identifiants retourne une réponse d'authentification contenant le résultat
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request, CancellationToken cancellationToken);
    }
}
