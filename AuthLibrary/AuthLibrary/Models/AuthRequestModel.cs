using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthLibrary.Models
{
    public class AuthRequestModel
    {
        public string? MobileOrEmail { get; set; }
        public bool IsLoginWithOtp { get; set; }
        public string? Password { get; set; }
        public int? CompanyCode { get; set; } = 0;
        public string? VerificationCode { get; set; }
      //  public string? ErrorMsg { get; set; }
      //  public int Ret {  get; set; }
        public string UserId {  get; set; }
      //  public string Role{  get; set; }
        //public string RoleId { get; set; }
        public int? IsResendCode { get; set; } = 0;

        public bool IsJwtToken { get; set; }
    }
}
