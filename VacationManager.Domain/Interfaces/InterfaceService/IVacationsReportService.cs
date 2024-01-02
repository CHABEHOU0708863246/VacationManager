using VacationManager.Domain.Models;

namespace VacationManager.Domain.Interfaces.InterfaceService
{
    /// <summary>
    /// Représente un service pour la gestion des rapports sur l'utilisation des congés
    /// </summary>
    public interface IVacationsReportService
    {

        // Récupère tous les rapports sur l'utilisation des congés
        Task<VacationsReport> GetAllVacationsStatisticsAsync(CancellationToken cancellationToken);
    }
}
