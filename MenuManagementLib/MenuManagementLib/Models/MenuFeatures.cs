using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuManagementLib.Models
{
    public class MenuFeatures
    {

        public int MenuId { get; set; }
        public int FeatureId { get; set; }
        public int CreatedBy { get; set; } = 0;
    }
}
