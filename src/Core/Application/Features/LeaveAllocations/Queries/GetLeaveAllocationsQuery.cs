using Application.Contracts.Logging;
using Application.Contracts.Persistence;
using Application.Dtos;
using AutoMapper;
using MediatR;

namespace Application.Features.LeaveAllocations.Queries;

public record GetLeaveAllocationsQuery() : IRequest<List<LeaveAllocationDto>>;

public class GetLeaveAllocationsQueryHandler : IRequestHandler<GetLeaveAllocationsQuery, List<LeaveAllocationDto>>
{
  private readonly IMapper _mapper;
  private readonly ILeaveAllocationRepository _leaveAllocationRepository;
  private readonly IAppLogger<GetLeaveAllocationsQueryHandler> _logger;

  public GetLeaveAllocationsQueryHandler(
  IMapper mapper,
  ILeaveAllocationRepository leaveAllocationRepository,
  IAppLogger<GetLeaveAllocationsQueryHandler> logger)
  {
    _mapper = mapper;
    _leaveAllocationRepository = leaveAllocationRepository;
    _logger = logger;
  }

  public async Task<List<LeaveAllocationDto>> Handle(GetLeaveAllocationsQuery request, CancellationToken cancellationToken)
  {
    var leaveAllocations = await _leaveAllocationRepository.GetAsync();
    var data = _mapper.Map<List<LeaveAllocationDto>>(leaveAllocations);
    _logger.LogInformation("Leave types were retrieved successfully");
    return data;
  }
}
