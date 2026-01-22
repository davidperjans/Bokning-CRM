using Application.Businesses.Commands;
using Application.Interface;
using Domain.Enum;
using Domain.Models;
using Moq;

namespace Tests.Unit.Businesses
{
    public class UpdateBusinessStatusHandlerTests
    {
        private readonly Mock<IBusinessRepository> _repositoryMock;
        private readonly UpdateBusinessStatusHandler _handler;

        public UpdateBusinessStatusHandlerTests()
        {
            _repositoryMock = new Mock<IBusinessRepository>();
            _handler = new UpdateBusinessStatusHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdateStatus_WhenBusinessExists()
        {
            // Arrange
            var businessId = Guid.NewGuid();
            var business = new Business 
            { 
                Id = businessId, 
                Name = "Test Business", 
                Status = BusinessStatus.Pending 
            };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(businessId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(business);

            var command = new UpdateBusinessStatusCommand(businessId, BusinessStatus.Active);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(BusinessStatus.Active, business.Status);
            _repositoryMock.Verify(r => r.UpdateAsync(business, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenBusinessDoesNotExist()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Business)null!);

            var command = new UpdateBusinessStatusCommand(Guid.NewGuid(), BusinessStatus.Active);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Business not found.", result.ErrorMessage);
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Business>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}