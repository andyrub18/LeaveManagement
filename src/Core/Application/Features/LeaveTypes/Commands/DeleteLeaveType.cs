using Application.Contracts.Persistence;
using Application.Exceptions;
using MediatR;

namespace Application.Features.LeaveTypes.Commands;

public class DeleteLeaveTypeCommand : IRequest
{
  public int Id { get; set; }
}

public class DeleteLeaveTypeCommandHandler : IRequestHandler<DeleteLeaveTypeCommand>
{

  private readonly ILeaveTypeRepository _leaveTypeRepository;

  public DeleteLeaveTypeCommandHandler(ILeaveTypeRepository leaveTypeRepository)
  {

    this._leaveTypeRepository = leaveTypeRepository;
  }

  public async Task Handle(DeleteLeaveTypeCommand request, CancellationToken cancellationToken)
  {
    //Validate incoming data

    //retrieve domain entity object
    var leaveTypeToDelete = await _leaveTypeRepository.GetByIdAsync(request.Id) ?? throw new NotFoundException(nameof(LeaveTypes), request.Id);

    //Remove from database
    await _leaveTypeRepository.DeleteAsync(leaveTypeToDelete);
  }
}