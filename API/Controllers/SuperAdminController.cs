using Application.Businesses.Commands;
using Application.Businesses.Queries.GetAllBusinesses;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/superadmin/getallbusinesses")]
    [Authorize(Policy = "SuperAdminOnly")] 
    public class SuperAdminController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SuperAdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBusinesses()
        {
            var result = await _mediator.Send(new GetAllBusinessesQuery());
            return Ok(result);
        }
        
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] BusinessStatus newStatus)
        {
            var result = await _mediator.Send(new UpdateBusinessStatusCommand(id, newStatus));

            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok(new { message = $"Företagets status har uppdaterats till {newStatus}." });
        }
    }
    
    
}