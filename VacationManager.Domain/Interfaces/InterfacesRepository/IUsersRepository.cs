using VacationManager.Domain.Models;

namespace VacationManager.Domain.Interfaces.InterfacesRepository
{
    /// <summary>
    ///  Représente un contract de depôt pour la gestion des utilisateurs
    /// </summary>
    public interface IUsersRepository
    {
        // Récupère tous les utilisateurs de manière asynchrone à partir de la source de données en utilisant la pagination
        Task<IEnumerable<Users>> GetPaginatedUsersAsync(int page, int pageSize, CancellationToken cancellationToken);

        // Récupère tous les utilisateurs de manière asynchrone à partir de la source de données.
        Task<IEnumerable<Users>> GetAllAsync(CancellationToken cancellationToken);

        // Récupère un utilisateur par son identifiant de manière asynchrone à partir de la source de données
        Task<Users> GetByIdAsync(int id, CancellationToken cancellationToken);

        // Ajoute un nouvel utilisateur de manière asynchrone à la source de données.
        Task<Users> AddAsync(Users user, CancellationToken cancellationToken);

        // Met à jour un utilisateur existant de manière asynchrone dans la source de données.
        Task<bool> UpdateAsync(Users user, CancellationToken cancellationToken);

        // Supprime un utilisateur par son identifiant de manière asynchrone dans la source de données.
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
