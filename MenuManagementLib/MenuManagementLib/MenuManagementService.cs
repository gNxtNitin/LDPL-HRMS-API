using AuthLibrary.Models;
using DatabaseManager;
using MenuManagementLib.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ModelsLibrary.Models;
using Newtonsoft.Json;
using System.Collections;
using System.Data;
//using System.Text.Json;


namespace MenuManagementLib
{
    public class MenuManagementService : IMenuManagementService
    {
        
        private readonly MiscDataSetting dataSetting = new MiscDataSetting();

        //Get all menus.
        public async Task<ResponseModel> GetMenuMaster()
        {
            ResponseModel responseModal = new ResponseModel();

            ArrayList arrList = new ArrayList();
            try
            {
                DAL.spArgumentsCollection(arrList, "@Flag", "G", "CHAR", "I");

                DAL.spArgumentsCollection(arrList, "@errorMsg", "", "VARCHAR", "O");
                DAL.spArgumentsCollection(arrList, "@ret", "", "INT", "O");


                DataSet ds = new DataSet();

                var res = DAL.RunStoredProcedureDsRetError("sp_GetSetMenu", arrList, ds);

                responseModal.code = res.Ret;
                responseModal.msg = res.ErrorMsg;

                if (res.Ret > 0 && ds != null && ds.Tables.Count > 0)
                {
                    responseModal.data = JsonConvert.SerializeObject(ds.Tables[0]);
                }
                else
                {
                    responseModal.data = string.Empty;
                }
            }
            catch (Exception ex)
            {
                responseModal.code = -1;
                responseModal.msg = "Error Occurred, Could Not Get Menu Details";
                responseModal.data = string.Empty;

            }

            return responseModal;
        }

        // Get menus based on menuId or userId.
        public async Task<ResponseModel> GetMenuByUserIdOrMenuId(int? UserId=0, int? menuId = 0)
        {
            ResponseModel responseModal = new ResponseModel();

            ArrayList arrList = new ArrayList();

            try
            {
                DALOR.spArgumentsCollection(arrList, "@p_flag", "I", "Char", "I", 1);
                DALOR.spArgumentsCollection(arrList, "@p_MenuId", menuId.ToString() ?? "0", "INT", "I");
                DALOR.spArgumentsCollection(arrList, "@p_UserId", (UserId ?? 0).ToString(), "INT", "I");

                

                DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");

                DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O", 4000);
                DALOR.spArgumentsCollection(arrList, "g_ResultSet", "", "REFCURSOR", "O");

                DataSet ds = new DataSet();
                var res = DALOR.RunStoredProcedureDsRetError("sp_GetSetMenu", arrList, ds);

                responseModal.code = res.Ret;
                responseModal.msg = res.ErrorMsg;

                if (res.Ret > 0 && ds != null && ds.Tables.Count > 0)
                {
                    //responseModal.data = JsonSerializer.Serialize(ds.Tables[0]);
                    responseModal.data = JsonConvert.SerializeObject(ds.Tables[0]);
                }
                else
                {
                    responseModal.data = string.Empty;
                }
            }
            catch (Exception ex)
            {
                responseModal.code = -1;
                responseModal.msg = "Error Occurred, Could Not Get Menu Details!";
                responseModal.data = string.Empty;
            }

            return responseModal;
        }

        //Create new menu.
        public async Task<ResponseModel> CreateNewMenu(MenuMaster menu)
        {
            ResponseModel response = new ResponseModel();
            ArrayList arrList = new ArrayList();
            try
            {
                DAL.spArgumentsCollection(arrList, "@Flag", "C", "CHAR", "I");
                DAL.spArgumentsCollection(arrList, "@MenuId", menu.MenuId.ToString(), "INT", "I");
                DAL.spArgumentsCollection(arrList, "@ParentId", menu.ParentId.ToString(), "INT", "I");
                DAL.spArgumentsCollection(arrList, "@MenuName", menu.MenuName.ToString(), "NVARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@Area", menu.Area.ToString(), "NVARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@ControllerName", menu.ControllerName.ToString(), "NVARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@ActionName", menu.ActionName.ToString(), "NVARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@Url", menu.Url.ToString(), "NVARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@Order", menu.Order.ToString(), "INT", "I");


                DAL.spArgumentsCollection(arrList, "@errorMsg", "", "VARCHAR", "O");
                DAL.spArgumentsCollection(arrList, "@ret", "", "INT", "O");

                var res = DAL.RunStoredProcedureRetError("sp_GetSetMenu", arrList);

                response.code = res.Ret;
                response.msg = res.ErrorMsg;
                response.data = string.Empty;
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.msg = "Error Occurred, Could Not Create Menu!";
                response.data = string.Empty;
            }

            return response;
        }

