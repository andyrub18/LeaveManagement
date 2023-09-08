using Application.Dtos;
using Application.Features.LeaveTypes.Commands;
using Application.Features.LeaveTypes.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LeaveTypesController : ControllerBase
{
  private readonly IMediator _mediator;

  public LeaveTypesController(IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpGet]
  public async Task<ActionResult<List<LeaveTypeDto>>> Get()
  {
    var leaveTypes = await _mediator.Send(new GetLeaveTypesQuery());
    return leaveTypes;
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<LeaveTypeDetailsDto>> Get(int id)
  {
    var leaveType = await _mediator.Send(new GetLeaveTypeDetailsQuery(id));
    return leaveType;
  }

  [HttpPost]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<int>> Post(CreateLeaveTypeCommand leaveType)
  {
    var response = await _mediator.Send(leaveType);
    return CreatedAtAction(nameof(Get), new { id = response });
  }

  [HttpPut("{id}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  [ProducesDefaultResponseType]
  public async Task<ActionResult> Put(UpdateLeaveTypeCommand leaveType)
  {
    await _mediator.Send(leaveType);
    return NoContent();
  }

  [HttpDelete("{id}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  [ProducesDefaultResponseType]
  public async Task<ActionResult> Delete(int id)
  {
    await _mediator.Send(new DeleteLeaveTypeCommand { Id = id });
    return NoContent();
  }
}