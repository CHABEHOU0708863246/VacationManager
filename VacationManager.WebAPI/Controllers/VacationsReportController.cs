using Microsoft.AspNetCore.Http;
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
        /// Récupère tous les rapports sur l'utilisation des congés
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("reports")]
        [SwaggerResponse(200, "Tous les rapports sur l'utilisation des congés retournés avec succès.", typeof(IEnumerable<VacationsReportDTO>))]
        [SwaggerResponse(500, "Erreur serveur interne.")]
        public async Task<IActionResult> GetAllReports(CancellationToken cancellationToken)
        {
            try
            {
                var reports = await _vacationsReportService.GetAllReportsAsync(cancellationToken);

                // Mapper les modèles de rapport vers des DTO pour le retour
                var reportsDTO = reports.Select(report => new VacationsReportDTO
                {
                    Id = report.Id,
                    UserId = report.UserId,
                    TotalPending = report.TotalPending,
                    TotalApproved = report.TotalApproved,
                    TotalRejected = report.TotalRejected,
                    CurrentBalance = report.CurrentBalance,
                    Status = report.Status,
                    ReportDate = report.ReportDate
                }).ToList();

                return Ok(reportsDTO);
            }
            catch (Exception ex)
            {
                // Gérer les exceptions spécifiques ici et retourner une réponse appropriée
                if (ex is Exception)
                {
                    return StatusCode(400, $"Erreur spécifique : {ex.Message}");
                }
                else
                {
                    return StatusCode(500, $"Erreur lors de la récupération des rapports : {ex.Message}");
                }
            }
        }
        #endregion

    }
}
