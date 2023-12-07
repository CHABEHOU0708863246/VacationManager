

using VacationManager.Domain.Models;

namespace VacationManager.Domain.Interfaces.InterfaceService
{
    /// <summary>
    ///  Représente un service pour les congés
    /// </summary>
    public interface IVacationsService
    {
        // Cette méthode récupère tous les congés de la base de données.
        Task<IEnumerable<Vacations>> GetAllVacationsAsync(CancellationToken cancellationToken);

        // Cette méthode récupère un congé spécifique de la base de données en utilisant son ID.
        Task<Vacations> GetVacationsByIdAsync(int id, CancellationToken cancellationToken);

        // Cette méthode crée un nouveau congé dans la base de données.
        Task<Vacations> CreateVacationsAsync(Vacations vacations, CancellationToken cancellationToken);

        // Cette méthode met à jour un congé existant dans la base de données.
        Task<bool> UpdateVacationsAsync(int id, Vacations vacations, CancellationToken cancellationToken);

        // Cette méthode supprime un congé existant de la base de données.
        Task<bool> DeleteVacationsAsync(int id, CancellationToken cancellationToken);
    }
}
