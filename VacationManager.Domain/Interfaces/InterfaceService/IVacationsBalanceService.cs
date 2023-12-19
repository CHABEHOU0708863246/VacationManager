using VacationManager.Domain.DTO;
using VacationManager.Domain.Models;

namespace VacationManager.Domain.Interfaces.InterfaceService
{
    public interface IVacationsBalanceService
    {
        // Cette méthode récupère le solde de congés d'un utilisateur spécifique.
        Task<VacationsBalance> GetVacationBalanceByUserIdAsync(int userId, CancellationToken cancellationToken);

        // Cette méthode met à jour le solde de congés d'un utilisateur spécifique.
        Task<bool> UpdateVacationBalanceAsync(int userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken);

        // Cette méthode retourne les détails des congés d'un utilisateur spécifique.
        Task<IEnumerable<VacationDetailsDTO>> GetVacationDetailsByUserIdAsync(int userId, CancellationToken cancellationToken);

        // Cette méthode retourne tous les détails des congés dans les moindres détails.
        Task<IEnumerable<VacationDetailsDTO>> GetAllVacationDetailsAsync(CancellationToken cancellationToken);

        // Cette méthode permet de valider ou de refuser les congés d'un utilisateur.
        Task<bool> ApproveOrRejectVacationAsync(int vacationId, string newStatus, CancellationToken cancellationToken);
    }
}
