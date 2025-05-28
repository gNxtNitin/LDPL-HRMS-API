using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobilePortalManagementLibrary.Models;
using DatabaseManager;
using Newtonsoft.Json;
using MobilePortalManagementLibrary.Interface;

namespace MobilePortalManagementLibrary.Implementation
{
    public class AdminManagementService : IAdminManagementService
    {
        public async Task<ResponseModel> GetData(string flag, string cid1)
        {
            ResponseModel responseModal = new ResponseModel();

            ArrayList arrList = new ArrayList();

            try
            {
                DALOR.spArgumentsCollection(arrList, "@p_flag", flag, "Char", "I", 1);

                DALOR.spArgumentsCollection(arrList, "p_Data1", cid1 ?? "string", "VARCHAR", "I");


                DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");

                DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O", 4000);
                DALOR.spArgumentsCollection(arrList, "g_ResultSet", "", "REFCURSOR", "O");
                DALOR.spArgumentsCollection(arrList, "g_ResultSet1", "", "REFCURSOR", "O");
                DALOR.spArgumentsCollection(arrList, "g_ResultSet2", "", "REFCURSOR", "O");
                DALOR.spArgumentsCollection(arrList, "g_ResultSet3", "", "REFCURSOR", "O");
                DALOR.spArgumentsCollection(arrList, "g_ResultSet4", "", "REFCURSOR", "O");
                //DALOR.spArgumentsCollection(arrList, "g_ResultSet", "", "REFCURSOR", "O");

                DataSet ds = new DataSet();
                var res = DALOR.RunStoredProcedureDsRetError("sp_GetData", arrList, ds);

                responseModal.code = res.Ret;
                responseModal.msg = res.ErrorMsg;

                if (res.Ret > 0 && ds != null && ds.Tables.Count > 0)
                {
                    //responseModal.data = JsonSerializer.Serialize(ds.Tables[0]);
                    if (flag == "U" || flag == "R")
                    {
                        List<string> l1 = new List<string>();
                        l1.Add(JsonConvert.SerializeObject(ds.Tables[0]));
                        l1.Add(JsonConvert.SerializeObject(ds.Tables[1]));
                        l1.Add(JsonConvert.SerializeObject(ds.Tables[2]));
                        l1.Add(JsonConvert.SerializeObject(ds.Tables[3]));
                        l1.Add(JsonConvert.SerializeObject(ds.Tables[4]));
                        responseModal.data = JsonConvert.SerializeObject(l1);
                    }
                    if (flag == "E")
                    {
                        List<string> l1 = new List<string>();
                        l1.Add(JsonConvert.SerializeObject(ds.Tables[0]));
                        l1.Add(JsonConvert.SerializeObject(ds.Tables[1]));
                        l1.Add(JsonConvert.SerializeObject(ds.Tables[2]));

                        responseModal.data = JsonConvert.SerializeObject(l1);
                    }
                    else if (flag == "I")
                    {
                        List<string> l1 = new List<string>();
                        l1.Add(JsonConvert.SerializeObject(ds.Tables[0]));
                        l1.Add(JsonConvert.SerializeObject(ds.Tables[1]));
                        l1.Add(JsonConvert.SerializeObject(ds.Tables[2]));
                        responseModal.data = JsonConvert.SerializeObject(l1);
                    }
                    else if (flag == "Z")
                    {
                        responseModal.data = JsonConvert.SerializeObject(ds.Tables[0]);
                    }

                }
                else
                {
                    responseModal.data = string.Empty;
                }
            }
            catch (Exception ex)
            {
                responseModal.code = -1;
                responseModal.msg = "Error Occurred, Could Not Get Details";
                responseModal.data = string.Empty;
            }

            return responseModal;
        }
        public async Task<ResponseModel> GetCreateUpdateDeleteZone(ZoneRequestModel zrm)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                //req.Password = await encDcService.Encrypt(req.Password);
                ArrayList arrList = new ArrayList();
                if (zrm.flag != "G")
                {

                    DALOR.spArgumentsCollection(arrList, "@p_flag", zrm.flag, "Char", "I", 1);
                    DALOR.spArgumentsCollection(arrList, "p_Sno", zrm.ZoneId ?? "0", "INT", "I");

                    DALOR.spArgumentsCollection(arrList, "p_Name", zrm.name, "VARCHAR", "I");


                    DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");
                    DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O");
                    DALOR.spArgumentsCollection(arrList, "g_ResultSet", "", "REFCURSOR", "O");
                    var res = DALOR.RunStoredProcedureDsRetError("sp_GetCreateSetDeleteZone", arrList);
                    response.code = res.Ret;
                    response.msg = res.ErrorMsg;
                }
                else
                {
                    DataSet ds = new DataSet();
                    DALOR.spArgumentsCollection(arrList, "@p_flag", zrm.flag, "Char", "I", 1);
                    DALOR.spArgumentsCollection(arrList, "p_Sno", zrm.ZoneId ?? "0", "INT", "I");

                    DALOR.spArgumentsCollection(arrList, "p_Name", zrm.name, "VARCHAR", "I");


                    DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");
                    DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O");
                    DALOR.spArgumentsCollection(arrList, "g_ResultSet", "", "REFCURSOR", "O");
                    var res = DALOR.RunStoredProcedureDsRetError("sp_GetCreateSetDeleteZone", arrList, ds);
                    response.code = res.Ret;
                    response.msg = res.ErrorMsg;

                    if (res.Ret > 0 && ds != null && ds.Tables.Count > 0)
                    {
                        //responseModal.data = JsonSerializer.Serialize(ds.Tables[0]);
                        response.data = JsonConvert.SerializeObject(ds.Tables[0]);
                    }
                    else
                    {
                        response.data = string.Empty;
                    }
                }

            }
            catch (Exception ex)
            {
                response.code = -1;
                response.data = ex.Message;
            }
            return await Task.FromResult(response);
        }
        public async Task<ResponseModel> GetCreateUpdateDeleteCity(CityRequestModel crm)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                //req.Password = await encDcService.Encrypt(req.Password);
                ArrayList arrList = new ArrayList();
                if (crm.flag != "G")
                {

                    DALOR.spArgumentsCollection(arrList, "@p_flag", crm.flag, "Char", "I", 1);
                    DALOR.spArgumentsCollection(arrList, "p_Sno", crm.CityId ?? "0", "INT", "I");

                    DALOR.spArgumentsCollection(arrList, "p_CName", crm.cname, "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_SName", crm.sname, "VARCHAR", "I");

                    DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");
                    DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O");
                    DALOR.spArgumentsCollection(arrList, "g_ResultSet", "", "REFCURSOR", "O");
                    var res = DALOR.RunStoredProcedureDsRetError("sp_GetCreateSetDeleteCity", arrList);
                    response.code = res.Ret;
                    response.msg = res.ErrorMsg;
                }
                else
                {
                    DataSet ds = new DataSet();
                    DALOR.spArgumentsCollection(arrList, "@p_flag", crm.flag, "Char", "I", 1);
                    DALOR.spArgumentsCollection(arrList, "p_Sno", crm.CityId ?? "0", "INT", "I");

                    DALOR.spArgumentsCollection(arrList, "p_CName", crm.cname, "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_SName", crm.sname, "VARCHAR", "I");


                    DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");
                    DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O");
                    DALOR.spArgumentsCollection(arrList, "g_ResultSet", "", "REFCURSOR", "O");
                    var res = DALOR.RunStoredProcedureDsRetError("sp_GetCreateSetDeleteCity", arrList, ds);
                    response.code = res.Ret;
                    response.msg = res.ErrorMsg;

                    if (res.Ret > 0 && ds != null && ds.Tables.Count > 0)
                    {
                        //responseModal.data = JsonSerializer.Serialize(ds.Tables[0]);
                        response.data = JsonConvert.SerializeObject(ds.Tables[0]);
                    }
                    else
                    {
                        response.data = string.Empty;
                    }
                }

            }
            catch (Exception ex)
            {
                response.code = -1;
                response.data = ex.Message;
            }
            return await Task.FromResult(response);
        }
        public async Task<ResponseModel> GetCreateUpdateDeleteSchool(SchoolRequestModel srm)
        {
            ResponseModel response = new ResponseModel();
            try
            {

                ArrayList arrList = new ArrayList();
                if (srm.flag != "G")
                {

                    DALOR.spArgumentsCollection(arrList, "@p_flag", srm.flag, "Char", "I", 1);

                    DALOR.spArgumentsCollection(arrList, "p_SCode", srm.SchoolCode ?? "", "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_SName", srm.SchoolName, "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_City", srm.City, "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_SAdd", srm.SAddress, "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_State", srm.State, "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_SCategory", srm.SchoolCategory, "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_Vendor", srm.VendorType, "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_EmpId", srm.EmpId, "VARCHAR", "I");



                    DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");
                    DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O");
                    DALOR.spArgumentsCollection(arrList, "g_ResultSet", "", "REFCURSOR", "O");
                    var res = DALOR.RunStoredProcedureDsRetError("sp_GetCreateSetDeleteSchool", arrList);
                    response.code = res.Ret;
                    response.msg = res.ErrorMsg;
                }
                else
                {
                    DataSet ds = new DataSet();
                    DALOR.spArgumentsCollection(arrList, "@p_flag", "G", "Char", "I", 1);

                    DALOR.spArgumentsCollection(arrList, "p_SCode", srm.SchoolCategory ?? "", "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_SName", srm.SchoolName, "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_City", srm.City, "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_SAdd", srm.SAddress, "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_State", srm.State, "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_SCategory", srm.SchoolCategory, "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_Vendor", srm.VendorType, "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_EmpId", srm.EmpId, "VARCHAR", "I");


                    DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");
                    DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O");
                    DALOR.spArgumentsCollection(arrList, "g_ResultSet", "", "REFCURSOR", "O");
                    var res = DALOR.RunStoredProcedureDsRetError("sp_GetCreateSetDeleteSchool", arrList, ds);
                    response.code = res.Ret;
                    response.msg = res.ErrorMsg;

                    if (res.Ret > 0 && ds != null && ds.Tables.Count > 0)
                    {
                        //responseModal.data = JsonSerializer.Serialize(ds.Tables[0]);
                        response.data = JsonConvert.SerializeObject(ds.Tables[0]);
                    }
                    else
                    {
                        response.data = string.Empty;
                    }
                }

            }
            catch (Exception ex)
            {
                response.code = -1;
                response.data = ex.Message;
            }
            return await Task.FromResult(response);
        }
        public async Task<ResponseModel> GetCreateUpdateDeleteEmployees(EmployeeRequestModel erm)
        {
            ResponseModel response = new ResponseModel();
            try
            {

                ArrayList arrList = new ArrayList();
                if (erm.flag != "G")
                {

                    DALOR.spArgumentsCollection(arrList, "@p_flag", erm.flag, "Char", "I", 1);
                    //DALOR.spArgumentsCollection(arrList, "p_Sno", erm.Sno ?? "", "VARCHAR", "I");
                    //DALOR.spArgumentsCollection(arrList, "p_Zone", erm.Zone, "NUMBER", "I");
                    //DALOR.spArgumentsCollection(arrList, "p_Role", erm.Role, "NUMBER", "I");
                    //DALOR.spArgumentsCollection(arrList, "p_NAME", erm.Name ?? "", "VARCHAR", "I");
                    //DALOR.spArgumentsCollection(arrList, "p_FName", erm.FName ?? "", "VARCHAR", "I");
                    //DALOR.spArgumentsCollection(arrList, "p_Mobile", erm.Mobile ?? "", "VARCHAR", "I");
                    //DALOR.spArgumentsCollection(arrList, "p_Designation", erm.Designation ?? "", "VARCHAR", "I");
                    //DALOR.spArgumentsCollection(arrList, "p_Gender", erm.Gender ?? "", "VARCHAR", "I");
                    //DALOR.spArgumentsCollection(arrList, "P_Address", erm.Address ?? "", "VARCHAR", "I");
                    //DALOR.spArgumentsCollection(arrList, "p_Email", erm.Email ?? "", "VARCHAR", "I");
                    //DALOR.spArgumentsCollection(arrList, "P_Password", erm.Password ?? "", "VARCHAR", "I");
                    //DALOR.spArgumentsCollection(arrList, "p_Manager", erm.Manager ?? "", "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_Sno", erm.EmpId ?? "", "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_Zone", erm.Zone != null ? erm.Zone.ToString() : "0", "INT", "I");
                    DALOR.spArgumentsCollection(arrList, "p_Role", erm.roleid, "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_NAME", erm.EName ?? "", "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_FName", erm.EFName ?? "", "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_Mobile", erm.Mobile ?? "", "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_Designation", erm.Designation ?? "", "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_Gender", erm.Gender ?? "", "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "P_Address", erm.Address ?? "", "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_Email", erm.Email ?? "", "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "P_Password", erm.Password ?? "", "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_Manager", erm.manager ?? "", "VARCHAR", "I");


                    DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");
                    DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O");
                    DALOR.spArgumentsCollection(arrList, "g_ResultSet", "", "REFCURSOR", "O");
                    var res = DALOR.RunStoredProcedureDsRetError("sp_GetCreateSetDeleteEmployees", arrList);
                    response.code = res.Ret;
                    response.msg = res.ErrorMsg;
                }
                else
                {
                    DataSet ds = new DataSet();
                    DALOR.spArgumentsCollection(arrList, "@p_flag", "G", "Char", "I", 1);

                    //DALOR.spArgumentsCollection(arrList, "p_Sno", erm.Sno ?? "", "VARCHAR", "I");
                    //DALOR.spArgumentsCollection(arrList, "p_Zone", erm.Zone, "NUMBER", "I");
                    //DALOR.spArgumentsCollection(arrList, "p_Role", erm.Role, "NUMBER", "I");
                    //DALOR.spArgumentsCollection(arrList, "p_NAME", erm.Name ?? "", "VARCHAR", "I");
                    //DALOR.spArgumentsCollection(arrList, "p_FName", erm.FName ?? "", "VARCHAR", "I");
                    //DALOR.spArgumentsCollection(arrList, "p_Mobile", erm.Mobile ?? "", "VARCHAR", "I");
                    //DALOR.spArgumentsCollection(arrList, "p_Designation", erm.Designation ?? "", "VARCHAR", "I");
                    //DALOR.spArgumentsCollection(arrList, "p_Gender", erm.Gender ?? "", "VARCHAR", "I");
                    //DALOR.spArgumentsCollection(arrList, "P_Address", erm.Address ?? "", "VARCHAR", "I");
                    //DALOR.spArgumentsCollection(arrList, "p_Email", erm.Email ?? "", "VARCHAR", "I");
                    //DALOR.spArgumentsCollection(arrList, "P_Password", erm.Password ?? "", "VARCHAR", "I");
                    //DALOR.spArgumentsCollection(arrList, "p_Manager", erm.Manager ?? "", "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_Sno", erm.EmpId ?? "", "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_Zone", "0", "INT", "I");
                    DALOR.spArgumentsCollection(arrList, "p_Role", erm.roleid, "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_NAME", erm.EName ?? "", "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_FName", erm.EFName ?? "", "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_Mobile", erm.Mobile ?? "", "VARCHAR", "I", 10);
                    DALOR.spArgumentsCollection(arrList, "p_Designation", erm.Designation ?? "", "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_Gender", erm.Gender ?? "", "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "P_Address", erm.Address ?? "", "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_Email", erm.Email ?? "", "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "P_Password", erm.Password ?? "", "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_Manager", erm.manager ?? "", "VARCHAR", "I");

                    DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");
                    DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O");
                    DALOR.spArgumentsCollection(arrList, "g_ResultSet", "", "REFCURSOR", "O");
                    var res = DALOR.RunStoredProcedureDsRetError("sp_GetCreateSetDeleteEmployees", arrList, ds);
                    response.code = res.Ret;
                    response.msg = res.ErrorMsg;

                    if (res.Ret > 0 && ds != null && ds.Tables.Count > 0)
                    {
                        //responseModal.data = JsonSerializer.Serialize(ds.Tables[0]);
                        response.data = JsonConvert.SerializeObject(ds.Tables[0]);
                    }
                    else
                    {
                        response.data = string.Empty;
                    }
                }

            }
            catch (Exception ex)
            {
                response.code = -1;
                response.data = ex.Message;
            }
            return await Task.FromResult(response);
        }
        public async Task<ResponseModel> AssignSchoolIncharge(SchoolRequestModel srm)
        {
            ResponseModel response = new ResponseModel();
            try
            {

                ArrayList arrList = new ArrayList();
                if (srm.flag != "G")
                {
                    DALOR.spArgumentsCollection(arrList, "@p_flag", srm.flag, "Char", "I", 1);
                    DALOR.spArgumentsCollection(arrList, "p_Empid", srm.Incharge, "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_City", string.IsNullOrEmpty(srm.City) ? "0" : srm.City, "INT", "I");
                    DALOR.spArgumentsCollection(arrList, "p_State", string.IsNullOrEmpty(srm.State) ? "0" : srm.State, "INT", "I");



                    DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");
                    DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O");
                    DALOR.spArgumentsCollection(arrList, "g_ResultSet", "", "REFCURSOR", "O");
                    var res = DALOR.RunStoredProcedureDsRetError("sp_AssignSchoolIncharge", arrList);
                    response.code = res.Ret;
                    response.msg = res.ErrorMsg;
                }
                else
                {
                    DataSet ds = new DataSet();
                    DALOR.spArgumentsCollection(arrList, "@p_flag", srm.flag ?? "G", "Char", "I", 1);
                    DALOR.spArgumentsCollection(arrList, "p_Empid", srm.EmpId ?? "str", "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_City", string.IsNullOrEmpty(srm.City) ? "0" : srm.City, "INT", "I");
                    DALOR.spArgumentsCollection(arrList, "p_State", string.IsNullOrEmpty(srm.State) ? "0" : srm.State, "INT", "I");




                    DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");
                    DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O");
                    DALOR.spArgumentsCollection(arrList, "g_ResultSet", "", "REFCURSOR", "O");
                    var res = DALOR.RunStoredProcedureDsRetError("sp_AssignSchoolIncharge", arrList, ds);
                    response.code = res.Ret;
                    response.msg = res.ErrorMsg;

                    if (res.Ret > 0 && ds != null && ds.Tables.Count > 0)
                    {
                        //responseModal.data = JsonSerializer.Serialize(ds.Tables[0]);
                        response.data = JsonConvert.SerializeObject(ds.Tables[0]);
                    }
                    else
                    {
                        response.data = string.Empty;
                    }
                }



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
