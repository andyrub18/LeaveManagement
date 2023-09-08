using Application.Contracts.Persistence;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence.DatabaseContext;

namespace Persistence.Repositories;

public class LeaveAllocationRepository : GenericRepository<LeaveAllocation>, ILeaveAllocationRepository
{
  public LeaveAllocationRepository(HrDatabaseContext context) : base(context)
  {
  }

  public Task AddAllocations(List<LeaveAllocation> allocations)
  {
    throw new NotImplementedException();
  }

  public async Task<bool> AllocationExists(string userId, int leaveTypeId, int period)
  {
    return await _context.LeaveAllocations!
      .AnyAsync(q => q.EmployeeId == userId
        && q.LeaveTypeId == leaveTypeId
        && q.Period == period);
  }

  public Task<LeaveAllocation?> GetLeaveAllocationsWithDetails(int id)
  {
    throw new NotImplementedException();
  }

  public async Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails()
  {
    var leaveAllocations = await _context.LeaveAllocations!
      .Include(q => q.LeaveType)
      .ToListAsync();

    return leaveAllocations;
  }

  public async Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails(string userId)
  {
    var leaveAllocations = await _context.LeaveAllocations!
      .Where(q => q.EmployeeId == userId)
      .Include(q => q.LeaveType)
      .ToListAsync();

    return leaveAllocations;
  }

  public async Task<LeaveAllocation?> GetUserAllocations(string userId, int leaveTypeId)
  {
    var leaveAllocations = await _context.LeaveAllocations!
      .FirstOrDefaultAsync(q => q.EmployeeId == userId && q.LeaveTypeId == leaveTypeId);

    return leaveAllocations;
  }
}