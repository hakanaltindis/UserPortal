using AutoMapper;
using MassTransit;
using UserPortal.Events.UserService;
using UserPortal.ManagementService.Data;
using UserPortal.ManagementService.Entities;

namespace UserPortal.ManagementService.Consumers
{
  public class UserServiceConsumer : IConsumer<UserRegistered>
  {
    private readonly IMapper _mapper;
    private readonly ManagementDbContext _dbContext;

    public UserServiceConsumer(IMapper mapper
      , ManagementDbContext dbContext)
    {
      _mapper = mapper;
      _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<UserRegistered> context)
    {
      var user = _mapper.Map<UserManagement>(context.Message);

      _dbContext.UserManagements.Add(user);
      await _dbContext.SaveChangesAsync();
    }
  }
}
