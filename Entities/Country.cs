using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities
{
    public class Country : BaseEntity
    {
        public string Title { get; set; }
        public string ShortTitle { get; set; }
        public string PhoneFormat { get; set; }
        public string PhoneCode { get; set; }
    }

    public class CountryConfig : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.Property(p => p.Title).HasMaxLength(50).IsRequired();
            builder.Property(p => p.ShortTitle).HasMaxLength(10).IsRequired();
            builder.Property(p => p.PhoneFormat).HasMaxLength(15).IsRequired();
        }
    }
}
