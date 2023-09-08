using Application.Dtos;
using Application.Features.LeaveRequests.Queries;
using Application.Features.LeaveRequests.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]

public class LeaveRequestsController : ControllerBase
{
  private readonly IMediator _mediator;

  public LeaveRequestsController(IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpGet]
  public async Task<ActionResult<List<LeaveRequestListDto>>> Get(bool isLoggedUser = false)
  {
    var leaveRequests = await _mediator.Send(new GetLeaveRequestListQuery(isLoggedUser));
    return Ok(leaveRequests);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<LeaveRequestDetailsDto>> Get(int id)
  {
    var leaveRequest = await _mediator.Send(new GetLeaveRequestDetailQuery(id));
    return Ok(leaveRequest);
  }

  [HttpPost]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult> Post(CreateLeaveRequestCommand leaveRequest)
  {
    await _mediator.Send(leaveRequest);
    return NoContent();
  }

  [HttpPut]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesDefaultResponseType]
  public async Task<ActionResult> Put(UpdateLeaveRequestCommand leaveRequest)
  {
    await _mediator.Send(leaveRequest);
    return NoContent();
  }

  [HttpDelete("{id}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesDefaultResponseType]
  public async Task<ActionResult> Delete(int id)
  {
    await _mediator.Send(new DeleteLeaveRequestCommand(id));
    return NoContent();
  }

  [HttpPut]
  [Route("CancelRequest")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesDefaultResponseType]
  public async Task<ActionResult> CancelRequest(CancelLeaveRequestCommand cancelLeaveRequest)
  {
    await _mediator.Send(cancelLeaveRequest);
    return NoContent();
  }

  [HttpPut]
  [Route("UpdateApproval")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesDefaultResponseType]
  public async Task<ActionResult> UpdateApproval(ChangeLeaveRequestApprovalCommand changeLeaveRequestApproval)
  {
    await _mediator.Send(changeLeaveRequestApproval);
    return NoContent();
  }
}