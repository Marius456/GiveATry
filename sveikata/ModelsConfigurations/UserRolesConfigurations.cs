using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using giveatry.Models;

namespace giveatry.ModelsConfigurations
{
    public class UserRolesConfigurations : IEntityTypeConfiguration<UserRoles>
    {
        public void Configure(EntityTypeBuilder<UserRoles> builder)
        {
            builder.HasKey(x => new { x.UserId, x.RoleName });
        }
    }
}
