

using VacationManager.Domain.Models;
using static VacationManager.Domain.Models.Vacations;

namespace VacationManager.Domain.Interfaces.InterfacesRepository
{
    /// <summary>
    ///  Représente un contract de depôt pour la gestion des congés
    /// </summary>
    public interface IVacationsRepository
    {
        // Cette méthode récupère tous les congés de la base de données.
        Task<IEnumerable<Vacations>> GetAllAsync(CancellationToken cancellationToken);

        // Cette méthode récupère un congé spécifique de la base de données en utilisant son ID.
        Task<Vacations> GetByIdAsync(int vacations, CancellationToken cancellationToken);

        // Cette méthode ajoute un nouveau congé à la base de données.
        Task<Vacations> AddAsync(Vacations vacations, CancellationToken cancellationToken);

        // Cette méthode met à jour un congé existant dans la base de données.
        Task<bool> UpdateAsync(Vacations vacations, CancellationToken cancellationToken);

        // Cette méthode supprime un congé existant de la base de données.
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);

        // Cette méthode récupère tous les congés soumis par un utilisateur spécifique dans une plage de dates spécifique.
        Task<IEnumerable<Vacations>> GetVacationsWithinRange(int userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken);

        // Cette méthode récupère tous les congés soumis.
        Task<IEnumerable<Vacations>> GetAllSubmittedVacationsAsync(CancellationToken cancellationToken);

        //Cette mmethode recupère les congés et Inclut les données des utilisateurs liés aux congés avec jointures
        Task<IEnumerable<Vacations>> GetVacationsWithUsersByUserIdAsync(int userId, CancellationToken cancellationToken);

        // Cette méthode récupère tous les congés avec le statut spécifié.
        Task<IEnumerable<Vacations>> GetVacationsByStatusAsync(VacationsStatus status, CancellationToken cancellationToken);
    }
}
