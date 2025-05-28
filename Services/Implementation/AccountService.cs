using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmailService.Library;

using EmailService.Library;
using ModelsLibrary.Models;
using NotificationSenderLib;
using NotificationSenderLib.Models;

//using ModelsLibrary.ResponseModels;
using Services.Interfaces;
using UserManagementLibrary.Interfaces;
using UserManagementLibrary.Models;

namespace Services.Implementation
{
    public class AccountService: IAccountService
    {
        private readonly IUserManagerService _userManagerService;
        private readonly IEmailSerivce _emailSerivce;
        private readonly INotificationSenderService _notificationSenderService;
        public AccountService(IUserManagerService userManagerService, IEmailSerivce emailSerivce, INotificationSenderService notificationSenderService)
        {
            _userManagerService = userManagerService;
            _emailSerivce = emailSerivce;
            _notificationSenderService = notificationSenderService;
            
        }
        public async Task<ResponseModel> CreateUserMaster(UserMasterReqModel umr)
        {
            ResponseModel responseModel = new ResponseModel();
            var response = await _userManagerService.CreateUserMaster(umr);
            if (response.code > 0)
            {
                VerificationReqModel req = new VerificationReqModel();
                req.MobileOrEmail = umr.Email;
                req.IsResendCode = 0;
                req.VerificationCode = "";
                response = await _userManagerService.SetOTP(req);

                EmailConfiguration emailconfigModel = new EmailConfiguration();
                //EmailConfiguration emailconfigModel = new EmailConfiguration();
                EmailDetails emailDetailModel = new EmailDetails();
                emailDetailModel.subject = "OTP Validation";
                emailDetailModel.ToEmailIds = umr.Email;
                emailDetailModel.body = string.Format(@"
    <html>
    <body>
        <p>Dear user,</p>
        
        <p>To complete your login to Smarterlead, please use the One-Time Password (OTP) below:</p>
        
        <p style=""font-weight: bold; font-size: 1.2em;"">Your OTP: <span style=""color: blue;"">{0}</span></p>
        
        <p>This OTP is valid for 5 minutes. Please enter it on the login page to securely access your account.</p>
        
        <p>If you did not request this login, please ignore this email or contact our support team immediately.</p>
        
        <p>Stay smart, stay secure!</p>
        
        <p>Best regards,<br>
        The Smarterlead Team</p>
    </body>
    </html>", responseModel.data);
                await _emailSerivce.QueueEmail(emailconfigModel, emailDetailModel);
            }
            return response;
        }
        public async Task<ResponseModel> UpdateUserMaster(UserMasterReqModel umr)
        {
            ResponseModel responseModel = new ResponseModel();
            var response = await _userManagerService.UpdateUserMaster(umr);
            if (response.code > 0)
            {
                NotificationSenderModel nsm = new NotificationSenderModel();
                nsm.UserId = response.code;
                nsm.Subject = "Profile Updated";
                nsm.Message = string.Format(@"
<html>
<body>
    <p>Dear user,</p>
    <p>Your Profile has been updated successfully.</p>
    <p>Stay smart, stay secure!</p>
    <p>Best regards,<br>Moduler Architect Team</p>
</body>
</html>");
                var pp = await _notificationSenderService.SendNotificationAsync(nsm);
            }
            return response;
        }
        public async Task<ResponseModel> DeleteUserMaster(UserMasterReqModel umr)
        {
            ResponseModel responseModel = new ResponseModel();
            var response = await _userManagerService.DeleteUserMaster(umr);
            if (response.code > 0)
            {
                NotificationSenderModel nsm = new NotificationSenderModel();
                nsm.UserId = response.code;
                nsm.Subject = "Profile Deactivated";
                nsm.Message = string.Format(@"
<html>
<body>
    <p>Dear user,</p>
    <p>Your Profile has been deactivated successfully.</p>
    <p>Stay smart, stay secure!</p>
    <p>Best regards,<br>Moduler Architect Team</p>
</body>
</html>");
                var pp = await _notificationSenderService.SendNotificationAsync(nsm);
            }
            return response;
        }
        public async Task<ResponseModel> GetUser(string userid)
        {
            ResponseModel responseModel = new ResponseModel();
            var response = await _userManagerService.GetUsers(userid);
            return response;
        }
        public async Task<ResponseModel> ResendOtp(VerificationReqModel lrm)
        {
            ResponseModel response = new ResponseModel();    
            response = await _userManagerService.SetOTP(lrm);
            EmailConfiguration emailconfigModel = new EmailConfiguration();
            //EmailConfiguration emailconfigModel = new EmailConfiguration();
            EmailDetails emailDetailModel = new EmailDetails();
            emailDetailModel.subject = "OTP Validation";
            emailDetailModel.ToEmailIds = lrm.MobileOrEmail;
            emailDetailModel.body = string.Format(@"
    <html>
    <body>
        <p>Dear user,</p>
        
        <p>To complete your login to Smarterlead, please use the One-Time Password (OTP) below:</p>
        
        <p style=""font-weight: bold; font-size: 1.2em;"">Your OTP: <span style=""color: blue;"">{0}</span></p>
        
        <p>This OTP is valid for 5 minutes. Please enter it on the login page to securely access your account.</p>
        
        <p>If you did not request this login, please ignore this email or contact our support team immediately.</p>
        
        <p>Stay smart, stay secure!</p>
        
        <p>Best regards,<br>
        The Smarterlead Team</p>
    </body>
    </html>", response.data);
            await _emailSerivce.QueueEmail(emailconfigModel, emailDetailModel);
            return response;
        }
    }
}
