using VacationManager.Domain.Models;

namespace VacationManager.Domain.Interfaces.InterfaceService
{
    /// <summary>
    /// Représente un service pour la gestion des rapports sur l'utilisation des congés
    /// </summary>
    public interface IVacationsReportService
    {

        // Récupère tous les rapports sur l'utilisation des congés
        Task<IEnumerable<VacationsReport>> GetAllReportsAsync(CancellationToken cancellationToken);

        // Récupère les rapports sur l'utilisation des congés pour un utilisateur spécifique
        Task<IEnumerable<VacationsReport>> GetReportsByUserIdAsync(int userId, CancellationToken cancellationToken);

        // Génère un rapport sur l'utilisation des congés pour un utilisateur sur une période donnée
        Task<VacationsReport> GenerateUserReportAsync(int userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken);

        // Récupère les rapports sur l'utilisation des congés pour une date spécifique
        Task<IEnumerable<VacationsReport>> GetReportsByDateAsync(DateTime reportDate, CancellationToken cancellationToken);

        // Récupère les rapports sur l'utilisation des congés avec un statut spécifique
        Task<IEnumerable<VacationsReport>> GetReportsByStatusAsync(string status, CancellationToken cancellationToken);

        // Récupère les rapports sur l'utilisation des congés créés entre deux dates spécifiques
        Task<IEnumerable<VacationsReport>> GetReportsBetweenDatesAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken);
    }
}
