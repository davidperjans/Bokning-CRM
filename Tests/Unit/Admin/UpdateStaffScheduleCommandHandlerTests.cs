using Application.Admin.Commands.UpdateSchedule;
using Application.Admin.DTOs;
using Application.Common;
using Application.Interface;
using Domain.Models;
using Moq;
using Xunit;

namespace Tests.Unit.Admin
{
    public class UpdateStaffScheduleCommandHandlerTests
    {
        private readonly Mock<IStaffRepository> _staffRepoMock = new();
        private readonly Mock<IBusinessRepository> _businessRepoMock = new();
        private readonly Mock<IUserContext> _userContextMock = new();
        private readonly UpdateStaffScheduleHandler _handler;

        public UpdateStaffScheduleCommandHandlerTests()
        {
            _handler = new UpdateStaffScheduleHandler(
                _staffRepoMock.Object, 
                _businessRepoMock.Object, 
                _userContextMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenUserIsOwner()
        {
            // Arrange
            var staffId = Guid.NewGuid();
            var businessId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            
            var inputDays = new List<WorkingHourDto>
            {
                new(DayOfWeek.Monday, "08:00", "17:00", false)
            };

            var command = new UpdateStaffScheduleCommand(staffId, inputDays);

            // Setup: Return a staff member belonging to the business
            _staffRepoMock.Setup(r => r.GetByIdAsync(staffId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Staff { Id = staffId, BusinessId = businessId });
            
            // Setup: Business is owned by the current user
            _businessRepoMock.Setup(r => r.GetByIdAsync(businessId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Business { Id = businessId, OwnerId = userId });

            _userContextMock.Setup(u => u.UserId).Returns(userId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            // Verify that the returned value matches our input list
            Assert.Equal(inputDays, result.Value);
            
            // Verify repository call with specific parameters
            _staffRepoMock.Verify(r => r.UpdateScheduleAsync(
                staffId, 
                It.IsAny<List<WorkingHour>>(), 
                It.IsAny<CancellationToken>()), 
                Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenAccessIsDenied()
        {
            // Arrange
            var staffId = Guid.NewGuid();
            var strangerId = Guid.NewGuid();
            var command = new UpdateStaffScheduleCommand(staffId, new List<WorkingHourDto>());

            // Staff belongs to a random business
            _staffRepoMock.Setup(r => r.GetByIdAsync(staffId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Staff { Id = staffId, BusinessId = Guid.NewGuid() });
    
            // Business is owned by someone else
            _businessRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Business { OwnerId = Guid.NewGuid() });

            // Current user is the "stranger"
            _userContextMock.Setup(u => u.UserId).Returns(strangerId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Access denied.", result.ErrorMessage);
            
            // Ensure the repository was NEVER called because access was denied
            _staffRepoMock.Verify(r => r.UpdateScheduleAsync(
                It.IsAny<Guid>(), 
                It.IsAny<List<WorkingHour>>(), 
                It.IsAny<CancellationToken>()), 
                Times.Never);
        }
    }
}