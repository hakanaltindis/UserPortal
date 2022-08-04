using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserPortal.ManagementService.Entities;

namespace UserPortal.ManagementService.Data.EntityTypeConfigurations
{
  public class UserManagementTypeBuilder : IEntityTypeConfiguration<UserManagement>
  {
    public void Configure(EntityTypeBuilder<UserManagement> builder)
    {
      builder.HasKey(e => e.Id);

      builder.Property(e => e.Id)
        .IsRequired();

      builder.Property(e => e.Username)
        .IsRequired();

      builder.Property(e => e.Email)
        .IsRequired();

      builder.Property(e => e.RegistrationStatus)
        .IsRequired();

      builder.Property(e => e.ProviderSource)
        .IsRequired(false);

      builder.Property(e => e.ProviderKey)
        .IsRequired(false);
    }
  }
}
