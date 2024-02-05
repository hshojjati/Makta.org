using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities
{
    public class Province : BaseEntity
    {
        public int CountryId { get; set; }
        public string Name { get; set; }

        //navigation properties
        public virtual Country Country { get; set; }
    }

    public class ProvinceConfig : IEntityTypeConfiguration<Province>
    {
        public void Configure(EntityTypeBuilder<Province> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(64);
        }
    }
}