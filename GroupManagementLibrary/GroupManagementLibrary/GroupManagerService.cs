using DatabaseManager;
using GroupManagementLibrary.Interfaces;
using GroupManagementLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupManagementLibrary
{
    public class GroupManagerService : IGroupManagerService
    {
        MiscDataSetting _miscDataSetting = new MiscDataSetting();

        /// <summary>
        /// Create a group's information in the system by calling a stored procedure.
        /// </summary>
        public async Task<ResponseModel> CreateGroupMaster(GroupMasterReqModel rq)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                ArrayList arrList = new ArrayList();
                DAL.spArgumentsCollection(arrList, "@GroupName", rq.GroupName ?? "", "VARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@Description", rq.Description ?? "", "VARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@flag", "C", "CHAR", "I");
                DAL.spArgumentsCollection(arrList, "@Ret", "", "INT", "O");
                DAL.spArgumentsCollection(arrList, "@ErrorMsg", "", "VARCHAR", "O");
                var res = DAL.RunStoredProcedureRetError("sp_GetSetDeleteGroup", arrList);
                response.code = res.Ret;
                response.msg = res.ErrorMsg;
                response.data = JsonConvert.SerializeObject(res);
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.data = ex.Message;
            }
            return await Task.FromResult(response);
        }

        /// <summary>
        /// Updates a role's information in the system by calling a stored procedure.
        /// </summary>
        public async Task<ResponseModel> UpdateGroupMaster(GroupMasterReqModel req)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                ArrayList arrList = new ArrayList();
                DAL.spArgumentsCollection(arrList, "@GroupId", req.GroupId, "INT", "I");
                DAL.spArgumentsCollection(arrList, "@GroupName", req.GroupName ?? "", "VARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@Description", req.Description ?? "", "VARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@flag", "U", "CHAR", "I");
                DAL.spArgumentsCollection(arrList, "@Ret", "", "INT", "O");
                DAL.spArgumentsCollection(arrList, "@ErrorMsg", "", "VARCHAR", "O");
                var res = DAL.RunStoredProcedureRetError("sp_GetSetDeleteGroup", arrList);
                response.code = res.Ret;
                response.msg = res.ErrorMsg;
                response.data = JsonConvert.SerializeObject(res);
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.data = ex.Message;
            }
            return await Task.FromResult(response);
        }

        /// <summary>
        /// Retrieve Group's information from the database by calling a stored procedure.
        /// </summary>
        public async Task<ResponseModel> GetGroupMaster(string groupId)
        {
            ResponseModel response = new ResponseModel();
            string flag = groupId == null || groupId == "" ? "G" : "I";
            groupId = groupId == null || groupId == "" ? "0" : groupId.ToString();
            try
            {
                DataSet ds = new DataSet();
                ArrayList arrList = new ArrayList();
                DAL.spArgumentsCollection(arrList, "@Flag", flag, "CHAR", "I");
                DAL.spArgumentsCollection(arrList, "@GroupId", groupId, "INT", "I");
                DAL.spArgumentsCollection(arrList, "@Ret", "", "INT", "O");
                DAL.spArgumentsCollection(arrList, "@ErrorMsg", "", "VARCHAR", "O");
                ds = DAL.RunStoredProcedure(ds, "sp_GetSetDeleteGroup", arrList);

                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Groups";
                }
                string data = _miscDataSetting.ConvertToJSON(ds);
                response.msg = "Success";
                response.data = data;
                response.code = 200;
            }
            catch (Exception ex)
            {
                response.code = -3;
                response.msg = ex.Message;
            }
            return await Task.FromResult(response);
        }      

        /// <summary>
        /// Delete a group from the database by calling a stored procedure.
        /// </summary>
        public async Task<ResponseModel> DeleteGroupMaster(GroupMasterReqModel req)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (string.IsNullOrEmpty(req.GroupId))
                {
                    throw new ArgumentException("GroupId cannot be null or empty.");
                }

                ArrayList arrList = new ArrayList();
                DAL.spArgumentsCollection(arrList, "@Flag", "D", "CHAR", "I");
                DAL.spArgumentsCollection(arrList, "@groupId", req.GroupId, "INT", "I");
                DAL.spArgumentsCollection(arrList, "@Ret", "", "INT", "O");
                DAL.spArgumentsCollection(arrList, "@ErrorMsg", "", "VARCHAR", "O");
                var res = DAL.RunStoredProcedureRetError("sp_GetSetDeleteGroup", arrList);
                response.code = res.Ret;        
                response.msg = res.ErrorMsg;
                response.data = JsonConvert.SerializeObject(res);
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.data = ex.Message;
            }        
            return await Task.FromResult(response);
        }

        /// <summary>
        /// Assigned Group to User from the database by calling a stored procedure.
        /// </summary>
        public async Task<ResponseModel> AssignOrUpdateUsersGroup(AssignGroupReqModel rq)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                ArrayList arrList = new ArrayList();
                DAL.spArgumentsCollection(arrList, "@GroupId", rq.GroupId, "INT", "I");
                DAL.spArgumentsCollection(arrList, "@UserId", rq.UserId, "INT", "I");
                DAL.spArgumentsCollection(arrList, "@flag", "C", "CHAR", "I");
                DAL.spArgumentsCollection(arrList, "@Ret", "", "INT", "O");
                DAL.spArgumentsCollection(arrList, "@ErrorMsg", "", "VARCHAR", "O");
                var res = DAL.RunStoredProcedureRetError("sp_AssignGroupToUsers", arrList);
                response.code = res.Ret;
                response.msg = res.ErrorMsg;
                response.data = JsonConvert.SerializeObject(res);
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.data = ex.Message;
            }
            return await Task.FromResult(response);
        }

        /// <summary>
        /// Retrieves information of groups by UserId from the database by calling a stored procedure.
        /// </summary>
        public async Task<ResponseModel> GetGroupByUserId(AssignGroupReqModel rq)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (string.IsNullOrEmpty(rq.UserId))
                {
                    response.code = -3;
                    response.msg = "Valid user ID is required.";
                    return response;
                }

                DataSet ds = new DataSet();
                ArrayList arrList = new ArrayList();
                DAL.spArgumentsCollection(arrList, "@Flag", "I", "CHAR", "I");
                DAL.spArgumentsCollection(arrList, "@UserId", rq.UserId, "INT", "I");
                DAL.spArgumentsCollection(arrList, "@Ret", "", "INT", "O");
                DAL.spArgumentsCollection(arrList, "@ErrorMsg", "", "VARCHAR", "O");
                ds = DAL.RunStoredProcedure(ds, "sp_AssignGroupToUsers", arrList);

                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Groups";
                }
                string data = _miscDataSetting.ConvertToJSON(ds);
                response.msg = "Success";
                response.data = data;
                response.code = 200;
            }
            catch (Exception ex)
            {
                response.code = -3;
                response.msg = ex.Message;
            }
            return await Task.FromResult(response);
        }

        /// <summary>
        /// Assigned Group to Role from the database by calling a stored procedure.
        /// </summary>
        public async Task<ResponseModel> AssignOrUpdateRolesGroup(AssignGroupReqModel rq)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                ArrayList arrList = new ArrayList();
                DAL.spArgumentsCollection(arrList, "@GroupId", rq.GroupId, "INT", "I");
                DAL.spArgumentsCollection(arrList, "@RoleId", rq.RoleId, "INT", "I");
                DAL.spArgumentsCollection(arrList, "@flag", "C", "CHAR", "I");
                DAL.spArgumentsCollection(arrList, "@Ret", "", "INT", "O");
                DAL.spArgumentsCollection(arrList, "@ErrorMsg", "", "VARCHAR", "O");
                var res = DAL.RunStoredProcedureRetError("sp_AssignGroupToRoles", arrList);
                response.code = res.Ret;
                response.msg = res.ErrorMsg;
                response.data = JsonConvert.SerializeObject(res);
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.data = ex.Message;
            }
            return await Task.FromResult(response);
        }

        /// <summary>
        /// Retrieves information of groups by RoleId from the database by calling a stored procedure.
        /// </summary>
        public async Task<ResponseModel> GetGroupByRoleId(AssignGroupReqModel rq)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (string.IsNullOrEmpty(rq.RoleId))
                {
                    response.code = -3;
                    response.msg = "Valid role ID is required.";
                    return response;
                }

                DataSet ds = new DataSet();
                ArrayList arrList = new ArrayList();
                DAL.spArgumentsCollection(arrList, "@Flag", "I", "CHAR", "I");
                DAL.spArgumentsCollection(arrList, "@RoleId", rq.RoleId, "INT", "I");
                DAL.spArgumentsCollection(arrList, "@Ret", "", "INT", "O");
                DAL.spArgumentsCollection(arrList, "@ErrorMsg", "", "VARCHAR", "O");
                ds = DAL.RunStoredProcedure(ds, "sp_AssignGroupToRoles", arrList);

                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Groups";
                }
                string data = _miscDataSetting.ConvertToJSON(ds);
                response.msg = "Success";
                response.data = data;
                response.code = 200;
            }
            catch (Exception ex)
            {
                response.code = -3;
                response.msg = ex.Message;
            }
            return await Task.FromResult(response);
        }

    }
}
