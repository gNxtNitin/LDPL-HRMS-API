using Microsoft.AspNetCore.Mvc.ModelBinding;
using MobilePortalManagementLibrary.Models;

namespace LDPLWEBAPI.Models
{
    public class DARequest
    {
        public string EmpId { get; set; }
        public decimal DA { get; set; }
        public decimal? Hotel { get; set; }
        public decimal? Other { get; set; }
        public float KM { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<IFormFile>? Bills { get; set; }
        public List<string>? Descriptions { get; set; }
    }
}
