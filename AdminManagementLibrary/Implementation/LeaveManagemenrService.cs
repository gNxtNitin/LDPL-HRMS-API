using DatabaseManager;
using MobilePortalManagementLibrary.Interface;
using MobilePortalManagementLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePortalManagementLibrary.Implementation
{
    public class LeaveManagemenrService:ILeaveManagementService
    {
        public async Task<ResponseModel> AddUpdateGetLeaveRecord(LeaveRequestModel lrm)
        {
            ResponseModel responseModal = new ResponseModel();

            ArrayList arrList = new ArrayList();

            try
            {
                if (lrm.flag != "G")
                {

                    DALOR.spArgumentsCollection(arrList, "p_flag", lrm.flag, "CHAR", "I", 1);
                    DALOR.spArgumentsCollection(arrList, "p_Id", lrm.Id.ToString(), "NUMBER", "I");
                    DALOR.spArgumentsCollection(arrList, "p_empId", lrm.EmpId, "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_FromDate", lrm.DTRangeFrom == null ? null : lrm.DTRangeFrom?.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture), "DATE", "I");
                    DALOR.spArgumentsCollection(arrList, "p_ToDate", lrm.DTRangeTo == null ? null : lrm.DTRangeTo?.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture), "DATE", "I");


                    DALOR.spArgumentsCollection(arrList, "p_Reason", lrm.LeaveReason, "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_IsHalfday", lrm.IsHalfDay == true ? "1" : "0", "NUMBER", "I");



                    DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");
                    DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O");
                    DALOR.spArgumentsCollection(arrList, "o_dailyepunch", "", "REFCURSOR", "O");

                    var res = DALOR.RunStoredProcedureDsRetError("G_SP_GetSetLeaveReportData", arrList);

                    responseModal.code = res.Ret;
                    responseModal.msg = res.ErrorMsg;
                }
                else
                {
                    DataSet ds = new DataSet();
                    DALOR.spArgumentsCollection(arrList, "p_flag", lrm.flag, "CHAR", "I",1);
                    DALOR.spArgumentsCollection(arrList, "p_Id", lrm.Id.ToString(), "NUMBER", "I");
                    DALOR.spArgumentsCollection(arrList, "p_empId", lrm.EmpId, "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_FromDate", string.Empty, "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_ToDate", string.Empty, "VARCHAR", "I");


                    DALOR.spArgumentsCollection(arrList, "p_Reason", lrm.LeaveReason, "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_IsHalfday", lrm.IsHalfDay == true ? "1" : "0", "NUMBER", "I");



                    DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");
                    DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O");
                    DALOR.spArgumentsCollection(arrList, "o_dailyepunch", "", "REFCURSOR", "O");

                    var res = DALOR.RunStoredProcedureDsRetError("G_SP_GetSetLeaveReportData", arrList, ds);

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

                

            }

            catch (Exception ex)
            {
                responseModal.code = -1;
                responseModal.msg = "Failed to Load Leave report request!";
                responseModal.data = string.Empty;
            }

            return responseModal;
        }
        public async Task<ResponseModel> AddUpdateGetTeamLeaveRecord(LeaveRequestModel lrm)
        {
            ResponseModel responseModal = new ResponseModel();

            ArrayList arrList = new ArrayList();

            try
            {
                if (lrm.flag != "G")
                {

                    DALOR.spArgumentsCollection(arrList, "p_flag", lrm.flag, "CHAR", "I", 1);
                    DALOR.spArgumentsCollection(arrList, "p_Id", lrm.Id.ToString(), "NUMBER", "I");
                    DALOR.spArgumentsCollection(arrList, "p_empId", lrm.EmpId, "VARCHAR", "I");

                    DALOR.spArgumentsCollection(arrList, "p_IsApproved", lrm.Status == true ? "1" : "0", "NUMBER", "I");



                    DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");
                    DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O");
                    DALOR.spArgumentsCollection(arrList, "o_dailyepunch", "", "REFCURSOR", "O");

                    var res = DALOR.RunStoredProcedureDsRetError("G_SP_GetSetTeamLeaveReportData", arrList);

                    responseModal.code = res.Ret;
                    responseModal.msg = res.ErrorMsg;
                }
                else
                {
                    DataSet ds = new DataSet();
                    DALOR.spArgumentsCollection(arrList, "p_flag", lrm.flag, "CHAR", "I", 1);
                    DALOR.spArgumentsCollection(arrList, "p_Id", lrm.Id.ToString(), "NUMBER", "I");
                    DALOR.spArgumentsCollection(arrList, "p_empId", lrm.EmpId, "VARCHAR", "I");
                    
                    DALOR.spArgumentsCollection(arrList, "p_IsApproved", lrm.Status == true ? "1" : "0", "NUMBER", "I");



                    DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");
                    DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O");
                    DALOR.spArgumentsCollection(arrList, "o_dailyepunch", "", "REFCURSOR", "O");

                    var res = DALOR.RunStoredProcedureDsRetError("G_SP_GetSetTeamLeaveReportData", arrList, ds);

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



            }

            catch (Exception ex)
            {
                responseModal.code = -1;
                responseModal.msg = "Failed to Load Leave report request!";
                responseModal.data = string.Empty;
            }

            return responseModal;
        }
    }
}
