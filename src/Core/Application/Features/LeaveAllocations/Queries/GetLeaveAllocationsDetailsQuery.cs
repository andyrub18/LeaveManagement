using Application.Contracts.Logging;
using Application.Contracts.Persistence;
using Application.Dtos;
using AutoMapper;
using MediatR;

namespace Application.Features.LeaveAllocations.Queries;

public record GetLeaveAllocationsDetailsQuery(int Id) : IRequest<LeaveAllocationDto>;

public class GetLeaveAllocationsDetailsQueryHandler : IRequestHandler<GetLeaveAllocationsDetailsQuery, LeaveAllocationDto>
{
  private readonly IMapper _mapper;
  private readonly ILeaveAllocationRepository _leaveAllocationRepository;
  private readonly IAppLogger<GetLeaveAllocationsDetailsQueryHandler> _logger;

  public GetLeaveAllocationsDetailsQueryHandler(
  IMapper mapper,
  ILeaveAllocationRepository leaveAllocationRepository,
  IAppLogger<GetLeaveAllocationsDetailsQueryHandler> logger)
  {
    _mapper = mapper;
    _leaveAllocationRepository = leaveAllocationRepository;
    _logger = logger;
  }
  public async Task<LeaveAllocationDto> Handle(GetLeaveAllocationsDetailsQuery request, CancellationToken cancellationToken)
  {
    var leaveAllocation = await _leaveAllocationRepository.GetByIdAsync(request.Id);
    var data = _mapper.Map<LeaveAllocationDto>(leaveAllocation);
    _logger.LogInformation("Leave types were retrieved successfully");
    return data;
  }
}
