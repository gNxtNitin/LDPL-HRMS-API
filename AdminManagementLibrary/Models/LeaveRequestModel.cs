using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePortalManagementLibrary.Models
{
    public class LeaveRequestModel
    {
        public string flag { get; set; } = "G"; // Default value for flag
        public int? Id { get; set; } = 0; // Default value for Id
        public string EmpId { get; set; }
        public DateTime? DTRangeFrom { get; set; }
        public DateTime? DTRangeTo { get; set; }
        public string? LeaveReason { get; set; } = string.Empty; // Default value for LeaveType
        public bool? IsHalfDay { get; set; } = false; // Default value for IsHalfDay
        public bool? Status { get; set; } = false;

    }
}
