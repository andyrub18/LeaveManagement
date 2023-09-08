using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
  public void Configure(EntityTypeBuilder<IdentityRole> builder)
  {
    builder.HasData(
      new IdentityRole
      {
        Id = "aad99899-3e0d-463b-aa3a-817254210a92",
        Name = "Employee",
        NormalizedName = "EMPLOYEE"
      },
      new IdentityRole
      {
        Id = "603f0c65-7799-4cfa-913e-a787c85b3ca2",
        Name = "Administrator",
        NormalizedName = "ADMINISTRATOR"
      }
    );
  }
}