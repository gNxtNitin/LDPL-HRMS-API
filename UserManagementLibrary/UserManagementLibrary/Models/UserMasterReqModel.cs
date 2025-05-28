using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementLibrary.Models
{
    public class UserMasterReqModel
    {
        public string UserId { get; set; }

        //[Required(ErrorMessage = "First Name is Required.")]
        public string FirstName { get; set; }

        //[Required(ErrorMessage = "Last Name is Required.")]
        public string LastName { get; set; }

        //[Required(ErrorMessage = "Date of birth is Required.")]
        public string DOB { get; set; }

        //[Required(ErrorMessage = "Email Address is Required.")]
        public string Email { get; set; }

        //[Required(ErrorMessage = "Mobile Number is Required.")]
        //[RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be exactly 10 digits.")]
        public string Mobile { get; set; }

        //[Required(ErrorMessage = "Password is required.")]

        //[DataType(DataType.Password)]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",ErrorMessage = "Set a strong password.")]


        //ErrorMessage = "Password must be at least 8 characters:\n" +
        //    "- At least one uppercase letter\n" +
        //    "- At least one lowercase letter\n" +
        //    "- At least one number\n" +
        //    "- At least one special character.")]
        public string Password { get; set; }
        public string FilePath { get; set; }
    }
}
