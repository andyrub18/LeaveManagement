using Domain;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Persistence.DatabaseContext;

namespace Persistence.IntegrationTests;

public class HrDatabaseContextTest
{
    private readonly HrDatabaseContext _hrDatabaseContext;

    public HrDatabaseContextTest()
    {
        var dbOptions = new DbContextOptionsBuilder<HrDatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        _hrDatabaseContext = new HrDatabaseContext(dbOptions);
    }

    [Fact]
    public async void Save_SetDateCreatedValue()
    {
        var LeaveType = new LeaveType
        {
            Id = 1,
            DefaultDays = 10,
            Name = "Test Vacation"
        };

        await _hrDatabaseContext.LeaveTypes!.AddAsync(LeaveType);
        await _hrDatabaseContext.SaveChangesAsync();

        LeaveType.DateCreated.Should().NotBeNull();
    }

    [Fact]
    public async void Save_SetDateModifiedValue()
    {
        var LeaveType = new LeaveType
        {
            Id = 1,
            DefaultDays = 10,
            Name = "Test Vacation"
        };

        await _hrDatabaseContext.LeaveTypes!.AddAsync(LeaveType);
        await _hrDatabaseContext.SaveChangesAsync();

        LeaveType.DateModified.Should().NotBeNull();
    }
}