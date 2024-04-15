using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Entities
{
    public class Rate : BaseEntity
    {
        public int CurrencyId { get; set; }
        public decimal SpentAmount { get; set; }
        public double Points { get; set; }
        public DateTime InsertDateTime { get; set; }

        // Navigation Properties
        public virtual Currency Currency { get; set; }
    }

    public class RateConfig : IEntityTypeConfiguration<Rate>
    {
        public void Configure(EntityTypeBuilder<Rate> builder)
        {
            builder.Property(p => p.SpentAmount).HasColumnType("Decimal(18,2)").IsRequired();
            builder.Property(p => p.InsertDateTime).HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
