namespace Application.Dtos;

public record LeaveAllocationDto(
  int Id,
  int NumberOfDays,
  LeaveTypeDto LeaveType,
  int LeaveTypeId,
  int Period
);