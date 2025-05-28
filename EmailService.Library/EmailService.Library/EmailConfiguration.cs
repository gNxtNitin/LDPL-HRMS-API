using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailService.Library
{
    public class EmailConfiguration
    {
        public Boolean ByPass { get; set; } = true;
        public string? SmtpServerAddress { get; set; } = "smtp.gmail.com";
        public int SmtpServerPort { get; set; } = 587;
        public string? SmtpServerUserId { get; set; } = "skullpatreon8@gmail.com";
        public string? SmtpServerPassword { get; set; } = "lwft ycee sftr vxjq";
        public string? EmailFromAddress { get; set; } = "skullpatreon8@gmail.com";
        public string? DisplayName { get; set; } = "Application";
    }
}
