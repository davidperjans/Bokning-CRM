using Application.Common;
using Application.Interface;
using Application.Services.Commands.UpdateService;
using Application.Services.DTOs;
using Domain.Models;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;
using Xunit;

namespace Tests.Unit.Services
{
    public class UpdateServiceCommandHandlerTests
    {
        private readonly Mock<IServiceRepository> _serviceRepoMock = new();
        private readonly Mock<IBusinessRepository> _businessRepoMock = new();
        private readonly Mock<IUserContext> _userContextMock = new();
        private readonly Mock<IValidator<UpdateServiceCommand>> _validatorMock = new();
        private readonly UpdateServiceCommandHandler _handler;

        public UpdateServiceCommandHandlerTests()
        {
            _handler = new UpdateServiceCommandHandler(
                _serviceRepoMock.Object,
                _businessRepoMock.Object,
                _userContextMock.Object,
                _validatorMock.Object);

            // Default: Validering lyckas
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<UpdateServiceCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenUserIsOwner()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var businessId = Guid.NewGuid();
            var serviceId = Guid.NewGuid();
            var command = new UpdateServiceCommand(serviceId, "Nytt Namn", "Besk", 45, 500, true);

            var existingService = new Service { Id = serviceId, BusinessId = businessId };
            var business = new Business { Id = businessId, OwnerId = userId };

            _serviceRepoMock.Setup(r => r.GetServiceByIdAsync(serviceId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingService);
            _businessRepoMock.Setup(r => r.GetByIdAsync(businessId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(business);
            _userContextMock.Setup(u => u.UserId).Returns(userId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            // FIX 1: Använd 'result.Value' istället för 'OperationResult.Value'
            Assert.Equal("Nytt Namn", result.Value.Name); 
    
            // FIX 2: Metoden heter UpdateAsync i ditt IServiceRepository
            _serviceRepoMock.Verify(r => r.UpdateServiceAsync(It.IsAny<Service>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenAccessIsDenied()
        {
            // Arrange
            var strangerId = Guid.NewGuid();
            var serviceId = Guid.NewGuid();
            var command = new UpdateServiceCommand(serviceId, "Hack", "Desc", 30, 0, true);

            _serviceRepoMock.Setup(r => r.GetServiceByIdAsync(serviceId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Service { Id = serviceId, BusinessId = Guid.NewGuid() });
            _businessRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Business { OwnerId = Guid.NewGuid() }); // Annan ägare
            _userContextMock.Setup(u => u.UserId).Returns(strangerId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Access denied.", result.ErrorMessage);
        }
    }
}