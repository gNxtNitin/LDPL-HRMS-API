using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationSenderLib.Models
{
    public class NotificationCenterModel
    {
        public int NCID { get; set; }             
        public int UserID { get; set; }           
        public string Message { get; set; }        
        public int IsSent { get; set; }            
        public DateTime? DateOfSent { get; set; }  
        public DateTime CreatedDate { get; set; }  
        public int CreatedBy { get; set; }
        public string Token { get; set; }

    }

}
