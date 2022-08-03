using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserPortal.UserService.Entities;

namespace UserPortal.UserService.Data.EntityTypeConfigurations
{
  public class UserTypeBuilder : IEntityTypeConfiguration<User>
  {
    public void Configure(EntityTypeBuilder<User> builder)
    {
      builder.HasKey(e => e.Id);

      builder.Property(e => e.Id)
        .IsRequired();

      builder.Property(e => e.Username)
        .IsRequired();

      builder.Property(e => e.Password)
        .IsRequired();

      builder.Property(e => e.Email)
        .IsRequired();
    }
  }
}
