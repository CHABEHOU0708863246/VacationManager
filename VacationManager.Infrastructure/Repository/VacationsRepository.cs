

using Microsoft.EntityFrameworkCore;
using VacationManager.Domain.Interfaces.InterfacesRepository;
using VacationManager.Domain.Models;
using VacationManager.Infrastructure.Data;
using static VacationManager.Domain.Models.Vacations;

namespace VacationManager.Infrastructure.Repository
{
    /// <summary>
    /// Represnte le depôt pour la gestion des congés en utilisant le contexte de la base de données
    /// </summary>
    public class VacationsRepository : IVacationsRepository
    {
        private readonly VacationManagerDbContext _databaseContext;

        public VacationsRepository(VacationManagerDbContext databaseContext)
        {
            _databaseContext = databaseContext;
        }



        #region Recupère tous les vacations de manière asynchrone incluant l'utilisateur qui a demandé un congé
        public async Task<IEnumerable<Vacations>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _databaseContext.Vacations
                .Include(v => v.Users)
                .ToListAsync(cancellationToken);
        }

        #endregion

        #region Récupère un congé (Vacation) par ID de manière asynchrone
        public async Task<Vacations> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _databaseContext.Vacations.FindAsync(id);
        }
        #endregion

        #region Ajoute un congé (Vacation) de manière asynchrone
        public async Task<Vacations> AddAsync(Vacations vacation, CancellationToken cancellationToken)
        {
            _databaseContext.Vacations.Add(vacation);
            await _databaseContext.SaveChangesAsync();
            return vacation;
        }
        #endregion

        #region  Met à jour un congé existant de manière asynchrone
        public async Task<bool> UpdateAsync(Vacations vacation, CancellationToken cancellationToken)
        {
            // Modifie l'état de l'entité pour indiquer qu'elle est modifiée
            _databaseContext.Entry(vacation).State = EntityState.Modified;

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

        #region Supprime un conge (Vacation) par ID de manière asynchrone
        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            // Recherche l'utilisateur en fonction de son ID
            var vacationsToDelete = await _databaseContext.Vacations.FindAsync(id);

            if (vacationsToDelete != null)
            {
                // Supprime l'utilisateur de la base de données
                _databaseContext.Vacations.Remove(vacationsToDelete);
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

        #region Récupère tous les congés avec le statut spécifié.
        public async Task<IEnumerable<Vacations>> GetVacationsByStatusAsync(VacationsStatus status, CancellationToken cancellationToken)
        {
            var query = _databaseContext.Vacations
                .Where(vacation => vacation.Status == status);

            return await query.ToListAsync(cancellationToken);
        }
        #endregion

        #region Retourne un congé de manière spécifique en utilisant l'id de l'utilisateur avec des filtres et retourne une liste
        public async Task<IEnumerable<Vacations>> GetVacationsWithUsersByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            return await _databaseContext.Vacations
              .Include(v => v.Users)
              .Where(v => v.UserId == userId)
              .ToListAsync(cancellationToken);
        }
        #endregion

        #region Logique pour récupérer les congés pour un utilisateur dans une plage de dates spécifiée
        public async Task<IEnumerable<Vacations>> GetVacationsWithinRange(int userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
        {
            return await _databaseContext.Vacations
                .Where(v => v.UserId == userId && v.StartDate >= startDate && v.EndDate <= endDate)
                .ToListAsync(cancellationToken);
        }
        #endregion

        #region Récupère tous les congés soumis de manière asynchrone
        public async Task<IEnumerable<Vacations>> GetAllSubmittedVacationsAsync(CancellationToken cancellationToken)
        {
            return await _databaseContext.Vacations
                .Where(v => v.Status == v.Status)
                .ToListAsync(cancellationToken);
        }
        #endregion
    }
}
