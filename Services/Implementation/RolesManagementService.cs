using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelsLibrary.Models;
using RoleManagementLibrary;
using RoleManagementLibrary.Models;
using Services.Interfaces;

namespace Services.Implementation
{
    public class RolesManagementService: IRolesManagementService
    {
        private readonly IRoleManagerService _roleManagerService;
        public RolesManagementService(IRoleManagerService roleManagerService)
        {
            _roleManagerService = roleManagerService;
        }
        public async Task<ResponseModel> CreateRoleMaster(RoleMasterReqModel rm)
        {
            ResponseModel responseModel = new ResponseModel();
            responseModel = await _roleManagerService.CreateRoleMaster(rm);
            return responseModel;
        }
        public async Task<ResponseModel> UpdateRoleMaster(RoleMasterReqModel rm)
        {
            ResponseModel responseModel = new ResponseModel();
            responseModel = await _roleManagerService.UpdateRoleMaster(rm);
            return responseModel;
        }
        public async Task<ResponseModel> DeleteRoleMaster(RoleMasterReqModel rm)
        {
            ResponseModel responseModel = new ResponseModel();
            responseModel = await _roleManagerService.DeleteRoleMaster(rm);
            return responseModel;
        }
        public async Task<ResponseModel> GetRoleMaster(string roleId)
        {
            ResponseModel responseModel = new ResponseModel();
            responseModel = await _roleManagerService.GetRoleMaster(roleId);
            return responseModel;
        }
        public async Task<ResponseModel> UpdateUserRole(AssignRoleReqModel ar)
        {
            ResponseModel responseModel = new ResponseModel();
            responseModel = await _roleManagerService.UpdateUsersRole(ar);
            return responseModel;
        }
        public async Task<ResponseModel> GetRoleByUserId(AssignRoleReqModel ar)
        {
            ResponseModel responseModel = new ResponseModel();
            responseModel = await _roleManagerService.GetRoleByUserId(ar);
            return responseModel;
        }

        public async Task<ResponseModel> GetRoleWithUsers()
        {
            ResponseModel responseModel = new ResponseModel();
            responseModel = await _roleManagerService.GetRolesWithUsers();
            return responseModel;
        }
    }
}
