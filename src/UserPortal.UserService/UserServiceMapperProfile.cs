using AutoMapper;
using UserPortal.Events.UserService;
using UserPortal.UserService.Entities;
using UserPortal.UserService.Models;

namespace UserPortal.UserService
{
  public class UserServiceMapperProfile : Profile
  {
    public UserServiceMapperProfile()
    {
      CreateMap<RegisterModel, User>()
        .ForMember(d => d.RegistrationStatus, m => m.MapFrom(_ => RegistrationStatus.WaitingForApproval))
        .ForMember(d => d.Status, m => m.MapFrom(_ => UserStatus.Enable))
        ;

      CreateMap<User, UserModel>()
        ;

      CreateMap<User, IUserRegistered>()
        .ForMember(d => d.SourceProvider, m => m.MapFrom(_ => "UserService"))
        .ForMember(d => d.SourceKey, m => m.MapFrom(s => s.Id.ToString()))
        ;

    }
  }
}
