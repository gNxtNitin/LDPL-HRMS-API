


namespace MobilePortalManagementLibrary.Models
{
    public class DARequestModel
    {
        
        public string EmpId { get; set; }      
        public decimal DA { get; set; }
        public decimal Hotel { get; set; }
        public decimal Other { get; set; }
        public float KM { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string FilenNames { get; set; }
        public string Descriptions { get; set; }

    }
}
