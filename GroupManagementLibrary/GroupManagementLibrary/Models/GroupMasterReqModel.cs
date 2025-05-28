using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupManagementLibrary.Models
{
    public class GroupMasterReqModel
    {
        public string GroupId { get; set; } 
        public string? GroupName { get; set; }
        public string? Description { get; set; }
        public int CreatedBy { get; set; } = 0;
    }
}
