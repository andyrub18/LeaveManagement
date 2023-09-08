using Application.Contracts.Persistence;
using Application.Exceptions;
using Application.Validators;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Features.LeaveAllocations.Commands;

public class UpdateLeaveAllocationCommand : IRequest
{
  public int Id { get; set; }
  public int NumberOfDays { get; set; }
  public int LeaveTypeId { get; set; }
  public int Period { get; set; }
}

public class UpdateLeaveAllocationCommandHandler : IRequestHandler<UpdateLeaveAllocationCommand>
{
  private readonly IMapper _mapper;
  private readonly ILeaveTypeRepository _leaveTypeRepository;
  private readonly ILeaveAllocationRepository _leaveAllocationRepository;

  public UpdateLeaveAllocationCommandHandler(
      IMapper mapper,
      ILeaveTypeRepository leaveTypeRepository,
      ILeaveAllocationRepository leaveAllocationRepository)
  {
    _mapper = mapper;
    _leaveTypeRepository = leaveTypeRepository;
    _leaveAllocationRepository = leaveAllocationRepository;
  }

  public async Task Handle(UpdateLeaveAllocationCommand request, CancellationToken cancellationToken)
  {
    var validator = new UpdateLeaveAllocationCommandValidator(_leaveTypeRepository, _leaveAllocationRepository);
    var validationResult = await validator.ValidateAsync(request);

    if (validationResult.Errors.Any())
      throw new BadRequestException("Invalid Leave Allocation", validationResult);

    var leaveAllocation = await _leaveAllocationRepository.GetByIdAsync(request.Id) ?? throw new NotFoundException(nameof(LeaveAllocation), request.Id);
    _mapper.Map(request, leaveAllocation);

    await _leaveAllocationRepository.UpdateAsync(leaveAllocation);
  }
}