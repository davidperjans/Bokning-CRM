using Application.Admin.Commands.CreateStaff;
using Application.Interface;
using Domain.Models;
using FluentAssertions;
using Moq;

namespace Tests.Unit.Admin
{
    public class CreateStaffCommandHandlerTests
    {
        private readonly Mock<IStaffRepository> _staffRepo = new();
        private readonly Mock<IServiceRepository> _serviceRepo = new();

        private CreateStaffCommandHandler CreateSut()
            => new(_staffRepo.Object, _serviceRepo.Object);

        [Fact]
        public async Task Handle_ShouldCreateStaff_WhenValidAndAllServicesExist()
        {
            // Arrange
            var businessId = Guid.NewGuid();
            var s1 = Guid.NewGuid();
            var s2 = Guid.NewGuid();

            var cmd = new CreateStaffCommand(
                BusinessId: businessId,
                Name: "  Anna Andersson  ",
                Title: " Frisör ",
                ImageUrl: " https://img ",
                Bio: " bio ",
                IsActive: true,
                QualifiedServiceIds: new[] { s1, s2 }
            );

            _serviceRepo
                .Setup(x => x.GetByIdsAsync(It.IsAny<IReadOnlyCollection<Guid>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Service>
                {
                new() { Id = s1 },
                new() { Id = s2 }
                });

            var sut = CreateSut();

            // Act
            var result = await sut.Handle(cmd, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value!.BusinessId.Should().Be(businessId);
            result.Value.Name.Should().Be("Anna Andersson");
            result.Value.Title.Should().Be("Frisör");
            result.Value.QualifiedServiceIds.Should().BeEquivalentTo(new[] { s1, s2 });

            _staffRepo.Verify(x => x.AddAsync(It.Is<Staff>(s =>
                s.BusinessId == businessId &&
                s.Name == "Anna Andersson" &&
                s.Title == "Frisör" &&
                s.IsActive == true &&
                s.QualifiedServices.Count == 2
            ), It.IsAny<CancellationToken>()), Times.Once);

        }

        [Fact]
        public async Task Handle_ShouldCreateStaff_WithNoServices_WhenEmptyServiceList()
        {
            // Arrange
            var businessId = Guid.NewGuid();

            var cmd = new CreateStaffCommand(
                BusinessId: businessId,
                Name: "Anna",
                Title: "Frisör",
                ImageUrl: null,
                Bio: null,
                IsActive: true,
                QualifiedServiceIds: Array.Empty<Guid>()
            );

            // Viktigt: handlern ska inte ens behöva hämta services om listan är tom
            var sut = CreateSut();

            // Act
            var result = await sut.Handle(cmd, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value!.QualifiedServiceIds.Should().BeEmpty();

            _serviceRepo.Verify(x => x.GetByIdsAsync(It.IsAny<IReadOnlyCollection<Guid>>(), It.IsAny<CancellationToken>()), Times.Never);
            _staffRepo.Verify(x => x.AddAsync(It.IsAny<Staff>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldFail_WhenSomeServicesAreMissing()
        {
            // Arrange
            var businessId = Guid.NewGuid();
            var requested1 = Guid.NewGuid();
            var requested2 = Guid.NewGuid(); // missing
            var requested3 = Guid.NewGuid(); // missing

            var cmd = new CreateStaffCommand(
                BusinessId: businessId,
                Name: "Anna",
                Title: "Frisör",
                ImageUrl: null,
                Bio: null,
                IsActive: true,
                QualifiedServiceIds: new[] { requested1, requested2, requested3 }
            );

            _serviceRepo
                .Setup(x => x.GetByIdsAsync(It.IsAny<IReadOnlyCollection<Guid>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Service> { new() { Id = requested1 } });

            var sut = CreateSut();

            // Act
            var result = await sut.Handle(cmd, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();

            _staffRepo.Verify(x => x.AddAsync(It.IsAny<Staff>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
