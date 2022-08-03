using Microsoft.EntityFrameworkCore;
using UserPortal.UserService.Entities;

namespace UserPortal.UserService.Data
{
  public class UserDbContext : DbContext
  {

    public UserDbContext(DbContextOptions<UserDbContext> options)
      : base(options)
    { }

#nullable disable
    public virtual DbSet<User> Users { get; set; }
#nullable enable

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.HasDefaultSchema("user");

      modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserDbContext).Assembly);

      base.OnModelCreating(modelBuilder);
    }
  }
}
