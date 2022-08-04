using Microsoft.AspNetCore.Mvc;
using UserPortal.UserService.Business;
using UserPortal.UserService.Data;
using UserPortal.UserService.Models;

namespace UserPortal.UserService.Controllers
{
  [ApiController]
  [Route("[Controller]")]
  public class UserController : ControllerBase
  {
    private readonly IUserService _service;

    public UserController(IUserService service)
    {
      _service = service;
    }

    [HttpGet]
    public IActionResult Get(int id)
    {
      var result = _service.GetById(id);

      if (!result.IsSucceed)
      {
        return NotFound();
      }

      return Ok(result.Value);
    }

    [HttpPost(Name = "Register")]
    public async Task<IActionResult> Register(RegisterModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var result = await _service.Register(model);

      if (!result.IsSucceed)
      {
        return BadRequest(result.ErrorMessage);
      }

      return Created($"~/user/{result.Value?.Id}", result.Value);
    }
  }
}
