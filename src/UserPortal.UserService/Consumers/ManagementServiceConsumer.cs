using MassTransit;
using UserPortal.Events;
using UserPortal.Events.ManagementService;
using UserPortal.UserService.Data;
using UserPortal.UserService.Entities;

namespace UserPortal.UserService.Consumers
{
  public class ManagementServiceConsumer :
    IConsumer<UserApproved>,
    IConsumer<UserDeclined>,
    IConsumer<UserEnabled>,
    IConsumer<UserDisabled>
  {
    private readonly UserDbContext _dbContext;

    public ManagementServiceConsumer(UserDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<UserApproved> context)
    {
      ValidateMessage(context.Message, out var id);

      TryGetUser(id, out var user);
      
      user.RegistrationStatus = RegistrationStatus.Approved;

      _dbContext.Users.Update(user);
      await _dbContext.SaveChangesAsync();
    }

    public async Task Consume(ConsumeContext<UserDeclined> context)
    {
      ValidateMessage(context.Message, out var id);

      TryGetUser(id, out var user);

      user.RegistrationStatus = RegistrationStatus.Declined;

      _dbContext.Users.Update(user);
      await _dbContext.SaveChangesAsync();
    }
    
    public async Task Consume(ConsumeContext<UserEnabled> context)
    {
      ValidateMessage(context.Message, out var id);

      TryGetUser(id, out var user);

      user.Status = UserStatus.Enable;

      _dbContext.Users.Update(user);
      await _dbContext.SaveChangesAsync();
    }

    public async Task Consume(ConsumeContext<UserDisabled> context)
    {
      ValidateMessage(context.Message, out var id);

      TryGetUser(id, out var user);

      user.Status = UserStatus.Disable;

      _dbContext.Users.Update(user);
      await _dbContext.SaveChangesAsync();
    }

    private void ValidateMessage<T>(T message, out int id)
      where T : EventBase
    {
      if (message.SourceProvider != "UserService")
      {
        throw
          new InvalidOperationException("This service is not owner for the message");
      }

      if (int.TryParse(message.SourceKey, out id))
      {
        throw
          new InvalidOperationException(
            $"The source key must be convertable to integer! The source key is {message.SourceKey}");
      }
    }

    private void TryGetUser(int id, out User user)
    {
      var temp = _dbContext.Users.Find(id);
      if (temp == null)
      {
        throw new UserNotFoundException(id);
      }

      user = temp;
    }
  }
}
