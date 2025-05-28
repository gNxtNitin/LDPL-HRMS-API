using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementLibrary.Models
{
    public class VerificationReqModel
    {
        public string? MobileOrEmail { get; set; }
        public string? VerificationCode { get; set; }
        public int? IsResendCode { get; set; } = 0;
    }
}
