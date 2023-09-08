using Application.Contracts.Identity;
using Application.Contracts.Persistence;
using Application.Exceptions;
using Application.Validators;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Features.LeaveAllocations.Commands;

public record CreateLeaveAllocationCommand(int LeaveTypeId) : IRequest;

public class CreateLeaveAllocationCommandHandler : IRequestHandler<CreateLeaveAllocationCommand>
{
  private readonly IMapper _mapper;
  private readonly ILeaveAllocationRepository _leaveAllocationRepository;
  private readonly ILeaveTypeRepository _leaveTypeRepository;
  private readonly IUserService _userService;

  public CreateLeaveAllocationCommandHandler(IMapper mapper,
      ILeaveAllocationRepository leaveAllocationRepository, ILeaveTypeRepository leaveTypeRepository, IUserService userService)
  {
    _mapper = mapper;
    _leaveAllocationRepository = leaveAllocationRepository;
    _leaveTypeRepository = leaveTypeRepository;
    _userService = userService;
  }

  public async Task Handle(CreateLeaveAllocationCommand request, CancellationToken cancellationToken)
  {
    var validator = new CreateLeaveAllocationCommandValidator(_leaveTypeRepository);
    var validationResult = await validator.ValidateAsync(request, cancellationToken);

    if (validationResult.Errors.Any())
      throw new BadRequestException("Invalid Leave Allocation Request", validationResult);

    // Get Leave type for allocations
    var leaveType = await _leaveTypeRepository.GetByIdAsync(request.LeaveTypeId) ?? throw new NotFoundException(nameof(LeaveType), request.LeaveTypeId);

    // Get Employees
    var employees = await _userService.GetEmployees();

    //Get Period
    var period = DateTime.Now.Year;

    var allocations = new List<LeaveAllocation>();
    foreach (var employee in employees)
    {
      var allocationExists = await _leaveAllocationRepository.AllocationExists(employee.Id, request.LeaveTypeId, period);
      if (allocationExists)
        continue;
      allocations.Add(new LeaveAllocation
      {
        EmployeeId = employee.Id,
        LeaveTypeId = leaveType.Id,
        NumberOfDays = leaveType.DefaultDays,
        Period = period
      });
    }

    if (allocations.Any())
      await _leaveAllocationRepository.AddAllocations(allocations);
  }
}