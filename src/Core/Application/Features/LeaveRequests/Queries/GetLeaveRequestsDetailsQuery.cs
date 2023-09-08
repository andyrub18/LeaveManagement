using Application.Contracts.Persistence;
using Application.Dtos;
using Application.Exceptions;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Features.LeaveRequests.Queries;

public record GetLeaveRequestDetailQuery(int Id) : IRequest<LeaveRequestDetailsDto>;

public class GetLeaveRequestDetailQueryHandler : IRequestHandler<GetLeaveRequestDetailQuery, LeaveRequestDetailsDto>
{
  private readonly ILeaveRequestRepository _leaveRequestRepository;
  private readonly IMapper _mapper;

  public GetLeaveRequestDetailQueryHandler(ILeaveRequestRepository leaveRequestRepository,
      IMapper mapper)
  {
    _leaveRequestRepository = leaveRequestRepository;
    _mapper = mapper;
  }
  public async Task<LeaveRequestDetailsDto> Handle(GetLeaveRequestDetailQuery request, CancellationToken cancellationToken)
  {
    var leaveRequest = _mapper.Map<LeaveRequestDetailsDto>(await _leaveRequestRepository.GetLeaveRequestsWithDetails(request.Id));

    if (leaveRequest == null)
      throw new NotFoundException(nameof(LeaveRequest), request.Id);

    // Add Employee details as needed

    return leaveRequest;
  }
}