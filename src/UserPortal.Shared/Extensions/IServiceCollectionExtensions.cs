using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace UserPortal.Shared.Extensions
{
  public static class IServiceCollectionExtensions
  {
    public static IServiceCollection RegisterDbContext<TDbContext>(this IServiceCollection services, IConfiguration configuration)
      where TDbContext : DbContext
    {
      var databaseProvider = configuration.GetSection(nameof(DatabaseProviderConfiguration)).Get<DatabaseProviderConfiguration>();

      var defaultConnection = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.Equals("Production") ?? false
        ? Environment.GetEnvironmentVariable("DefaultConnection")
        : configuration.GetConnectionString("DefaultConnection");

      ValidateEnvironmentVariable("DefaultConnection", defaultConnection);

      services.AddDbContext<TDbContext>(opt =>
      {
        opt.UseNpgsql(defaultConnection, sql =>
        {
          sql.MigrationsAssembly(typeof(TDbContext).Assembly.GetName().Name);
          sql.MigrationsHistoryTable("__EFMigrationsHistory", databaseProvider.DefaultSchema);
        });
      });

      return services;
    }

    public static IServiceCollection RegisterRabbitMQ(this IServiceCollection services, IConfiguration configuration)
    {
      string? host, user, password;

      if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
      {
        host = Environment.GetEnvironmentVariable("RabbitMqHost");
        user = Environment.GetEnvironmentVariable("RabbitMqUser");
        password = Environment.GetEnvironmentVariable("RabbitMqPassowrd");
      }
      else
      {
        host = configuration.GetValue<string?>("RabbitMq:Host", null);
        user = configuration.GetValue<string?>("RabbitMq:User", null);
        password = configuration.GetValue<string?>("RabbitMq:Password", null);
      }


      ValidateEnvironmentVariable("RabbitMqHost", host);

      ValidateEnvironmentVariable("RabbitMqUser", user);

      ValidateEnvironmentVariable("RabbitMqPassowrd", password);

      services.AddMassTransit(x =>
      {
        x.UsingRabbitMq((context, cfg) =>
        {
          cfg.Host(host, "/", h =>
          {
            h.Username(user);
            h.Password(password);
          });

          cfg.ConfigureEndpoints(context);
        });
      });

      services.AddOptions<MassTransitHostOptions>()
        .Configure(opt =>
        {
          opt.WaitUntilStarted = true;
          opt.StartTimeout = TimeSpan.FromSeconds(10);
          opt.StopTimeout = TimeSpan.FromSeconds(30);
        });

      return services;
    }

    private static void ValidateEnvironmentVariable(string nameOfVariable, string? valueOfVariable)
    {
      if (string.IsNullOrWhiteSpace(valueOfVariable))
      {
        throw new ArgumentNullException(nameOfVariable, $"The {nameOfVariable} must be passed in Environment Variables");
      }
    }

    public static IServiceCollection RegisterBussinessServices(this IServiceCollection services, Assembly serviceAssembly, Assembly implementationAssembly)
    {
      var typeOfIService = typeof(IBusinessService);

      var interfaces = serviceAssembly.GetTypes()
          .Where(t =>
              typeOfIService.IsAssignableFrom(t)
              && t.IsInterface
              && t != typeOfIService);

      foreach (var i in interfaces)
      {
        var typeOfImplementation = implementationAssembly.GetTypes().FirstOrDefault(t => i.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
        if (typeOfImplementation == null) continue;

        services.AddScoped(i, typeOfImplementation);
      }


      return services;
    }
  }
}
