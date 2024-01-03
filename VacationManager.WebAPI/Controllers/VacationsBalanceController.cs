using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VacationManager.Domain.DTO;
using VacationManager.Domain.Interfaces.InterfaceService;

namespace VacationManager.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacationsBalanceController : ControllerBase
    {
        private readonly IVacationsBalanceService _vacationsBalanceService;

        public VacationsBalanceController(IVacationsBalanceService vacationsBalanceService)
        {
            _vacationsBalanceService = vacationsBalanceService;
        }

        #region Endpoint qui retourne le solde de congé de tous les utilisateurs
        /// <summary>
        /// Retourne le solde de congé de tous les utilisateurs
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("all")]
        [SwaggerResponse(200, "Solde de congés de tous les utilisateurs retourné avec succès.", typeof(IEnumerable<VacationDetailsDTO>))]
        [SwaggerResponse(500, "Erreur serveur interne.")]
        public async Task<ActionResult<IEnumerable<VacationDetailsDTO>[]>> GetVacationDetails(CancellationToken cancellationToken)
        {
            try
            {
                var vacationDetails = await _vacationsBalanceService.GetAllVacationDetailsAsync(cancellationToken);

                return Ok(vacationDetails);
            }
            catch (Exception ex)
            {
                // Gérer les exceptions ici
                return StatusCode(500, $"Une erreur est survenue : {ex.Message}");
            }
        }
        #endregion

        #region Endpoint qui rerourne le solde de congé en fonction de l'id de l'utilisateur
        /// <summary>
        /// Rerourne le solde de congé en fonction de l'id de l'utilisateur
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        [SwaggerResponse(200, "Solde de congés retourné avec succès.", typeof(VacationsBalanceDTO))]
        [SwaggerResponse(404, "L'utilisateur spécifié n'existe pas.")]
        [SwaggerResponse(500, "Erreur serveur interne.")]
        public async Task<IActionResult> GetVacationBalanceByUserId(int userId, CancellationToken cancellationToken)
        {
            try
            {
                var vacationBalance = await _vacationsBalanceService.GetVacationBalanceByUserIdAsync(userId, cancellationToken);

                if (vacationBalance != null)
                {
                    return Ok(vacationBalance);
                }
                else
                {
                    return NotFound("Le solde de congés de l'utilisateur spécifié n'existe pas.");
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Paramètre invalide : {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Une erreur s'est produite : {ex.Message}");
            }
        }
        #endregion

        #region Endpoint qui met a jour la balance de congés
        /// <summary>
        /// Met a jour la balance de congés
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{userId}/{startDate}/{endDate}")]
        [SwaggerResponse(200, "Mise à jour de la balance de congés effectuée avec succès.")]
        [SwaggerResponse(400, "Requête invalide.")]
        [SwaggerResponse(500, "La mise à jour de la balance de congés a échoué.")]
        public async Task<IActionResult> UpdateVacationBalance(int userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
        {
            try
            {
                // Récupère le solde de congés actuel de l'utilisateur.
                var vacationBalance = await _vacationsBalanceService.GetVacationBalanceByUserIdAsync(userId, cancellationToken);

                // Si le solde de congés n'existe pas, renvoie une réponse avec le code d'état 404.
                if (vacationBalance == null)
                {
                    return NotFound("Le solde de congés de l'utilisateur spécifié n'existe pas.");
                }

                // Calcule la durée du congé demandé.
                int vacationDuration = (endDate - startDate).Days;

                // Met à jour le solde de congés de l'utilisateur.
                vacationBalance.UsedVacationBalance += vacationDuration;
                vacationBalance.RemainingVacationBalance -= vacationDuration;

                // Met à jour le solde de congés dans la base de données.
                var updated = await _vacationsBalanceService.UpdateVacationBalanceAsync(userId, startDate, endDate, cancellationToken);

                // Si la mise à jour a réussi, renvoie une réponse avec le code d'état 200.
                if (updated)
                {
                    return Ok();
                }
                else
                {
                    return StatusCode(500, "La mise à jour de la balance de congés a échoué.");
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Paramètre invalide : {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Une erreur s'est produite : {ex.Message}");
            }
        }
        #endregion

        #region Endpoint pour la Validation ou le refus de congés
        /// <summary>
        /// Validation ou refus de congés
        /// </summary>
        /// <param name="vacationId"></param>
        /// <param name="newStatus"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("approve-or-reject/{vacationId}")]
        [SwaggerResponse(200, "Le statut de la demande de congé a été mis à jour avec succès.")]
        [SwaggerResponse(400, "Requête invalide.")]
        [SwaggerResponse(404, "La demande de congé spécifiée n'existe pas.")]
        [SwaggerResponse(500, "Une erreur serveur interne s'est produite.")]
        public async Task<IActionResult> ApproveOrRejectVacation([FromRoute] int vacationId, string newStatus, CancellationToken cancellationToken)
        {
            try
            {
                // Utilisation de la méthode mise à jour du service
                var updated = await _vacationsBalanceService.ApproveOrRejectVacationAsync(vacationId, newStatus, cancellationToken);

                if (updated)
                {
                    return Ok("Le statut de la demande de congé a été mis à jour avec succès.");
                }
                else
                {
                    return StatusCode(500, "La mise à jour du statut de la demande de congé a échoué.");
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Paramètre invalide : {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Une erreur s'est produite : {ex.Message}");
            }
        }
        #endregion


    }
}
