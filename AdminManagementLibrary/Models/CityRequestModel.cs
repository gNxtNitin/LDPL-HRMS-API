using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePortalManagementLibrary.Models
{
    public class CityRequestModel
    {
        public string? flag { get; set; } = "G";
        public string? cname { get; set; } = string.Empty;
        public string? sname { get; set; } = string.Empty;
        public string? CityId { get; set; }
    }
}
