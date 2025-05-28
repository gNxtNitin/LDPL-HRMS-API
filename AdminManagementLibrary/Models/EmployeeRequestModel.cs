using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MobilePortalManagementLibrary.Models
{
    public class EmployeeRequestModel
    {
        public string? flag { get; set; } = "G";
        [JsonProperty("roleid")]
        public string? roleid { get; set; }

        [JsonProperty("Empid")]
        public string? EmpId { get; set; }
        [JsonProperty("zone")]
        public string? Zone { get; set; }

        [JsonProperty("Ename")]
        public string? EName { get; set; }

        [JsonProperty("Email")]
        public string? Email { get; set; }

        [JsonProperty("Efname")]
        public string? EFName { get; set; }

        [JsonProperty("Password")]
        public string? Password { get; set; }

        [JsonProperty("Designation")]
        public string? Designation { get; set; }

        [JsonProperty("Department")]
        public string? Department { get; set; }

        [JsonProperty("Gender")]
        public string? Gender { get; set; }

        [JsonProperty("Mobile")]
        public string? Mobile { get; set; }

        [JsonProperty("Address")]
        public string? Address { get; set; }

        //[JsonProperty("ACCOUNT_MANAGER")]
        public string? manager { get; set; }
    }
}
