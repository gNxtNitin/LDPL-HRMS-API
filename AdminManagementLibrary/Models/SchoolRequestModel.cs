using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MobilePortalManagementLibrary.Models
{
    public class SchoolRequestModel
    {
        public string? flag { get; set; } = "G";
        [JsonPropertyName("school_code")]
        public string? SchoolCode { get; set; }

        [JsonPropertyName("school_name")]
        public string? SchoolName { get; set; }

        [JsonPropertyName("ename")]
        public string? EName { get; set; } = string.Empty;

        [JsonPropertyName("empid")]
        public string? EmpId { get; set; } = string.Empty;

        [JsonPropertyName("saddress")]
        public string? SAddress { get; set; } = string.Empty;

        [JsonPropertyName("city")]
        public string? City { get; set; } = string.Empty;

        [JsonPropertyName("state")]
        public string? State { get; set; } = string.Empty;

        [JsonPropertyName("school_category")]
        public string? SchoolCategory { get; set; } = string.Empty;

        [JsonPropertyName("vendor_type")]
        public string? VendorType { get; set; } = string.Empty;

        [JsonPropertyName("account_manager")]
        public string? AccountManager { get; set; }

        [JsonPropertyName("incharge")]
        public string? Incharge { get; set; } = string.Empty;
    }
}
