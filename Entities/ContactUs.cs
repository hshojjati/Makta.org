using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Entities
{
    public class ContactUs : BaseEntity
    {
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string SenderPhone { get; set; }
        public string ReceiverName { get; set; }
        public string Message { get; set; }
        public DateTime SentDate { get; set; }
    }

    public class ContactUsConfig : IEntityTypeConfiguration<ContactUs>
    {
        public void Configure(EntityTypeBuilder<ContactUs> builder)
        {
            builder.Property(p => p.SenderName).HasMaxLength(100);
            builder.Property(p => p.SenderEmail).HasMaxLength(100);
            builder.Property(p => p.SenderPhone).HasMaxLength(100);
            builder.Property(p => p.ReceiverName).HasMaxLength(100);
            builder.Property(p => p.Message).HasMaxLength(500);
        }
    }
}
