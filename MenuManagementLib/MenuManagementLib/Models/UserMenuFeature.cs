using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuManagementLib.Models
{
    public class UserMenuFeature
    {
        public int userId { get; set; }
        public int menuId { get; set; }
        public int featureId { get; set; }
        public string featureCode { get; set; } = string.Empty;
    }
}
