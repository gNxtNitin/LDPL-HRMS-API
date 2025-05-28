using DatabaseManager;
using ModelsLibrary.Models;
using Newtonsoft.Json;
using RoleManagementLibrary.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RoleManagementLibrary
{
    public class RoleManagerService : IRoleManagerService
    {
        MiscDataSetting _miscDataSetting = new MiscDataSetting();

        /// <summary>
        /// Creates a role's information in the system by calling a stored procedure.
        /// </summary>
        public async Task<ResponseModel> CreateRoleMaster(RoleMasterReqModel req)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                ArrayList arrList = new ArrayList();
                DAL.spArgumentsCollection(arrList, "@RoleName", req.RoleName ?? "", "VARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@Description", req.Description ?? "", "VARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@flag", "C", "CHAR", "I");             
                DAL.spArgumentsCollection(arrList, "@Ret", "", "INT", "O");
                DAL.spArgumentsCollection(arrList, "@ErrorMsg", "", "VARCHAR", "O");
                var res = DAL.RunStoredProcedureRetError("sp_GetSetDeleteRole", arrList);
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
        public async Task<ResponseModel> UpdateRoleMaster(RoleMasterReqModel req)
        {
            ResponseModel response = new ResponseModel();
                try
                {
                    ArrayList arrList = new ArrayList();
                    DAL.spArgumentsCollection(arrList, "@RoleId", req.RoleId, "INT", "I");
                    DAL.spArgumentsCollection(arrList, "@RoleName", req.RoleName ?? "", "VARCHAR", "I");
                    DAL.spArgumentsCollection(arrList, "@Description", req.Description ?? "", "VARCHAR", "I");
                    DAL.spArgumentsCollection(arrList, "@flag", "U", "CHAR", "I");
                    DAL.spArgumentsCollection(arrList, "@Ret", "", "INT", "O");
                    DAL.spArgumentsCollection(arrList, "@ErrorMsg", "", "VARCHAR", "O");
                    var res = DAL.RunStoredProcedureRetError("sp_GetSetDeleteRole", arrList);
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
        /// Retrieves information of roles from the database by calling a stored procedure.
        /// </summary>
        public async Task<ResponseModel> GetRoleMaster(string roleId)
        {
            ResponseModel response = new ResponseModel();
            string flag = roleId == null || roleId == "" ? "G" : "I";
            roleId = roleId == null || roleId == "" ? "0" : roleId.ToString();
            try
            {
                DataSet ds = new DataSet();
                ArrayList arrList = new ArrayList();
                DAL.spArgumentsCollection(arrList, "@Flag", flag, "CHAR", "I");
                DAL.spArgumentsCollection(arrList, "@RoleId", roleId, "INT", "I");
                DAL.spArgumentsCollection(arrList, "@Ret", "", "INT", "O");
                DAL.spArgumentsCollection(arrList, "@ErrorMsg", "", "VARCHAR", "O");
                ds = DAL.RunStoredProcedure(ds, "sp_GetSetDeleteRole", arrList);

                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Roles";
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
        /// Deletes a roles from the database by calling a stored procedure.
        /// </summary>
        public async Task<ResponseModel> DeleteRoleMaster(RoleMasterReqModel req)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (string.IsNullOrEmpty(req.RoleId))
                {
                    throw new ArgumentException("RoleId cannot be null or empty.");
                }
                ArrayList arrList = new ArrayList();
                DAL.spArgumentsCollection(arrList, "@Flag", "D", "CHAR", "I");
                DAL.spArgumentsCollection(arrList, "@RoleId", req.RoleId, "INT", "I");
                DAL.spArgumentsCollection(arrList, "@Ret", "", "INT", "O");
                DAL.spArgumentsCollection(arrList, "@ErrorMsg", "", "VARCHAR", "O");
                var res = DAL.RunStoredProcedureRetError("sp_GetSetDeleteRole", arrList);
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
        /// Update Assigned Role to User from the database by calling a stored procedure.
        /// </summary>
        public async Task<ResponseModel> UpdateUsersRole(AssignRoleReqModel rq)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                ArrayList arrList = new ArrayList();
                DAL.spArgumentsCollection(arrList, "@RoleId", rq.RoleId, "INT", "I");
                DAL.spArgumentsCollection(arrList, "@UserId", rq.UserId, "INT", "I");
                DAL.spArgumentsCollection(arrList, "@flag", "C", "CHAR", "I");
                DAL.spArgumentsCollection(arrList, "@Ret", "", "INT", "O");
                DAL.spArgumentsCollection(arrList, "@ErrorMsg", "", "VARCHAR", "O");
                var res = DAL.RunStoredProcedureRetError("sp_AssignRole", arrList);
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
        /// Retrieves information of roles by UserID from the database by calling a stored procedure.
        /// </summary>
        public async Task<ResponseModel> GetRoleByUserId(AssignRoleReqModel rq)
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
                ds = DAL.RunStoredProcedure(ds, "sp_GetSetDeleteRole", arrList);

                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Roles";
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

        public async Task<ResponseModel> GetRolesWithUsers()
        {
            ResponseModel response = new ResponseModel();
            try
            {
                ArrayList arrList = new ArrayList();

                DataSet ds = new DataSet();
                //ArrayList arrList = new ArrayList();
                DAL.spArgumentsCollection(arrList, "@Flag", "A", "CHAR", "I");

                DAL.spArgumentsCollection(arrList, "@Ret", "", "INT", "O");
                DAL.spArgumentsCollection(arrList, "@ErrorMsg", "", "VARCHAR", "O");
                ds = DAL.RunStoredProcedure(ds, "sp_GetSetDeleteRole", arrList);


                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Roles";
             
                }
                string data = JsonConvert.SerializeObject(ds.Tables[0]);
                response.msg = "Success";
                response.data = data;
                response.code = 200;




            }
            catch (Exception ex)
            {
                response.code = -1;
                response.data = ex.Message;
            }
            return await Task.FromResult(response);
        }

    }
}
