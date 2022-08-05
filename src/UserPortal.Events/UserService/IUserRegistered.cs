namespace UserPortal.Events.UserService
{
  public class UserRegistered
  {
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string SourceProvider { get; set; } = string.Empty;
    public string SourceKey { get; set; } = string.Empty;
  }
}
