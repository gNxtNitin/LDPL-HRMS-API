
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailService.Library
{
    public class EmailTemplate
    {
        public int TemplateId  {get;set; }
        public EmailType EmailTypeId {get;set; }
        public string TemplateName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsActive { get; set; }
    }
}
