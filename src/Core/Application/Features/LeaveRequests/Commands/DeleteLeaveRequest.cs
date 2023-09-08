using Application.Contracts.Persistence;
using Application.Exceptions;
using Domain;
using MediatR;

namespace Application.Features.LeaveRequests.Commands;

public record DeleteLeaveRequestCommand(int Id) : IRequest;

public class DeleteLeaveRequestCommandHandler : IRequestHandler<DeleteLeaveRequestCommand>
{
  private readonly ILeaveRequestRepository _leaveRequestRepository;

  public DeleteLeaveRequestCommandHandler(ILeaveRequestRepository leaveRequestRepository)
  {
    _leaveRequestRepository = leaveRequestRepository;
  }

  public async Task Handle(DeleteLeaveRequestCommand request, CancellationToken cancellationToken)
  {
    var leaveRequest = await _leaveRequestRepository.GetByIdAsync(request.Id);

    if (leaveRequest == null)
      throw new NotFoundException(nameof(LeaveRequest), request.Id);

    await _leaveRequestRepository.DeleteAsync(leaveRequest);
  }
}