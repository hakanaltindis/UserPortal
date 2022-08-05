using System.ComponentModel.DataAnnotations;

namespace UserPortal.UserService.Models
{
  public class UpdateModel
  {
    [Required]
    public string? Username { get; set; }
    public string? Email { get; set; }
  }
}
