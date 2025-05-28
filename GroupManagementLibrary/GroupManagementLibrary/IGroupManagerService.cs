using GroupManagementLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupManagementLibrary.Interfaces
{
    public interface IGroupManagerService
    {
        //Creates a New Role
        public Task<ResponseModel> CreateGroupMaster(GroupMasterReqModel req);

        //Update Role's Information
        public Task<ResponseModel> UpdateGroupMaster(GroupMasterReqModel req);

        //Retrieve information of Group
        public Task<ResponseModel> GetGroupMaster(string? groupId);

        //Deletes a information of Group from Database
        public Task<ResponseModel> DeleteGroupMaster(GroupMasterReqModel req);

        //Assigning Or Updating Group to Users
        public Task<ResponseModel> AssignOrUpdateUsersGroup(AssignGroupReqModel rq);

        //Retrieves information of Groups by UserId
        public Task<ResponseModel> GetGroupByUserId(AssignGroupReqModel rq);

        //Assigning Or Updating Group to Roles
        public Task<ResponseModel> AssignOrUpdateRolesGroup(AssignGroupReqModel rq);

        //Retrieves information of Groups by RoleId
        public Task<ResponseModel> GetGroupByRoleId(AssignGroupReqModel rq);
    }
}
