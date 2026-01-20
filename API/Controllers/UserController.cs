using Application.Users.Commands.CreateUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Common;
using Application.Users.Queries.GetCurrentUser;

namespace API.Controllers
{
    [ApiController]
    [Route("api/users")]
    
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("createuser")]
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
        
        [HttpGet("currentuser")]
        public async Task<IActionResult> GetCurrentProfile()
        {
            // Vi skickar vår query via MediatR
            var result = await _mediator.Send(new GetCurrentUserQuery());

            if (!result.IsSuccess)
            {
                // Om användaren inte hittas eller token är ogiltig
                return result.ErrorMessage == "Användaren hittades inte i databasen." 
                    ? NotFound(new { message = result.ErrorMessage }) 
                    : Unauthorized(new { message = result.ErrorMessage });
            }

            return Ok(result.Value);
        }
    }
}