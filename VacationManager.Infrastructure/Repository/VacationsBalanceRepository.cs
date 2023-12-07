using Microsoft.EntityFrameworkCore;
using VacationManager.Domain.DTO;
using VacationManager.Domain.Interfaces.InterfacesRepository;
using VacationManager.Domain.Models;
using VacationManager.Infrastructure.Data;

namespace VacationManager.Infrastructure.Repository
{
    public class VacationsBalanceRepository : IVacationsBalanceRepository
    {
        private readonly VacationManagerDbContext _databaseContext;
        private readonly IUsersRepository _usersRepository;

        public VacationsBalanceRepository(VacationManagerDbContext databaseContext, IUsersRepository usersRepository)
        {
            _databaseContext = databaseContext;
            _usersRepository = usersRepository;
        }

        #region recupère les congé et Inclut les données des utilisateurs liés aux congés avec jointures
        public async Task<IEnumerable<Vacations>> GetVacationsWithUsersByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            return await _databaseContext.Vacations
              .Include(v => v.Users)
              .Where(v => v.UserId == userId)
              .ToListAsync(cancellationToken);
        }
        #endregion

        #region Logique pour récupérer le solde de congé par l'ID de l'utilisateur depuis la base de données
        public async Task<VacationsBalance> GetByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            var existingBalance = await _databaseContext.VacationsBalances.FirstOrDefaultAsync(vb => vb.UserId == userId, cancellationToken);

            return existingBalance;
        }
        #endregion

        #region Logique pour mettre à jour le solde de congé en fonction des congés utilisés
        public async Task<bool> UpdateAsync(int userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
        {
            try
            {
                var existingBalance = await GetByUserIdAsync(userId, cancellationToken);

                if (existingBalance == null)
                {
                    existingBalance = new VacationsBalance
                    {
                        UserId = userId,
                        InitialVacationBalance = 23
                    };
                    _databaseContext.VacationsBalances.Add(existingBalance);
                }

                if (userId <= 0)
                {
                    throw new ArgumentException("L'ID de l'utilisateur doit être supérieur ou égal à 0.", nameof(userId));
                }

                if (startDate > endDate)
                {
                    throw new ArgumentException("La date de début doit être antérieure ou égale à la date de fin.", nameof(startDate));
                }

                var userIds = await _usersRepository.GetAllAsync(cancellationToken);
                var userIdsList = userIds.Select(u => u.Id).ToList();

                var vacationDetails = await _databaseContext.VacationsBalances
                  .Where(v => userIdsList.Contains(v.UserId))
                  .ToListAsync(cancellationToken);

                int usedVacationDays = CalculateUsedVacationDays(vacationDetails.Cast<Vacations>());

                existingBalance.UsedVacationBalance = usedVacationDays;
                existingBalance.RemainingVacationBalance = existingBalance.InitialVacationBalance - usedVacationDays;

                await _databaseContext.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }
        #endregion

        #region Effectue le calcul du nombre de jours de congé utilisés en fonction d'une collection de congés spécifique.
        private int CalculateUsedVacationDays(IEnumerable<Vacations> vacations)
        {
            return vacations.Sum(v => (v.EndDate - v.StartDate).Days + 1);
        }
        #endregion

        public async Task<IEnumerable<VacationDetailsDTO>> GetVacationDetailsByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            // Récupère les vacances de l'utilisateur
            var vacations = await _databaseContext.Vacations
              .Include(v => v.Users)
              .Where(v => v.UserId == userId)
              .ToListAsync(cancellationToken);

            // Récupère le solde de congé de l'utilisateur
            var vacationBalance = await _databaseContext.VacationsBalances.FirstOrDefaultAsync(vb => vb.UserId == userId, cancellationToken);

            // Calcule le solde utilisé en fonction des vacances
            int usedVacationDays = CalculateUsedVacationDays(vacations.Cast<Vacations>());

            // Crée l'objet VacationDetailsDTO
            var vacationDetailsDTO = new VacationDetailsDTO
            {
                UserId = userId,
                UserName = $"{vacations.FirstOrDefault().Users.FirstName} {vacations.FirstOrDefault().Users.LastName}",
                StartDate = vacations.FirstOrDefault().StartDate,
                EndDate = vacations.FirstOrDefault().EndDate,
                Type = vacations.FirstOrDefault().Type,
                Justification = vacations.FirstOrDefault().Justification,
                InitialBalance = vacationBalance.InitialVacationBalance,
                UsedBalance = usedVacationDays,
                RemainingBalance = vacationBalance.InitialVacationBalance - usedVacationDays,
            };

            return new List<VacationDetailsDTO>() { vacationDetailsDTO };
        }


        public async Task<VacationsBalance> CalculateRemainingBalance(int userId, CancellationToken cancellationToken)
        {
            var existingBalance = await GetByUserIdAsync(userId, cancellationToken);
            existingBalance.RemainingVacationBalance = existingBalance.InitialVacationBalance - existingBalance.UsedVacationBalance;

            return existingBalance;
        }
    }
}