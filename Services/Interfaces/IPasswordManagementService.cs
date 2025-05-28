

using AuthLibrary.Models;
using ModelsLibrary.Models;
using PasswordManagementLibrary.Models;

namespace Services.Interfaces
{
    public interface IPasswordManagementService
    {
        public Task<ResponseModel> SendForgotEmail(AuthRequestModel lrm, string webHostUrl);
        public Task<ResponseModel> ResetPassword(AuthRequestModel auth);

        public Task<ResponseModel> ValidateResetPasswordToken(string token);

        public Task<ResponseModel> GetPasswordValidationRules();

        public Task<ResponseModel> UpdatePasswordPolicy(PasswordPolicyMaster PasswordPolicyMaster);

        public Task<ResponseModel> GetPasswordPolicy();
    }
}
