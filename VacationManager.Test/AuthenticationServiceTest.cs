using Microsoft.Extensions.Options;
using Moq;
using VacationManager.Domain.Configurations.Jwt;
using VacationManager.Domain.Interfaces.InterfaceService;
using VacationManager.Domain.Models;
using VacationManager.Domain.Models.Authentification;
using VacationManager.Domain.Services.Authentification;

namespace VacationManager.Test
{
    [TestClass]
    public class AuthenticationServiceTest
    {
        private Mock<IUsersService> mockUsersService;
        private Mock<IOptions<JwtSettings>> mockJwtSettings;
        private AuthenticationService service;

        [TestInitialize]
        public void TestInitialize()
        {
            mockUsersService = new Mock<IUsersService>();
            mockJwtSettings = new Mock<IOptions<JwtSettings>>();
            var mockSettings = new JwtSettings { Secret = "secretKey", Issuer = "YourIssuer" };
            mockJwtSettings.Setup(s => s.Value).Returns(mockSettings);
            service = new AuthenticationService(mockUsersService.Object, mockJwtSettings.Object);
        }

        [TestMethod]
        public async Task AuthenticateAsync_ValidUserAndPassword_ReturnsAuthenticationResponse()
        {
            // Arrange
            var mockUser = new Users { Email = "test@email.com", Password = "hashedPassword" };
            mockUsersService.Setup(s => s.GetUserByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockUser);

            // Act
            var request = new AuthenticationRequest { Email = "test@email.com", Password = "hashedPassword" };
            var response = await service.AuthenticateAsync(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Token);
            Assert.AreEqual(mockUser.Roles.Name, response.Role);
        }

        [TestMethod]
        public async Task AuthenticateAsync_InvalidUser_ReturnsNull()
        {
            // Arrange
            mockUsersService.Setup(s => s.GetUserByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns((string email, CancellationToken token) => Task.FromResult<Users>(null));

            // Act
            var request = new AuthenticationRequest { Email = "test@email.com", Password = "hashedPassword" };
            var response = await service.AuthenticateAsync(request, CancellationToken.None);

            // Assert
            Assert.IsNull(response);
        }

        [TestMethod]
        public async Task AuthenticateAsync_InvalidPassword_ReturnsNull()
        {
            // Arrange
            var mockUser = new Users { Email = "test@email.com", Password = "hashedPassword" };
            mockUsersService.Setup(s => s.GetUserByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockUser);

            // Act
            var request = new AuthenticationRequest { Email = "test@email.com", Password = "wrongPassword" };
            var response = await service.AuthenticateAsync(request, CancellationToken.None);

            // Assert
            Assert.IsNull(response);
        }
    }
}


