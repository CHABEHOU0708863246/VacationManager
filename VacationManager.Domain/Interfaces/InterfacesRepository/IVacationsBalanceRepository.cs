using VacationManager.Domain.DTO;
using VacationManager.Domain.Models;

namespace VacationManager.Domain.Interfaces.InterfacesRepository
{
    /// <summary>
    /// Cette interface définit les méthodes pour un dépôt qui gère le solde de congés des utilisateurs.
    /// </summary>
    public interface IVacationsBalanceRepository
    {
        // Cette méthode récupère le solde de congés d'un utilisateur spécifique.
        Task<VacationsBalance> GetByUserIdAsync(int userId, CancellationToken cancellationToken);

        // Cette méthode met à jour le solde de congés d'un utilisateur spécifique.
        Task<bool> UpdateAsync(int userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken);

        // Méthode pour récupérer les détails des congés par utilisateur
        Task<IEnumerable<VacationDetailsDTO>> GetVacationDetailsByUserIdAsync(int userId, CancellationToken cancellationToken);

        // Cette méthode permet de mettre à jour le statut d'une demande de congé spécifique pour un utilisateur.
        Task<bool> UpdateVacationStatusAsync(int vacationId, Vacations.VacationsStatus newStatus, CancellationToken cancellationToken);

        // Cette methode gere le calcul du solde de congé en fontion du status
        Task<bool> UpdateBalanceByVacationStatusAsync(int userId, Vacations.VacationsStatus status, CancellationToken cancellationToken);

        // Supprime un solde de congés en fonction de l'id de lutilisateur
        Task<bool> DeleteVacationBalanceAsync(int userId, CancellationToken cancellationToken);

        // Ajoute un solde de congés pour une nouvelle année
        Task AddAsync(VacationsBalance balance, CancellationToken cancellationToken);

    }
}
