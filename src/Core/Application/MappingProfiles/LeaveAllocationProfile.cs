using Application.Dtos;
using Application.Features.LeaveAllocations.Commands;
using AutoMapper;
using Domain;

namespace Application.MappingProfiles;

public class LeaveAllocationProfile : Profile
{
  public LeaveAllocationProfile()
  {
    CreateMap<LeaveAllocation, LeaveAllocationDto>().ReverseMap();
    CreateMap<CreateLeaveAllocationCommand, LeaveAllocation>();
    CreateMap<UpdateLeaveAllocationCommand, LeaveAllocation>();
  }

}
