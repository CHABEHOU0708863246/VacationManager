using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VacationManager.Domain.DTO;
using VacationManager.Domain.Interfaces.InterfaceService;
using VacationManager.Domain.Models;

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
        public async Task<ActionResult<VacationsReport>> GetAllReports(CancellationToken cancellationToken)
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

    }
}
