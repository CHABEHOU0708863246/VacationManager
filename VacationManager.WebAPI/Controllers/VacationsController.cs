
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VacationManager.Domain.DTO;
using VacationManager.Domain.Interfaces.InterfaceService;
using VacationManager.Domain.Models;
using static VacationManager.Domain.Models.Vacations;

namespace VacationManager.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacationsController : ControllerBase
    {
        private readonly IVacationsService _vacationsService;

        public VacationsController(IVacationsService vacationsService)
        {
            _vacationsService = vacationsService;
        }

        #region Endpoint qui retourne tous les vacations (congé).
        /// <summary>
        /// Retourne tous les vacations (congé).
        /// </summary>
        /// <returns>La liste de toute les congés.</returns>
        [HttpGet]
        [SwaggerResponse(200, "Liste de congé retournée avec succès.", typeof(IEnumerable<VacationsDTO>))]
        [SwaggerResponse(404, "Vacation non trouvé.")]
        [SwaggerResponse(500, "Erreur serveur interne.")]
        public async Task<IActionResult> GetAllVacations(CancellationToken cancellationToken)
        {
            try
            {
                var vacations = await _vacationsService.GetAllVacationsAsync(cancellationToken);

                if (vacations != null && vacations.Any())
                {
                    var vacationsViewModels = new List<VacationsDTO>();

                    foreach (var vacation in vacations)
                    {
                        var user = vacation.Users;

                        var vacationViewModel = new VacationsDTO
                        {
                            Id = vacation.Id,
                            UserId = vacation.UserId,
                            StartDate = vacation.StartDate,
                            EndDate = vacation.EndDate,
                            Type = vacation.Type,
                            Status = vacation.Status.ToString(),
                            CreatedDate = vacation.CreatedDate,
                            Comments = vacation.Comments,
                            Justification = vacation.Justification
                        };

                        vacationsViewModels.Add(vacationViewModel);
                    }

                    return Ok(vacationsViewModels);
                }
                else
                {
                    return Ok(new List<VacationsDTO>());
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        #endregion

        #region Endpoint qui retourne un vacation par son ID.
        /// <summary>
        /// Retourne un vacation par son ID.
        /// </summary>
        /// <param name="id">L'ID de vacation.</param>
        /// <returns>vacation correspondant à l'ID spécifié.</returns>
        [HttpGet("{id}")]
        [SwaggerResponse(200, "Vacations retourné avec succès.", typeof(VacationsDTO))]
        [SwaggerResponse(404, "Vacation non trouvé.")]
        [SwaggerResponse(500, "Erreur serveur interne.")]
        public async Task<IActionResult> GetVacationsById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var vacations = await _vacationsService.GetVacationsByIdAsync(id, cancellationToken);
                if (vacations == null)
                {
                    return NotFound();
                }
                return Ok(vacations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Une erreur s’est produite lors de la récupération de congé : {ex.Message}");
            }
        }
        #endregion

        #region Endpoint qui cree un nouveau congé
        /// <summary>
        /// Cree un nouveau congé
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Un utilisateur nouvellement créé</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /User
        ///        {
        ///          "UsersID": 1,
        ///          "StartDate": "datetime",
        ///          "EndDate": "datetime",
        ///          "Type": "string",
        ///          "Status": "number",
        ///          "CreatedDate": "datetime",
        ///          "Comment": "string
        ///          "Justification" : "string",
        ///         }
        ///
        /// </remarks>
        /// <response code="201">Retourne l’élément nouvellement créé</response>
        /// <response code="400">Si l’élément est nul</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateVacations([FromBody] VacationsDTO vacationsDTO, CancellationToken cancellationToken)
        {
            try
            {
                var vacations = new Vacations
                {
                    UserId = vacationsDTO.UserId,
                    StartDate = vacationsDTO.StartDate,
                    EndDate = vacationsDTO.EndDate,
                    Type = vacationsDTO.Type,
                    Status = VacationsStatus.Attente,
                    CreatedDate = vacationsDTO.CreatedDate,
                    Comments = vacationsDTO.Comments,
                    Justification = vacationsDTO.Justification
                };

                var createdVacations = await _vacationsService.CreateVacationsAsync(vacations, cancellationToken);

                return CreatedAtAction(nameof(GetVacationsById), new { id = createdVacations.Id }, createdVacations);
            }
            catch (Exception ex)
            {
                // Log des détails de l'exception
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"InnerException: {ex.InnerException.Message}");
                    Console.WriteLine($"InnerException StackTrace: {ex.InnerException.StackTrace}");
                }
                return StatusCode(500, $"Une erreur s’est produite lors de la création de congé : {ex.Message}");
            }
        }
        #endregion

        #region Endpoint qui Modifie un congé par son ID.
        /// <summary>
        /// Modifie un congé par son ID.
        /// </summary>
        [HttpPut("{id}")]
        [SwaggerResponse(200, "Utilisateur mis à jour avec succès.", typeof(VacationsDTO))]
        [SwaggerResponse(404, "Utilisateur non trouvé.")]
        [SwaggerResponse(500, "Erreur serveur interne.")]
        public async Task<IActionResult> UpdateVacations(int id, [FromBody] VacationsDTO vacationsDTO, CancellationToken cancellationToken)
        {
            try
            {
                var vacations = await _vacationsService.GetVacationsByIdAsync(id, cancellationToken);

                if (vacations == null)
                {
                    return NotFound();
                }
                vacations.UserId = vacationsDTO.UserId;
                vacations.StartDate = vacationsDTO.StartDate;
                vacations.EndDate = vacationsDTO.EndDate;
                vacations.Type = vacationsDTO.Type;
                vacations.Status = VacationsStatus.Attente;
                vacations.CreatedDate = vacationsDTO.CreatedDate;
                vacations.Comments = vacationsDTO.Comments;
                vacations.Justification = vacationsDTO.Justification;

                var updated = await _vacationsService.UpdateVacationsAsync(id, vacations, cancellationToken);

                if (updated)
                {
                    return Ok(vacations);
                }
                else
                {
                    return StatusCode(500, "La mise à jour de l'utilisateur a échoué.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Une erreur s’est produite lors de la mise à jour de l'utilisateur : {ex.Message}");
            }
        }
        #endregion

        #region Endpoint qui Supprime un vacation par son ID.
        /// <summary>
        /// Supprime un vacation par son ID.
        /// </summary>
        /// <param name="id">L'ID de vacation à supprimer.</param>
        /// <returns>204 No Content si la suppression réussit.</returns>
        [HttpDelete("{id}")]
        [SwaggerResponse(204, "Vacations supprimé avec succès.")]
        [SwaggerResponse(500, "Erreur serveur interne.")]
        public async Task<IActionResult> DeleteVacations(int id, CancellationToken cancellationToken)
        {
            try
            {
                var deleted = await _vacationsService.DeleteVacationsAsync(id, cancellationToken);

                if (deleted)
                {
                    return NoContent();
                }
                else
                {
                    return StatusCode(500, "La suppression de congé a échoué.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Une erreur s’est produite lors de la suppression de l'utilisateur : {ex.Message}");
            }
        }
        #endregion
    }
}
