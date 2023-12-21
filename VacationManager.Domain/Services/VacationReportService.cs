using VacationManager.Domain.DTO;
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

        public VacationReportService(IVacationsReportRepository vacationsReportRepository)
        {
            _vacationsReportRepository = vacationsReportRepository;
        }

        #region Récupère les statistiques sur l'utilisation des congés
        public async Task<IEnumerable<VacationsReport>> GetAllReportsAsync(CancellationToken cancellationToken)
        {
            try
            {
                // Récupération de tous les rapports depuis le repository
                var allVacations = await _vacationsReportRepository.GetAllAsync(cancellationToken);

                // Retourne la liste complète des rapports
                return allVacations;
            }
            catch (Exception ex)
            {
                // Gestion des exceptions éventuelles
                throw new Exception($"Erreur lors de la récupération des rapports : {ex.Message}");
            }
        }
        #endregion

        #region Récupère les rapports sur l'utilisation des congés pour un utilisateur spécifique
        public async Task<IEnumerable<VacationsReport>> GetReportsByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            try
            {
                return await _vacationsReportRepository.GetByUserIdAsync(userId, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération des rapports pour l'utilisateur avec l'ID {userId} : {ex.Message}");
            }
        }
        #endregion

        #region Génère un rapport sur l'utilisation des congés pour un utilisateur sur une période donnée
        public async Task<VacationsReport> GenerateUserReportAsync(int userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
        {
            try
            {
                return await _vacationsReportRepository.GenerateUserReportAsync(userId, startDate, endDate, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la génération du rapport pour l'utilisateur avec l'ID {userId} entre {startDate} et {endDate} : {ex.Message}");
            }
        }
        #endregion

        #region Récupère les rapports sur l'utilisation des congés pour une date spécifique
        public async Task<IEnumerable<VacationsReport>> GetReportsByDateAsync(DateTime reportDate, CancellationToken cancellationToken)
        {
            try
            {
                return await _vacationsReportRepository.GetByDateAsync(reportDate, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération des rapports pour la date spécifiée {reportDate} : {ex.Message}");
            }
        }
        #endregion

        #region Récupère les rapports sur l'utilisation des congés avec un statut spécifique
        public async Task<IEnumerable<VacationsReport>> GetReportsByStatusAsync(string status, CancellationToken cancellationToken)
        {
            try
            {
                return await _vacationsReportRepository.GetByStatusAsync(status, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération des rapports avec le statut spécifié {status} : {ex.Message}");
            }
        }
        #endregion

        #region Récupère les rapports sur l'utilisation des congés créés entre deux dates spécifiques
        public async Task<IEnumerable<VacationsReport>> GetReportsBetweenDatesAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
        {
            try
            {
                return await _vacationsReportRepository.GetBetweenDatesAsync(startDate, endDate, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération des rapports entre les dates spécifiées : {ex.Message}");
            }
        }
        #endregion

    }
}
