

namespace VacationManager.Domain.Models
{
    /// <summary>
    /// Représente les Roles soit employé, Administrateur et Gestionnaire
    /// </summary>
    public class Roles
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Users> Users { get; set; } // Relation avec la table Users
    }


}
