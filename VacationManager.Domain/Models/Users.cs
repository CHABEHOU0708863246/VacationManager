

namespace VacationManager.Domain.Models
{
    /// <summary>
    /// Représente un utilisateur
    /// </summary>
    public class Users
    {
        // Identifiant de l'utilisateur
        public int Id { get; set; }
        // Informations personnelles de l'utilisateur
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        // Autres informations personnelles
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string PostalAddress { get; set; }
        public string City { get; set; }
        public string Profession { get; set; }
        public string MaritalStatus { get; set; }
        public int NumberOfChildren { get; set; }
        public string Nationality { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateCreateAccount { get; set; }
        public DateTime DateUpdateAccount { get; set; }
        /*public string ResetPasswordToken { get; set; }*/

        // Relations avec d'autres tables
        public int RoleID { get; set; }
        public Roles Roles { get; set; } // Relation avec la table Roles
        public ICollection<Vacations> UserVacations { get; set; }  // Relation avec la table Vacations
        public ICollection<VacationsBalance> UserVacationsBalances { get; set; } // Relation avec la table VacationsBalance
        public ICollection<VacationsCalendar> UserVacationsCalendars { get; set; } // Relation avec la table VacationsCalendar
        public ICollection<VacationsReport> VacationsReports { get; set; } // Relation avec la table VacationsReport

    }


}
