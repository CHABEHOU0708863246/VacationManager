

using VacationManager.Domain.Models;

namespace VacationManager.Domain.Interfaces.InterfaceService
{
    /// <summary>
    ///  Représente un service pour les roles
    /// </summary>
    public interface IRolesService
    {
        // Cette méthode récupère tous les rôles de la base de données.
        Task<IEnumerable<Roles>> GetAllRolesAsync(CancellationToken cancellationToken);

        // Cette méthode récupère un rôle spécifique de la base de données en utilisant son ID.
        Task<Roles> GetRolesByIdAsync(int id, CancellationToken cancellationToken);

        // Cette méthode crée un nouveau rôle dans la base de données.
        Task<Roles> CreateRolesAsync(Roles role, CancellationToken cancellationToken);

        // Cette méthode met à jour un rôle existant dans la base de données.
        Task<bool> UpdateRolesAsync(int id, Roles role, CancellationToken cancellationToken);

        // Cette méthode supprime un rôle existant de la base de données.
        Task<bool> DeleteRolesAsync(int id, CancellationToken cancellationToken);
    }
}
