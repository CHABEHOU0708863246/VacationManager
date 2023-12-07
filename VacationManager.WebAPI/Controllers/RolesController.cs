using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VacationManager.Domain.DTO;
using VacationManager.Domain.Interfaces.InterfaceService;
using VacationManager.Domain.Models;

namespace VacationManager.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        public readonly IRolesService _rolesService;

        public RolesController(IRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        /// <summary>
        /// Retourne tous les rôles.
        /// </summary>
        /// <returns>La liste de tous les rôles.</returns>
        [HttpGet]
        [SwaggerResponse(200, "Liste de rôles retournée avec succès.", typeof(IEnumerable<RolesDTO>))]
        [SwaggerResponse(500, "Erreur serveur interne.")]
        public async Task<IActionResult> GetAllRoles(CancellationToken cancellationToken)
        {
            try
            {
                var roles = await _rolesService.GetAllRolesAsync(cancellationToken);

                // Si des rôles sont trouvés, les convertit en modèles de vue et renvoie une réponse OK.
                if (roles != null && roles.Any())
                {
                    var rolesViewModels = roles.Select(role => new RolesDTO
                    {
                        Id = role.Id,
                        Name = role.Name
                    });

                    return Ok(rolesViewModels);
                }

                // Si aucun rôle n'est trouvé, renvoie une réponse OK avec une liste vide.
                else
                {
                    return Ok(new List<RolesDTO>());
                }
            }

            // Si une exception est levée, renvoie une réponse avec le code d'état 500 et le message d'erreur.
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        /// <summary>
        /// Retourne un rôle par son ID.
        /// </summary>
        /// <param name="id">L'ID du rôle.</param>
        /// <returns>Le rôle correspondant à l'ID spécifié.</returns>
        [HttpGet("{id:int}", Name = "GetRoleById")]
        [SwaggerResponse(200, "Rôle retourné avec succès.", typeof(RolesDTO))]
        [SwaggerResponse(404, "Rôle non trouvé.")]
        [SwaggerResponse(500, "Erreur serveur interne.")]
        public async Task<IActionResult> GetRoleById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var role = await _rolesService.GetRolesByIdAsync(id, cancellationToken);

                if (role == null)
                {
                    return NotFound();
                }

                return Ok(role);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Une erreur s’est produite lors de la récupération du rôle : {ex.Message}");
            }
        }

        /// <summary>
        /// Cree un nouveau role
        /// </summary>
        /// <param name="RoleName"></param>
        /// <returns>Un Role nouvellement créé</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Rôle
        ///     {
        ///        "id": 1,
        ///        "name": "RoleName",
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Retourne l’élément nouvellement créé</response>
        /// <response code="400">Si l’élément est nul</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateRoles([FromBody] RolesDTO roleDTO, CancellationToken cancellationToken)
        {
            try
            {
                var role = new Roles
                {
                    Name = roleDTO.Name
                };

                var createdRole = await _rolesService.CreateRolesAsync(role, cancellationToken);

                // Si le rôle est créé avec succès, renvoie une réponse créée avec le rôle.
                return CreatedAtAction(nameof(GetRoleById), new { id = createdRole.Id }, createdRole);
            }

            // Si une exception est levée, renvoie une réponse avec le code d'état 500 et le message d'erreur.
            catch (Exception ex)
            {
                return StatusCode(500, $"Une erreur s’est produite lors de la création du rôle : {ex.Message}");
            }
        }

        /// <summary>
        /// Modifie un rôle par son ID.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoles(int id, [FromBody] RolesDTO roleDTO, CancellationToken cancellationToken)
        {
            try
            {
                var role = await _rolesService.GetRolesByIdAsync(id, cancellationToken);

                // Si le rôle est trouvé, met à jour le rôle et renvoie une réponse OK avec le rôle mis à jour.
                if (role == null)
                {
                    return NotFound();
                }

                role.Name = roleDTO.Name;

                var updated = await _rolesService.UpdateRolesAsync(id, role, cancellationToken);

                if (updated)
                {
                    return Ok(role);
                }
                else
                {
                    return StatusCode(500, "La mise à jour du rôle a échoué.");
                }
            }

            // Si une exception est levée, renvoie une réponse avec le code d'état 500 et le message d'erreur.
            catch (Exception ex)
            {
                return StatusCode(500, $"Une erreur s’est produite lors de la mise à jour du rôle : {ex.Message}");
            }
        }

        /// <summary>
        /// Supprime un rôle par son ID.
        /// </summary>
        /// <param name="id">L'ID du rôle à supprimer.</param>
        /// <returns>204 No Content si la suppression réussit.</returns>
        [HttpDelete("{id}")]
        [SwaggerResponse(204, "Rôle supprimé avec succès.")]
        [SwaggerResponse(500, "Erreur serveur interne.")]
        public async Task<IActionResult> DeleteUsers(int id, CancellationToken cancellationToken)
        {
            try
            {
                var deleted = await _rolesService.DeleteRolesAsync(id, cancellationToken);

                if (deleted)
                {
                    return NoContent();
                }
                else
                {
                    return StatusCode(500, "La suppression du rôle à échoué.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Une erreur s’est produite lors de la suppression de l'utilisateur : {ex.Message}");
            }
        }
    }
}
