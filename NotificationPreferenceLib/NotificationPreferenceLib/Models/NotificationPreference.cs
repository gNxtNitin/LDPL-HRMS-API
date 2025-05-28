using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationPreferenceLib.Models
{
    public class NotificationPreference
    {
        public int NPID { get; set; }
        public string Preference { get; set; }  // Assuming this is mandatory, hence non-nullable
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }  // Nullable if user info might be missing
        public string? ModifiedBy { get; set; }  // Nullable if the preference is not yet modified
        public DateTime? ModifiedDate { get; set; }  // Nullable in case no modification has occurred
    }

    // Represents user-specific notification preferences
    public class UserNotificationPreference
    {
        public int UNPID { get; set; }  // Unique ID for the User Notification Preference
        public int NPID { get; set; }  // ID for the Notification Preference (foreign key)
        public int UserId { get; set; }  // ID of the user
        public int? CreatedBy { get; set; }  // Assuming this is always set
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }  // Nullable in case it hasn't been modified
        public DateTime? ModifiedDate { get; set; }  // Nullable if no modification has been made

        public NotificationPreference Preference { get; set; }  // Full preference details
    }
}
