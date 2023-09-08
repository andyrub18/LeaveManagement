using Application.Contracts.Persistence;
using Application.Exceptions;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Features.LeaveAllocations.Commands;

public record DeleteLeaveAllocationCommand(int Id) : IRequest;

public class DeleteLeaveAllocationCommandHandler : IRequestHandler<DeleteLeaveAllocationCommand>
{
  private readonly ILeaveAllocationRepository _leaveAllocationRepository;
  private readonly IMapper _mapper;

  public DeleteLeaveAllocationCommandHandler(ILeaveAllocationRepository leaveAllocationRepository, IMapper mapper)
  {
    this._leaveAllocationRepository = leaveAllocationRepository;
    _mapper = mapper;
  }

  public async Task Handle(DeleteLeaveAllocationCommand request, CancellationToken cancellationToken)
  {
    var leaveAllocation = await _leaveAllocationRepository.GetByIdAsync(request.Id) ?? throw new NotFoundException(nameof(LeaveAllocation), request.Id);
    await _leaveAllocationRepository.DeleteAsync(leaveAllocation);
  }
}