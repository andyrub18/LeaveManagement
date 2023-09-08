using Application.Contracts.Persistence;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence.DatabaseContext;

namespace Persistence.Repositories;

public class LeaveTypeRepository : GenericRepository<LeaveType>, ILeaveTypeRepository
{
  public LeaveTypeRepository(HrDatabaseContext context) : base(context)
  {
  }

  public async Task<bool> IsLeaveTypeUnique(string name)
  {
    return !(await _context.LeaveTypes!.AnyAsync(q => q.Name == name));
  }
}