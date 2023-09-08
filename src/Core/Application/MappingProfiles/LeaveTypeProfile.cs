using Application.Dtos;
using Application.Features.LeaveTypes.Commands;
using AutoMapper;
using Domain;

namespace Application.MappingProfiles;

public class LeaveTypeProfile : Profile
{
  public LeaveTypeProfile()
  {
    CreateMap<LeaveType, LeaveTypeDto>().ReverseMap();
    CreateMap<LeaveType, LeaveTypeDetailsDto>();
    CreateMap<CreateLeaveTypeCommand, LeaveType>();
    CreateMap<UpdateLeaveTypeCommand, LeaveType>();
  }
}
