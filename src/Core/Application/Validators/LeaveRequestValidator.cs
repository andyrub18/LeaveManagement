using Application.Contracts.Persistence;
using Application.Dtos;
using Application.Features.LeaveRequests.Commands;
using FluentValidation;

namespace Application.Validators;

public class BaseLeaveRequestValidator : AbstractValidator<BaseLeaveRequest>
{
  private readonly ILeaveTypeRepository _leaveTypeRepository;

  public BaseLeaveRequestValidator(ILeaveTypeRepository leaveTypeRepository)
  {
    _leaveTypeRepository = leaveTypeRepository;
    RuleFor(p => p.StartDate)
        .LessThan(p => p.EndDate).WithMessage("{PropertyName} must be before {ComparisonValue}");

    RuleFor(p => p.EndDate)
        .GreaterThan(p => p.StartDate).WithMessage("{PropertyName} must be after {ComparisonValue}");

    RuleFor(p => p.LeaveTypeId)
        .GreaterThan(0)
        .MustAsync(LeaveTypeMustExist)
        .WithMessage("{PropertyName} does not exist.");
  }

  private async Task<bool> LeaveTypeMustExist(int id, CancellationToken arg2)
  {
    var leaveType = await _leaveTypeRepository.GetByIdAsync(id);
    return leaveType is not null;
  }
}

public class CreateLeaveRequestCommandValidator : AbstractValidator<CreateLeaveRequestCommand>
{
  private readonly ILeaveTypeRepository _leaveTypeRepository;

  public CreateLeaveRequestCommandValidator(ILeaveTypeRepository leaveTypeRepository)
  {
    _leaveTypeRepository = leaveTypeRepository;
    Include(new BaseLeaveRequestValidator(_leaveTypeRepository));
  }
}

public class UpdateLeaveRequestCommandValidator : AbstractValidator<UpdateLeaveRequestCommand>
{
  private readonly ILeaveTypeRepository _leaveTypeRepository;
  private readonly ILeaveRequestRepository _leaveRequestRepository;

  public UpdateLeaveRequestCommandValidator(ILeaveTypeRepository leaveTypeRepository, ILeaveRequestRepository leaveRequestRepository)
  {
    _leaveTypeRepository = leaveTypeRepository;
    _leaveRequestRepository = leaveRequestRepository;

    Include(new BaseLeaveRequestValidator(_leaveTypeRepository));

    RuleFor(p => p.Id)
            .NotNull()
            .MustAsync(LeaveRequestMustExist)
            .WithMessage("{PropertyName} must be present");
  }

  private async Task<bool> LeaveRequestMustExist(int id, CancellationToken arg2)
  {
    var leaveAllocation = await _leaveRequestRepository.GetByIdAsync(id);
    return leaveAllocation is not null;
  }

}

public class ChangeLeaveRequestApprovalCommandValidator : AbstractValidator<ChangeLeaveRequestApprovalCommand>
{
  public ChangeLeaveRequestApprovalCommandValidator()
  {
    RuleFor(p => p.Approved)
        .NotNull()
        .WithMessage("Approval status cannot be null");
  }
}