using AutoMapper;
using UserPortal.Events.UserService;
using UserPortal.ManagementService.Entities;

namespace UserPortal.ManagementService
{
  public class ManagementServiceMapperProfile : Profile
  {
    public ManagementServiceMapperProfile()
    {
      CreateMap<UserRegistered, UserManagement>()
        .ForMember(d => d.RegistrationStatus, m => m.MapFrom(_ => RegistrationStatus.WaitingForApproval))
        ;
    }
  }
}
