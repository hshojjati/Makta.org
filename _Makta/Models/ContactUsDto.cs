using Entities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WebFramework.Base;

namespace Makta.Models
{
    public class ContactUsDto : BaseDto<ContactUsDto, ContactUs>
    {
        [Required]
        [DisplayName("Name")]
        public string SenderName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email")]
        public string SenderEmail { get; set; }


        [Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        [DisplayName("Phone")]
        public string SenderPhone { get; set; }

        public string ReceiverName { get; set; }

        [Required]
        [DisplayName("Message")]
        [StringLength(500, MinimumLength = 6, ErrorMessage = "Message must be at least 6 characters")]
        public string Message { get; set; }

        public DateTime SentDate { get; set; }
    }
}