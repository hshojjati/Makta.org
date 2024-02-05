using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities
{
    public class RolePermision : BaseEntity
    {
        public string RoleNames { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }

    }

    public class RolePermisionConfig : IEntityTypeConfiguration<RolePermision>
    {
        public void Configure(EntityTypeBuilder<RolePermision> builder)
        {
            builder.Property(p => p.RoleNames).HasMaxLength(250);
            builder.Property(p => p.Controller).HasMaxLength(100);
            builder.Property(p => p.Action).HasMaxLength(100);
            builder.ToTable("RolePermisions", "Security");
        }
    }

}
