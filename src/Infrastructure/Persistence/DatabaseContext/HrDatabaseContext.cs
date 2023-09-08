using Application.Contracts.Identity;
using Domain;
using Domain.common;
using Microsoft.EntityFrameworkCore;

namespace Persistence.DatabaseContext;

public class HrDatabaseContext : DbContext
{
  private readonly IUserService _userService;

  public HrDatabaseContext(DbContextOptions<HrDatabaseContext> options, IUserService userService) : base(options)
  {
    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    _userService = userService;
  }

  public DbSet<LeaveType>? LeaveTypes { get; set; }
  public DbSet<LeaveAllocation>? LeaveAllocations { get; set; }
  public DbSet<LeaveRequest>? LeaveRequests { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(HrDatabaseContext).Assembly);
    base.OnModelCreating(modelBuilder);
  }

  public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    foreach (var entry in base.ChangeTracker.Entries<BaseEntity>()
      .Where(q => q.State == EntityState.Added || q.State == EntityState.Modified))
    {
      entry.Entity.DateModified = DateTime.Now;
      entry.Entity.ModifiedBy = _userService.UserId;
      if (entry.State == EntityState.Added)
      {
        entry.Entity.DateCreated = DateTime.Now;
        entry.Entity.CreatedBy = _userService.UserId;
      }
    }

    return base.SaveChangesAsync(cancellationToken);
  }
}