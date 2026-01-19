using Application.Users.Commands.CreateUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Common;

namespace API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                if (result.ErrorMessage == "E-postadressen används redan.")
                    return Conflict(new { message = result.ErrorMessage });

                if (result.ValidationErrors != null && result.ValidationErrors.Any())
                    return BadRequest(new { errors = result.ValidationErrors });

                return BadRequest(new { message = result.ErrorMessage });
            }

            // Returnera 201 Created med UserDto i bodyn
            return CreatedAtAction(nameof(Register), new { id = result.Value!.Id }, result.Value);
        }
    }
}