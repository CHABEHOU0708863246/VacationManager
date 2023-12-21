

using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VacationManager.Domain.Interfaces.InterfaceService.Authentification;
using VacationManager.Domain.Models.Authentification;

namespace VacationManager.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        #region Endpoint pour l'Authentification d'un utilisateur en utilisant un e-mail et un mot de passe.
        /// <summary>
        /// Authentification d'un utilisateur en utilisant un e-mail et un mot de passe.
        /// </summary>
        /// <param name="user">Les informations d'authentification de l'utilisateur.</param>
        /// <response code="200">Authentification réussie, renvoie le jeton d'authentification.</response>
        /// <response code="401">Échec de l'authentification.</response>
        [HttpPost]
        [Route("login")]
        [SwaggerResponse(200, "Authentification réussie", typeof(AuthenticationResponse))]
        [SwaggerResponse(401, "Échec de l'authentification")]
        public async Task<IActionResult> Login([FromBody] AuthenticationRequest user, CancellationToken cancellationToken)
        {
            // Crée une nouvelle demande d'authentification.
            var authenticationRequest = new AuthenticationRequest
            {
                Email = user.Email,
                Password = user.Password
            };

            // Authentifie l'utilisateur.
            var response = await _authenticationService.AuthenticateAsync(authenticationRequest, cancellationToken);

            // Si l'authentification échoue, renvoie une réponse non autorisée.
            if (response == null)
            {
                return Unauthorized();
            }

            // Si l'authentification réussit, renvoie une réponse OK avec le jeton d'authentification et le rôle de l'utilisateur.
            return Ok(new AuthenticationResponse
            {
                Token = response.Token,
                Role = response.Role,
            });
        }
        #endregion

    }
}
