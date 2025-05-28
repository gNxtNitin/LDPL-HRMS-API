using DatabaseManager;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Web.Models.ViewModel;
using UserAccessControlLibrary.Interfaces;
using UserAccessControlLibrary.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace UserAccessControlLibrary
{
    public class UserAccessControlRepository: IUserAccessControlRepository
    {
        EncDcService encDcService = new EncDcService();
        MiscDataSetting _miscDataSetting = new MiscDataSetting();
        //public async Task<ResponseModel> CreateOrSetUser(UserMasterReqModel req, char flag)
        //{
        //    ResponseModel response = new ResponseModel();
        //    flag = 'C';
        //    try
        //    {
        //        req.Password = await encDcService.Encrypt(req.Password);

        //        ArrayList arrList = new ArrayList();
        //        DAL.spArgumentsCollection(arrList, "@FirstName", req.FirstName ?? "", "VARCHAR", "I");
        //        DAL.spArgumentsCollection(arrList, "@LastName", req.LastName ?? "", "VARCHAR", "I");
        //        DAL.spArgumentsCollection(arrList, "@DOB", req.DOB ?? "", "SMALLDATETIME", "I");
        //        //DAL.spArgumentsCollection(arrList, "@Address", req.Address ?? "", "VARCHAR", "I");
        //        DAL.spArgumentsCollection(arrList, "@Email", req.Email ?? "", "NVARCHAR", "I");
        //        DAL.spArgumentsCollection(arrList, "@Mobile", req.Mobile ?? "", "NVARCHAR", "I");
        //        DAL.spArgumentsCollection(arrList, "@Password", req.Password ?? "", "NVARCHAR", "I");
        //        DAL.spArgumentsCollection(arrList, "@flag", "C", "CHAR", "I");
        //        DAL.spArgumentsCollection(arrList, "@Ret", "", "INT", "O");
        //        DAL.spArgumentsCollection(arrList, "@ErrorMsg", "", "VARCHAR", "O");
        //        var res = DAL.RunStoredProcedureRetError("sp_GetSetDeleteUsers", arrList);
        //        response.code = res.Ret;
        //        response.msg = res.ErrorMsg;
        //        response.data = JsonConvert.SerializeObject(res);
        //    }
        //    catch (Exception ex)
        //    {
        //        response.code = -1;
        //        response.data = ex.Message;
        //    }
        //    return await Task.FromResult(response);

        //}
        //public async Task<ResponseModel> GetUserList(int? UserId, char flag)
        //{
        //    ResponseModel response = new ResponseModel();
        //    try
        //    {


        //        DataSet ds = new DataSet();
        //        ArrayList arrList = new ArrayList();
        //        DAL.spArgumentsCollection(arrList, "@userId", UserId.ToString(), "INT", "I");
        //        DAL.spArgumentsCollection(arrList, "@flag", flag.ToString(), "CHAR", "I");
        //        DAL.spArgumentsCollection(arrList, "@Ret", "", "INT", "O");
        //        DAL.spArgumentsCollection(arrList, "@ErrorMsg", "", "VARCHAR", "O");

        //        ds = DAL.RunStoredProcedure(ds, "sp_GetSetDeleteUsers", arrList);

        //        if (ds.Tables.Count > 0)
        //        {
        //            ds.Tables[0].TableName = "Users";
        //            response.code = 1;
        //            response.data = _miscDataSetting.ConvertToJSON(ds.Tables[0]);
        //            response.msg = "Success";
        //        }
        //        else
        //        {
        //            response.code = -2;
        //            response.msg = "No data found.";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response.code = -1;
        //        response.msg = ex.Message;
        //    }

        //    return await Task.FromResult(response);
        //}
        //public async Task<ResponseModel> DeleteUser(int UserId, char flag)
        //{
        //    ResponseModel response = new ResponseModel();

        //    try
        //    {
        //        ArrayList arrList = new ArrayList();
        //        DAL.spArgumentsCollection(arrList, "@userId", UserId.ToString(), "INT", "I");
        //        DAL.spArgumentsCollection(arrList, "@flag", flag.ToString(), "CHAR", "I");
        //        DAL.spArgumentsCollection(arrList, "@Ret", "", "INT", "O");
        //        DAL.spArgumentsCollection(arrList, "@ErrorMsg", "", "VARCHAR", "O");
        //        var res = DAL.RunStoredProcedureRetError("sp_GetSetDeleteUsers", arrList);
        //        response.code = res.Ret;
        //        response.msg = res.ErrorMsg;
        //        response.data = JsonConvert.SerializeObject(res);
        //    }
        //    catch (Exception ex)
        //    {
        //        response.code = -1;
        //        response.data = ex.Message;
        //    }
        //    return await Task.FromResult(response);
        //}

        public async Task<ResponseModel> GetMenuSubMenuAsync(int UserId, char flag)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                DataSet ds = new DataSet();
                ArrayList arrList = new ArrayList();
              //  DAL.spArgumentsCollection(arrList, "@userId", UserId.ToString(), "INT", "I");
                DAL.spArgumentsCollection(arrList, "@flag", flag.ToString(), "CHAR", "I");
                DAL.spArgumentsCollection(arrList, "@Ret", "", "INT", "O");
                DAL.spArgumentsCollection(arrList, "@ErrorMsg", "", "VARCHAR", "O");
                ds = DAL.RunStoredProcedure(ds, "sp_GetSetMenuAccess", arrList);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Menus";
                    response.code = 1;
                    response.data = _miscDataSetting.ConvertToJSON(ds.Tables[0]);
                    response.msg = "Success";
                }
                else
                {
                    response.code = -2;
                    response.msg = "No data found.";
                }
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.msg = ex.Message;
            }
            return await Task.FromResult(response);
        }
        public async Task<ResponseModel> UpdateMenuPermissionsAsync(MenuPermissionModel menu, char flag)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                DataSet ds = new DataSet();
                ArrayList arrList = new ArrayList();
                DAL.spArgumentsCollection(arrList, "@MenuId", menu.MenuId, "INT", "I");
                DAL.spArgumentsCollection(arrList, "@RoleId", menu.RoleId, "INT", "I");
                DAL.spArgumentsCollection(arrList, "@PermissionId", menu.PermissionId, "INT", "I");
                DAL.spArgumentsCollection(arrList, "@MenuPermissionId", menu.MenuPermissionId, "INT", "I");
                DAL.spArgumentsCollection(arrList, "@flag", flag.ToString(), "CHAR", "I");
                DAL.spArgumentsCollection(arrList, "@Ret", "", "INT", "O");
                DAL.spArgumentsCollection(arrList, "@ErrorMsg", "", "VARCHAR", "O");
                var res = DAL.RunStoredProcedureRetError("sp_GetSetMenuAccess", arrList);
                response.code = res.Ret;
                response.msg = res.ErrorMsg;
                response.data = JsonConvert.SerializeObject(res);
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.msg = ex.Message;
            }
            return await Task.FromResult(response);
        }


    

    }
}
