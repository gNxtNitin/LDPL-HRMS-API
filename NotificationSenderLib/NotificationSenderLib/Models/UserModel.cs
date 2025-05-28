using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationSenderLib.Models
{
    public class UserResponse
    {
        public List<User> Users { get; set; }
    }

    public class User
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserType { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string DOB { get; set; }
        public string LastLoginDate { get; set; }
        public string UserStatus { get; set; }
    }

}
