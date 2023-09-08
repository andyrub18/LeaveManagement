using Application.Contracts.Identity;
using Application.Contracts.Persistence;
using Application.Dtos;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Features.LeaveRequests.Queries;
public record GetLeaveRequestListQuery(bool IsLoggedInUser) : IRequest<List<LeaveRequestListDto>>;

public class GetLeaveRequestListQueryHandler : IRequestHandler<GetLeaveRequestListQuery, List<LeaveRequestListDto>>
{
  private readonly ILeaveRequestRepository _leaveRequestRepository;
  private readonly IMapper _mapper;
  private readonly IUserService _userService;

  public GetLeaveRequestListQueryHandler(ILeaveRequestRepository leaveRequestRepository,
      IMapper mapper,
      IUserService userService)
  {
    _leaveRequestRepository = leaveRequestRepository;
    _mapper = mapper;
    _userService = userService;
  }

  public async Task<List<LeaveRequestListDto>> Handle(GetLeaveRequestListQuery request, CancellationToken cancellationToken)
  {
    List<LeaveRequest>? leaveRequests;
    List<LeaveRequestListDto>? requests;

    // Check if it is logged in employee
    if (request.IsLoggedInUser)
    {
      var userId = _userService.UserId;
      leaveRequests = await _leaveRequestRepository.GetLeaveRequestsWithDetails(userId);

      var employee = await _userService.GetEmployee(userId);
      requests = _mapper.Map<List<LeaveRequestListDto>>(leaveRequests);
      foreach (var req in requests)
      {
        req.Employee = employee;
      }
    }
    else
    {
      leaveRequests = await _leaveRequestRepository.GetLeaveRequestsWithDetails();
      requests = _mapper.Map<List<LeaveRequestListDto>>(leaveRequests);
      foreach (var req in requests)
      {
        req.Employee = await _userService.GetEmployee(req.RequestingEmployeeId);
      }
    }

    // Fill requests with employee information

    return requests;
  }
}