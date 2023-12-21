using VacationManager.Domain.Models;

namespace VacationManager.Domain.Interfaces.InterfacesRepository
{
    /// <summary>
    /// Représente un contrat de dépôt pour la gestion des rapports sur l'utilisation des congés
    /// </summary>
    public interface IVacationsReportRepository
    {

        // Récupère tous les rapports sur l'utilisation des congés de manière asynchrone à partir de la source de données.
        Task<IEnumerable<VacationsReport>> GetAllAsync(CancellationToken cancellationToken);

        // Récupère les rapports sur l'utilisation des congés pour un utilisateur spécifique de manière asynchrone à partir de la source de données.
        Task<IEnumerable<VacationsReport>> GetByUserIdAsync(int userId, CancellationToken cancellationToken);

        // Génère un rapport sur l'utilisation des congés pour un utilisateur sur une période donnée.
        Task<VacationsReport> GenerateUserReportAsync(int userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken);

        // Récupère les rapports sur l'utilisation des congés pour une date spécifique.
        Task<IEnumerable<VacationsReport>> GetByDateAsync(DateTime reportDate, CancellationToken cancellationToken);

        // Récupère les rapports sur l'utilisation des congés pour une date spécifique.
        Task<IEnumerable<VacationsReport>> GetByStatusAsync(string status, CancellationToken cancellationToken);

        // Exemple de méthode pour récupérer les rapports entre deux dates spécifiques
        Task<IEnumerable<VacationsReport>> GetBetweenDatesAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken);
    }
}
