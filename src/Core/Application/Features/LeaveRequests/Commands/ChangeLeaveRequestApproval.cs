using Application.Contracts.Email;
using Application.Contracts.Persistence;
using Application.Exceptions;
using Application.Models.Email;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Features.LeaveRequests.Commands;

public record ChangeLeaveRequestApprovalCommand(int Id, bool Approved) : IRequest;

public class ChangeLeaveRequestApprovalCommandHandler : IRequestHandler<ChangeLeaveRequestApprovalCommand>
{
  private readonly IMapper _mapper;
  private readonly IEmailSender _emailSender;
  private readonly ILeaveRequestRepository _leaveRequestRepository;
  private readonly ILeaveTypeRepository _leaveTypeRepository;
  private readonly ILeaveAllocationRepository _leaveAllocationRepository;

  public ChangeLeaveRequestApprovalCommandHandler(
       ILeaveRequestRepository leaveRequestRepository, ILeaveTypeRepository leaveTypeRepository, IMapper mapper, IEmailSender emailSender, ILeaveAllocationRepository leaveAllocationRepository)
  {
    _leaveRequestRepository = leaveRequestRepository;
    _leaveTypeRepository = leaveTypeRepository;
    _mapper = mapper;
    _emailSender = emailSender;
    _leaveAllocationRepository = leaveAllocationRepository;
  }

  public async Task Handle(ChangeLeaveRequestApprovalCommand request, CancellationToken cancellationToken)
  {
    var leaveRequest = await _leaveRequestRepository.GetByIdAsync(request.Id) ?? throw new NotFoundException(nameof(LeaveRequest), request.Id);
    leaveRequest.Approved = request.Approved;
    await _leaveRequestRepository.UpdateAsync(leaveRequest);

    // if request is approved, get and update the employee's allocations
    if (request.Approved)
    {
      int daysRequested = (int)(leaveRequest.EndDate - leaveRequest.StartDate).TotalDays;
      var allocation = await _leaveAllocationRepository.GetUserAllocations(leaveRequest.RequestingEmployeeId, leaveRequest.LeaveTypeId)
        ?? throw new NotFoundException("No allocation for this user", leaveRequest.RequestingEmployeeId);
      allocation.NumberOfDays -= daysRequested;

      await _leaveAllocationRepository.UpdateAsync(allocation);
    }

    // send confirmation email
    try
    {
      var email = new EmailMessage
      {
        To = string.Empty, /* Get email from employee record */
        Body = $"The approval status for your leave request for {leaveRequest.StartDate:D} to {leaveRequest.EndDate:D} has been updated.",
        Subject = "Leave Request Approval Status Updated"
      };
      await _emailSender.SendEmail(email);
    }
    catch (Exception)
    {
      // log error
    }
  }
}