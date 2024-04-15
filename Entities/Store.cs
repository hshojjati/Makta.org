using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities
{
    public class Store : BaseEntity
    {
        public string PartnerId { get; set; }
        public string StoreName { get; set; }
        public string StoreKey { get; set; }
        public bool IsActive { get; set; }
        public string OwnerId { get; set; }

        // Navigation Properties
        public virtual ApplicationUser Partner { get; set; }
        public virtual ApplicationUser Owner { get; set; }
    }

    public class StoreConfig : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> builder)
        {
            builder.Property(p => p.StoreName).HasMaxLength(100).IsRequired();
            builder.Property(p => p.StoreKey).HasMaxLength(15).IsRequired();
        }
    }
}
