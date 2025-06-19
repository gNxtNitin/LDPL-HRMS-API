using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePortalManagementLibrary.Models
{
    public class EPunchRequestModel
    {
        public string EmpID { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public string FileName { get; set; } = string.Empty;
        public float KM { get; set; }
        public string SchoolId { get; set; }
        public string Location { get; set; }
        public string Address { get; set; }
        public bool IsAddressMatched { get; set; }

    }
}
