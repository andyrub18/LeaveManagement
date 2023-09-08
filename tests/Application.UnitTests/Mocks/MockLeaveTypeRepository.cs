using Application.Contracts.Persistence;
using Domain;

namespace Application.UnitTests.Mocks;

public class MockLeaveTypeRepository
{
  public static ILeaveTypeRepository GetMockLeaveTypeRepository()
  {
    var leaveTypes = new List<LeaveType>
    {
      new() {
        Id = 1,
        DefaultDays = 10,
        Name = "Test Vacation",
      },
      new() {
        Id = 2,
        DefaultDays = 15,
        Name = "Test sick",
      },
      new() {
        Id = 3,
        DefaultDays = 18,
        Name = "Test Maternity",
      },
    };

    var mockRepo = Substitute.For<ILeaveTypeRepository>();
    mockRepo.GetAsync().Returns(leaveTypes);
    mockRepo.GetByIdAsync(1).Returns(leaveTypes[0]);
    mockRepo.CreateAsync(default!).ReturnsForAnyArgs(x =>
    {
      leaveTypes.Add(x.Arg<LeaveType>());
      return Task.CompletedTask;
    });

    return mockRepo;
  }
}