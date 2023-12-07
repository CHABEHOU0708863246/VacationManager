﻿namespace VacationManager.Domain.Models
{
    /// <summary>
    /// Représente un rapport sur l'utilisation des congés.
    /// </summary>
    public class VacationsReport
    {
        public int Id { get; set; }
        public int UserId { get; set; } // Clé étrangère vers la table Users

        // Statistiques sur l'utilisation des congés
        public int PendingRequests { get; set; } // Nombre de demandes en attente d'approbation
        public int ApprovedRequests { get; set; } // Nombre de demandes approuvées
        public int RejectedRequests { get; set; } // Nombre de demandes rejetées
        public int CurrentBalance { get; set; } // Solde actuel de congés de l'utilisateur
        public DateTime ReportDate { get; set; } // Date du rapport
        public Users Users { get; set; } // Relation avec la table Users
    }
}
