using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace UserPortal.Shared.Extensions
{
  public static class IApplicationBuilderExtensions
  {
    public static void ApplyMigration<TDbContext>(this IApplicationBuilder builder)
      where TDbContext : DbContext
    {
      using var scope = builder.ApplicationServices.CreateScope();

      var context = scope.ServiceProvider.GetRequiredService<TDbContext>();

      context.Database.Migrate();
    }
  }
}
