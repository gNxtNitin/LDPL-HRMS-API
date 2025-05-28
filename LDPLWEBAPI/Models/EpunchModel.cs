using Microsoft.AspNetCore.Mvc.ModelBinding;
using MobilePortalManagementLibrary.Models;

namespace LDPLWEBAPI.Models
{
    public class EpunchModel
    {
        public string EmpID { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public float? KM { get; set; }
        public string SchoolId { get; set; }
        public string? Location { get; set; }
        public string? Address { get; set; }
        public IFormFile? EPhoto { get; set; }
    }
}
