namespace VacationManager.Domain.DTO
{
    public class VacationsDTO
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Comments { get; set; }
        public string Justification { get; set; }
    }
}
