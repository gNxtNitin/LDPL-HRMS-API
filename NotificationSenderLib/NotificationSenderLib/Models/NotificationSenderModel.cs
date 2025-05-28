using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationSenderLib.Models
{
    public class NotificationSenderModel
    {
        public int UserId { get; set; }
        public Guid? token { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Subject { get; set; }
        public int NotificationID { get; set; }
        public string Message { get; set; }
        public int IsSent { get; set; } // Sent or Failed.
        public DateTime SentDate { get; set; }
        public int isHTML { get; set; } = 1;
        public string NotificationType { get; set; } 
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? NCID { get; set; }
    }
}
