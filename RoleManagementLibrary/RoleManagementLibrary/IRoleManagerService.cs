using ModelsLibrary.Models;
using RoleManagementLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoleManagementLibrary
{
    public interface IRoleManagerService
    {
        // Creates a New Role
        public Task<ResponseModel> CreateRoleMaster(RoleMasterReqModel req);

        // Updates a Role's information 
        public Task<ResponseModel> UpdateRoleMaster(RoleMasterReqModel req);

        // Retrieves information of roles 
        public Task<ResponseModel> GetRoleMaster(string roleId);

        // Deletes a roles from the database
        public Task<ResponseModel> DeleteRoleMaster(RoleMasterReqModel req);

        // Assigning Role to User 
        public Task<ResponseModel> UpdateUsersRole(AssignRoleReqModel rq);

        // Retrieves information of roles by UserId
        public Task<ResponseModel> GetRoleByUserId(AssignRoleReqModel rq);

        public Task<ResponseModel> GetRolesWithUsers();
    }
}
