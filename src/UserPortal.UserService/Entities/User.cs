

namespace UserPortal.UserService.Entities
{
  public class User
  {
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
    public UserStatus Status { get; set; }
    public RegistrationStatus RegistrationStatus { get; set; }
  }

  public enum UserStatus
  {
    Enable,
    Disable,
  }

  public enum RegistrationStatus
  {
    WaitingForApproval,
    Approved,
  }
}
