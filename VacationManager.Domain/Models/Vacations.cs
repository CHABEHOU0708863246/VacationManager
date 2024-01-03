

namespace VacationManager.Domain.Models
{
    /// <summary>
    /// Représente un congé.
    /// </summary>
    public class Vacations
    {
        public int Id { get; set; }
        public int UserId { get; set; } // Clé étrangère vers la table Utilisateurs
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Type { get; set; } // "Congé annuel", "Congé de maladie", "Congé sans solde", etc.
        public VacationsStatus Status { get; set; } // En attente par defaut
        public DateTime CreatedDate { get; set; }
        public string Comments { get; set; }
        public string Justification { get; set; }
        public Users Users { get; set; }
        public ICollection<VacationsCalendar> Calendars { get; set; }

        public enum VacationsStatus
        {
            Attente,
            Approuve,
            Rejected
        }
    }

}
