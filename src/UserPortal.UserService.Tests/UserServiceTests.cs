using AutoMapper;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Shouldly;
using UserPortal.UserService.Business;
using UserPortal.UserService.Data;
using UserPortal.UserService.Models;

namespace UserPortal.UserService.Tests
{
  public class UserServiceTests
  {
    private readonly UserDbContext _context;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IPublishEndpoint> _mockPublishEndpoint;
    private IUserService _service;

    public UserServiceTests()
    {
      var options = new DbContextOptionsBuilder<UserDbContext>()
        .UseInMemoryDatabase(databaseName: "UserDatabase")
        .Options;

      var confBuilder = new ConfigurationBuilder()
        .AddEnvironmentVariables()
        .AddJsonFile("appsettings.json");


      _context = new UserDbContext(options, confBuilder.Build());

      _context.Users.Add(
        new Entities.User
        {
          Username = "Hakan",
          Password = "a@a.com",
          Email = "a@a.com",
          RegistrationStatus = Entities.RegistrationStatus.WaitingForApproval,
          Status = Entities.UserStatus.Enable
        });

      _context.SaveChanges();

      _mockMapper = new Mock<IMapper>();
      _mockPublishEndpoint = new Mock<IPublishEndpoint>();

      _service = new Business.UserService(_context, _mockMapper.Object, _mockPublishEndpoint.Object);

    }

    [Fact]
    public async Task Register_WhenUsernameExists_Failed()
    {
      // Arrange
      var registerModel = new RegisterModel
      {
        Username = "Hakan",
      };


      // Act
      var result = await _service.Register(registerModel);

      // Assert
      result.IsSucceed.ShouldBeFalse();
      result.ErrorMessage.ShouldBe("The username already exists.");
    }

    [Fact]
    public async Task Register_WhenEmailExists_Failed()
    {
      // Arrange
      var registerModel = new RegisterModel
      {
        Username = Guid.NewGuid().ToString(),
        Email = "a@a.com",
      };

      // Act
      var result = await _service.Register(registerModel);

      // Assert
      result.IsSucceed.ShouldBeFalse();
      result.ErrorMessage.ShouldBe("The email already exists.");
    }

  }
}
