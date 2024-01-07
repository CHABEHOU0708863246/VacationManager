using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using VacationManager.Domain.Configurations.Jwt;
using VacationManager.Domain.Interfaces.InterfaceService;
using VacationManager.Domain.Interfaces.InterfaceService.Authentification;
using VacationManager.Domain.Models;
using VacationManager.Domain.Models.Authentification;
using VacationManager.Domain.Tools;

namespace VacationManager.Domain.Services.Authentification
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUsersService _usersService;
        private readonly JwtSettings _jwtSettings;

        public AuthenticationService(IUsersService usersService, IOptions<JwtSettings> jwtSettings)
        {
            _usersService = usersService;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request, CancellationToken cancellationToken)
        {
            // Recherche de l'utilisateur par e-mail
            var user = await _usersService.GetUserByEmailAsync(request.Email, cancellationToken);

            if (user == null || !PasswordTool.VerifyPassword(request.Password, user.Password))
            {
                return null; // L'authentification a échoué
            }

            // Générer le jeton JWT
            string token = GenerateJwtToken(user);

            return new AuthenticationResponse
            {
                Token = token,
                Role = user.Roles.Name,
            };
        }


        private string GenerateJwtToken(Users user)
        {
            var key = JwtSecurityKey.Create(_jwtSettings.Secret);
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.PrimarySid, Convert.ToString(user.Id) ),
                new Claim(ClaimTypes.Role, user.Roles.ToString())
            };

            var token = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Issuer,
                claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}