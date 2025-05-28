using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditAndLoggingLibrary.Models
{
    public class AuditMasterReqModel
    {
            public string AuditLogID { get; set; }
            public string UserID { get; set; }
            public string IPAddress { get; set; }
            public string Module { get; set; }
            public string Action { get; set; }
            public string ActionStatus { get; set; }
            public string SessionID { get; set; }
        
    }

}

