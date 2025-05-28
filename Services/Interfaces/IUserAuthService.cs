using AuthLibrary.Models;
using ModelsLibrary.Models;

namespace Services.Interfaces
{
    public interface IUserAuthService
    {
        public Task<ResponseModel> AuthenticateUser(AuthRequestModel lrm);
        public Task<ResponseModel> ValidateOTP(AuthRequestModel lrm);
        
        Task<ResponseModel> UserInfo(string userId);


    }
}
