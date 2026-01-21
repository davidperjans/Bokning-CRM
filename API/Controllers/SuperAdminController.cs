using Application.Businesses.Queries.GetAllBusinesses;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/superadmin/getallbusinesses")]
    [Authorize(Roles = nameof(UserRole.SuperAdmin))] // Endast SuperAdmin har tillträde
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
    }
}