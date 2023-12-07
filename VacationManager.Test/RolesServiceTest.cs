

using Moq;
using VacationManager.Domain.Interfaces.InterfaceService;
using VacationManager.Domain.Interfaces.InterfacesRepository;
using VacationManager.Domain.Models;
using VacationManager.Domain.Services;

namespace VacationManager.Test
{
    [TestClass]
    public class RolesServiceTest
    {
        private Mock<IRolesRepository> _rolesRepositoryMock;
        private IRolesService _rolesService;

        [TestInitialize]
        public void Initialize()
        {
            _rolesRepositoryMock = new Mock<IRolesRepository>();
            _rolesService = new RolesService(_rolesRepositoryMock.Object);
        }

        [TestMethod]
        [Description("Teste la méthode GetAllRolesAsync pour s'assurer qu'elle renvoie une liste de rôles correcte.")]
        public async Task GetAllRolesAsync_ReturnListOfRoles()
        {
            //Arrange 
            var expectedRoles = new List<Roles>
            {
                new Roles { Id = 1, Name = "Admin"},
                new Roles { Id = 2, Name = "Manager"},
                new Roles { Id = 3, Name = "Employee"}
            };

            // Utilisez CancellationToken.None à des fins de test
            CancellationToken cancellationToken = CancellationToken.None;

            // Configuration simulée : Configurer le comportement _rolesRepositoryMock pour la méthode GetAllAsync
            _rolesRepositoryMock.Setup(r => r.GetAllAsync(cancellationToken)).ReturnsAsync(expectedRoles);

            //Act: Exécuter la méthode testée
            var roles = await _rolesService.GetAllRolesAsync(cancellationToken);

            //Assert: Vérifier si le résultat réel correspond au résultat attendu
            CollectionAssert.AreEqual(expectedRoles, new List<Roles>(roles));
        }

        [TestMethod]
        [Description("Teste la méthode GetRolesByIdAsync pour vérifier qu'elle renvoie le rôle attendu.")]
        public async Task GetRolesByIdAsync_ReturnsRole()
        {
            // Arrange
            int roleId = 1; // ID du rôle à récupérer
            var expectedRole = new Roles { Id = roleId, Name = "Admin" }; // Rôle attendu

            CancellationToken cancellationToken = CancellationToken.None;

            _rolesRepositoryMock.Setup(r => r.GetByIdAsync(roleId, cancellationToken)).ReturnsAsync(expectedRole);

            // Act
            var role = await _rolesService.GetRolesByIdAsync(roleId, cancellationToken);

            // Assert
            Assert.IsNotNull(role);
            Assert.AreEqual(expectedRole.Id, role.Id);
            Assert.AreEqual(expectedRole.Name, role.Name);
        }

        [TestMethod]
        [Description("Teste la méthode GetRolesByIdAsync avec un ID de rôle invalide.")]
        public async Task GetRolesByIdAsync_InvalidId_ThrowsArgumentException()
        {
            // Arrange
            int invalidRoleId = 0; // ID invalide
            CancellationToken cancellationToken = CancellationToken.None;

            // Assert & Act
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                // Act
                await _rolesService.GetRolesByIdAsync(invalidRoleId, cancellationToken);
            });
        }

        [TestMethod]
        [Description("Teste la méthode CreateRolesAsync pour vérifier la création d'un nouveau rôle.")]
        public async Task CreateRolesAsync_CreatesNewRole()
        {
            // Arrange
            var newRole = new Roles { Id = 1, Name = "Admin" }; // Rôle à créer

            CancellationToken cancellationToken = CancellationToken.None;

            _rolesRepositoryMock.Setup(r => r.AddAsync(newRole, cancellationToken)).ReturnsAsync(newRole);

            // Act
            var createdRole = await _rolesService.CreateRolesAsync(newRole, cancellationToken);

            // Assert
            Assert.IsNotNull(createdRole);
            Assert.AreEqual(newRole.Id, createdRole.Id);
            Assert.AreEqual(newRole.Name, createdRole.Name);
        }

        [TestMethod]
        [Description("Teste la méthode CreateRolesAsync avec un rôle null.")]
        public async Task CreateRolesAsync_NullRole_ThrowsArgumentNullException()
        {
            // Arrange
            Roles nullRole = null; // Rôle null
            CancellationToken cancellationToken = CancellationToken.None;

            // Assert & Act
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                // Act
                await _rolesService.CreateRolesAsync(nullRole, cancellationToken);
            });
        }

        [TestMethod]
        [Description("Teste la méthode UpdateRolesAsync pour vérifier la mise à jour d'un rôle.")]
        public async Task UpdateRolesAsync_UpdatesExistingRole()
        {
            // Arrange
            int roleId = 1;
            var existingRole = new Roles { Id = roleId, Name = "Admin" }; // Rôle existant
            var updatedRole = new Roles { Id = roleId, Name = "Manager" }; // Rôle mis à jour

            CancellationToken cancellationToken = CancellationToken.None;

            _rolesRepositoryMock.Setup(r => r.GetByIdAsync(roleId, cancellationToken)).ReturnsAsync(existingRole);
            _rolesRepositoryMock.Setup(r => r.UpdateAsync(existingRole, cancellationToken)).ReturnsAsync(true);

            // Act
            var result = await _rolesService.UpdateRolesAsync(roleId, updatedRole, cancellationToken);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(updatedRole.Name, existingRole.Name);
        }

        [TestMethod]
        [Description("Teste la méthode UpdateRolesAsync avec un ID invalide.")]
        public async Task UpdateRolesAsync_InvalidId_ThrowsArgumentNullException()
        {
            // Arrange
            int invalidId = 0;
            var role = new Roles { Id = 1, Name = "Admin" };

            CancellationToken cancellationToken = CancellationToken.None;

            // Assert & Act
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                // Act
                await _rolesService.UpdateRolesAsync(invalidId, role, cancellationToken);
            });
        }

        [TestMethod]
        [Description("Teste la méthode UpdateRolesAsync avec un rôle null.")]
        public async Task UpdateRolesAsync_NullRole_ThrowsArgumentNullException()
        {
            // Arrange
            int roleId = 1;
            Roles nullRole = null;

            CancellationToken cancellationToken = CancellationToken.None;

            // Assert & Act
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                // Act
                await _rolesService.UpdateRolesAsync(roleId, nullRole, cancellationToken);
            });
        }

        [TestMethod]
        [Description("Teste la méthode DeleteRolesAsync pour vérifier la suppression d'un rôle.")]
        public async Task DeleteRolesAsync_DeletesExistingRole()
        {
            // Arrange
            int roleId = 1;
            var existingRole = new Roles { Id = roleId, Name = "Admin" }; // Rôle existant

            CancellationToken cancellationToken = CancellationToken.None;

            _rolesRepositoryMock.Setup(r => r.DeleteAsync(roleId, cancellationToken)).ReturnsAsync(true);

            // Act
            var result = await _rolesService.DeleteRolesAsync(roleId, cancellationToken);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [Description("Teste la méthode DeleteRolesAsync avec un ID invalide.")]
        public async Task DeleteRolesAsync_InvalidId_ThrowsArgumentNullException()
        {
            // Arrange
            int invalidId = 0;

            CancellationToken cancellationToken = CancellationToken.None;

            // Assert & Act
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                // Act
                await _rolesService.DeleteRolesAsync(invalidId, cancellationToken);
            });
        }
    }
}
