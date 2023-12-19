namespace VacationManager.Domain.Configurations.Helper
{
    /// <summary>
    /// Représente une requête pour approuver ou rejeter une demande.
    /// </summary>
    public class ApproveOrRejectRequest
    {
        public string NewStatus { get; set; }
    }
}
