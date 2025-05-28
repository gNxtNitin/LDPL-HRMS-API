using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorAndExceptionHandling.Library
{
    public class ErrorLogModel
    {
        public int Id { get; set; }
        public string? ExceptionMessage { get; set; }
        public string? StackTrace { get; set; }
        public string? LogLevel { get; set; }
        public string? Controller { get; set; }
        public string? Action { get; set; }
        public DateTime Timestamp { get; set; }
        public string? UserId { get; set; } // Optional
    }
}
