using System.Collections.Generic;

namespace Common
{
    public class SiteSettings
    {
        public CommonSetting CommonSetting { get; set; }
        public List<EmailSetting> EmailSettingList { get; set; }
        public IdentitySetting IdentitySetting { get; set; }
        public SMSSetting SMSSetting { get; set; }
    }

    public class CommonSetting
    {
        public string ProtectionSecretKey { get; set; }
    }

    public class IdentitySetting
    {
        public bool PasswordRequireDigit { get; set; }
        public int PasswordRequiredLength { get; set; }
        public bool PasswordRequireNonAlphanumic { get; set; }
        public bool PasswordRequireUppercase { get; set; }
        public bool PasswordRequireLowercase { get; set; }
        public bool RequireUniqueEmail { get; set; }
    }

    public class EmailSetting
    {
        public string SettingName { get; set; }
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class SMSSetting
    {
        public string ClientNo { get; set; }
        public string APIPassword { get; set; }
        public string ClientIDHash { get; set; }
        public string LineNo { get; set; }
        public string Url { get; set; }
    }
}