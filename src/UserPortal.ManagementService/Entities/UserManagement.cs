namespace UserPortal.ManagementService.Entities
{
  public class UserManagement
  {
    public virtual int Id { get; set; }
    public virtual string? Username { get; set; }
    public virtual string? Email { get; set; }
    public RegistrationStatus RegistrationStatus { get; set; }
    public string? ProviderSource { get; set; }
    public string? ProviderKey { get; set; }
  }
}
