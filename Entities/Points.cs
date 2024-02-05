using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Entities
{
    public class Points : BaseEntity
    {
        public int StoreId { get; set; }
        public string ClientId { get; set; }
        public decimal SpentAmount { get; set; }
        public int CurrencyId { get; set; }
        public double EarnedPoints { get; set; }
        public int PointRateId { get; set; }
        public DateTime InsertDateTime { get; set; }
        public string Comments { get; set; }

        // Navigation Properties
        public virtual Currency Currency { get; set; }
    }

    public class PointConfig : IEntityTypeConfiguration<Points>
    {
        public void Configure(EntityTypeBuilder<Points> builder)
        {
            builder.Property(p => p.SpentAmount).HasColumnType("Decimal(18,2)").IsRequired();
            builder.Property(p => p.InsertDateTime).HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
