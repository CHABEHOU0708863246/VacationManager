using Microsoft.EntityFrameworkCore;
using VacationManager.Domain.Interfaces.InterfacesRepository;
using VacationManager.Domain.Models;
using VacationManager.Infrastructure.Data;

namespace VacationManager.Infrastructure.Repository
{
    /// <summary>
    /// Représente le dépôt pour la gestion des rapports sur l'utilisation des congés en utilisant le contexte de la base de données.
    /// </summary>
    public class VacationsReportRepository : IVacationsReportRepository
    {
        private readonly VacationManagerDbContext _databaseContext;

        public VacationsReportRepository(VacationManagerDbContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        #region Récupère tous les rapports sur l'utilisation des congés de manière asynchrone à partir de la source de données.

        public async Task<int> GetTotalVacationsAsync(CancellationToken cancellationToken)
        {
            return await _databaseContext.Vacations.CountAsync(cancellationToken);
        }

        public async Task<int> GetTotalPendingVacationsAsync(CancellationToken cancellationToken)
        {
            return await _databaseContext.Vacations
                .CountAsync(v => v.Status == Vacations.VacationsStatus.Attente, cancellationToken);
        }

        public async Task<int> GetTotalApprovedVacationsAsync(CancellationToken cancellationToken)
        {
            return await _databaseContext.Vacations
                .CountAsync(v => v.Status == Vacations.VacationsStatus.Approuve, cancellationToken);
        }

        public async Task<int> GetTotalRejectedVacationsAsync(CancellationToken cancellationToken)
        {
            return await _databaseContext.Vacations
                .CountAsync(v => v.Status == Vacations.VacationsStatus.Rejected, cancellationToken);
        }

        public async Task<int> GetTotalUsersAsync(CancellationToken cancellationToken)
        {
            return await _databaseContext.Users.CountAsync(cancellationToken);
        }
        #endregion

        #region Récupère tous les rapports sur l'utilisation des congés pour un utilisateur spécifique

        #endregion


    }
}
