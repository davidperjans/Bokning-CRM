using Application.Services.Commands.CreateService;
using Application.Services.Commands.UpdateService;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Kräver att man är inloggad för att IUserContext ska fungera
public class ServicesController : ControllerBase
{
    private readonly IMediator _mediator;
    public ServicesController(IMediator mediator) => _mediator = mediator;

    [HttpPost("createservice")]
    public async Task<IActionResult> CreateService([FromBody] CreateServiceCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateService(Guid id, [FromBody] UpdateServiceCommand command)
    {
        // Säkerställ att ID i URL matchar ID i bodyn
        if (id != command.Id) return BadRequest("ID mismatch");

        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.ErrorMessage);
        
    }
}