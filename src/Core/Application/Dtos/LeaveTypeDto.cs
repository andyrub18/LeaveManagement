namespace Application.Dtos;

public record LeaveTypeDto(
  int Id,
  string Name,
  int DefaultDays
);

public record LeaveTypeDetailsDto(
  int Id,
  string Name,
  int DefaultDays,
  DateTime? DateCreated,
  DateTime? DateModified
);