using Application.Contracts.Persistence;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence.DatabaseContext;

namespace Persistence.Repositories;

public class LeaveRequestRepository : GenericRepository<LeaveRequest>, ILeaveRequestRepository
{
  public LeaveRequestRepository(HrDatabaseContext context) : base(context)
  {
  }

  public async Task<LeaveRequest?> GetLeaveRequestsWithDetails(int id)
  {
    var leaveRequest = await _context.LeaveRequests!
      .Include(q => q.LeaveType)
      .FirstOrDefaultAsync(q => q.Id == id);
    return leaveRequest;
  }

  public async Task<List<LeaveRequest>> GetLeaveRequestsWithDetails()
  {
    var leaveRequests = await _context.LeaveRequests!
      .Where(e => !string.IsNullOrWhiteSpace(e.RequestingEmployeeId))
      .Include(q => q.LeaveType)
      .ToListAsync();
    return leaveRequests;
  }

  public async Task<List<LeaveRequest>> GetLeaveRequestsWithDetails(string userId)
  {
    var leaveRequest = await _context.LeaveRequests!
      .Where(q => q.RequestingEmployeeId == userId)
      .Include(q => q.LeaveType)
      .ToListAsync();

    return leaveRequest;
  }
}