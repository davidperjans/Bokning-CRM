using API.Controllers;
using Application.Admin.Commands.UpdateSchedule;
using Application.Admin.DTOs;
using Application.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.Unit.Admin
{
    public class AdminControllerTests
    {
        private readonly Mock<ISender> _mediatorMock = new();
        private readonly AdminController _controller;

        public AdminControllerTests()
        {
            _controller = new AdminController(_mediatorMock.Object);
        }

        [Fact]
        public async Task UpdateStaffSchedule_ShouldReturnBadRequest_WhenIdMismatch()
        {
            // Arrange
            var urlId = Guid.NewGuid();
            var bodyId = Guid.NewGuid(); // Different IDs to trigger mismatch
            var command = new UpdateStaffScheduleCommand(bodyId, new List<WorkingHourDto>());

            // Act
            var result = await _controller.UpdateStaffSchedule(urlId, command, CancellationToken.None);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("ID mismatch: URL ID matchar inte bodyns StaffId.", badRequest.Value);
        }

        [Fact]
        public async Task UpdateStaffSchedule_ShouldReturnOk_WhenSuccess()
        {
            // Arrange
            var staffId = Guid.NewGuid();
            var expectedSchedule = new List<WorkingHourDto> 
            { 
                new(DayOfWeek.Monday, "09:00", "18:00", false) 
            };
            var command = new UpdateStaffScheduleCommand(staffId, expectedSchedule);
            
            // FIX: Setup the mock to return OperationResult<List<WorkingHourDto>>
            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(OperationResult<List<WorkingHourDto>>.Success(expectedSchedule));

            // Act
            var result = await _controller.UpdateStaffSchedule(staffId, command, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            // Verify that the data returned by the mediator is what the controller returns
            Assert.Equal(expectedSchedule, okResult.Value);
        }
    }
}