using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailService.Library
{
    public enum EmailType
    {
        OTPEmail=1,
        ForgetPasswordResetUrlEmail,
        WelcomeOnRegisterEmail
    }
}
