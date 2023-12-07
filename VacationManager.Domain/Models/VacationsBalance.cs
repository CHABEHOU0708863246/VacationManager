namespace VacationManager.Domain.Models
{
    /// <summary>
    /// Représente le solde de congé d'un utilisateur.
    /// </summary>
    public class VacationsBalance
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int InitialVacationBalance { get; set; } // Solde initial de congés 
        public int UsedVacationBalance { get; set; } // Solde utilisé de congés
        public int RemainingVacationBalance { get; set; } // Solde restant de congés
        public Users Users { get; set; }
    }
}
