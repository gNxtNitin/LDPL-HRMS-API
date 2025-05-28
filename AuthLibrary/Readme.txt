Packages needs to install into your web project:
=>Microsoft.EntityFrameworkCore.SqlServer(6.0.0)
=>Microsoft.Extensions.DependencyInjection.Abstractions(6.0.0)
=>Newtonsoft.Json(13.0.3)
=>System.Data.SqlClient(4.8.6)

Add services to the web:
=>builder.Services.AddScoped<IAuthService, AuthService>();
=>builder.Services.AddScoped<MiscDataSetting>();

Add jwt in appSetting.json file.
 "jwt": {
   "Issuer": "Issuer",
   "Audience": "Audience",
   "Key": "ThisIsAReallyLongSecretKeyThatIsAtLeast32Bytes!",
   "ExpiryMinutes": 10
 }


Add AuthLibrary's DLL into your web project
=>Create a folder named Reference in web. Add DLL into this folder.

In controller:
=>private readonly IAuthService _authService;

 
 public AccountController(IAuthService authService)
 {
     _authService = authService;
 }

Need to create a LoginReq model into your web project;
Fields are:
     public string? MobileOrEmail { get; set; }
     public bool IsLoginWithOtp { get; set; }
     public string? Password { get; set; }
     public string? VerificationCode { get; set; }
     public string UserId {  get; set; }
     public int? IsResendCode { get; set; } = 0;
     public bool IsJwtToken { get; set; }

 
Now you need create a object of LoginReq model, then assign the values to the model's fields.
Example:
	LoginReqModel req = new LoginReqModel();
 	req.MobileOrEmail = "budhdev@gmail.com";
 	req.Password = "Test@123";
 	req.IsJwtToken = true;
 	req.IsLoginWithOtp = true;

Now you have to create a variable to store the result of Authentication.
Example:
	//Here, it returns code, data and msg and having values userId, verification code and message respectively.
	var result = await _authService.AuthenticateUser(req);
	
	

 