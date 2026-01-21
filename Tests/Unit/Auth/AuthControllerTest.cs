using API.Controllers;
using Application.Common;
using Application.Interface;
using Application.Users.Commands.ForgotPassword;
using Application.Users.Commands.LoginUser;
using Application.Users.Commands.RefreshToken;
using Application.Users.Commands.ResetPassword;
using Application.Users.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.UnitTests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new AuthController(_mediatorMock.Object);
        }

        #region Login Tests
        [Fact]
        public async Task Login_ShouldReturnOk_WhenCredentialsAreValid()
        {
            // Arrange
            var command = new LoginCommand("test@test.se", "Password123");
            var loginResponse = new LoginResponseDto(
                "fake-jwt-token",
                "fake-refresh-token",
                DateTime.Now.AddDays(7),
                new UserDto(Guid.NewGuid(), "test@test.se", "Mikael", "Daskalou", "070", DateTime.Now, "Admin", DateTime.Now)
            );

            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(OperationResult<LoginResponseDto>.Success(loginResponse));

            // Act
            var result = await _controller.Login(command);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(loginResponse, okResult.Value);
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenCredentialsAreInvalid()
        {
            // Arrange
            var command = new LoginCommand("wrong@test.se", "WrongPass");
            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(OperationResult<LoginResponseDto>.Failure("Invalid email or password."));

            // Act
            var result = await _controller.Login(command);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
        }
        #endregion

        #region Refresh Token Tests
        [Fact]
        public async Task RefreshToken_ShouldReturnOk_WhenTokenIsValid()
        {
            // Arrange
            var request = new RefreshTokenRequest("valid-refresh-token");
            var loginResponse = new LoginResponseDto("new-jwt", "new-refresh", DateTime.Now, null!);

            _mediatorMock.Setup(m => m.Send(It.IsAny<RefreshTokenCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(OperationResult<LoginResponseDto>.Success(loginResponse));

            // Act
            var result = await _controller.RefreshToken(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }
        #endregion

        #region Forgot/Reset Password Tests
        [Fact]
        public async Task ForgotPassword_ShouldAlwaysReturnOk_ForPrivacy()
        {
            // Arrange
            var request = new ForgotPasswordRequest("any@email.com");
            _mediatorMock.Setup(m => m.Send(It.IsAny<ForgotPasswordCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(OperationResult<bool>.Success(true));

            // Act
            var result = await _controller.ForgotPassword(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task ResetPassword_ShouldReturnOk_WhenTokenIsCorrect()
        {
            // Arrange
            var request = new ResetPasswordRequest("valid-token", "NewPassword123!");
            _mediatorMock.Setup(m => m.Send(It.IsAny<ResetPasswordCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(OperationResult<bool>.Success(true));

            // Act
            var result = await _controller.ResetPassword(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task ResetPassword_ShouldReturnBadRequest_WhenTokenIsExpired()
        {
            // Arrange
            var request = new ResetPasswordRequest("expired-token", "NewPassword123!");
            _mediatorMock.Setup(m => m.Send(It.IsAny<ResetPasswordCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(OperationResult<bool>.Failure("Invalid or expired reset token."));

            // Act
            var result = await _controller.ResetPassword(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }
        #endregion
    }
}