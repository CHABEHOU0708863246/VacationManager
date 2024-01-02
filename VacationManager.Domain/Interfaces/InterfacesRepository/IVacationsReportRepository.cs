namespace VacationManager.Domain.Interfaces.InterfacesRepository
{
    /// <summary>
    /// Représente un contrat de dépôt pour la gestion des rapports sur l'utilisation des congés
    /// </summary>
    public interface IVacationsReportRepository
    {

        // Récupère tous les rapports sur l'utilisation des congés de manière asynchrone à partir de la source de données.
        Task<int> GetTotalVacationsAsync(CancellationToken cancellationToken);
        Task<int> GetTotalPendingVacationsAsync(CancellationToken cancellationToken);
        Task<int> GetTotalApprovedVacationsAsync(CancellationToken cancellationToken);
        Task<int> GetTotalRejectedVacationsAsync(CancellationToken cancellationToken);
        Task<int> GetTotalUsersAsync(CancellationToken cancellationToken);
    }
}
