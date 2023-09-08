using Application.Dtos;
using Application.Features.LeaveAllocations.Commands;
using Application.Features.LeaveAllocations.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LeaveAllocationsController : ControllerBase
{
  private readonly IMediator _mediator;

  public LeaveAllocationsController(IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpGet]
  public async Task<ActionResult<List<LeaveAllocationDto>>> Get(bool isLoggedInUser = false)
  {
    var leaveAllocations = await _mediator.Send(new GetLeaveAllocationsQuery());
    return Ok(leaveAllocations);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<LeaveAllocationDto>> Get(int id)
  {
    var leaveAllocation = await _mediator.Send(new GetLeaveAllocationsDetailsQuery(id));
    return Ok(leaveAllocation);
  }

  [HttpPost]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult> Post(CreateLeaveAllocationCommand leaveAllocation)
  {
    await _mediator.Send(leaveAllocation);
    return NoContent();
  }

  [HttpPut]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesDefaultResponseType]
  public async Task<ActionResult> Put(UpdateLeaveAllocationCommand leaveAllocation)
  {
    await _mediator.Send(leaveAllocation);
    return NoContent();
  }

  [HttpDelete("{id}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesDefaultResponseType]
  public async Task<ActionResult> Delete(int id)
  {
    await _mediator.Send(new DeleteLeaveAllocationCommand(id));
    return NoContent();
  }
}