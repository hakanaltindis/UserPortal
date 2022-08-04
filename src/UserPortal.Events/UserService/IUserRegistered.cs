using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserPortal.Events.UserService
{
  public interface IUserRegistered
  {
    string UserName { get; set; }
    string Password { get; set; }
    string Email { get; set; }
    string? SourceProvider { get; set; }
    string? SourceKey { get; set; }
  }
}
