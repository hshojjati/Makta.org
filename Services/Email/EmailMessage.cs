using MimeKit;
using System.Collections.Generic;

namespace Services.Email
{
    public class EmailMessage
    {
        public MailboxAddress From { get; set; }
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public EmailMessage(IEnumerable<MailboxAddress> to, string subject, string content)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to);

            Subject = subject;

            Content = content;
        }
    }
}
