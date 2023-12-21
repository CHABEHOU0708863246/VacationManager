using static VacationManager.Domain.Models.Vacations;

namespace VacationManager.Domain.Models
{
    /// <summary>
    /// Représente un rapport sur l'utilisation des congés.
    /// </summary>
    public class VacationsReport
    {
        public int Id { get; set; }
        public int UserId { get; set; } // Clé étrangère vers la table Users

        // Statistiques sur l'utilisation des congés
        public int TotalDemand { get; set; } // Nombre total de demande
        public int TotalPending { get; set; } // Nombre de demandes en attente d'approbation
        public int TotalApproved { get; set; } // Nombre de demandes approuvées
        public int TotalRejected { get; set; } // Nombre de demandes rejetées
        public int CurrentBalance { get; set; } // Solde actuel de congés de l'utilisateur
        public VacationsStatus Status { get; set; } // Statut approuve ou rejete
        public DateTime ReportDate { get; set; } // Date du rapport
        public Users Users { get; set; } // Relation avec la table Users
    }
}