        //Update menu.
        public async Task<ResponseModel> UpdateMenu(MenuMaster menu)
        {
            ResponseModel response = new ResponseModel();

            ArrayList arrList = new ArrayList();
            try
            {
                DAL.spArgumentsCollection(arrList, "@Flag", "U", "CHAR", "I");
                DAL.spArgumentsCollection(arrList, "@MenuId", menu.MenuId.ToString(), "INT", "I");
                DAL.spArgumentsCollection(arrList, "@ParentId", menu.ParentId.ToString(), "INT", "I");
                DAL.spArgumentsCollection(arrList, "@MenuName", menu.MenuName.ToString(), "NVARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@Area", menu.Area.ToString(), "NVARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@ControllerName", menu.ControllerName.ToString(), "NVARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@ActionName", menu.ActionName.ToString(), "NVARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@Url", menu.Url.ToString(), "NVARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@Order", menu.Order.ToString(), "INT", "I");


                DAL.spArgumentsCollection(arrList, "@errorMsg", "", "VARCHAR", "O");
                DAL.spArgumentsCollection(arrList, "@ret", "", "INT", "O");

                var res = DAL.RunStoredProcedureRetError("sp_GetSetMenu", arrList);

                response.code = res.Ret;
                response.msg = res.ErrorMsg;
                response.data = string.Empty;
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.msg = "Error Occurred, Could Not Update Menu!";
                response.data = string.Empty;
            }

            return response;
        }

        //Menu deletion.
        public async Task<ResponseModel> DeleteMenu(int menuId)
        {
            ResponseModel response = new ResponseModel();
            ArrayList arrList = new ArrayList();
            try
            {
                DAL.spArgumentsCollection(arrList, "@Flag", "D", "CHAR", "I");
                DAL.spArgumentsCollection(arrList, "@MenuId", menuId.ToString(), "INT", "I");

                DAL.spArgumentsCollection(arrList, "@errorMsg", "", "VARCHAR", "O");
                DAL.spArgumentsCollection(arrList, "@ret", "", "INT", "O");

                var res = DAL.RunStoredProcedureRetError("sp_GetSetMenu", arrList);

                response.code = res.Ret;
                response.msg = res.ErrorMsg;
                response.data = string.Empty;
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.msg = "Error Occurred, Could Not Update Menu!";
                response.data = string.Empty;
            }

            return response;
        }

        public async Task<ResponseModel> GetMenuFeatureMaster()
        {
            ResponseModel response = new ResponseModel();

            try
            {
                ArrayList arrList = new ArrayList();
                DAL.spArgumentsCollection(arrList, "@Flag", "G", "CHAR", "I");
                DAL.spArgumentsCollection(arrList, "@errorMsg", "", "VARCHAR", "O");
                DAL.spArgumentsCollection(arrList, "@ret", "", "INT", "O");
                DataSet dataSet = new DataSet();
                DAL.RunStoredProcedure(dataSet, "sp_GetSetDeleteMenuFeatureMaster", arrList);

                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    response.code = 200;
                    response.msg = "OK";
                    response.data = JsonConvert.SerializeObject(dataSet.Tables[0]);
                }
                else
                {
                    response.code = -1;
                    response.msg = "Failed to retrieve data";
                    response.data = string.Empty;
                }

            }
            catch (Exception ex)
            {
                response.code = -1;
                response.msg = ex.Message;
                response.data = string.Empty;
            }

            return response;
        }

