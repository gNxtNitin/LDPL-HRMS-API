
using AuthLibrary.Interface;

using Services.Interfaces;
using EmailService.Library;
using NotificationSenderLib;
using NotificationSenderLib.Models;
using ModelsLibrary.Models;
using AuthLibrary.Models;


namespace Services.Implementation
{
    public class UserAuthService : IUserAuthService
    {
        private readonly IAuthService _authService;
        private IEmailSerivce _emailSerivce;
        private readonly INotificationSenderService _notificationSenderService;
        //private readonly IUserManagerService _userManagerService;
        
        public UserAuthService(IAuthService authService,IEmailSerivce emailSerivce, INotificationSenderService notificationSenderService)
        {
            _authService = authService;
            _emailSerivce = emailSerivce;
            _notificationSenderService = notificationSenderService;
        }

        public async Task<ResponseModel> AuthenticateUser(AuthRequestModel lrm)
        {
            var resp = await _authService.AuthenticateUser(lrm);
            //            ResponseModel responseModel = new ResponseModel();
            //            if (lrm.IsResendCode > 0)
            //            {
            //                responseModel = await _authService.SetOTP(lrm);
            //                EmailConfiguration emailconfigModel = new EmailConfiguration();
            //                EmailDetails emailDetailModel = new EmailDetails();
            //                emailDetailModel.subject = "OTP Validation";
            //                emailDetailModel.ToEmailIds = lrm.MobileOrEmail;
            //                emailDetailModel.body = string.Format(@"
            //    <html>
            //    <body>
            //        <p>Dear user,</p>

            //        <p>To complete your login to Smarterlead, please use the One-Time Password (OTP) below:</p>

            //        <p style=""font-weight: bold; font-size: 1.2em;"">Your OTP: <span style=""color: blue;"">{0}</span></p>

            //        <p>This OTP is valid for 5 minutes. Please enter it on the login page to securely access your account.</p>

            //        <p>If you did not request this login, please ignore this email or contact our support team immediately.</p>

            //        <p>Stay smart, stay secure!</p>

            //        <p>Best regards,<br>
            //        Moduler Architect Team</p>
            //    </body>
            //    </html>", responseModel.data);
            //                await _emailSerivce.QueueEmail(emailconfigModel,emailDetailModel);
            //                return responseModel;
            //            }
            //            responseModel = await _authService.AuthenticateUser(lrm);
            //            if(responseModel.code > 0)
            //            {
            //                NotificationSenderModel nsm = new NotificationSenderModel();
            //                nsm.UserId = responseModel.code;
            //                nsm.Subject = "Login Attempt";
            //                nsm.Message = string.Format(@"
            //<html>
            //<body>
            //    <p>Dear user,</p>
            //    <p>Your Login Attempt was Success.</p>
            //    <p>Stay smart, stay secure!</p>
            //    <p>Best regards,<br>Moduler Architect Team</p>
            //</body>
            //</html>");
            //                var pp = await _notificationSenderService.SendNotificationAsync(nsm);
            //            }

            //return responseModel;
            return resp;
        }

        public async Task<ResponseModel> ValidateOTP(AuthRequestModel lrm)
        {
            ResponseModel responseModel = new ResponseModel();
            var response = await _authService.ValidateOTP(lrm);
            //if (responseModel.code > 0)
            //{
            //    NotificationSenderModel nsm = new NotificationSenderModel();
            //    nsm.UserId = responseModel.code;
            //    nsm.Subject = "Login Attempt";
            //    nsm.Message = string.Format(@"
            //    <html>
            //    <body>
            //        <p>Dear user,</p>
            //        <p>Your Login Attempt was Success.</p>
            //        <p>Stay smart, stay secure!</p>
            //        <p>Best regards,<br>Moduler Architect Team</p>
            //    </body>
            //    </html>");
            //    var pp = await _notificationSenderService.SendNotificationAsync(nsm);
            //}
            return response;
        }
       
        public async Task<ResponseModel> UserInfo(string uId)
        {
            ResponseModel responseModel = new ResponseModel();
            var response = await _authService.UserDetail(uId);
            return response;
        }



    }
}
