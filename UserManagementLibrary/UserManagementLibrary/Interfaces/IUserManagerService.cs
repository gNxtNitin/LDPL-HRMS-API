using ModelsLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementLibrary.Models;

namespace UserManagementLibrary.Interfaces
{
    public interface IUserManagerService
    {
        //Create a new User
        public Task<ResponseModel> CreateUserMaster(UserMasterReqModel req);

        //Update the information of User
        public Task<ResponseModel> UpdateUserMaster(UserMasterReqModel req);

        //Set OTP after Creating Or Updating User
        public Task<ResponseModel> SetOTP(VerificationReqModel rq);

        //Get the List of Users
        public Task<ResponseModel> GetUsers(string userId);  
        
        //Delete the record of User
        public Task<ResponseModel> DeleteUserMaster(UserMasterReqModel req);

    }
}
