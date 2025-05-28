UserManagement Library :-


#Packages needs to install into your web project:
=>Microsoft.EntityFrameworkCore.SqlServer(6.0.0)
=>Microsoft.Extensions.DependencyInjection.Abstractions(6.0.0)
=>Newtonsoft.Json(13.0.3)
=>System.Data.SqlClient(4.8.6)
=>Microsoft.Data.SqlClient (5.2.2)


#Add services to the web in Program.cs :
=>builder.Services.AddScoped<IUserManagerService, UserManagerservice>();


#Add UserManagementLibrary's DLL into your web project
=>Create a folder named Reference in web. Add DLL into this folder.


#In controller:
=>private readonly IUserManagerservice _userManagerservice;


#Need to create a UserMasterReqModel req model for Create User into your web project;
Fields are:
        public int UserId { get; set; } = 0;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DOB { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Password { get; set; }
        public string FilePath{ get; set; }

& LoginReqModel req1 model for Set OTP ;
Fields are:
        public string? MobileOrEmail { get; set; }
        public string? VerificationCode { get; set; }
        public int? IsResendCode { get; set; } = 0;


#Now you need create a object of model, then assign the values to the model's fields.
Example:
	    UserMasterReqModel req = new UserMasterReqModel ();
            req.FirstName = "Demo";
            req.LastName = "Test";
            req.DOB = "2000-08-07";
            req.Email = "test@gmail.com";
            req.Mobile = "7060504030";
            req.Password = "t123";
            req.FilePath = "AdminProfile_pic.jpg";

           LoginReqModel req1 = new LoginReqModel ();
           req1.MobileOrEmail = "7060504030";
           req1.VerificationCode = "1111";


#Now you have to create a variable to store the result of Creating/Updating User,Set OTP,Get User and Delete User
Example:
	//Here, it returns code, data and message respectively.
	var result = await _userManagerService.CreateOrSetUser(req); // For Creating User with flag 'C'
        var result1 = await _userManagerService.UpdateUser(req); // For Updating User with flag 'U'
        var userResponse = await _userManagerService.GetUsers(userId); //For Getting List of All Users with flag 'G' Or Individully with flag 'I'
        var response = await _userManagerService.DeleteUserMaster(req); //For Delete User with flag 'D'

        var otpResponse = await _userManagerService.SetOTP(req1); //For Set OTP after Creation Or Updating User







	