using Application.Contracts.Persistence;
using Application.Features.LeaveTypes.Commands;
using FluentValidation;

namespace Application.Validators;

public class CreateLeaveTypeCommandValidator : AbstractValidator<CreateLeaveTypeCommand>
{
  private readonly ILeaveTypeRepository _leaveTypeRepository;

  public CreateLeaveTypeCommandValidator(ILeaveTypeRepository leaveTypeRepository)
  {
    _leaveTypeRepository = leaveTypeRepository;

    RuleFor(p => p.Name)
      .NotEmpty().WithMessage("{PropertyName} is required")
      .NotNull()
      .MaximumLength(70).WithMessage("{PropertyName} must be fewer than 70 characters");

    RuleFor(p => p.DefaultDays)
      .LessThan(100).WithMessage("{PropertyName} cannot exceed 100")
      .GreaterThan(1).WithMessage("{PropertyName} cannot be less than 1");

    RuleFor(q => q)
      .MustAsync(LeaveTypeNameUnique)
      .WithMessage("Leave type already exists");
  }

  private Task<bool> LeaveTypeNameUnique(CreateLeaveTypeCommand command, CancellationToken token)
  {
    return _leaveTypeRepository.IsLeaveTypeUnique(command.Name);
  }
}

public class UpdateLeaveTypeCommandValidator : AbstractValidator<UpdateLeaveTypeCommand>
{
  private readonly ILeaveTypeRepository _leaveTypeRepository;

  public UpdateLeaveTypeCommandValidator(ILeaveTypeRepository leaveTypeRepository)
  {
    _leaveTypeRepository = leaveTypeRepository;

    RuleFor(p => p.Id)
      .NotNull()
      .MustAsync(LeaveTypeMustExist);

    RuleFor(p => p.Name)
      .NotEmpty().WithMessage("{PropertyName} is required")
      .NotNull()
      .MaximumLength(70).WithMessage("{PropertyName} must be fewer than 70 characters");

    RuleFor(p => p.DefaultDays)
      .LessThan(100).WithMessage("{PropertyName} cannot exceed 100")
      .GreaterThan(1).WithMessage("{PropertyName} cannot be less than 1");

    RuleFor(q => q)
      .MustAsync(LeaveTypeNameUnique)
      .WithMessage("Leave type already exists");
  }

  private async Task<bool> LeaveTypeMustExist(int id, CancellationToken token)
  {
    var leaveType = await _leaveTypeRepository.GetByIdAsync(id);
    return leaveType is not null;
  }

  private Task<bool> LeaveTypeNameUnique(UpdateLeaveTypeCommand command, CancellationToken token)
  {
    return _leaveTypeRepository.IsLeaveTypeUnique(command.Name);
  }
}