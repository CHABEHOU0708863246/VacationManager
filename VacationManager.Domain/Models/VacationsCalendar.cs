

namespace VacationManager.Domain.Models
{
    /// <summary>
    /// Représente un calendrier de congés.
    /// </summary>
    public class VacationsCalendar
    {
        public int Id { get; set; }
        public int UserId { get; set; } // Clé étrangère vers la table Users
        public int VacationId { get; set; } // Clé étrangère vers la table Vacations
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public Users Users { get; set; } // Propriété de navigation vers l'utilisateur associé
        public Vacations Vacations { get; set; } // Propriété de navigation vers le congé associé
    }

}
