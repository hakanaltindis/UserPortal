using AutoMapper;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using UserPortal.Events.UserService;
using UserPortal.Shared;
using UserPortal.UserService.Data;
using UserPortal.UserService.Entities;
using UserPortal.UserService.Models;

namespace UserPortal.UserService.Business
{
  public class UserService : IUserService
  {
    private readonly UserDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public UserService(UserDbContext dbContext
      , IMapper mapper
      , IPublishEndpoint publishEndpoint)
    {
      _dbContext = dbContext;
      _mapper = mapper;
      _publishEndpoint = publishEndpoint;
    }

    public Result<UserModel> GetById(int id)
    {
      var user = _dbContext.Users.Find(id);
      if (user == null)
      {
        return Result.Error<UserModel>($"The user does not exist with the id {id}");
      }

      return Result.Ok(_mapper.Map<UserModel>(user));
    }

    public async Task<Result<UserModel>> Login(LoginModel model)
    {
      var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
      if (user == null)
      {
        return Result.Error<UserModel>("The username does not exists.");
      }

      if (user.Password != model.Password)
      {
        return Result.Error<UserModel>("The username or password are wrong.");
      }

      if (user.RegistrationStatus != RegistrationStatus.Approved)
      {
        return Result.Error<UserModel>("The user is not approved yet.");
      }

      if (user.Status != UserStatus.Enable)
      {
        return Result.Error<UserModel>("The user is not enabled.");
      }

      if (user.Password != model.Password)
      {
        return Result.Error<UserModel>("The username or password are wrong.");
      }

      return Result.Ok(_mapper.Map<UserModel>(user));
    }

    public async Task<Result<UserModel>> Register(RegisterModel model)
    {
      if (_dbContext.Users.Any(u => u.Username == model.Username))
      {
        return Result.Error<UserModel>("The username already exists.");
      }

      if (_dbContext.Users.Any(u => u.Email == model.Email))
      {
        return Result.Error<UserModel>("The email already exists.");
      }

      var user = _mapper.Map<User>(model);

      var entityEntry = _dbContext.Users.Add(user);
      await _dbContext.SaveChangesAsync();

      await _publishEndpoint.Publish(_mapper.Map<UserRegistered>(entityEntry.Entity));

      return Result.Ok(_mapper.Map<UserModel>(entityEntry.Entity));
    }

    public async Task<Result<UserModel>> UpdateProfile(int id, UpdateModel model)
    {
      var user = await _dbContext.Users.FindAsync(id);
      if (user == null)
      {
        return Result.Error<UserModel>($"The user cannot be found with the id {id}");
      }

      if (_dbContext.Users.Any(u => u.Username == model.Username && u.Id != id))
      {
        return Result.Error<UserModel>($"The username is already using. Please enter different one.");
      }

      if (_dbContext.Users.Any(u => u.Email == model.Email && u.Id != id))
      {
        return Result.Error<UserModel>($"The email is already using. Please enter different one.");
      }

      user.Username = model.Username;
      user.Email = model.Email;

      _dbContext.Users.Update(user);
      await _dbContext.SaveChangesAsync();

      return Result.Ok(_mapper.Map<UserModel>(user));
    }
  }

  public interface IUserService : IBusinessService
  {
    Result<UserModel> GetById(int id);
    Task<Result<UserModel>> Login(LoginModel model);
    Task<Result<UserModel>> Register(RegisterModel model);
    Task<Result<UserModel>> UpdateProfile(int id, UpdateModel model);
  }
}
