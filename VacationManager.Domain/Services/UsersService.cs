using VacationManager.Domain.Interfaces.InterfaceService;
using VacationManager.Domain.Interfaces.InterfacesRepository;
using VacationManager.Domain.Models;

namespace VacationManager.Domain.Services
{
    /// <summary>
    ///  Représente le service pour la gestion des utilisateurs
    /// </summary>
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;

        public UsersService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        #region Retourne les utilisateurs en utilisant une pagination
        public async Task<IEnumerable<Users>> GetUsersWithPaginationAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            int startIndex = (pageNumber - 1) * pageSize;

            // Récupérer les utilisateurs en fonction de l'index de départ et de la taille de la page
            var users = await _usersRepository.GetUsersWithPaginationAsync(startIndex, pageSize, cancellationToken);

            return users;
        }

        #endregion

        #region Recupère tous les utilisateurs de manière asynchrone
        public async Task<IEnumerable<Users>> GetAllUsersAsync(CancellationToken cancellationToken)
        {
            return await _usersRepository.GetAllAsync(cancellationToken);
        }
        #endregion

        #region Récupère un utilisateur par ID de manière asynchrone
        public async Task<Users> GetUsersByIdAsync(int id, CancellationToken cancellationToken)
        {
            if (id == 0)
            {
                throw new ArgumentException(nameof(id));
            }

            return await _usersRepository.GetByIdAsync(id, cancellationToken);
        }

        #endregion

        #region Crée un nouvel utilisateur de manière asynchrone
        public async Task<Users> CreateUsersAsync(Users user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return await _usersRepository.AddAsync(user, cancellationToken);
        }
        #endregion

        #region Met à jour un utilisateur de manière asynchrone
        public async Task<bool> UpdateUsersAsync(int id, Users user, CancellationToken cancellationToken)
        {
            if (id == 0)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var existingUser = await _usersRepository.GetByIdAsync(id, cancellationToken);

            if (existingUser == null)
            {
                return false;
            }

            //Verifie si les valeur ont changé avant de faire une mise a jour
            if (existingUser.FirstName != user.FirstName || existingUser.LastName != user.LastName || existingUser.Email != user.Email || existingUser.Password != user.Password || existingUser.Gender != user.Gender || existingUser.PhoneNumber != user.PhoneNumber || existingUser.PostalAddress != user.PostalAddress || existingUser.City != user.City || existingUser.Profession != user.Profession || existingUser.MaritalStatus != user.MaritalStatus || existingUser.NumberOfChildren != user.NumberOfChildren || existingUser.Nationality != user.Nationality || existingUser.DateOfBirth != user.DateOfBirth || existingUser.DateCreateAccount != user.DateCreateAccount || existingUser.DateUpdateAccount != user.DateUpdateAccount || existingUser.RoleID != user.RoleID)

            {
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Email = user.Email;
                existingUser.Password = user.Password;
                existingUser.Gender = user.Gender;
                existingUser.PhoneNumber = user.PhoneNumber;
                existingUser.PostalAddress = user.PostalAddress;
                existingUser.City = user.City;
                existingUser.Profession = user.Profession;
                existingUser.MaritalStatus = user.MaritalStatus;
                existingUser.NumberOfChildren = user.NumberOfChildren;
                existingUser.Nationality = user.Nationality;
                existingUser.DateOfBirth = user.DateOfBirth;
                existingUser.DateCreateAccount = user.DateCreateAccount;
                existingUser.DateUpdateAccount = user.DateUpdateAccount;
                existingUser.RoleID = user.RoleID;
            }


            return await _usersRepository.UpdateAsync(existingUser, cancellationToken);
        }

        #endregion

        #region Supprime un utilisateur de manière asynchrone
        public async Task<bool> DeleteUsersAsync(int id, CancellationToken cancellationToken)
        {
            if (id == 0)
            {
                throw new ArgumentNullException(nameof(id));
            }
            return await _usersRepository.DeleteAsync(id, cancellationToken);
        }
        #endregion

        #region Authentification
        public async Task<Users> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            try
            {
                var users = await _usersRepository.GetAllAsync(cancellationToken);
                var user = users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la recherche de l'utilisateur par e-mail : {ex.Message}");
            }
        }
        #endregion
    }
}