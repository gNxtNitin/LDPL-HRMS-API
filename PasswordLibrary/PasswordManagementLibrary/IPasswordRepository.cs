using AuthLibrary.Models;
using ModelsLibrary.Models;
using PasswordManagementLibrary.Models;


namespace PasswordManagementLibrary
{
    public interface IPasswordRepository
    {

        public Task<ResponseModel> SendForgotPasswordEmail(AuthRequestModel req, string userName, string applicationName);
        public Task<ResponseModel> ResetPassword(AuthRequestModel req);

        public Task<ResponseModel> GeneratePasswordResetToken(AuthRequestModel req);

        public Task<ResponseModel> ValidateResetPasswordToken(string tokenHash);

        public Task<ResponseModel> GetPasswordPolicyValidationRules();

        public Task<ResponseModel> UpdatePasswordPolicy(PasswordPolicyMaster PasswordPolicyMaster);
        public Task<ResponseModel> GetPasswordPolicyMaster();


        //public Task QueueEmail(string toEmailIds, string subject, string body, string token, int isHTML = 1);
    }
}
