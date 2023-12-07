

using Moq;
using VacationManager.Domain.Interfaces.InterfaceService;
using VacationManager.Domain.Interfaces.InterfacesRepository;
using VacationManager.Domain.Models;
using VacationManager.Domain.Services;

namespace VacationManager.Test
{
    [TestClass]
    public class VacationsServiceTest
    {
        private Mock<IVacationsRepository> _vacationsRepositoryMock;
        private IVacationsService _vacationsService;

        [TestInitialize]
        public void Initialize()
        {
            _vacationsRepositoryMock = new Mock<IVacationsRepository>();
            _vacationsService = new VacationsService(_vacationsRepositoryMock.Object);
        }

        [TestMethod]
        [Description("Teste la méthode GetAllVacationsAsync pour vérifier la récupération de toutes les vacances.")]
        public async Task GetAllVacationsAsync_ReturnsAllVacations()
        {
            // Arrange
            var expectedVacations = new List<Vacations>
            {
                new Vacations { Id = 1, Type = "Paid", StartDate = new DateTime(2023, 1, 1), EndDate = new DateTime(2023, 1, 5) },
                new Vacations { Id = 2, Type = "Unpaid", StartDate = new DateTime(2023, 2, 1), EndDate = new DateTime(2023, 2, 10) }
                // Ajoutez d'autres vacances simulées selon vos besoins
            };

            CancellationToken cancellationToken = CancellationToken.None;

            _vacationsRepositoryMock.Setup(r => r.GetAllAsync(cancellationToken)).ReturnsAsync(expectedVacations);

            // Act
            var result = await _vacationsService.GetAllVacationsAsync(cancellationToken);

            // Assert
            CollectionAssert.AreEqual(expectedVacations, new List<Vacations>(result));
        }

        [TestMethod]
        [Description("Teste la méthode GetVacationsByIdAsync pour vérifier la récupération d'un congé par son ID.")]
        public async Task GetVacationsByIdAsync_ReturnsVacationById()
        {
            // Arrange
            int vacationId = 1; // ID du congé attendu
            var expectedVacation = new Vacations { Id = vacationId, Type = "Paid", StartDate = new DateTime(2023, 1, 1), EndDate = new DateTime(2023, 1, 5) };

            CancellationToken cancellationToken = CancellationToken.None;

            _vacationsRepositoryMock.Setup(r => r.GetByIdAsync(vacationId, cancellationToken)).ReturnsAsync(expectedVacation);

            // Act
            var result = await _vacationsService.GetVacationsByIdAsync(vacationId, cancellationToken);

            // Assert
            Assert.AreEqual(expectedVacation, result);
        }


        [TestMethod]
        [Description("Teste la méthode CreateVacationsAsync pour vérifier la création d'un nouveau congé.")]
        public async Task CreateVacationsAsync_ReturnsCreatedVacation()
        {
            // Arrange
            var newVacation = new Vacations { UserId = 1, Type = "Paid", StartDate = new DateTime(2023, 1, 1), EndDate = new DateTime(2023, 1, 5) };

            CancellationToken cancellationToken = CancellationToken.None;

            _vacationsRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Vacations>(), cancellationToken)).ReturnsAsync(newVacation);

            // Act
            var result = await _vacationsService.CreateVacationsAsync(newVacation, cancellationToken);

            // Assert
            Assert.AreEqual(newVacation, result);
            Assert.IsNotNull(result.CreatedDate); // Vérifie si la date de création est définie
        }


        [TestMethod]
        [Description("Teste la méthode UpdateVacationsAsync pour vérifier la mise à jour d'un congé existant.")]
        public async Task UpdateVacationsAsync_ReturnsTrueOnSuccessfulUpdate()
        {
            // Arrange
            int existingVacationId = 1;
            var existingVacation = new Vacations { Id = existingVacationId, Type = "Paid", StartDate = new DateTime(2023, 1, 1), EndDate = new DateTime(2023, 1, 5) };

            var updatedVacation = new Vacations { Id = existingVacationId, Type = "Unpaid", StartDate = new DateTime(2023, 2, 1), EndDate = new DateTime(2023, 2, 5) };

            CancellationToken cancellationToken = CancellationToken.None;

            _vacationsRepositoryMock.Setup(r => r.GetByIdAsync(existingVacationId, cancellationToken)).ReturnsAsync(existingVacation);
            _vacationsRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Vacations>(), cancellationToken)).ReturnsAsync(true);

            // Act
            var result = await _vacationsService.UpdateVacationsAsync(existingVacationId, updatedVacation, cancellationToken);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(existingVacation.Type, updatedVacation.Type);
            Assert.AreEqual(existingVacation.StartDate, updatedVacation.StartDate);
            Assert.AreEqual(existingVacation.EndDate, updatedVacation.EndDate);
            // Ajoutez d'autres assertions pour vérifier la mise à jour des autres propriétés si nécessaire
        }

        [TestMethod]
        [Description("Teste la méthode DeleteVacationsAsync pour vérifier la suppression d'un congé existant.")]
        public async Task DeleteVacationsAsync_ReturnsTrueOnSuccessfulDeletion()
        {
            // Arrange
            int existingVacationId = 1;
            CancellationToken cancellationToken = CancellationToken.None;

            _vacationsRepositoryMock.Setup(r => r.DeleteAsync(existingVacationId, cancellationToken)).ReturnsAsync(true);

            // Act
            var result = await _vacationsService.DeleteVacationsAsync(existingVacationId, cancellationToken);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [Description("Teste la méthode DeleteVacationsAsync avec un ID invalide.")]
        public async Task DeleteVacationsAsync_InvalidId_ReturnsFalse()
        {
            // Arrange
            int invalidVacationId = 1;
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await _vacationsService.DeleteVacationsAsync(invalidVacationId, cancellationToken);

            // Assert
            Assert.IsFalse(result);
        }

    }
}
