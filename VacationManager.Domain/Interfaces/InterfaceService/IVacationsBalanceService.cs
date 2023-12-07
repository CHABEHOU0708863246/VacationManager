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
        // Cette methodde retourne les solde de conge des utilisateurs en specifiant leur ID
        Task<VacationDetailsDTO> GetVacationDetailsByUserIdAsync(int userId, CancellationToken cancellationToken);

        // Cette methode retourne tous les solde de congés dans les moindres details
        Task<IEnumerable<VacationDetailsDTO>> GetAllVacationDetailsAsync(CancellationToken cancellationToken);
    }
}
