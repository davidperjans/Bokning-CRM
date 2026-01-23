using Application.Common;
using Application.Interface;
using Application.Services.Commands.CreateService;
using Domain.Models;
using Moq;
using Xunit;

namespace Tests.Unit.Services
{
    public class CreateServiceHandlerTests
    {
        private readonly Mock<IBusinessRepository> _businessRepoMock;
        private readonly Mock<IUserContext> _userContextMock;
        private readonly CreateServiceHandler _handler;

        public CreateServiceHandlerTests()
        {
            _businessRepoMock = new Mock<IBusinessRepository>();
            _userContextMock = new Mock<IUserContext>();
            _handler = new CreateServiceHandler(_businessRepoMock.Object, _userContextMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenUserIsOwner()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var businessId = Guid.NewGuid();
            var command = new CreateServiceCommand(businessId, "Massage", "Deep tissue", 60, 800, true);

            _userContextMock.Setup(x => x.UserId).Returns(userId);
            _userContextMock.Setup(x => x.Role).Returns("BusinessOwner");

            _businessRepoMock.Setup(x => x.GetByIdAsync(businessId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Business { Id = businessId, OwnerId = userId });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _businessRepoMock.Verify(x => x.AddServiceAsync(It.IsAny<Service>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenUserIsSuperAdminButNotOwner()
        {
            // Arrange
            var adminId = Guid.NewGuid();
            var ownerId = Guid.NewGuid();
            var businessId = Guid.NewGuid();
            var command = new CreateServiceCommand(businessId, "Admin Service", "Desc", 30, 100, true);

            _userContextMock.Setup(x => x.UserId).Returns(adminId);
            _userContextMock.Setup(x => x.Role).Returns("SuperAdmin"); // SuperAdmin-rollen testas här

            _businessRepoMock.Setup(x => x.GetByIdAsync(businessId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Business { Id = businessId, OwnerId = ownerId });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _businessRepoMock.Verify(x => x.AddServiceAsync(It.IsAny<Service>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserIsNotOwnerAndNotAdmin()
        {
            // Arrange
            var strangerId = Guid.NewGuid();
            var ownerId = Guid.NewGuid();
            var businessId = Guid.NewGuid();
            var command = new CreateServiceCommand(businessId, "Unauthorized", "Desc", 30, 100, true);

            _userContextMock.Setup(x => x.UserId).Returns(strangerId);
            _userContextMock.Setup(x => x.Role).Returns("User");

            _businessRepoMock.Setup(x => x.GetByIdAsync(businessId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Business { Id = businessId, OwnerId = ownerId });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Access denied.", result.ErrorMessage);
            _businessRepoMock.Verify(x => x.AddServiceAsync(It.IsAny<Service>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}