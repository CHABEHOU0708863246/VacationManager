

using Microsoft.EntityFrameworkCore;
using VacationManager.Domain.Interfaces.InterfacesRepository;
using VacationManager.Domain.Models;
using VacationManager.Infrastructure.Data;

namespace VacationManager.Infrastructure.Repository
{
    /// <summary>
    /// Represnte le depôt pour la gestion des roles en utilisant le contexte de la base de données
    /// </summary>
    public class RolesRepository : IRolesRepository
    {
        private readonly VacationManagerDbContext _databaseContext;

        public RolesRepository(VacationManagerDbContext databasecontext)
        {
            _databaseContext = databasecontext;
        }

        #region Récupère tous les Roles de manière asynchrone incluant les utilisateur par role distinct
        public async Task<IEnumerable<Roles>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _databaseContext.Roles.ToListAsync();
            /*return await _databaseContext.Roles.Include(u => u.Users).ToListAsync();*/
        }
        #endregion

        #region Récupère un Role par ID de manière asynchrone
        public async Task<Roles> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                return await _databaseContext.Roles.FindAsync(new object[] { id }, cancellationToken);
            }
            catch (Exception ex)
            {
                // Loggez l'erreur ici
                Console.WriteLine($"Erreur lors de la récupération du rôle par ID : {ex.Message}");
                throw; // Renvoie l'exception pour indiquer qu'il y a eu un problème lors de la récupération du rôle.
            }
        }
        #endregion

        #region Ajoute un nouveau role de manière asynchrone
        public async Task<Roles> AddAsync(Roles role, CancellationToken cancellationToken)
        {
            // Ajoute un rôle à la base de données
            _databaseContext.Roles.Add(role);
            // Enregistre les modifications dans la base de données
            await _databaseContext.SaveChangesAsync();
            // Renvoie le rôle
            return role;
        }
        #endregion

        #region Met à jour un Role existant de manière asynchrone
        public async Task<bool> UpdateAsync(Roles role, CancellationToken cancellationToken)
        {
            // Modifie l'état de l'entité pour indiquer qu'elle est modifiée
            _databaseContext.Entry(role).State = EntityState.Modified;

            try
            {
                // Enregistre les modifications dans la base de données
                await _databaseContext.SaveChangesAsync();
                // Renvoie un booléen indiquant le succès de la mise à jour
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                // Gestion des conflits de concurrence si nécessaire
                return false;
            }
        }
        #endregion

        #region Supprime un Role par ID de manière asynchrone
        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            // Recherche le rôle en fonction de son ID
            var roleToDelete = await _databaseContext.Roles.FindAsync(id);

            if (roleToDelete != null)
            {
                // Supprime le rôle de la base de données
                _databaseContext.Roles.Remove(roleToDelete);
                // Enregistre les modifications dans la base de données
                await _databaseContext.SaveChangesAsync();
                // Renvoie un booléen indiquant le succès de la suppression
                return true;
            }
            else
            {
                // Renvoie false si le rôle n'est pas trouvé
                return false;
            }
        }
        #endregion
    }
}
