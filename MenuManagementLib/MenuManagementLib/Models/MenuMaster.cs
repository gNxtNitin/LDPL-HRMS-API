using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuManagementLib.Models
{
    public class MenuMaster
    {
        public int MenuId { get; set; }
        public int? ParentId { get; set; }
        public string MenuName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string Url { get; set; }
        public int Order { get; set; }
        public string Area { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }

        public string? IconClass { get; set; } = "ti ti-face-id-error";
    }
}
