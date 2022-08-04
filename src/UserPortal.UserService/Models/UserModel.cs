using UserPortal.UserService.Entities;

namespace UserPortal.UserService.Models
{
  public class UserModel
  {
    public virtual int Id { get; set; }
    public virtual string? Username { get; set; }
    public virtual string? Password { get; set; }
    public virtual string? Email { get; set; }
    public virtual UserStatus Status { get; set; }
    public virtual RegistrationStatus RegistrationStatus { get; set; }
  }
}
