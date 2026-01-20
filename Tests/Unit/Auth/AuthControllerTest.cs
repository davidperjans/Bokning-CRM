using API.Controllers;
using Application.Common;
using Application.Users.Commands.CreateUser;
using Application.Users.Dtos;
using Application.Users.Queries.GetCurrentUser;
using Domain.Enum; // Krävs för UserRole
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.UnitTests.Controllers
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
            // Arrange - Skicka in argument i parentesen för Primary Constructor
            var command = new CreateUserCommand(
                "test@test.se", 
                "Password123", 
                "Mikael", 
                "Persson", 
                "0701234567", 
                DateTime.Now.AddYears(-25), 
                UserRole.User
            );

            var userDto = new UserDto(
                Guid.NewGuid(), 
                "test@test.se", 
                "Mikael", 
                "Persson", 
                "0701234567", 
                DateTime.Now.AddYears(-25), 
                UserRole.User.ToString(), // Om DTO:n förväntar sig sträng här
                DateTime.Now
            );
            
            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
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
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetCurrentUserQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(OperationResult<UserDto>.Failure("Användaren är inte inloggad.")); // Detta triggar 401

            // Act
            var result = await _controller.GetCurrentProfile();

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }
    }
}