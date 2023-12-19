using Microsoft.Extensions.Options;
using VacationManager.Domain.Configurations.Helper;
using VacationManager.Domain.DTO;
using VacationManager.Domain.Interfaces.InterfaceService;
using VacationManager.Domain.Interfaces.InterfacesRepository;
using VacationManager.Domain.Models;
using static VacationManager.Domain.Models.Vacations;

namespace VacationManager.Domain.Services
{
    public class VacationsBalanceService : IVacationsBalanceService
    {
        #region Dependencies
        private readonly IVacationsRepository _vacationsRepository;
        private readonly int _initialBalance;
        private readonly IUsersRepository _usersRepository;
        private readonly IVacationsBalanceRepository _vacationsBalanceRepository;
        private readonly List<DateTime> _holidays;
        #endregion

        #region Constructeur
        public VacationsBalanceService(IVacationsRepository vacationsRepository, IUsersRepository usersRepository, IOptions<VacationOptions> options, IVacationsBalanceRepository vacationsBalanceRepository, List<DateTime> holidays)
        {
            _vacationsRepository = vacationsRepository;
            _initialBalance = options.Value.InitialBalance;
            _usersRepository = usersRepository;
            _vacationsBalanceRepository = vacationsBalanceRepository;
            _holidays = holidays;
        }
        #endregion

        #region Récupération des détails de congés de tous les utilisateurs
        public async Task<IEnumerable<VacationDetailsDTO>> GetAllVacationDetailsAsync(CancellationToken cancellationToken)
        {
            // Récupère la liste de tous les utilisateurs
            var users = await _usersRepository.GetAllAsync(cancellationToken);

            // Transforme la liste d'utilisateurs en une liste de tâches de récupération des détails de vacances
            var vacationDetailsTasks = users.Select(user => GetVacationDetailsByUserIdAsync(user.Id, cancellationToken));

            // Attend que toutes les tâches de récupération des détails de vacances soient terminées
            var vacationDetails = await Task.WhenAll(vacationDetailsTasks);

            // Aplatit le tableau de Task<IEnumerable<VacationDetailsDTO>> en un seul IEnumerable<VacationDetailsDTO>
            var allVacationDetails = vacationDetails.SelectMany(vd => vd);

            // Renvoie la liste complète des détails de vacances
            return allVacationDetails;
        }
        #endregion

        #region Récupération des détails de congés d'un utilisateur spécifique
        public async Task<IEnumerable<VacationDetailsDTO>> GetVacationDetailsByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            var vacations = await _vacationsRepository.GetVacationsWithUsersByUserIdAsync(userId, cancellationToken);

            if (vacations == null || !vacations.Any())
            {
                return new List<VacationDetailsDTO>();
            }

            var vacationDetails = vacations.Select(v => new VacationDetailsDTO
            {
                UserId = userId,
                UserName = v.Users.FirstName + " " + v.Users.LastName,
                StartDate = v.StartDate,
                EndDate = v.EndDate,
                Type = v.Type,
                Justification = v.Justification,
                InitialBalance = _initialBalance,
                UsedBalance = CalculateUsedVacationDaysWithHoliday(vacations, userId),
                RemainingBalance = _initialBalance - CalculateUsedVacationDaysWithHoliday(vacations, userId)
            });

            return vacationDetails;
        }
        #endregion

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

            var usedVacationDays = CalculateUsedVacationDaysWithHoliday(vacationsWithUsers, userId);

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

        #region Réinitialiser le solde initial de congé à chaque nouvelle année
        public async Task ResetInitialBalanceAsync(int userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
        {
            // Vérifie que les dates de début et de fin sont valides.
            if (startDate > endDate)
            {
                throw new ArgumentException("La date de début doit être inférieure ou égale à la date de fin");
            }

            // Récupère le solde de congés de l'utilisateur.
            var vacationBalance = await _vacationsBalanceRepository.GetByUserIdAsync(userId, cancellationToken);

            // Réinitialise le solde initial de congé à zéro.
            vacationBalance.InitialVacationBalance = 0;

            // Met à jour le solde de congés dans le dépôt.
            await _vacationsBalanceRepository.UpdateAsync(userId, startDate, endDate, cancellationToken);
        }

        #endregion

        #region Methode privé pour prendre en compte les jours fériés et les week-ends lors du calcul du solde de vacances
        private int CalculateUsedVacationDaysWithHoliday(IEnumerable<Vacations> vacations, int userId)
        {
            return vacations.Where(v => v.UserId == userId)
                           .Sum(v =>
                           {
                               var startDate = v.StartDate;
                               var endDate = v.EndDate;
                               var totalDays = 0;

                               while (startDate <= endDate)
                               {
                                   if (startDate.DayOfWeek != DayOfWeek.Saturday &&
                                      startDate.DayOfWeek != DayOfWeek.Sunday &&
                                      !IsHoliday(startDate))
                                   {
                                       totalDays++;
                                   }

                                   startDate = startDate.AddDays(1);
                               }

                               return totalDays;
                           });
        }
        #endregion

        #region Implementation de la methode Holiday
        private bool IsHoliday(DateTime date)
        {
            return _holidays.Contains(date);
        }
        #endregion

        #region Cette méthode permet de valider ou de refuser les congés d'un utilisateur.
        public async Task<bool> ApproveOrRejectVacationAsync(int vacationId, string newStatus, CancellationToken cancellationToken)
        {
            var vacation = await _vacationsRepository.GetByIdAsync(vacationId, cancellationToken);

            if (vacation == null)
            {
                throw new ArgumentException("La demande de congé n'existe pas");
            }

            // Convertir la chaîne newStatus en énumération VacationsStatus
            if (!Enum.TryParse(newStatus, out VacationsStatus status))
            {
                throw new ArgumentException("Le statut n'est pas valide");
            }

            // Vérifier si le nouveau statut est différent du statut actuel
            if (vacation.Status == status)
            {
                throw new ArgumentException("Le statut est déjà défini sur " + newStatus);
            }

            // Modifier le statut de la demande de congé
            vacation.Status = status;

            // Enregistrer les modifications dans le dépôt
            bool updateResult = await _vacationsRepository.UpdateAsync(vacation, cancellationToken);

            // Retourner le résultat de la mise à jour
            return updateResult;
        }

        #endregion

    }
}
