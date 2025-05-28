using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Web.Models.ViewModel;
using UserAccessControlLibrary.Models;

namespace UserAccessControlLibrary.Interfaces
{
    public interface IUserControlService
    {
        //public Task<ResponseModel> CreateOrSetUser(UserMasterReqModel rq, char flag);
        //public Task<ResponseModel> GetUserList(int? UserId, char flag);
        
        //public Task<ResponseModel> DeleteUser(int UserId,char flag);

        public Task<ResponseModel> GetMenuSubMenuAsync(int UserId, char flag);
        public Task<ResponseModel> UpdateMenuPermissionsAsync(MenuPermissionModel menu, char flag);
    }
    
}
