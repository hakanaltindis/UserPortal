using AutoMapper;
using MassTransit;
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

      await _publishEndpoint.Publish(_mapper.Map<IUserRegistered>(entityEntry.Entity));

      return Result.Ok(_mapper.Map<UserModel>(entityEntry.Entity));
    }
  }

  public interface IUserService : IBusinessService
  {
    Result<UserModel> GetById(int id);
    Task<Result<UserModel>> Register(RegisterModel model);
  }
}
