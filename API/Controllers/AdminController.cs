using Application.Admin.Commands.CreateStaff;
using Application.Admin.Commands.UpdateSchedule;
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
        
        [HttpPut("staff/{id}/schedule")]
        public async Task<IActionResult> UpdateStaffSchedule(Guid id, [FromBody] UpdateStaffScheduleCommand command, CancellationToken ct)
        {
            // Säkerställ att ID i URL matchar ID i bodyn
            if (id != command.StaffId) 
                return BadRequest("ID mismatch: URL ID matchar inte bodyns StaffId.");

            var result = await _mediator.Send(command, ct);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result);
        }
    }
}
