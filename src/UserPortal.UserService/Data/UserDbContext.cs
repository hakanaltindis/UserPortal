using Microsoft.EntityFrameworkCore;
using UserPortal.Shared;
using UserPortal.UserService.Entities;

namespace UserPortal.UserService.Data
{
  public class UserDbContext : DbContext
  {
    private readonly IConfiguration _configuration;

    public UserDbContext(DbContextOptions<UserDbContext> options
      , IConfiguration configuration)
      : base(options)
    {
      _configuration = configuration;
    }

#nullable disable
    public virtual DbSet<User> Users { get; set; }
#nullable enable

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      var dbConfiguration = _configuration.GetSection(nameof(DatabaseProviderConfiguration)).Get<DatabaseProviderConfiguration>();

      modelBuilder.HasDefaultSchema(dbConfiguration.DefaultSchema);

      modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

      base.OnModelCreating(modelBuilder);
    }
  }
}
