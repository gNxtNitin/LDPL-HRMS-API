using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasswordManagementLibrary;
using Services.Interfaces;
using NotificationSenderLib.Models;
using NotificationSenderLib;
using System.Web;
using EmailService.Library;
using ModelsLibrary.Models;
using PasswordManagementLibrary.Models;
using AuthLibrary.Models;
//using UserManagementLibrary.Models;

namespace Services.Implementation
{
    public class PasswordManagementService: IPasswordManagementService
    {
        private IPasswordRepository _passwordRepository;
        private readonly INotificationSenderService _notificationSenderService;
        private readonly IEmailSerivce _emailSerivce;

        public PasswordManagementService(IPasswordRepository passwordRepository, INotificationSenderService notificationSenderService, IEmailSerivce emailSerivce)
        {
            _passwordRepository = passwordRepository;
            _notificationSenderService = notificationSenderService;
            _emailSerivce = emailSerivce;
        }

        
        public async Task<ResponseModel> SendForgotEmail(AuthRequestModel req, string webHostUrl)
        {

            ResponseModel response = await _passwordRepository.GeneratePasswordResetToken(req);
            bool isSent = false;
            if (response.code > 0)
            {
                await _emailSerivce.QueueEmail(new EmailConfiguration(), new EmailDetails()
                {
                    body = string.Format(@"<html>" +
                        "<body>" +
                        "<p>Dear user,</p>" +
                        "<p>We have received a request to reset your password. Please click the link below to reset your password.</p>" +
                        "<p><a href='{0}'>Reset Password</a></p>" +
                        "<p>If you did not request a password reset, please ignore this email.</p>" +
                        "<p>Stay smart, stay secure!</p>" +
                        "<p>Best regards,<br>Moduler Architect Team</p>" +
                        "</body>" +
                        "</html>", string.Concat(webHostUrl, "?authResetToken=", response.data.ToString())),

                    subject = "Password Reset",
                    ToEmailIds = response.msg

                });

                response.msg = "Reset Password Email Sent";
            }
           
            

            return response;

        }

        public async Task<ResponseModel> ResetPassword(AuthRequestModel authRequest)
        {
            ResponseModel responseModel = new ResponseModel();

            responseModel = await _passwordRepository.ResetPassword(authRequest);

            //if (responseModel.code > 0)
            //{
            //    NotificationSenderModel nsm = new NotificationSenderModel();
            //    nsm.UserId = responseModel.code;
            //    nsm.Subject = "Password Updated";
            //    nsm.Message = string.Format(@"
            //    <html>
            //    <body>
            //        <p>Dear user,</p>
            //        <p>Your Profile Password has been updated successfully.</p>
            //        <p>Stay smart, stay secure!</p>
            //        <p>Best regards,<br>Moduler Architect Team</p>
            //    </body>
            //    </html>");

            //    var pp = await _notificationSenderService.SendNotificationAsync(nsm);
            //}

            return responseModel;
        }

        public async Task<ResponseModel> ValidateResetPasswordToken(string token)
        {
            ResponseModel res = await _passwordRepository.ValidateResetPasswordToken(token);

            return res;
        }

        public async Task<ResponseModel> GetPasswordValidationRules()
        {
            ResponseModel res = await _passwordRepository.GetPasswordPolicyValidationRules();
            return res;
        }

        public async Task<ResponseModel> GetPasswordPolicy()
        {
            ResponseModel res = await _passwordRepository.GetPasswordPolicyMaster();

            return res;
        }
        public async Task<ResponseModel> UpdatePasswordPolicy(PasswordPolicyMaster PasswordPolicyMaster)
        {
            ResponseModel res = await _passwordRepository.UpdatePasswordPolicy(PasswordPolicyMaster);

            return res;
        }
    }
   
}
