using Microsoft.Extensions.Options;
using Moq;
using VacationManager.Domain.Configurations.Helper;
using VacationManager.Domain.Interfaces.InterfacesRepository;
using VacationManager.Domain.Models;
using VacationManager.Domain.Services;

namespace VacationManager.Test
{
    [TestClass]
    public class VacationsBalanceServiceTest
    {
        private Mock<IVacationsRepository> _vacationsRepositoryMock;
        private Mock<IUsersRepository> _usersRepositoryMock;
        private Mock<IVacationsBalanceRepository> _vacationsBalanceRepositoryMock;
        private VacationsBalanceService _vacationsBalanceService;
        private readonly int _initialBalance;

        [TestInitialize]
        public void Setup()
        {
            _vacationsRepositoryMock = new Mock<IVacationsRepository>();
            _usersRepositoryMock = new Mock<IUsersRepository>();
            _vacationsBalanceRepositoryMock = new Mock<IVacationsBalanceRepository>();

            var vacationOptions = new VacationOptions { InitialBalance = 23 };
            var optionsWrapper = new OptionsWrapper<VacationOptions>(vacationOptions);

            _vacationsBalanceService = new VacationsBalanceService(
                _vacationsRepositoryMock.Object,
                _usersRepositoryMock.Object,
                optionsWrapper,
                _vacationsBalanceRepositoryMock.Object,
                new List<DateTime>());
        }

        [TestMethod]
        public async Task GetAllVacationDetailsAsync_ShouldReturnAllVacationDetails()
        {
            // Arrange
            var users = new List<Users>
          {
              new Users { Id = 1, FirstName = "John", LastName = "Doe" },
              new Users { Id = 2, FirstName = "Jane", LastName = "Doe" }
          };
            var vacations = new List<Vacations>
          {
              new Vacations { UserId = 1, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(10), Type = "Type1", Justification = "Justification1" },
              new Vacations { UserId = 2, StartDate = DateTime.Now.AddDays(15), EndDate = DateTime.Now.AddDays(25), Type = "Type2", Justification = "Justification2" }
          };

            _usersRepositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(users);
            _vacationsRepositoryMock.Setup(r => r.GetVacationsWithUsersByUserIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(vacations);

            // Act
            var result = await _vacationsBalanceService.GetAllVacationDetailsAsync(CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }



        [TestMethod]
        [Description("Teste la méthode UpdateVacationBalanceAsync pour vérifier la mise à jour du solde de congés d'un utilisateur")]
        public async Task UpdateVacationBalanceAsync_ShouldUpdateVacationBalance()
        {
            // Arrange
            var userId = 1;
            var startDate = DateTime.Now;
            var endDate = DateTime.Now.AddDays(10);
            var user = new Users { Id = userId, FirstName = "John", LastName = "Doe" };
            var vacations = new List<Vacations>
        {
            new Vacations { UserId = userId, StartDate = startDate, EndDate = endDate }
        };

            _usersRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            _vacationsRepositoryMock.Setup(r => r.GetVacationsWithinRange(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Vacations>());
            _vacationsRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Vacations>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _vacationsBalanceService.UpdateVacationBalanceAsync(userId, startDate, endDate, CancellationToken.None);

            // Assert
            Assert.IsTrue(result);
        }


        [TestMethod]
        public async Task GetVacationBalanceByUserIdAsync_ShouldReturnVacationBalance()
        {
            // Arrange
            var userId = 1;
            var vacations = new List<Vacations>
           {
               new Vacations { UserId = userId, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(10), Type = "Type1", Justification = "Justification1" }
           };
            var users = new List<Users>
           {
               new Users { Id = userId, FirstName = "John", LastName = "Doe" }
           };

            _vacationsRepositoryMock.Setup(r => r.GetVacationsWithUsersByUserIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(vacations);

            // Act
            var result = await _vacationsBalanceService.GetVacationBalanceByUserIdAsync(userId, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userId, result.UserId);
            Assert.AreEqual(23, result.InitialVacationBalance);
            Assert.AreEqual(0, result.UsedVacationBalance);
            Assert.AreEqual(23, result.RemainingVacationBalance);
        }

        [TestMethod]
        [Description("Teste la méthode ResetInitialBalanceAsync pour vérifier la réinitialisation du solde initial de congés")]
        public async Task ResetInitialBalanceAsync_ShouldResetVacationBalance()
        {
            // Arrange
            var userId = 1;
            var startDate = DateTime.Now;
            var endDate = DateTime.Now.AddDays(10);
            var vacationBalance = new VacationsBalance { UserId = userId, InitialVacationBalance = 23, UsedVacationBalance = 0, RemainingVacationBalance = 23 };

            _vacationsBalanceRepositoryMock.Setup(r => r.GetByUserIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(vacationBalance);

            // Act
            await _vacationsBalanceService.ResetInitialBalanceAsync(userId, startDate, endDate, CancellationToken.None);

            // Assert
            Assert.AreEqual(0, vacationBalance.InitialVacationBalance);
        }

        [TestMethod]
        public async Task GetVacationDetailsByUserIdAsync_ShouldReturnVacationDetails()
        {
            // Arrange
            var userId = 1;
            var vacations = new List<Vacations>
          {
              new Vacations { UserId = userId, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(10), Type = "Type1", Justification = "Justification1" }
          };
            var users = new List<Users>
          {
              new Users { Id = userId, FirstName = "John", LastName = "Doe" }
          };

            _vacationsRepositoryMock.Setup(r => r.GetVacationsWithUsersByUserIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(vacations);

            // Act
            var result = await _vacationsBalanceService.GetVacationDetailsByUserIdAsync(userId, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userId, result.UserId);
            Assert.AreEqual(users[0].FirstName + " " + users[0].LastName, result.UserName);
            Assert.AreEqual(vacations[0].StartDate, result.StartDate);
            Assert.AreEqual(vacations[0].EndDate, result.EndDate);
            Assert.AreEqual(vacations[0].Type, result.Type);
            Assert.AreEqual(vacations[0].Justification, result.Justification);
            Assert.AreEqual(23, result.InitialBalance);
            Assert.AreEqual(0, result.UsedBalance);
            Assert.AreEqual(23, result.RemainingBalance);
        }

    }
}
