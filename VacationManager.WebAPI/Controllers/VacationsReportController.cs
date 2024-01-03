using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VacationManager.Domain.DTO;
using VacationManager.Domain.Interfaces.InterfaceService;

namespace VacationManager.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacationsReportController : ControllerBase
    {
        private readonly IVacationsReportService _vacationsReportService;

        public VacationsReportController(IVacationsReportService vacationsReportService)
        {
            _vacationsReportService = vacationsReportService;
        }


        #region Enpoint qui récupère tous les rapports sur l'utilisation des congés
        /// <summary>
        /// Récupérer tous les rapports sur l'utilisation des congés
        /// </summary>
        /// <returns></returns>
        [HttpGet("all-reports")]
        [SwaggerResponse(200, "Liste des rapport de congés retournée avec succès.", typeof(IEnumerable<VacationsReportDTO>))]
        [SwaggerResponse(404, "Rapport de congés non trouvé.")]
        [SwaggerResponse(500, "Erreur serveur interne.")]
        public async Task<ActionResult<VacationsReportDTO>> GetAllReports(CancellationToken cancellationToken)
        {
            try
            {
                var reportStatistics = await _vacationsReportService.GetAllVacationsStatisticsAsync(cancellationToken);
                return Ok(reportStatistics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Une erreur s'est produite : {ex.Message}");
            }
        }
        #endregion

        #region Endpoint pour récupérer les rapports sur l'utilisation des congés pour un utilisateur spécifique
        /// <summary>
        /// Récupérer les rapports sur l'utilisation des congés pour un utilisateur spécifique
        /// </summary>
        /// <param name="userId">L'ID de l'utilisateur</param>
        /// <returns></returns>
        [HttpGet("reports/{userId}")]
        [SwaggerResponse(200, "Rapport de congés pour l'utilisateur spécifié retourné avec succès.", typeof(VacationsReportDTO))]
        [SwaggerResponse(404, "Rapport de congés pour l'utilisateur spécifié non trouvé.")]
        [SwaggerResponse(500, "Erreur serveur interne.")]
        public async Task<ActionResult<VacationsReportDTO>> GetUserReports(int userId, CancellationToken cancellationToken)
        {
            try
            {
                var reportStatistics = await _vacationsReportService.GetVacationsStatisticsForUserAsync(userId, cancellationToken);
                return Ok(reportStatistics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Une erreur s'est produite : {ex.Message}");
            }
        }
        #endregion

    }
}
