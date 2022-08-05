using Microsoft.AspNetCore.Mvc;
using UserPortal.UserService.Business;
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
    public IActionResult Get()
    {
      return Ok("Test");
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
      var result = _service.GetById(id);

      if (!result.IsSucceed)
      {
        return NotFound();
      }

      return Ok(result.Value);
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
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

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var result = await _service.Login(model);

      if (!result.IsSucceed)
      {
        return BadRequest(result.ErrorMessage);
      }

      return Ok(result.Value);
    }

    [HttpPut("UpdateProfile/{id}")]
    public async Task<IActionResult> UpdateProfile([FromRoute] int id, [FromBody] UpdateModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var result = await _service.UpdateProfile(id, model);

      if (!result.IsSucceed)
      {
        return BadRequest(result.ErrorMessage);
      }

      return Ok(result.Value);
    }
  }
}
