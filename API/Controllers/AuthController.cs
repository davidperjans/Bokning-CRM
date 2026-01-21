using Application.Interface;
using Application.Users.Commands.ForgotPassword;
using Application.Users.Commands.LoginUser;
using Application.Users.Commands.RefreshToken;
using Application.Users.Commands.ResetPassword;
using Application.Users.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return Unauthorized(new { message = result.ErrorMessage });
            }

            return Ok(result.Value);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            // Vi skickar in den gamla Refresh Token till vår nya Handler
            var result = await _mediator.Send(new RefreshTokenCommand(request.RefreshToken));

            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.Value);
        }
        
        [HttpPost("forgot-password")]
        [AllowAnonymous] // Alla ska kunna nå denna även utan att vara inloggade
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var result = await _mediator.Send(new ForgotPasswordCommand(request.Email));

            // Vi returnerar alltid 200 OK med ett generellt meddelande
            return Ok(new { message = "If an account with that email exists, we have sent a reset link." });
        }
        
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var result = await _mediator.Send(new ResetPasswordCommand(request.Token, request.NewPassword));

            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(new { message = "Password has been successfully reset." });
        }

    }
}