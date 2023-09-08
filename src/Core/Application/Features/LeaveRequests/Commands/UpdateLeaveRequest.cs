using Application.Contracts.Email;
using Application.Contracts.Logging;
using Application.Contracts.Persistence;
using Application.Dtos;
using Application.Exceptions;
using Application.Models.Email;
using Application.Validators;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Features.LeaveRequests.Commands;

public class UpdateLeaveRequestCommand : BaseLeaveRequest, IRequest
{
  public int Id { get; set; }
  public string RequestComments { get; set; } = string.Empty;
  public bool Cancelled { get; set; }
}

public class UpdateLeaveRequestCommandHandler : IRequestHandler<UpdateLeaveRequestCommand>
{
  private readonly IMapper _mapper;
  private readonly IEmailSender _emailSender;
  private readonly IAppLogger<UpdateLeaveRequestCommandHandler> _appLogger;
  private readonly ILeaveRequestRepository _leaveRequestRepository;
  private readonly ILeaveTypeRepository _leaveTypeRepository;

  public UpdateLeaveRequestCommandHandler(
       ILeaveRequestRepository leaveRequestRepository, ILeaveTypeRepository leaveTypeRepository, IMapper mapper, IEmailSender emailSender, IAppLogger<UpdateLeaveRequestCommandHandler> appLogger)
  {
    _leaveRequestRepository = leaveRequestRepository;
    _leaveTypeRepository = leaveTypeRepository;
    _mapper = mapper;
    this._emailSender = emailSender;
    this._appLogger = appLogger;
  }

  public async Task Handle(UpdateLeaveRequestCommand request, CancellationToken cancellationToken)
  {
    var leaveRequest = await _leaveRequestRepository.GetByIdAsync(request.Id) ?? throw new NotFoundException(nameof(LeaveRequest), request.Id);
    var validator = new UpdateLeaveRequestCommandValidator(_leaveTypeRepository, _leaveRequestRepository);
    var validationResult = await validator.ValidateAsync(request, cancellationToken);

    if (validationResult.Errors.Any())
      throw new BadRequestException("Invalid Leave Request", validationResult);

    _mapper.Map(request, leaveRequest);

    await _leaveRequestRepository.UpdateAsync(leaveRequest);

    try
    {
      // send confirmation email
      var email = new EmailMessage
      {
        To = string.Empty, /* Get email from employee record */
        Body = $"Your leave request for {request.StartDate:D} to {request.EndDate:D} " +
                  $"has been updated successfully.",
        Subject = "Leave Request Updated"
      };

      await _emailSender.SendEmail(email);
    }
    catch (Exception ex)
    {
      _appLogger.LogWarning(ex.Message);
    }
  }
}