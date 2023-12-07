

using VacationManager.Domain.Models;

namespace VacationManager.Domain.Interfaces.InterfacesRepository
{
    /// <summary>
    ///  Représente un contract de depôt pour la gestion des rôles
    /// </summary>
    public interface IRolesRepository
    {
        // Cette méthode récupère tous les rôles de la base de données.
        Task<IEnumerable<Roles>> GetAllAsync(CancellationToken cancellationToken);

        // Cette méthode récupère un rôle spécifique de la base de données en utilisant son ID.
        Task<Roles> GetByIdAsync(int id, CancellationToken cancellationToken);

        // Cette méthode ajoute un nouveau rôle à la base de données.
        Task<Roles> AddAsync(Roles role, CancellationToken cancellationToken);

        // Cette méthode met à jour un rôle existant dans la base de données.
        Task<bool> UpdateAsync(Roles role, CancellationToken cancellationToken);

        // Cette méthode supprime un rôle existant de la base de données.
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