        public async Task<ResponseModel> GetMenuFeaturesByUserIdMenuId(int userId, int menuId)
        {
            ResponseModel response = new ResponseModel();

            try
            {
                ArrayList arrList = new ArrayList();
                DAL.spArgumentsCollection(arrList, "@Flag", "G", "CHAR", "I");
                DAL.spArgumentsCollection(arrList, "@UserId", userId.ToString(), "INT", "I");
                DAL.spArgumentsCollection(arrList, "@MenuId", menuId.ToString(), "CHAR", "I");
                DAL.spArgumentsCollection(arrList, "@errorMsg", "", "VARCHAR", "O");
                DAL.spArgumentsCollection(arrList, "@ret", "", "INT", "O");
                DataSet dataSet = new DataSet();
                DAL.RunStoredProcedure(dataSet, "sp_GetSetDeleteMenuFeatures", arrList);

                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    response.code = 200;
                    response.msg = "OK";
                    response.data = JsonConvert.SerializeObject(dataSet);
                }
                else
                {
                    response.code = -1;
                    response.msg = "Failed to retrieve data";
                    response.data = string.Empty;
                }


            }
            catch (Exception ex)
            {
                response.code = -1;
                response.msg = "Failed to retrieve data";
                response.data = string.Empty;
            }

            return response;
        }

        public async Task<ResponseModel> AddMenuFeatureMasterRecord(MenuFeatureMaster menuFeatureMaster)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                ArrayList arrList = new ArrayList();
                DAL.spArgumentsCollection(arrList, "@flag", "C", "CHAR", "I");
                DAL.spArgumentsCollection(arrList, "@FeatureName", menuFeatureMaster.FeatureName, "VARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@FeatureCode", menuFeatureMaster.FeatureCode, "VARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@FeatureDescription", menuFeatureMaster.FeatureDescription, "VARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@CreatedBy", menuFeatureMaster.CreatedBy.ToString(), "INT", "I");
                DAL.spArgumentsCollection(arrList, "@errorMsg", "", "VARCHAR", "O");
                DAL.spArgumentsCollection(arrList, "@ret", "", "INT", "O");

                var ds = DAL.RunStoredProcedureRetError("sp_GetSetDeleteMenuFeatureMaster", arrList);

                response.msg = ds.ErrorMsg;
                response.code = ds.Ret;
                response.data = string.Empty;
            }
            catch (Exception ex)
            {
                response.msg = ex.Message;
                response.code = -1;
                response.data = string.Empty;
            }

            return response;
        }

        public async Task<ResponseModel> DeleteMenuFeatureMasterRecord(int featureId)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                ArrayList arrList = new ArrayList();
                DAL.spArgumentsCollection(arrList, "@flag", "D", "CHAR", "I");
                DAL.spArgumentsCollection(arrList, "@FeatureId", featureId.ToString(), "VARCHAR", "I");

                DAL.spArgumentsCollection(arrList, "@errorMsg", "", "VARCHAR", "O");
                DAL.spArgumentsCollection(arrList, "@ret", "", "INT", "O");

                var ds = DAL.RunStoredProcedureRetError("sp_GetSetDeleteMenuFeatureMaster", arrList);

                response.msg = ds.ErrorMsg;
                response.code = ds.Ret;
                response.data = string.Empty;
            }
            catch (Exception ex)
            {
                response.msg = ex.Message;
                response.code = -1;
                response.data = string.Empty;
            }

            return response;
        }

        public async Task<ResponseModel> UpdateMenuFeatureMasterRecord(MenuFeatureMaster menuFeatureMaster)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                ArrayList arrList = new ArrayList();
                DAL.spArgumentsCollection(arrList, "@flag", "U", "CHAR", "I");
                DAL.spArgumentsCollection(arrList, "@FeatureId", menuFeatureMaster.FeatureId.ToString(), "INT", "I");
                DAL.spArgumentsCollection(arrList, "@FeatureName", menuFeatureMaster.FeatureName, "VARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@FeatureCode", menuFeatureMaster.FeatureCode, "VARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@FeatureDescription", menuFeatureMaster.FeatureDescription, "VARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@ModifiedBy", menuFeatureMaster.ModifiedBy.ToString(), "INT", "I");
                DAL.spArgumentsCollection(arrList, "@errorMsg", "", "VARCHAR", "O");
                DAL.spArgumentsCollection(arrList, "@ret", "", "INT", "O");

                var ds = DAL.RunStoredProcedureRetError("sp_GetSetDeleteMenuFeatureMaster", arrList);

                response.msg = ds.ErrorMsg;
                response.code = ds.Ret;
                response.data = string.Empty;
            }
            catch (Exception ex)
            {
                response.msg = ex.Message;
                response.code = -1;
                response.data = string.Empty;
            }

            return response;
        }

        public async Task<ResponseModel> AddMenuFeature(MenuFeatures menuFeatures)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                ArrayList arrList = new ArrayList();
                DAL.spArgumentsCollection(arrList, "@flag", "C", "CHAR", "I");
                DAL.spArgumentsCollection(arrList, "@MenuId", menuFeatures.MenuId.ToString(), "VARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@FeatureId", menuFeatures.FeatureId.ToString(), "VARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@CreatedBy", menuFeatures.CreatedBy.ToString(), "VARCHAR", "I");

                DAL.spArgumentsCollection(arrList, "@errorMsg", "", "VARCHAR", "O");
                DAL.spArgumentsCollection(arrList, "@ret", "", "INT", "O");

                var ds = DAL.RunStoredProcedureRetError("sp_GetSetDeleteMenuFeatures", arrList);

                response.msg = ds.ErrorMsg;
                response.code = ds.Ret;
                response.data = string.Empty;
            }
            catch (Exception ex)
            {
                response.msg = ex.Message;
                response.code = -1;
                response.data = string.Empty;
            }

            return response;
        }

        public async Task<ResponseModel> DeleteMenuFeature(MenuFeatures menuFeatures)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                ArrayList arrList = new ArrayList();
                DAL.spArgumentsCollection(arrList, "@flag", "D", "CHAR", "I");
                DAL.spArgumentsCollection(arrList, "@MenuId", menuFeatures.MenuId.ToString(), "INT", "I");
                DAL.spArgumentsCollection(arrList, "@FeatureId", menuFeatures.FeatureId.ToString(), "INT", "I");

                DAL.spArgumentsCollection(arrList, "@errorMsg", "", "VARCHAR", "O");
                DAL.spArgumentsCollection(arrList, "@ret", "", "INT", "O");

                var ds = DAL.RunStoredProcedureRetError("sp_GetSetDeleteMenuFeatures", arrList);

                response.msg = ds.ErrorMsg;
                response.code = ds.Ret;
                response.data = string.Empty;
            }
            catch (Exception ex)
            {
                response.msg = ex.Message;
                response.code = -1;
                response.data = string.Empty;
            }

            return response;
        }


        public async Task<ResponseModel> GetRoleMenuAccess(int roleId)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                ArrayList arrList = new ArrayList();
                DAL.spArgumentsCollection(arrList, "@Flag", "G", "CHAR", "I");
                DAL.spArgumentsCollection(arrList, "@RoleId", roleId.ToString(), "INT", "I");

                DAL.spArgumentsCollection(arrList, "@ErrorMsg", "", "VARCHAR", "O");
                DAL.spArgumentsCollection(arrList, "@Ret", "", "INT", "O");



                DataSet dataSet = new DataSet();

                DAL.RunStoredProcedure(dataSet, "sp_GetSetRoleMenuAccess", arrList);

                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    response.code = 200;
                    response.msg = "OK";
                    response.data = JsonConvert.SerializeObject(dataSet.Tables[0]);
                }
                else
                {
                    response.code = -1;
                    response.msg = "Failed to retrieve data";
                    response.data = string.Empty;
                }
            }
            catch (Exception ex)
            {
                response.msg = ex.Message;
                response.code = -1;
                response.data = string.Empty;
            }

            return response;
        }


        public async Task<ResponseModel> AddOrUpdateRoleMenuAccess(int roleId, int menuId)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                ArrayList arrList = new ArrayList();
                DAL.spArgumentsCollection(arrList, "@Flag", "S", "CHAR", "I");
                DAL.spArgumentsCollection(arrList, "@RoleId", roleId.ToString(), "INT", "I");
                DAL.spArgumentsCollection(arrList, "@MenuId", menuId.ToString(), "INT", "I");

                DAL.spArgumentsCollection(arrList, "@ErrorMsg", "", "VARCHAR", "O");
                DAL.spArgumentsCollection(arrList, "@Ret", "", "INT", "O");



                DataSet dataSet = new DataSet();

                var res = DAL.RunStoredProcedureRetError("sp_GetSetRoleMenuAccess", arrList);

                response.code = res.Ret;
                response.msg = res.ErrorMsg;
                response.data = string.Empty;

            }
            catch (Exception ex)
            {
                response.msg = ex.Message;
                response.code = -1;
                response.data = string.Empty;
            }

            return response;
        }
    }
}
