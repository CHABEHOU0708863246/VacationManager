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
        public async Task<IEnumerable<VacationsReport>> GetAllAsync(CancellationToken cancellationToken)
        {
            try
            {
                // Récupération de tous les rapports avec certaines informations associées
                var allVacations = await _databaseContext.VacationsReports
                    .Include(v => v.Users)
                    .Select(v => new VacationsReport
                    {
                        UserId = v.UserId,
                        TotalDemand = v.TotalDemand,
                        TotalPending = v.TotalPending,
                        TotalApproved = v.TotalApproved,
                        TotalRejected = v.TotalRejected,
                    })
                    .ToListAsync(cancellationToken);

                return allVacations;
            }
            catch (Exception ex)
            {
                // Gestion des exceptions éventuelles
                throw new Exception($"Erreur lors de la récupération des rapports depuis la base de données : {ex.Message}");
            }
        }
        #endregion

        #region Récupère les rapports sur l'utilisation des congés pour un utilisateur spécifique de manière asynchrone à partir de la source de données.
        public async Task<IEnumerable<VacationsReport>> GetByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            return await _databaseContext.VacationsReports.Where(report => report.UserId == userId).ToListAsync(cancellationToken);
        }
        #endregion

        #region Génère un rapport sur l'utilisation des congés pour un utilisateur sur une période donnée.
        public async Task<VacationsReport> GenerateUserReportAsync(int userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
        {
            return await _databaseContext.VacationsReports
                .FirstOrDefaultAsync(report => report.UserId == userId &&
                                              report.ReportDate >= startDate &&
                                              report.ReportDate <= endDate,
                                      cancellationToken);
        }
        #endregion

        #region Récupère les rapports sur l'utilisation des congés pour une date spécifique.
        public async Task<IEnumerable<VacationsReport>> GetByDateAsync(DateTime reportDate, CancellationToken cancellationToken)
        {
            return await _databaseContext.VacationsReports
                .Where(report => report.ReportDate.Date == reportDate.Date)
                .ToListAsync(cancellationToken);
        }
        #endregion

        #region Récupère les rapports sur l'utilisation des congés avec un statut spécifique.
        public async Task<IEnumerable<VacationsReport>> GetByStatusAsync(string status, CancellationToken cancellationToken)
        {
            return await _databaseContext.VacationsReports
                .Where(report => report.Status.ToString() == status)
                .ToListAsync(cancellationToken);
        }
        #endregion

        #region Récupère les rapports sur l'utilisation des congés créés entre deux dates spécifiques.
        public async Task<IEnumerable<VacationsReport>> GetBetweenDatesAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
        {
            return await _databaseContext.VacationsReports
                .Where(report => report.ReportDate >= startDate && report.ReportDate <= endDate)
                .ToListAsync(cancellationToken);
        }
        #endregion

    }
}
