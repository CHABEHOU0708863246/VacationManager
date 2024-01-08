using Microsoft.EntityFrameworkCore;
using VacationManager.Domain.DTO;
using VacationManager.Domain.Interfaces.InterfacesRepository;
using VacationManager.Domain.Models;
using VacationManager.Infrastructure.Data;
using static VacationManager.Domain.Models.Vacations;

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
        public async Task<VacationsBalance> GetByUserIdAsync(int userId,CancellationToken cancellationToken)
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
                if (userId <= 0)
                {
                    throw new ArgumentException("L'ID de l'utilisateur doit être supérieur ou égal à 0.", nameof(userId));
                }

                if (startDate > endDate)
                {
                    throw new ArgumentException("La date de début doit être antérieure ou égale à la date de fin.", nameof(startDate));
                }

                var existingBalance = await GetByUserIdAsync(userId, cancellationToken);

                if (existingBalance == null)
                {
                    existingBalance = new VacationsBalance
                    {
                        UserId = userId,
                        InitialVacationBalance = 23,
                        Year = DateTime.Now.Year
                    };
                    _databaseContext.VacationsBalances.Add(existingBalance);
                }

                int usedDays = (endDate - startDate).Days + 1;

                // Vérifiez si les vacances s’étendent sur plusieurs années
                if (startDate.Year != endDate.Year)
                {
                    // Calculer les jours pour l’année en cours
                    DateTime endOfYear = new DateTime(startDate.Year, 12, 31);
                    int daysThisYear = (endOfYear - startDate).Days + 1;

                    // Calculer les jours pour l’année suivante
                    DateTime startOfNextYear = new DateTime(endDate.Year, 1, 1);
                    int daysNextYear = (endDate - startOfNextYear).Days + 1;

                    // Mettre à jour le solde de l’exercice en cours
                    existingBalance.RemainingVacationBalance -= daysThisYear;

                    // Créer ou récupérer le solde de l’année prochaine
                    var nextYearBalance = await GetByUserIdAsync(userId, cancellationToken);
                    if (nextYearBalance == null)
                    {
                        nextYearBalance = new VacationsBalance
                        {
                            UserId = userId,
                            Year = endDate.Year,
                            InitialVacationBalance = 23
                        };
                        _databaseContext.VacationsBalances.Add(nextYearBalance);
                    }
                    nextYearBalance.RemainingVacationBalance += daysNextYear;
                }
                else
                {
                    existingBalance.RemainingVacationBalance -= usedDays;
                }

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
            int totalUsedDays = 0;

            foreach (var vacation in vacations)
            {
                // Check if vacation spans across multiple years
                int days = (vacation.EndDate - vacation.StartDate).Days + 1;
                totalUsedDays += days;
            }

            return totalUsedDays;
        }

        #endregion

        #region Donne les details de congé en fonction de l'id de l'utilisateur ou employee
        public async Task<IEnumerable<VacationDetailsDTO>> GetVacationDetailsByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            var vacations = await _databaseContext.Vacations
                .Include(v => v.Users)
                .Where(v => v.UserId == userId)
                .ToListAsync(cancellationToken);

            if (!vacations.Any())
            {
                return Enumerable.Empty<VacationDetailsDTO>();
            }

            var vacationBalance = await GetByUserIdAsync(userId, cancellationToken);

            List<VacationDetailsDTO> vacationDetailsList = new List<VacationDetailsDTO>();

            foreach (var vacation in vacations)
            {
                int usedVacationDays = (vacation.EndDate - vacation.StartDate).Days + 1;

                var vacationDetailsDTO = new VacationDetailsDTO
                {
                    UserId = userId,
                    UserName = $"{vacation.Users.FirstName} {vacation.Users.LastName}",
                    StartDate = vacation.StartDate,
                    EndDate = vacation.EndDate,
                    Type = vacation.Type,
                    Justification = vacation.Justification,
                    InitialBalance = vacationBalance.InitialVacationBalance,
                    UsedBalance = usedVacationDays,
                    RemainingBalance = vacationBalance.InitialVacationBalance - usedVacationDays,
                };

                vacationDetailsList.Add(vacationDetailsDTO);
            }

            return vacationDetailsList;
        }
        #endregion

        #region Calcul du solde restant
        public async Task<VacationsBalance> CalculateRemainingBalance(int userId, CancellationToken cancellationToken)
        {
            var existingBalance = await GetByUserIdAsync(userId, cancellationToken);

            var vacations = await _databaseContext.Vacations
                .Where(v => v.UserId == userId)
                .ToListAsync(cancellationToken);

            int usedVacationDays = CalculateUsedVacationDays(vacations);

            existingBalance.UsedVacationBalance = usedVacationDays;
            existingBalance.RemainingVacationBalance = existingBalance.InitialVacationBalance - usedVacationDays;

            return existingBalance;
        }

        #endregion

        #region Cette méthode permet de valider ou de refuser les congé en fonction de l'id de congés si le status du congé est en attente ou acceptre le solde initial est calcule mais si le statut est refusée alors le solde initial revien à 23
        public async Task<bool> ApproveOrRejectVacationAsync(int vacationId, CancellationToken cancellationToken, VacationsStatus newStatus)
        {
            try
            {
                var vacation = await _databaseContext.Vacations.FirstOrDefaultAsync(v => v.Id == vacationId, cancellationToken);

                if (vacation == null)
                {
                    throw new ArgumentException("La demande de congé n'existe pas");
                }

                var existingBalance = await GetByUserIdAsync(vacation.UserId, cancellationToken) ?? new VacationsBalance
                {
                    UserId = vacation.UserId,
                    InitialVacationBalance = 23 // Solde initial par défaut
                };

                // Mettre à jour le solde en fonction du nouveau statut
                switch (newStatus)
                {
                    case VacationsStatus.Approuve:
                    case VacationsStatus.Attente:
                        var usedDays = (vacation.EndDate - vacation.StartDate).Days + 1;
                        existingBalance.UsedVacationBalance += usedDays;
                        existingBalance.RemainingVacationBalance = existingBalance.InitialVacationBalance - existingBalance.UsedVacationBalance;
                        break;
                    case VacationsStatus.Rejected:
                        existingBalance.RemainingVacationBalance = 23;
                        break;
                    default:
                        break;
                }

                vacation.Status = newStatus;
                await _databaseContext.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        #endregion

        #region Vérifie si une demande de congé spécifique existe.
        public async Task<bool> CheckVacationExistsAsync(int vacationId, CancellationToken cancellationToken)
        {
            var vacation = await _databaseContext.Vacations.FirstOrDefaultAsync(v => v.Id == vacationId, cancellationToken);
            return vacation != null;
        }
        #endregion

        #region Met a jour directement le status de congé valider ou refuser
        public async Task<bool> UpdateVacationStatusAsync(int vacationId, VacationsStatus newStatus, CancellationToken cancellationToken)
        {
            try
            {
                var vacation = await _databaseContext.Vacations.FirstOrDefaultAsync(v => v.Id == vacationId, cancellationToken);

                if (vacation == null)
                {
                    throw new ArgumentException("La demande de congé n'existe pas");
                }

                var existingBalance = await GetByUserIdAsync(vacation.UserId, cancellationToken) ?? new VacationsBalance
                {
                    UserId = vacation.UserId,
                    InitialVacationBalance = 23 // Solde initial par défaut
                };

                if (newStatus == VacationsStatus.Approuve || newStatus == VacationsStatus.Attente)
                {
                    var usedDays = (vacation.EndDate - vacation.StartDate).Days + 1;
                    existingBalance.UsedVacationBalance += usedDays;
                    existingBalance.RemainingVacationBalance = existingBalance.InitialVacationBalance - existingBalance.UsedVacationBalance;
                }
                else if (newStatus == VacationsStatus.Rejected)
                {
                    existingBalance.RemainingVacationBalance = 23; // Réinitialiser le solde initial en cas de refus
                }

                vacation.Status = newStatus;

                await _databaseContext.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        #endregion

        #region Gere le calcul du solde de congé en fonction du status du congé
        public async Task<bool> UpdateBalanceByVacationStatusAsync(int userId, Vacations.VacationsStatus status, CancellationToken cancellationToken)
        {
            try
            {
                var balance = await _databaseContext.VacationsBalances.FirstOrDefaultAsync(vb => vb.UserId == userId);

                if (balance == null)
                {
                    return false; // Gérer le cas où le solde de congé pour cet utilisateur n'existe pas encore
                }

                // Mise à jour du solde en fonction du statut du congé
                switch (status)
                {
                    case Vacations.VacationsStatus.Attente:
                    case Vacations.VacationsStatus.Approuve:
                        balance.RemainingVacationBalance = balance.InitialVacationBalance - balance.UsedVacationBalance;
                        break;
                    case Vacations.VacationsStatus.Rejected:
                        balance.RemainingVacationBalance = 0; // Réinitialisation du solde à zéro
                        break;
                    default:
                        // Traitez d'autres cas de statut si nécessaire
                        break;
                }

                // Sauvegarde des changements dans la base de données
                await _databaseContext.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la mise à jour du solde de congés : {ex.Message}");
                throw;
            }
        }
        #endregion

        #region Supprime un solde de congé en fonction de l'id de l'utiulisateur depuis la base de données
        public async Task<bool> DeleteVacationBalanceAsync(int userId, CancellationToken cancellationToken)
        {
            try
            {
                var balanceToDelete = await _databaseContext.VacationsBalances.FirstOrDefaultAsync(vb => vb.UserId == userId, cancellationToken);

                if (balanceToDelete != null)
                {
                    _databaseContext.VacationsBalances.Remove(balanceToDelete);
                    await _databaseContext.SaveChangesAsync(cancellationToken);
                    return true; // La suppression a réussi
                }

                return false; // Aucun solde de congés trouvé pour cet utilisateur
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Erreur lors de la suppression du solde de congés : {ex.Message}");
                throw; // Gérer spécifiquement les exceptions liées à la base de données
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur inattendue lors de la suppression du solde de congés : {ex.Message}");
                throw; // Capturer toutes les autres exceptions inattendues
            }
        }

        #endregion

        #region Ajoute un solde de congés pour une nouvelle année
        public async Task AddAsync(VacationsBalance balance, CancellationToken cancellationToken)
        {
            try
            {
                _databaseContext.VacationsBalances.Add(balance);
                await _databaseContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'ajout du solde de congés : {ex.Message}");
                throw;
            }
        }
        #endregion

    }
}