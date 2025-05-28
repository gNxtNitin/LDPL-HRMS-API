using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAccessControlLibrary.Models
{
    public class RequestModel
    {
        public string? MobileOrEmail { get; set; } = null;
        public string? VerificationCode { get; set; }
        public string? DOB { get; set; }
        public int? IsResendCode { get; set; }
        public string? V { get; set; }
        public string? Password { get; set; }
    }
}
