using Moq;
using VacationManager.Domain.Interfaces.InterfaceService;
using VacationManager.Domain.Interfaces.InterfacesRepository;
using VacationManager.Domain.Models;
using VacationManager.Domain.Services;

namespace VacationManager.Test
{
    [TestClass]
    public class UsersServiceTest
    {
        private Mock<IUsersRepository> _usersRepositoryMock;
        private IUsersService _usersService;

        [TestInitialize]
        public void Initialize()
        {
            _usersRepositoryMock = new Mock<IUsersRepository>();
            _usersService = new UsersService(_usersRepositoryMock.Object);
        }

        [TestMethod]
        [Description("Teste la méthode GetAllUsersAsync pour vérifier le retour de tous les utilisateurs.")]
        public async Task GetAllUsersAsync_ReturnsAllUsers()
        {
            // Arrange
            var expectedUsers = new List<Users>
            {
                new Users { Id = 1, FirstName = "User 1" },
                new Users { Id = 2, FirstName = "User 2" },
            };

            CancellationToken cancellationToken = CancellationToken.None;

            _usersRepositoryMock.Setup(r => r.GetAllAsync(cancellationToken)).ReturnsAsync(expectedUsers);

            // Act
            var result = await _usersService.GetAllUsersAsync(cancellationToken);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedUsers.Count, result.Count());
            CollectionAssert.AreEqual(expectedUsers, new List<Users>(result));
        }

        [TestMethod]
        [Description("Teste la méthode CreateUsersAsync pour vérifier la création d'un nouvel utilisateur.")]
        public async Task CreateUsersAsync_CreatesNewUser()
        {
            // Arrange
            var newUser = new Users { Id = 1, FirstName = "New User" };

            CancellationToken cancellationToken = CancellationToken.None;

            _usersRepositoryMock.Setup(r => r.AddAsync(newUser, cancellationToken)).ReturnsAsync(newUser);

            // Act
            var result = await _usersService.CreateUsersAsync(newUser, cancellationToken);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(newUser.Id, result.Id);
            Assert.AreEqual(newUser.FirstName, result.FirstName);
        }

        [TestMethod]
        [Description("Teste la méthode CreateUsersAsync pour vérifier la gestion d'une entrée null.")]
        public async Task CreateUsersAsync_ThrowsExceptionForNullUser()
        {
            // Arrange
            Users nullUser = null;

            CancellationToken cancellationToken = CancellationToken.None;

            // Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                var result = await _usersService.CreateUsersAsync(nullUser, cancellationToken);
            });
        }

        [TestMethod]
        [Description("Teste la méthode UpdateUsersAsync pour vérifier la mise à jour d'un utilisateur.")]
        public async Task UpdateUsersAsync_UpdatesExistingUser()
        {
            // Arrange
            int userId = 1;
            var existingUser = new Users { Id = userId, FirstName = "Existing", LastName = "User" };
            var updatedUser = new Users { Id = userId, FirstName = "Updated", LastName = "User" };

            CancellationToken cancellationToken = CancellationToken.None;

            _usersRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(existingUser);

            _usersRepositoryMock.Setup(r => r.UpdateAsync(existingUser, cancellationToken)).ReturnsAsync(true);

            // Act
            var result = await _usersService.UpdateUsersAsync(userId, updatedUser, cancellationToken);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(updatedUser.FirstName, existingUser.FirstName);
            Assert.AreEqual(updatedUser.LastName, existingUser.LastName);
        }

        [TestMethod]
        [Description("Teste la méthode UpdateUsersAsync pour vérifier la gestion d'un identifiant utilisateur invalide.")]
        public async Task UpdateUsersAsync_ThrowsExceptionForInvalidUserId()
        {
            // Arrange
            int invalidUserId = 0;
            var updatedUser = new Users { Id = invalidUserId, FirstName = "Updated", LastName = "User" };

            CancellationToken cancellationToken = CancellationToken.None;

            // Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                var result = await _usersService.UpdateUsersAsync(invalidUserId, updatedUser, cancellationToken);
            });
        }

        [TestMethod]
        [Description("Teste la méthode UpdateUsersAsync pour vérifier la gestion d'un utilisateur null.")]
        public async Task UpdateUsersAsync_ThrowsExceptionForNullUser()
        {
            // Arrange
            int userId = 1;
            Users nullUser = null;

            CancellationToken cancellationToken = CancellationToken.None;

            // Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                var result = await _usersService.UpdateUsersAsync(userId, nullUser, cancellationToken);
            });
        }

        [TestMethod]
        [Description("Teste la méthode DeleteUsersAsync pour vérifier la suppression d'un utilisateur existant.")]
        public async Task DeleteUsersAsync_DeletesExistingUser()
        {
            // Arrange
            int userId = 1;

            CancellationToken cancellationToken = CancellationToken.None;

            _usersRepositoryMock.Setup(r => r.DeleteAsync(userId, cancellationToken)).ReturnsAsync(true);

            // Act
            var result = await _usersService.DeleteUsersAsync(userId, cancellationToken);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [Description("Teste la méthode DeleteUsersAsync pour vérifier la gestion d'un identifiant utilisateur invalide.")]
        public async Task DeleteUsersAsync_ThrowsExceptionForInvalidUserId()
        {
            // Arrange
            int invalidUserId = 0;

            CancellationToken cancellationToken = CancellationToken.None;

            // Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                var result = await _usersService.DeleteUsersAsync(invalidUserId, cancellationToken);
            });
        }
    }
}
