using Application.Contracts.Email;
using Application.Contracts.Persistence;
using Application.Exceptions;
using Application.Models.Email;
using Domain;
using MediatR;

namespace Application.Features.LeaveRequests.Commands;

public record CancelLeaveRequestCommand(int Id) : IRequest;

public class CancelLeaveRequestCommandHandler : IRequestHandler<CancelLeaveRequestCommand>
{
  private readonly ILeaveRequestRepository _leaveRequestRepository;
  private readonly ILeaveAllocationRepository _leaveAllocationRepository;
  private readonly IEmailSender _emailSender;

  public CancelLeaveRequestCommandHandler(ILeaveRequestRepository leaveRequestRepository, IEmailSender emailSender, ILeaveAllocationRepository leaveAllocationRepository)
  {
    _leaveRequestRepository = leaveRequestRepository;
    _emailSender = emailSender;
    _leaveAllocationRepository = leaveAllocationRepository;
  }

  public async Task Handle(CancelLeaveRequestCommand request, CancellationToken cancellationToken)
  {
    var leaveRequest = await _leaveRequestRepository.GetByIdAsync(request.Id) ?? throw new NotFoundException(nameof(LeaveRequest), request.Id);
    leaveRequest.Cancelled = true;
    await _leaveRequestRepository.UpdateAsync(leaveRequest);

    // if already approved, re-evaluate the employee's allocations for the leave type
    if (leaveRequest.Approved.HasValue)
    {
      int daysRequested = (int)(leaveRequest.EndDate - leaveRequest.StartDate).TotalDays;
      var allocation = await _leaveAllocationRepository.GetUserAllocations(leaveRequest.RequestingEmployeeId, leaveRequest.LeaveTypeId)
       ?? throw new NotFoundException("No allocation for this user", leaveRequest.RequestingEmployeeId);
      allocation.NumberOfDays += daysRequested;

      await _leaveAllocationRepository.UpdateAsync(allocation);
    }

    // send confirmation email
    try
    {
      var email = new EmailMessage
      {
        To = string.Empty, /* Get email from employee record */
        Body = $"Your leave request for {leaveRequest.StartDate:D} to {leaveRequest.EndDate:D} has been cancelled successfully.",
        Subject = "Leave Request Cancelled"
      };

      await _emailSender.SendEmail(email);
    }
    catch (Exception)
    {
      // log error
    }
  }
}