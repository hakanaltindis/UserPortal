using Microsoft.EntityFrameworkCore;
using UserPortal.ManagementService.Entities;
using UserPortal.Shared;

namespace UserPortal.ManagementService.Data
{
  public class ManagementDbContext : DbContext
  {
    private readonly IConfiguration _configuration;

    public ManagementDbContext(DbContextOptions<ManagementDbContext> options
      , IConfiguration configuration)
      : base(options)
    {
      _configuration = configuration;
    }

#nullable disable
    public virtual DbSet<UserManagement> UserManagements { get; set; }
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
