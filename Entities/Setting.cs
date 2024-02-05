using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities
{
    public class Setting : AuditEntity
    {
        public string CommunityName { get; set; }
        public string CommunityEmail { get; set; }
        public string Domain { get; set; }
        public string Url => $"https://{Domain}";
        public string Twitter { get; set; }
        public string Linkedin { get; set; }
        public string Telegram { get; set; }
        public string GitHub { get; set; }
        public string Instagram { get; set; }
    }

    public class SettingConfig : IEntityTypeConfiguration<Setting>
    {
        public void Configure(EntityTypeBuilder<Setting> builder)
        {
        }
    }
}
