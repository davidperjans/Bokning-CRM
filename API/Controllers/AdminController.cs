using Application.Admin.Commands.CreateStaff;
using Application.Admin.Commands.UpdateBookingStatus;
using Application.Admin.Commands.UpdateSchedule;
using Application.Admin.Queries;
using Application.Bookings.Queries.GetMyBookingsBusiness;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace API.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Policy = "AdminOnly")]
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

        // GET /api/admin/bookings
        [HttpGet("/api/admin/bookings")]
        public async Task<IActionResult> GetAllBookingsForBusiness(CancellationToken ct)
        {
            var result = await _mediator.Send(new GetAllBookingsForBusinessQuery(), ct);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        // PUT /api/admin/bookings/{id}/status
        [HttpPut("bookings/{id:guid}/status")]
        public async Task<IActionResult> UpdateBookingStatus(
            [FromRoute] Guid id,
            [FromBody] UpdateBookingStatusRequest body,
            CancellationToken ct)
        {
            var command = new UpdateBookingStatusCommand(id, body.NewStatus);

            var result = await _mediator.Send(command, ct);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("dashboard/stats")]
        public async Task<IActionResult> GetDashboardStats(CancellationToken ct)
        {
            var result = await _mediator.Send(new GetAdminDashboardStatsQuery(), ct);

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

        [HttpGet("debug/claims")]
        public IActionResult DebugClaims()
        {
            return Ok(User.Claims.Select(c => new { c.Type, c.Value }));
        }
    }

    public sealed class UpdateBookingStatusRequest
    {
        public BookingStatus NewStatus { get; set; }
    }

}
