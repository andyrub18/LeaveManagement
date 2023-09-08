using Application.Contracts.Logging;
using Application.Contracts.Persistence;
using Application.Dtos;
using Application.Features.LeaveTypes.Queries;
using Application.MappingProfiles;
using Application.UnitTests.Mocks;
using AutoMapper;

namespace Application.UnitTests.Features.LeaveTypes.Queries;

public class GetLeaveTypeListQueryHandlerTests
{
  private readonly ILeaveTypeRepository _leaveTypeRepository;
  private readonly IMapper _mapper;
  private readonly IAppLogger<GetLeaveTypesQueryHandler> _mockAppLoger;

  public GetLeaveTypeListQueryHandlerTests()
  {
    _leaveTypeRepository = MockLeaveTypeRepository.GetMockLeaveTypeRepository();
    var mapperConfig = new MapperConfiguration(c =>
    {
      c.AddProfile<LeaveTypeProfile>();
    });

    _mapper = mapperConfig.CreateMapper();
    _mockAppLoger = Substitute.For<IAppLogger<GetLeaveTypesQueryHandler>>();
  }

  [Fact]
  public async Task GetLeaveTypeListTest()
  {
    var handler = new GetLeaveTypesQueryHandler(_mapper, _leaveTypeRepository, _mockAppLoger);

    var result = await handler.Handle(new GetLeaveTypesQuery(), CancellationToken.None);
    result.Should().BeOfType<List<LeaveTypeDto>>();
    result.Count.Should().Be(3);
  }
}