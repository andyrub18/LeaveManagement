using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
  public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
  {
    builder.HasData(
      new IdentityUserRole<string>
      {
        RoleId = "603f0c65-7799-4cfa-913e-a787c85b3ca2",
        UserId = "072efcfb-ae68-4654-bbbb-20947dfb1dee"
      },
      new IdentityUserRole<string>
      {
        RoleId = "aad99899-3e0d-463b-aa3a-817254210a92",
        UserId = "ff20c2ab-2aa9-4bf4-9c92-5abd5a1cc332"
      }
    );
  }
}