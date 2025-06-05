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
using System.Net.Mail;
using System.Text.RegularExpressions;
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
                string email = response.msg;

                if(string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email))
                {
                    response.code = -404;
                    response.msg = "Email address Not Found!";
                    response.data = null;
                    return response;
                }
                
                Regex EmailRegex = new Regex(@"^[A-Za-z0-9]+([._%+-]?[A-Za-z0-9]+)*@[A-Za-z0-9-]+(\.[A-Za-z0-9-]+)*\.[A-Za-z]{2,}$",
                                RegexOptions.Compiled | RegexOptions.IgnoreCase);
                bool isValid = EmailRegex.IsMatch(email);
                
                if(!isValid)
                {
                    response.code = -400;
                    response.msg = "Invalid email address";
                    response.data = null;

                    return response;
                }

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
                        "</html>", string.Concat(webHostUrl, "/auth/resetpassword?authResetToken=", response.data.ToString())),

                    subject = "Password Reset",
                    ToEmailIds = response.msg

                });


                response.data = new { token = response.data, toEmail = response.msg };
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
