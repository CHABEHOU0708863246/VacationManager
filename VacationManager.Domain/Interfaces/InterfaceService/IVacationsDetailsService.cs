using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManager.Domain.DTO;

namespace VacationManager.Domain.Interfaces.InterfaceService
{
    internal interface IVacationsDetailsService
    {
        // Cette méthode retourne les détails des congés d'un utilisateur spécifique.
        Task<IEnumerable<VacationDetailsDTO>> GetVacationDetailsByUserIdAsync(int userId, CancellationToken cancellationToken);

        // Cette méthode retourne tous les détails des congés dans les moindres détails.
        Task<IEnumerable<VacationDetailsDTO>> GetAllVacationDetailsAsync(CancellationToken cancellationToken);
    }
}
