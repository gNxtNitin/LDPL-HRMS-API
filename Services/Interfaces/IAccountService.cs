using ModelsLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagementLibrary.Models;

namespace Services.Interfaces
{
    public interface IAccountService
    {
        public Task<ResponseModel> CreateUserMaster(UserMasterReqModel umr);
        public Task<ResponseModel> UpdateUserMaster(UserMasterReqModel umr);
        public Task<ResponseModel> DeleteUserMaster(UserMasterReqModel umr);
        public Task<ResponseModel> GetUser(string userid);
    }
}
