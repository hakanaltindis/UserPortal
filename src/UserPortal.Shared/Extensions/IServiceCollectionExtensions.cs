using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UserPortal.Shared.Extensions
{
  public static class IServiceCollectionExtensions
  {
    public static IServiceCollection RegisterDbContext<TDbContext>(this IServiceCollection services, IConfiguration configuration)
      where TDbContext : DbContext
    {
      var databaseProvider = configuration.GetSection(nameof(DatabaseProviderConfiguration)).Get<DatabaseProviderConfiguration>();

      Console.WriteLine("ENVIRONMENT ======>>>>>>>>>> " + Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));

      var defaultConnection = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.Equals("Production") ?? false
        ? Environment.GetEnvironmentVariable("DefaultConnection")
        : configuration.GetConnectionString("DefaultConnection");

      Console.WriteLine("DEFAULT CONNECTION ==========>>>>>>>>>> " + defaultConnection);

      if (string.IsNullOrWhiteSpace(defaultConnection))
      {
        throw new ArgumentNullException(nameof(defaultConnection), $"The {nameof(defaultConnection)} must be passed as DefaultConnection in Environment Variables");
      }

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
  }
}
