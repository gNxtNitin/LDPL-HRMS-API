using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Web.Models.ViewModel;
using UserAccessControlLibrary.Interfaces;
using UserAccessControlLibrary.Models;

namespace UserAccessControlLibrary
{

    public class UserControlService : IUserControlService
    {
        private readonly IUserAccessControlRepository _userAccessControlRepository;

        public UserControlService(IUserAccessControlRepository userAccessControlRepository)
        {
            _userAccessControlRepository = userAccessControlRepository;
        }

        //public async Task<ResponseModel> CreateOrSetUser(UserMasterReqModel rq, char flag)
        //{
        //    var result = 0;
        //    ResponseModel response = new ResponseModel();

        //    try
        //    {
        //        response = await _userAccessControlRepository.CreateOrSetUser(rq, flag);
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return response;
        //}
        //public async Task<ResponseModel> GetUserList(int? UserId, char flag)
        //{
        //    ResponseModel respone = new ResponseModel();
        //    try
        //    {

        //        if (UserId != null && UserId!= 0)
        //        {
        //            respone = await _userAccessControlRepository.GetUserList(UserId, 'I');
        //        }
        //        else
        //        {
        //            respone = await _userAccessControlRepository.GetUserList(UserId, flag);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return respone;
        //}
        //public async Task<ResponseModel> DeleteUser(int UserId, char flag)
        //{
        //    ResponseModel respone = new ResponseModel();
        //    try
        //    {
        //            respone = await _userAccessControlRepository.DeleteUser(UserId, flag);
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return respone;
        //}
        public async Task<ResponseModel> GetMenuSubMenuAsync(int UserId, char flag)
        {
            ResponseModel respone = new ResponseModel();
            try
            {
                    respone = await _userAccessControlRepository.GetMenuSubMenuAsync(UserId, flag);
                
            }
            catch (Exception ex)
            {
            }
            return respone;
        }

        public async Task<ResponseModel> UpdateMenuPermissionsAsync(MenuPermissionModel menu, char flag)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                // Call the repository method to update permissions
                response = await _userAccessControlRepository.UpdateMenuPermissionsAsync(menu, flag);
            }
            catch (Exception ex)
            {
                
            }
            return response;
        }




    }

}

