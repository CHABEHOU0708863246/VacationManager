namespace VacationManager.Domain.DTO
{
    public class UsersDTO
    {
        public int? Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PostalAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Profession { get; set; } = string.Empty;
        public string MaritalStatus { get; set; } = string.Empty;
        public int NumberOfChildren { get; set; } 
        public string Nationality { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; } 
        public DateTime DateCreateAccount { get; set; }
        public DateTime DateUpdateAccount { get; set; }

        // Propriétés pour la gestion des rôles
        public int RoleID { get; set; }
        public string? RoleName { get; set; }

    }
}
