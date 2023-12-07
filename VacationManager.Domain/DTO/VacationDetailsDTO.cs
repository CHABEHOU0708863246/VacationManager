namespace VacationManager.Domain.DTO
{
    public class VacationDetailsDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Type { get; set; }
        public string Justification { get; set; }
        public int InitialBalance { get; set; }
        public int UsedBalance { get; set; }
        public int RemainingBalance { get; set; }
    }
}
