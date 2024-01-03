using VacationManager.Domain.Interfaces.InterfaceService;
using VacationManager.Domain.Interfaces.InterfacesRepository;
using VacationManager.Domain.Models;

namespace VacationManager.Domain.Services
{
    /// <summary>
    /// Représente le service pour la gestion des rapports sur l'utilisation des congés
    /// </summary>
    public class VacationReportService : IVacationsReportService
    {
        private readonly IVacationsReportRepository _vacationsReportRepository;
        private readonly IVacationsService _vacationsService;
        private readonly IVacationsBalanceService _vacationsBalanceService;
        private readonly IUsersService _usersService;

        public VacationReportService(IVacationsReportRepository vacationsReportRepository, IVacationsService vacationsService, IVacationsBalanceService vacationsBalanceService, IUsersService usersService)
        {
            _vacationsReportRepository = vacationsReportRepository;
            _vacationsService = vacationsService;
            _vacationsBalanceService = vacationsBalanceService;
            _usersService = usersService;
        }

        #region Récupère les statistiques sur l'utilisation des congés
        public async Task<VacationsReport> GetAllVacationsStatisticsAsync(CancellationToken cancellationToken)
        {
            var statistics = new VacationsReport();

            // Récupérer le nombre total de congés
            statistics.TotalDemand = (await _vacationsService.GetAllVacationsAsync(cancellationToken)).Count();

            // Récupérer le nombre total de congés en attente
            statistics.TotalPending = (await _vacationsService.GetAllVacationsAsync(cancellationToken))
                .Count(v => v.Status == Vacations.VacationsStatus.Attente);

            // Récupérer le nombre total de congés approuvés
            statistics.TotalApproved = (await _vacationsService.GetAllVacationsAsync(cancellationToken))
                .Count(v => v.Status == Vacations.VacationsStatus.Approuve);

            // Récupérer le nombre total de congés refusés
            statistics.TotalRejected = (await _vacationsService.GetAllVacationsAsync(cancellationToken))
                .Count(v => v.Status == Vacations.VacationsStatus.Rejected);

            // Récupérer le nombre total d'utilisateurs
            statistics.TotalUsers = (await _usersService.GetAllUsersAsync(cancellationToken)).Count();

            return statistics;
        }
        #endregion

        #region Récupère tous les rapports sur l'utilisation des congés pour un utilisateur specifique ou l'utilisateur connecté
        public async Task<VacationsReport> GetVacationsStatisticsForUserAsync(int userId, CancellationToken cancellationToken)
        {
            var statisticsForUser = new VacationsReport();

            // Récupérer le nombre total de congés pour l'utilisateur spécifié
            statisticsForUser.TotalDemand = (await _vacationsService.GetAllVacationsForUserAsync(userId, cancellationToken)).Count();

            // Récupérer le nombre total de congés en attente pour l'utilisateur spécifié
            statisticsForUser.TotalPending = (await _vacationsService.GetAllVacationsForUserAsync(userId, cancellationToken))
                .Count(v => v.Status == Vacations.VacationsStatus.Attente);

            // Récupérer le nombre total de congés approuvés pour l'utilisateur spécifié
            statisticsForUser.TotalApproved = (await _vacationsService.GetAllVacationsForUserAsync(userId, cancellationToken))
                .Count(v => v.Status == Vacations.VacationsStatus.Approuve);

            // Récupérer le nombre total de congés refusés pour l'utilisateur spécifié
            statisticsForUser.TotalRejected = (await _vacationsService.GetAllVacationsForUserAsync(userId, cancellationToken))
                .Count(v => v.Status == Vacations.VacationsStatus.Rejected);

            return statisticsForUser;
        }
        #endregion



    }
}
