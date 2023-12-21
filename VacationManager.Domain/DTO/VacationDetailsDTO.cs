using static VacationManager.Domain.Models.Vacations;

namespace VacationManager.Domain.DTO
{
    public class VacationDetailsDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime StartDate { get; set; } // Correspondant à la propriété de Vacations
        public DateTime EndDate { get; set; } // Correspondant à la propriété de Vacations
        public string Type { get; set; } // Correspondant à la propriété de Vacations
        public VacationsStatus Status { get; set; } // Correspondant à la propriété de Vacations
        public string Justification { get; set; }  // Correspondant à la propriété de Vacations
        public int InitialBalance { get; set; } // Correspondant à la propriété de VacationsBalance
        public int UsedBalance { get; set; } // Correspondant à la propriété de VacationsBalance
        public int RemainingBalance { get; set; } // Correspondant à la propriété de VacationsBalance
    }
}
