using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelsLibrary.Models;
using RoleManagementLibrary.Models;

namespace Services.Interfaces
{
    public interface IRolesManagementService
    {
        Task<ResponseModel> CreateRoleMaster(RoleMasterReqModel rm);
        Task<ResponseModel> UpdateRoleMaster(RoleMasterReqModel rm);
        Task<ResponseModel> DeleteRoleMaster(RoleMasterReqModel rm);
        //Task<ResponseModel> GetRoleMaster(RoleMasterReqModel rm);
        Task<ResponseModel> GetRoleMaster(string roleId);
        Task<ResponseModel> UpdateUserRole(AssignRoleReqModel ar);
        Task<ResponseModel> GetRoleByUserId(AssignRoleReqModel ar);

        Task<ResponseModel> GetRoleWithUsers();
    }
}
