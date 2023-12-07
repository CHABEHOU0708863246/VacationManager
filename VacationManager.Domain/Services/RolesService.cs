

using VacationManager.Domain.Interfaces.InterfaceService;
using VacationManager.Domain.Interfaces.InterfacesRepository;
using VacationManager.Domain.Models;

namespace VacationManager.Domain.Services
{
    /// <summary>
    ///  Représente le service pour la gestion des rôles
    /// </summary>
    public class RolesService : IRolesService
    {
        private readonly IRolesRepository _rolesRepository;

        public RolesService(IRolesRepository rolesRepository)
        {
            _rolesRepository = rolesRepository;
        }

        #region Recupère tous les roles de manière asynchrone
        public async Task<IEnumerable<Roles>> GetAllRolesAsync(CancellationToken cancellationToken)
        {
            return await _rolesRepository.GetAllAsync(cancellationToken);
        }
        #endregion

        #region Récupère un role par ID de manière asynchrone
        public async Task<Roles> GetRolesByIdAsync(int id, CancellationToken cancellationToken)
        {
            if (id == 0)
            {
                throw new ArgumentException(nameof(id));
            }

            return await _rolesRepository.GetByIdAsync(id, cancellationToken);
        }

        #endregion

        #region Crée un nouvel Role de manière asynchrone
        public async Task<Roles> CreateRolesAsync(Roles role, CancellationToken cancellationToken)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            return await _rolesRepository.AddAsync(role, cancellationToken);
        }
        #endregion

        #region Met à jour un Role de manière asynchrone
        public async Task<bool> UpdateRolesAsync(int id, Roles role, CancellationToken cancellationToken)
        {
            if (id == 0)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            var existingRole = await _rolesRepository.GetByIdAsync(id, cancellationToken);

            if (existingRole == null)
            {
                return false;
            }

            existingRole.Name = role.Name;

            return await _rolesRepository.UpdateAsync(existingRole, cancellationToken);
        }

        #endregion

        #region Supprime un Role de manière asynchrone
        public async Task<bool> DeleteRolesAsync(int id, CancellationToken cancellationToken)
        {
            if (id == 0)
            {
                throw new ArgumentNullException(nameof(id));
            }
            return await _rolesRepository.DeleteAsync(id, cancellationToken);
        }
        #endregion
    }
}
