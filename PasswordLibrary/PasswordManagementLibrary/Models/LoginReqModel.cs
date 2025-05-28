using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManagementLibrary.Models
{
    public class LoginReqModel
    {
        public string UserId { get; set; } = string.Empty;
        public string? MobileOrEmail { get; set; }
        public Boolean IsLoginWithOtp { get; set; }
        public string? Password { get; set; }
        public string? VerificationCode { get; set; }
        public int? IsResendCode { get; set; } = 0;
    }
}
