using Application.Contracts.Email;
using Application.Contracts.Persistence;
using Application.Dtos;
using Application.Exceptions;
using Application.Models.Email;
using Application.Validators;
using Domain;
using AutoMapper;
using MediatR;
using Application.Contracts.Identity;

namespace Application.Features.LeaveRequests.Commands;

public class CreateLeaveRequestCommand : BaseLeaveRequest, IRequest
{
  public string RequestComments { get; set; } = string.Empty;
}

public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand>
{
  private readonly IEmailSender _emailSender;
  private readonly IMapper _mapper;
  private readonly ILeaveTypeRepository _leaveTypeRepository;
  private readonly ILeaveRequestRepository _leaveRequestRepository;
  private readonly ILeaveAllocationRepository _leaveAllocationRepository;
  private readonly IUserService _userService;

  public CreateLeaveRequestCommandHandler(IEmailSender emailSender,
      IMapper mapper, ILeaveTypeRepository leaveTypeRepository, ILeaveRequestRepository leaveRequestRepository, IUserService userService, ILeaveAllocationRepository leaveAllocationRepository)
  {
    _emailSender = emailSender;
    _mapper = mapper;
    _leaveTypeRepository = leaveTypeRepository;
    _leaveRequestRepository = leaveRequestRepository;
    _userService = userService;
    _leaveAllocationRepository = leaveAllocationRepository;
  }

  public async Task Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
  {
    var validator = new CreateLeaveRequestCommandValidator(_leaveTypeRepository);
    var validationResult = await validator.ValidateAsync(request, cancellationToken);

    if (validationResult.Errors.Any())
      throw new BadRequestException("Invalid Leave Request", validationResult);

    // Get requesting employee's id
    var employeeId = _userService.UserId;
    if (string.IsNullOrEmpty(employeeId))
      throw new BadRequestException("No user is not connected");

    // Check on employee's allocation
    var allocation = await _leaveAllocationRepository.GetUserAllocations(employeeId, request.LeaveTypeId);

    // if allocations aren't enough, return validation error with message
    if (allocation is null)
    {
      validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure(nameof(request.LeaveTypeId),
          "You do not have any allocations for this leave type."));
      throw new BadRequestException("Invalid Leave Request", validationResult);
    }

    int daysRequested = (int)(request.EndDate - request.StartDate).TotalDays;
    if (daysRequested > allocation.NumberOfDays)
    {
      validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure(
          nameof(request.EndDate), "You do not have enough days for this request"));
      throw new BadRequestException("Invalid Leave Request", validationResult);
    }

    // Create leave request
    var leaveRequest = _mapper.Map<LeaveRequest>(request);
    leaveRequest.RequestingEmployeeId = employeeId;
    leaveRequest.DateRequested = DateTime.Now;
    await _leaveRequestRepository.CreateAsync(leaveRequest);

    // send confirmation email
    try
    {
      var email = new EmailMessage
      {
        To = string.Empty, /* Get email from employee record */
        Body = $"Your leave request for {request.StartDate:D} to {request.EndDate:D} " +
              $"has been submitted successfully.",
        Subject = "Leave Request Submitted"
      };

      await _emailSender.SendEmail(email);
    }
    catch (Exception)
    {
      //// Log or handle error,
    }
  }
}