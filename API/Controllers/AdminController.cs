using Application.Admin.Commands.CreateStaff;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace API.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Policy = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly ISender _mediator;
        public AdminController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("staff")]
        public async Task<IActionResult> CreateStaffMember([FromBody] CreateStaffCommand command, CancellationToken ct)
        {
            var result = await _mediator.Send(command, ct);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
