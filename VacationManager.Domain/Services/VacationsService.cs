

using VacationManager.Domain.Interfaces.InterfaceService;
using VacationManager.Domain.Interfaces.InterfacesRepository;
using VacationManager.Domain.Models;

namespace VacationManager.Domain.Services
{
    /// <summary>
    ///  Représente le service pour la gestion des congés (Vacations) 
    /// </summary>
    public class VacationsService : IVacationsService
    {
        private readonly IVacationsRepository _vacationsRepository;

        public VacationsService(IVacationsRepository vacationsRepository)
        {
            _vacationsRepository = vacationsRepository;
        }

        #region Recupère toute les congés (Vacations) de manière asynchrone
        public async Task<IEnumerable<Vacations>> GetAllVacationsAsync(CancellationToken cancellationToken)
        {
            return await _vacationsRepository.GetAllAsync(cancellationToken);
        }
        #endregion

        #region Recupère un congé (Vacations) par son Id de manière asynchrone
        public async Task<Vacations> GetVacationsByIdAsync(int id, CancellationToken cancellationToken)
        {
            if (id == 0)
            {
                throw new ArgumentException(nameof(id));
            }

            return await _vacationsRepository.GetByIdAsync(id, cancellationToken);
        }
        #endregion

        #region Création de nouveau congé (Vacations) de manière asynchrone
        public async Task<Vacations> CreateVacationsAsync(Vacations vacations, CancellationToken cancellationToken)
        {
            if (vacations == null)
            {
                throw new ArgumentNullException(nameof(vacations));
            }

            vacations.CreatedDate = DateTime.Now;

            return await _vacationsRepository.AddAsync(vacations, cancellationToken);
        }
        #endregion

        #region Met à jour un congé (Vacations) par son Id
        public async Task<bool> UpdateVacationsAsync(int id, Vacations vacations, CancellationToken cancellationToken)
        {
            if (id == 0)
            {
                throw new ArgumentException(nameof(id));
            }

            if (vacations == null)
            {
                throw new ArgumentNullException(nameof(vacations));
            }

            var existingVacations = await _vacationsRepository.GetByIdAsync(id, cancellationToken);

            if (existingVacations == null)
            {
                return false;
            }

            // Mettre à jour les propriétés nécessaires
            existingVacations.StartDate = vacations.StartDate;
            existingVacations.EndDate = vacations.EndDate;
            existingVacations.Type = vacations.Type;
            existingVacations.Status = vacations.Status;
            existingVacations.Comments = vacations.Comments;
            existingVacations.Justification = vacations.Justification;

            return await _vacationsRepository.UpdateAsync(existingVacations, cancellationToken);
        }
        #endregion

        #region Supprime un congé (Vacations) de manière asynchrone
        public async Task<bool> DeleteVacationsAsync(int id, CancellationToken cancellationToken)
        {
            if (id == 0)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await _vacationsRepository.DeleteAsync(id, cancellationToken);
        }
        #endregion
    }
}
