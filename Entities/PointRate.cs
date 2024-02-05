using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Entities
{
    public class PointRate : BaseEntity
    {
        public int CurrencyId { get; set; }
        public decimal SpentAmount { get; set; }
        public double Points { get; set; }
        public DateTime InsertDateTime { get; set; }

        // Navigation Properties
        public virtual Currency Currency { get; set; }
    }

    public class PointRateConfig : IEntityTypeConfiguration<PointRate>
    {
        public void Configure(EntityTypeBuilder<PointRate> builder)
        {
            builder.Property(p => p.SpentAmount).HasColumnType("Decimal(18,2)").IsRequired();
            builder.Property(p => p.InsertDateTime).HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
