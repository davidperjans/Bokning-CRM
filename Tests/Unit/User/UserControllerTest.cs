using API.Controllers;
using Application.Common;
using Application.Users.Commands.CreateUser;
using Application.Users.Dtos;
using Application.Users.Queries.GetCurrentUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Domain.Enum; 
using Xunit;

namespace Tests.Unit.User
{
    public class UserControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new UserController(_mediatorMock.Object);
        }

        [Fact]
        public async Task Register_ShouldReturnCreated_WhenCommandSucceeds()
        {
            // Arrange
            // Skicka in alla parametrar direkt i konstruktorn istället för att använda {}
            var command = new CreateUserCommand(
                "test@test.se", 
                "Password123", 
                "Mikael", 
                "Persson", 
                "0701234567", 
                DateTime.Now.AddYears(-25), 
                UserRole.User
            );

            // Gör samma sak för UserDto om den också har en Primary Constructor
            var userDto = new UserDto(
                Guid.NewGuid(), 
                "test@test.se", 
                "Mikael", 
                "Persson", 
                "0701234567", 
                DateTime.Now.AddYears(-25), 
                "User", 
                DateTime.Now
            );
    
            _mediatorMock.Setup(m => m.Send(command, default))
                .ReturnsAsync(OperationResult<UserDto>.Success(userDto));

            // Act
            var result = await _controller.Register(command);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(201, createdAtActionResult.StatusCode);
        }

        [Fact]
        public async Task GetCurrentProfile_ShouldReturnUnauthorized_WhenUserNotLoggedIn()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetCurrentUserQuery>(), default))
                .ReturnsAsync(OperationResult<UserDto>.Failure("Användaren är inte inloggad."));

            // Act
            var result = await _controller.GetCurrentProfile();

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }
    }
}