using Application.Models.Identity;

namespace Application.Dtos;

public abstract class BaseLeaveRequest
{
  public DateTime StartDate { get; set; }
  public DateTime EndDate { get; set; }
  public int LeaveTypeId { get; set; }
}

public class LeaveRequestListDto
{
  public int Id { get; set; }
  public Employee Employee { get; set; } = new();
  public string RequestingEmployeeId { get; set; } = string.Empty;
  public LeaveTypeDto LeaveType { get; set; } = default!;
  public DateTime DateRequested { get; set; }
  public DateTime StartDate { get; set; }
  public DateTime EndDate { get; set; }
  public bool? Approved { get; set; }
  public bool? Cancelled { get; set; }

}

public record LeaveRequestDetailsDto(
  int Id,
  Employee Employee,
  DateTime StartDate,
  DateTime EndDate,
  string RequestingEmployeeId,
  LeaveTypeDto LeaveType,
  int LeaveTypeId,
  DateTime DateRequested,
  string RequestComments,
  DateTime? DateActioned,
  bool? Approved,
  bool Cancelled
);