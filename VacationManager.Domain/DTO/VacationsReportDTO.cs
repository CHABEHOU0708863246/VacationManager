namespace VacationManager.Domain.DTO
{
    public class VacationsReportDTO
    {
        // Statistiques sur l'utilisation des congés
        public int TotalDemand { get; set; } // Nombre total de demande
        public int TotalPending { get; set; } // Nombre de demandes en attente d'approbation
        public int TotalApproved { get; set; } // Nombre de demandes approuvées
        public int TotalRejected { get; set; } // Nombre de demandes rejetées
        public DateTime ReportDate { get; set; } // Date du rapport
        public int TotalUsers { get; set; }
    }
}
