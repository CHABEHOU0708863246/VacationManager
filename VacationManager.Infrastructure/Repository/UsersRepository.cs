

using Microsoft.EntityFrameworkCore;
using VacationManager.Domain.Interfaces.InterfacesRepository;
using VacationManager.Domain.Models;
using VacationManager.Infrastructure.Data;

namespace VacationManager.Infrastructure.Repository
{
    /// <summary>
    /// Represnte le depôt pour la gestion des utilisateurs en utilisant le contexte de la base de données
    /// </summary>
    public class UsersRepository : IUsersRepository
    {
        private readonly VacationManagerDbContext _databaseContext;

        public UsersRepository(VacationManagerDbContext databasecontext)
        {
            _databaseContext = databasecontext;
        }

        #region Récupère une liste paginée d'utilisateurs à partir de la base de données.
        public async Task<IEnumerable<Users>> GetUsersWithPaginationAsync(int startIndex, int pageSize, CancellationToken cancellationToken)
        {
            return await _databaseContext.Users
                                        .Include(u => u.Roles)
                                        .OrderBy(u => u.Id)
                                        .Skip(startIndex)
                                        .Take(pageSize)
                                        .ToListAsync(cancellationToken);
        }
        #endregion

        #region Récupère tous les utilisateur de manière asynchrone incluant leur roles distinct
        public async Task<IEnumerable<Users>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _databaseContext.Users.Include(u => u.Roles).ToListAsync();
        }
        #endregion

        #region Récupère un utilisateur par ID de manière asynchrone
        public async Task<Users> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            IQueryable<Users> query = _databaseContext.Users;

            return await query.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }
        #endregion

        #region Ajoute un nouveau utilisateur de manière asynchrone
        public async Task<Users> AddAsync(Users user, CancellationToken cancellationToken)
        {
            // Ajoute un utilisateur à la base de données
            _databaseContext.Users.Add(user);
            // Enregistre les modifications dans la base de données
            await _databaseContext.SaveChangesAsync();
            // Renvoie l'utilisateur
            return user;
        }
        #endregion

        #region Met à jour un Utilisateur existant de manière asynchrone
        public async Task<bool> UpdateAsync(Users user, CancellationToken cancellationToken)
        {
            using var transaction = await _databaseContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                _databaseContext.Entry(user).State = EntityState.Modified;
                await _databaseContext.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                return false;
            }
        }
        #endregion

        #region Supprime un utilisateur par ID de manière asynchrone
        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            // Recherche l'utilisateur en fonction de son ID
            var userToDelete = await _databaseContext.Users.FindAsync(id);

            if (userToDelete != null)
            {
                // Supprime l'utilisateur de la base de données
                _databaseContext.Users.Remove(userToDelete);
                // Enregistre les modifications dans la base de données
                await _databaseContext.SaveChangesAsync();
                // Renvoie un booléen indiquant le succès de la suppression
                return true;
            }
            else
            {
                // Renvoie false si l'utilisateur n'est pas trouvé
                return false;
            }
        }
        #endregion

    }
}
