using Microsoft.Extensions.Options;
using VacationManager.Domain.Configurations.Helper;
using VacationManager.Domain.DTO;
using VacationManager.Domain.Interfaces.InterfaceService;
using VacationManager.Domain.Interfaces.InterfacesRepository;
using VacationManager.Domain.Models;

namespace VacationManager.Domain.Services
{
    public class VacationsBalanceService : IVacationsBalanceService
    {
        private readonly IVacationsRepository _vacationsRepository;
        private readonly int _initialBalance;
        private readonly IUsersRepository _usersRepository;



        public VacationsBalanceService(IVacationsRepository vacationsRepository, IUsersRepository usersRepository, IOptions<VacationOptions> options)
        {
            _vacationsRepository = vacationsRepository;
            _initialBalance = options.Value.InitialBalance;
            _usersRepository = usersRepository;
        }

        public async Task<IEnumerable<VacationDetailsDTO>> GetAllVacationDetailsAsync(CancellationToken cancellationToken)
        {
            // Récupère la liste de tous les utilisateurs
            var users = await _usersRepository.GetAllAsync(cancellationToken);

            // Transforme la liste d'utilisateurs en une liste de tâches de récupération des détails de vacances
            var vacationDetailsTasks = users.Select(user => GetVacationDetailsByUserIdAsync(user.Id, cancellationToken));

            // Attend que toutes les tâches de récupération des détails de vacances soient terminées
            var vacationDetails = await Task.WhenAll(vacationDetailsTasks);

            // Renvoie la liste complète des détails de vacances
            return vacationDetails;
        }

        public async Task<VacationDetailsDTO> GetVacationDetailsByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            var vacations = await _vacationsRepository.GetVacationsWithUsersByUserIdAsync(userId, cancellationToken);

            if (vacations == null || !vacations.Any())
            {
                return new VacationDetailsDTO
                {
                    UserId = userId,
                    UserName = string.Empty,
                    StartDate = DateTime.MinValue,
                    EndDate = DateTime.MinValue,
                    Type = string.Empty,
                    Justification = string.Empty,
                    InitialBalance = _initialBalance,
                    UsedBalance = 0,
                    RemainingBalance = 0
                };
            }

            var usedVacationDays = CalculateUsedVacationDays(vacations, userId);

            var remainingVacationDays = _initialBalance - usedVacationDays;

            return new VacationDetailsDTO
            {
                UserId = userId,
                UserName = vacations.First().Users.FirstName + " " + vacations.First().Users.LastName,
                StartDate = vacations.First().StartDate,
                EndDate = vacations.First().EndDate,
                Type = vacations.First().Type,
                Justification = vacations.First().Justification,
                InitialBalance = _initialBalance,
                UsedBalance = usedVacationDays,
                RemainingBalance = remainingVacationDays
            };
        }

        #region Cette méthode récupère le solde de congés d'un utilisateur spécifique.
        public async Task<VacationsBalance> GetVacationBalanceByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            if (userId <= 0)
            {
                throw new ArgumentException(nameof(userId));
            }

            var vacationsWithUsers = await _vacationsRepository.GetVacationsWithUsersByUserIdAsync(userId, cancellationToken);

            if (vacationsWithUsers == null || !vacationsWithUsers.Any())
            {
                return null;
            }

            var usedVacationDays = CalculateUsedVacationDays(vacationsWithUsers, userId);

            var remainingVacationDays = _initialBalance - usedVacationDays;

            return new VacationsBalance
            {
                UserId = userId,
                InitialVacationBalance = _initialBalance,
                UsedVacationBalance = usedVacationDays,
                RemainingVacationBalance = remainingVacationDays
            };
        }
        #endregion

        #region Cette méthode met à jour le solde de congés d'un utilisateur spécifique.
        public async Task<bool> UpdateVacationBalanceAsync(int userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
        {
            // Vérifie si les données fournies sont valides.
            if (userId <= 0 || startDate == default || endDate == default)
            {
                throw new ArgumentException("Données non valides");
            }

            // Vérifie si l'utilisateur existe.
            var user = await _usersRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
            {
                throw new ArgumentException("L'utilisateur n'existe pas");
            }

            // Valide les dates.
            if (startDate > endDate)
            {
                throw new ArgumentException("La date de début doit être inférieure ou égale à la date de fin");
            }
            if (startDate < DateTime.Today)
            {
                throw new ArgumentException("La date de début doit être supérieure ou égale à la date du jour");
            }

            // Vérifie si les dates fournies chevauchent d'autres enregistrements de congés.
            var vacationsWithinRange = await _vacationsRepository.GetVacationsWithinRange(userId, startDate, endDate, cancellationToken);
            if (vacationsWithinRange.Any())
            {
                throw new ArgumentException("Les dates fournies chevauchent d'autres enregistrements de congés");
            }

            // Crée un nouvel enregistrement de congés.
            var vacations = new Vacations
            {
                UserId = userId,
                StartDate = startDate,
                EndDate = endDate
            };

            // Met à jour l'enregistrement de congés existant dans le dépôt.
            bool updateResult = await _vacationsRepository.UpdateAsync(vacations, cancellationToken);

            // Renvoie le résultat de la mise à jour.
            return updateResult;
        }
        #endregion

        #region Cette méthode calcule le nombre de jours de congé utilisés.
        private int CalculateUsedVacationDays(IEnumerable<Vacations> vacations, int userId)
        {
            return vacations.Where(v => v.UserId == userId)
                            .Sum(v => (v.EndDate - v.StartDate).Days + 1);
        }
        #endregion

    }
}
