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
    }
}
