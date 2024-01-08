using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VacationManager.Domain.DTO;
using VacationManager.Domain.DTO.ResetPassword;
using VacationManager.Domain.Interfaces.InterfaceService;
using VacationManager.Domain.Models;
using VacationManager.Domain.Services;
using VacationManager.Domain.Tools;

namespace VacationManager.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        #region Endpoint qui retourne la liste paginée des utilisateurs (5 utilisateurs par page).
        /// <summary>
        /// Retourne la liste paginée des utilisateurs (5 utilisateurs par page).
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("paginate")]
        [SwaggerResponse(200, "Liste paginée des utilisateurs retournée avec succès.", typeof(IEnumerable<UsersDTO>))]
        [SwaggerResponse(500, "Erreur serveur interne.")]
        public async Task<IActionResult> GetAllUsersWithoutPagination(CancellationToken cancellationToken, int pageNumber = 1, int pageSize = 5)
        {
            try
            {
                var users = await _usersService.GetUsersWithPaginationAsync(pageNumber, pageSize, cancellationToken);

                if (users != null && users.Any())
                {
                    var userViewModels = users.Select(u => new UsersDTO
                    {
                        Id = u.Id,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Email = u.Email,
                        Password = u.Password,
                        Gender = u.Gender,
                        PhoneNumber = u.PhoneNumber,
                        PostalAddress = u.PostalAddress,
                        City = u.City,
                        Profession = u.Profession,
                        MaritalStatus = u.MaritalStatus,
                        NumberOfChildren = u.NumberOfChildren,
                        Nationality = u.Nationality,
                        DateOfBirth = u.DateOfBirth,
                        DateCreateAccount = u.DateCreateAccount,
                        DateUpdateAccount = u.DateUpdateAccount,
                        RoleID = u.RoleID,
                        RoleName = u.Roles.Name
                    });

                    return Ok(userViewModels);
                }
                else
                {
                    return Ok(new List<UsersDTO>());
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de la récupération des utilisateurs : {ex.Message}");
            }
        }
        #endregion

        #region Endpoint qui retourne tous les utilisateurs.
        /// <summary>
        /// Retourne tous les utilisateurs.
        /// </summary>
        /// <returns>La liste de tous les rôles.</returns>
        [HttpGet]
        [SwaggerResponse(200, "Liste de utilisateurs retournée avec succès.", typeof(IEnumerable<UsersDTO>))]
        [SwaggerResponse(404, "Utilisateur non trouvé.")]
        [SwaggerResponse(500, "Erreur serveur interne.")]

        public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
        {
            try
            {
                var users = await _usersService.GetAllUsersAsync(cancellationToken);

                if (users != null && users.Any())
                {
                    var userViewModels = users.Select(u => new UsersDTO
                    {
                        Id = u.Id,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Email = u.Email,
                        Password = u.Password,
                        Gender = u.Gender,
                        PhoneNumber = u.PhoneNumber,
                        PostalAddress = u.PostalAddress,
                        City = u.City,
                        Profession = u.Profession,
                        MaritalStatus = u.MaritalStatus,
                        NumberOfChildren = u.NumberOfChildren,
                        Nationality = u.Nationality,
                        DateOfBirth = u.DateOfBirth,
                        DateCreateAccount = u.DateCreateAccount,
                        DateUpdateAccount = u.DateUpdateAccount,
                        RoleID = u.RoleID,
                        RoleName = u.Roles.Name
                    });

                    return Ok(userViewModels);
                }
                else
                {
                    return Ok(new List<UsersDTO>());
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de la récupération de tous les utilisateurs : {ex.Message}");
            }
        }
        #endregion

        #region Endpoint retourne un utilisateur par son ID.
        /// <summary>
        /// Retourne un utilisateur par son ID.
        /// </summary>
        /// <param name="id">L'ID de l'utilisateur.</param>
        /// <returns>L'utilisateur correspondant à l'ID spécifié.</returns>
        [HttpGet("{id:int}")]
        [SwaggerResponse(200, "Utilisateur retourné avec succès.", typeof(UsersDTO))]
        [SwaggerResponse(404, "Utilisateur non trouvé.")]
        [SwaggerResponse(500, "Erreur serveur interne.")]
        public async Task<IActionResult> GetUsersById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _usersService.GetUsersByIdAsync(id, cancellationToken);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Une erreur s’est produite lors de la récupération de l'utilisateur : {ex.Message}");
            }
        }
        #endregion

        #region Endpoint qui cree un nouveau utilisateur
        /// <summary>
        /// Cree un nouveau utilisateur
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Un utilisateur nouvellement créé</returns>
        /// <response code="201">Retourne l’élément nouvellement créé</response>
        /// <response code="400">Si l’élément est nul</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUsers([FromBody] UsersDTO userDTO, CancellationToken cancellationToken)
        {
            try
            {
                var user = new Users
                {
                    FirstName = userDTO.FirstName,
                    LastName = userDTO.LastName,
                    Email = userDTO.Email,
                    Password = PasswordTool.HashPassword(userDTO.Password),
                    Gender = userDTO.Gender,
                    PhoneNumber = userDTO.PhoneNumber,
                    PostalAddress = userDTO.PostalAddress,
                    City = userDTO.City,
                    Profession = userDTO.Profession,
                    MaritalStatus = userDTO.MaritalStatus,
                    NumberOfChildren = userDTO.NumberOfChildren,
                    Nationality = userDTO.Nationality,
                    DateOfBirth = userDTO.DateOfBirth,
                    DateCreateAccount = userDTO.DateCreateAccount,
                    DateUpdateAccount = userDTO.DateUpdateAccount,
                    RoleID = userDTO.RoleID
                };

                var createdUser = await _usersService.CreateUsersAsync(user, cancellationToken);

                return CreatedAtAction(nameof(GetUsersById), new { id = createdUser.Id }, createdUser);
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
                return StatusCode(500, $"Erreur lors de la création de l'utilisateur : {ex.Message}");
            }
        }
        #endregion

        #region Endpoint qui modifie un utilisateur par son ID.
        /// <summary>
        /// Modifie un utilisateur par son ID.
        /// </summary>
        [HttpPut("{id}")]
        [SwaggerResponse(200, "Utilisateur mis à jour avec succès.", typeof(UsersDTO))]
        [SwaggerResponse(404, "Utilisateur non trouvé.")]
        [SwaggerResponse(500, "Erreur serveur interne.")]
        public async Task<IActionResult> UpdateUsers(int id, [FromBody] UsersDTO userDTO, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _usersService.GetUsersByIdAsync(id, cancellationToken);

                if (user == null)
                {
                    return NotFound();
                }

                user.FirstName = userDTO.FirstName;
                user.LastName = userDTO.LastName;
                user.Email = userDTO.Email;
                user.Password = PasswordTool.HashPassword(userDTO.Password);
                user.Gender = userDTO.Gender;
                user.PhoneNumber = userDTO.PhoneNumber;
                user.PostalAddress = userDTO.PostalAddress;
                user.City = userDTO.City;
                user.Profession = userDTO.Profession;
                user.MaritalStatus = userDTO.MaritalStatus;
                user.NumberOfChildren = userDTO.NumberOfChildren;
                user.Nationality = userDTO.Nationality;
                user.DateOfBirth = userDTO.DateOfBirth;
                user.DateCreateAccount = userDTO.DateCreateAccount;
                user.DateUpdateAccount = userDTO.DateUpdateAccount;
                user.RoleID = userDTO.RoleID;

                var updated = await _usersService.UpdateUsersAsync(id, user, cancellationToken);

                if (updated)
                {
                    return Ok(user);
                }
                else
                {
                    return StatusCode(500, "La mise à jour de l'utilisateur a échoué.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de la mise à jour de l'utilisateur : {ex.Message}");
            }
        }
        #endregion

        #region Enpoint qui supprime un utilisateur par son ID.
        /// <summary>
        /// Supprime un utilisateur par son ID.
        /// </summary>
        /// <param name="id">L'ID de l'utilisateur à supprimer.</param>
        /// <returns>204 No Content si la suppression réussit.</returns>
        [HttpDelete("{id}")]
        [SwaggerResponse(204, "Utilisateur supprimé avec succès.")]
        [SwaggerResponse(404, "Utilisateur non trouvé.")]
        [SwaggerResponse(500, "Erreur serveur interne.")]
        public async Task<IActionResult> DeleteUsers(int id, CancellationToken cancellationToken)
        {
            try
            {
                var deleted = await _usersService.DeleteUsersAsync(id, cancellationToken);

                if (deleted)
                {
                    return NoContent();
                }
                else
                {
                    return StatusCode(500, "La suppression de l'utilisateur a échoué.");
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
