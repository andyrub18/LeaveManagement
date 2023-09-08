using Application.Contracts.Logging;
using Application.Contracts.Persistence;
using Application.Exceptions;
using Application.Validators;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Features.LeaveTypes.Commands;

public class UpdateLeaveTypeCommand : IRequest
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public int DefaultDays { get; set; }
}

public class UpdateLeaveTypeCommandHandler : IRequestHandler<UpdateLeaveTypeCommand>
{
  private readonly IMapper _mapper;
  private readonly ILeaveTypeRepository _leaveTypeRepository;
  private readonly IAppLogger<UpdateLeaveTypeCommandHandler> _logger;

  public UpdateLeaveTypeCommandHandler(
  IMapper mapper,
  ILeaveTypeRepository leaveTypeRepository,
  IAppLogger<UpdateLeaveTypeCommandHandler> logger)
  {
    _mapper = mapper;
    _leaveTypeRepository = leaveTypeRepository;
    _logger = logger;
  }

  public async Task Handle(UpdateLeaveTypeCommand request, CancellationToken cancellationToken)
  {
    // Validate incoming data
    var validator = new UpdateLeaveTypeCommandValidator(_leaveTypeRepository);
    var validationResult = await validator.ValidateAsync(request);

    if (validationResult.Errors.Any())
    {
      _logger.LogWarning("Validation errors in update request for {0} - {1}", nameof(LeaveType), request.Id);
      throw new BadRequestException("Invalid Leave type", validationResult);
    }

    // convert to domain entity object
    var leaveTypeToUpdate = _mapper.Map<LeaveType>(request);

    // add to database
    await _leaveTypeRepository.UpdateAsync(leaveTypeToUpdate);
  }

}