using Application.Contracts.Logging;
using Application.Contracts.Persistence;
using Application.Dtos;
using AutoMapper;
using MediatR;

namespace Application.Features.LeaveTypes.Queries;

public record GetLeaveTypesQuery : IRequest<List<LeaveTypeDto>>;

public class GetLeaveTypesQueryHandler : IRequestHandler<GetLeaveTypesQuery, List<LeaveTypeDto>>
{
  private readonly IMapper _mapper;
  private readonly ILeaveTypeRepository _leaveTypeRepository;
  private readonly IAppLogger<GetLeaveTypesQueryHandler> _logger;

  public GetLeaveTypesQueryHandler(
  IMapper mapper,
  ILeaveTypeRepository leaveTypeRepository,
  IAppLogger<GetLeaveTypesQueryHandler> logger)
  {
    _mapper = mapper;
    _leaveTypeRepository = leaveTypeRepository;
    _logger = logger;
  }

  public async Task<List<LeaveTypeDto>> Handle(GetLeaveTypesQuery request, CancellationToken cancellationToken)
  {
    var leaveTypes = await _leaveTypeRepository.GetAsync();
    var data = _mapper.Map<List<LeaveTypeDto>>(leaveTypes);
    _logger.LogInformation("Leave types were retrieved successfully");
    return data;
  }
}