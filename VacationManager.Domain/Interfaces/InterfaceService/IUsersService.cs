using VacationManager.Domain.Models;

namespace VacationManager.Domain.Interfaces.InterfaceService
{
    /// <summary>
    ///  Représente un service pour la gestion des utilisateurs
    /// </summary>
    public interface IUsersService
    {

        // Renvoie une page d'utilisateurs avec pagination
        Task<IEnumerable<Users>> GetUsersWithPaginationAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);

        // Récupère tous les utilisateurs
        Task<IEnumerable<Users>> GetAllUsersAsync(CancellationToken cancellationToken);

        // Récupère un utilisateur par son identifiant
        Task<Users> GetUsersByIdAsync(int id, CancellationToken cancellationToken);

        // Crée un nouvel utilisateur
        Task<Users> CreateUsersAsync(Users user, CancellationToken cancellationToken);

        // Met à jour les informations d'un utilisateur
        Task<bool> UpdateUsersAsync(int id, Users user, CancellationToken cancellationToken);

        // Supprime un utilisateur par son identifiant
        Task<bool> DeleteUsersAsync(int id, CancellationToken cancellationToken);

        // Récupère un utilisateur par son adresse e-mail
        Task<Users> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
    }
}
