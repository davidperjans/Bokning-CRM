using Application.Users.Commands.LoginUser;
using Application.Users.Commands.RefreshToken;
using Application.Users.Dtos;
using MediatR;
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
    }
}