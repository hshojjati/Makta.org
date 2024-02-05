using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Entities
{
    public class ApplicationUser : IdentityUser, IBaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BusinessName { get; set; }
        public bool ChangedPassword { get; set; }
        public string Avatar { get; set; }
        public bool IsActive { get; set; }
        public int CountryId { get; set; }
        public int? ProvinceId { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public UserTitle? UserTitle { get; set; }
        public string City { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string UnitNo { get; set; }
        public string PostalCode { get; set; }
        public bool EmailBlast { get; set; }
        public DateTime InsertDateTime { get; set; }

        // Navigation Properties
        public virtual Country Country { get; set; }
        public virtual Province Province { get; set; }
    }

    public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(p => p.FirstName).HasMaxLength(64);
            builder.Property(p => p.LastName).HasMaxLength(64);
            builder.Property(p => p.PhoneNumber).HasMaxLength(16).IsRequired();
            builder.Property(p => p.Avatar).HasMaxLength(256);
            builder.Property(p => p.InsertDateTime).HasDefaultValueSql("GETUTCDATE()");
            builder.ToTable("User", "Security");
        }
    }
}