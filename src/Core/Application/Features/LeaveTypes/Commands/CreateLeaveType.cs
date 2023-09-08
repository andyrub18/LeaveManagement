using Application.Contracts.Persistence;
using Application.Exceptions;
using Application.Validators;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Features.LeaveTypes.Commands;

public class CreateLeaveTypeCommand : IRequest<int>
{
  public string Name { get; set; } = string.Empty;
  public int DefaultDays { get; set; }
}

public class CreateLeaveTypeCommandHandler : IRequestHandler<CreateLeaveTypeCommand, int>
{
  private readonly IMapper _mapper;
  private readonly ILeaveTypeRepository _leaveTypeRepository;

  public CreateLeaveTypeCommandHandler(IMapper mapper, ILeaveTypeRepository leaveTypeRepository)
  {
    _mapper = mapper;
    _leaveTypeRepository = leaveTypeRepository;
  }

  public async Task<int> Handle(CreateLeaveTypeCommand request, CancellationToken cancellationToken)
  {
    // Validate incoming data
    var validator = new CreateLeaveTypeCommandValidator(_leaveTypeRepository);
    var validationResult = await validator.ValidateAsync(request, cancellationToken);

    if (validationResult.Errors.Any())
      throw new BadRequestException("Invalid Leave type", validationResult);

    // convert to domain entity object
    var leaveTypeToCreate = _mapper.Map<LeaveType>(request);

    // add to database
    await _leaveTypeRepository.CreateAsync(leaveTypeToCreate);

    // retun record id
    return leaveTypeToCreate.Id;
  }
}