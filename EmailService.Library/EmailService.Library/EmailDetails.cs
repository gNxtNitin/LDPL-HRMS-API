using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailService.Library
{
    public class EmailDetails
    {
        public string? ToEmailIds { get; set; } = "pratapvansh584@gmail.com";
        public string? subject { get; set; } = "Testing email";
        public string? body { get; set; } = "Please Focus on your work";
        public string? token { get; set; } = Guid.NewGuid().ToString();
        public bool hasAttachment { get; set; }
        public int isHTML { get; set; } = 1;
    }
}
