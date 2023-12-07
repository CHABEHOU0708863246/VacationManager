namespace VacationManager.Domain.DTO
{
    public class VacationsCalendarDTO
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public int VacationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
    }

}
