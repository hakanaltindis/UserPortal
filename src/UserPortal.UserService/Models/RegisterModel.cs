using System.ComponentModel.DataAnnotations;

namespace UserPortal.UserService.Models
{
  public class RegisterModel
  {
    [Required]
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
  }
}
