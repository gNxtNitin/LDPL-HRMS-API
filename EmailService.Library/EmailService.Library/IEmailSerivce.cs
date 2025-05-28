using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailService.Library
{
    public interface IEmailSerivce
    {
        public Task QueueEmail(EmailConfiguration emailConfiguration, EmailDetails emailDetails);

        //public Task<EmailTemplate> GetEmailTemplate(EmailType emailType);
    }
}
