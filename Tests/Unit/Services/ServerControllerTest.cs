using API.Controllers;
using Application.Common;
using Application.Services.Commands.UpdateService;
using Application.Services.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.Unit.Controllers
{
    public class ServicesControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock = new();
        private readonly ServicesController _controller;

        public ServicesControllerTests()
        {
            _controller = new ServicesController(_mediatorMock.Object);
        }

        [Fact]
        public async Task UpdateService_ShouldReturnOk_WhenSuccess()
        {
            // Arrange
            var serviceId = Guid.NewGuid();
            var command = new UpdateServiceCommand(serviceId, "Service", "Desc", 30, 100, true);
            var dto = new UpdateServiceDto(serviceId, "Service", "Desc", 30, 100, true, Guid.NewGuid());
            
            _mediatorMock.Setup(m => m.Send(command, default))
                .ReturnsAsync(OperationResult<UpdateServiceDto>.Success(dto));

            // Act
            var result = await _controller.UpdateService(serviceId, command);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dto, okResult.Value);
        }

        [Fact]
        public async Task UpdateService_ShouldReturnBadRequest_WhenIdMismatch()
        {
            // Arrange
            var urlId = Guid.NewGuid();
            var bodyId = Guid.NewGuid(); // Olika ID:n
            var command = new UpdateServiceCommand(bodyId, "Service", "Desc", 30, 100, true);

            // Act
            var result = await _controller.UpdateService(urlId, command);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("ID mismatch", badRequest.Value);
        }
    }
}