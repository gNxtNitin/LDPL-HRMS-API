using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupManagementLibrary.Models
{
    public class AssignGroupReqModel
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string GroupId { get; set; }
    }
}
