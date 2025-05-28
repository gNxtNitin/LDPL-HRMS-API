using DatabaseManager;
using MobilePortalManagementLibrary.Interface;
using MobilePortalManagementLibrary.Models;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Collections;
using System.Data;
using System.Globalization;

namespace MobilePortalManagementLibrary.Implementation
{
    public class DAManagementService : IDAManagementService
    {
        public async Task<ResponseModel> AddDARecord(DARequestModel dARequestModel)
        {
            ResponseModel responseModal = new ResponseModel();

            ArrayList arrList = new ArrayList();

            try
            {

                DALOR.spArgumentsCollection(arrList, "p_flag", "C", "CHAR", "I",1);
                DALOR.spArgumentsCollection(arrList, "p_daid", "0", "VARCHAR", "IO");  // IN OUT
                DALOR.spArgumentsCollection(arrList, "p_empid", dARequestModel.EmpId, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_daempId", "", "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_da", dARequestModel.DA.ToString(), "NUMBER", "I");
                DALOR.spArgumentsCollection(arrList, "p_hotel", dARequestModel.Hotel.ToString(), "NUMBER", "I");
                DALOR.spArgumentsCollection(arrList, "p_other", dARequestModel.Other.ToString(), "NUMBER", "I");
              
                DALOR.spArgumentsCollection(arrList, "p_km", dARequestModel.KM.ToString() ?? "0", "NUMBER", "I");
                DALOR.spArgumentsCollection(arrList, "p_fromDate", dARequestModel.FromDate.ToString("dd-MM-yyyy"), "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_toDate", dARequestModel.ToDate.ToString("dd-MM-yyyy"), "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_dastatus", "NO", "CHAR", "I");

                DALOR.spArgumentsCollection(arrList, "p_bill_file_names", dARequestModel.FilenNames, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_descriptions", dARequestModel.Descriptions, "VARCHAR", "I");

                DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");
                DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O");

                var res = DALOR.RunStoredProcedureDsRetError("G_SP_InsertOrUpdateDARecord", arrList);

                responseModal.code = res.Ret;
                responseModal.msg = res.ErrorMsg;
                responseModal.data = string.Empty;

            }
            catch (Exception ex)
            {
                responseModal.code = -1;
                responseModal.msg = "Failed to Add DA Record!";
                responseModal.data = string.Empty;
            }

            return responseModal;
        }

        public async Task<ResponseModel> GetDAReportData(ReportRequest reportRequest)
        {
            ResponseModel responseModal = new ResponseModel();

            ArrayList arrList = new ArrayList();
            DateTime today = DateTime.Today;
            DateTime monthStartDate = new DateTime(today.Year, today.Month, 1);

            reportRequest.DTRangeFrom = reportRequest.DTRangeFrom == DateTime.MinValue ? monthStartDate : reportRequest.DTRangeFrom;
            reportRequest.DTRangeTo = reportRequest.DTRangeTo == DateTime.MinValue ? today : reportRequest.DTRangeTo;

            try
            {

                DALOR.spArgumentsCollection(arrList, "p_empId", reportRequest.EmpId, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_isTeam", reportRequest.IsTeamData ? "1" : "0", "INT", "I");
                DALOR.spArgumentsCollection(arrList, "p_dt_from", reportRequest.DTRangeFrom.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) ,"DATE", "I");
                DALOR.spArgumentsCollection(arrList, "p_dt_to", reportRequest.DTRangeTo.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture), "DATE", "I");


                DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");
                DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O");
                DALOR.spArgumentsCollection(arrList, "o_dareportdata", "", "REFCURSOR", "O");



                DataSet ds = new DataSet();
                var res = DALOR.RunStoredProcedureDsRetError("G_SP_GetDAReportByRole", arrList, ds);

                responseModal.code = res.Ret;
                responseModal.msg = res.ErrorMsg;

                if (res.Ret > 0 && ds != null && ds.Tables.Count > 0)
                {
                    responseModal.data = JsonConvert.SerializeObject(ds.Tables[0]);
                }
            }
            catch (Exception ex)
            {
                responseModal.code = -1;
                responseModal.msg = "Failed to get DA report data!";
                responseModal.data = string.Empty;
            }

            return responseModal;
        }

        public async Task<ResponseModel> GetKMByDateRange(string userId, DateTime fromDate, DateTime toDate)
        {
            ResponseModel responseModal = new ResponseModel();
            ArrayList arrList = new ArrayList();

            try
            {
                DALOR.spArgumentsCollection(arrList, "p_flag", "F", "CHAR", "I", 1);
                DALOR.spArgumentsCollection(arrList, "p_empId", userId, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_lattitude", "0", "NUMBER", "I");
                DALOR.spArgumentsCollection(arrList, "p_longitude", "0", "NUMBER", "I");
                DALOR.spArgumentsCollection(arrList, "p_ephoto", "", "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_km", "0", "NUMBER", "I");
                DALOR.spArgumentsCollection(arrList, "p_address", "", "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_location", "", "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_schoolId", "", "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_fromDate", fromDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture), "DATE", "I");
                DALOR.spArgumentsCollection(arrList, "p_toDate", toDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture), "DATE", "I");

                DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");
                DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O");
                DALOR.spArgumentsCollection(arrList, "o_dailyepunch", "", "REFCURSOR", "O");

                DataSet ds = new DataSet();
                var res = DALOR.RunStoredProcedureDsRetError("G_SP_GetSetDailyEPunch", arrList, ds);
                responseModal.code = res.Ret;
                responseModal.msg = res.ErrorMsg;

                if (res.Ret > 0 && ds != null && ds.Tables.Count > 0)
                {
                    responseModal.data = ds.Tables[0].Rows[0]["TOTAL_KM"];
                }
            }
            catch (Exception ex)
            {
                responseModal.code = -1;
                responseModal.msg = "Failed to get KM value!";
                responseModal.data = "0";
            }

            return responseModal;
        }

        public async Task<ResponseModel> ApproveRejectDA(DAApproveRejectReq dAApproveRejectReq)
        {
            ResponseModel responseModal = new ResponseModel();

            ArrayList arrList = new ArrayList();

            try
            {

                DALOR.spArgumentsCollection(arrList, "p_flag", "U", "CHAR", "I", 1);
                DALOR.spArgumentsCollection(arrList, "p_daid", dAApproveRejectReq.DAID, "VARCHAR", "IO");
                DALOR.spArgumentsCollection(arrList, "p_empid", dAApproveRejectReq.ARBy, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_daempId", dAApproveRejectReq.DAEmpId, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_da", "0", "NUMBER", "I");
                DALOR.spArgumentsCollection(arrList, "p_hotel", "0", "NUMBER", "I");
                DALOR.spArgumentsCollection(arrList, "p_other", "0", "NUMBER", "I");

                DALOR.spArgumentsCollection(arrList, "p_km", "0", "NUMBER", "I");
                DALOR.spArgumentsCollection(arrList, "p_fromDate",DateTime.Now.ToString("dd-MM-yyyy"), "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_toDate", DateTime.Now.ToString("dd-MM-yyyy"), "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_dastatus", dAApproveRejectReq.IsApproved ? "YES":"NO", "CHAR", "I");

                DALOR.spArgumentsCollection(arrList, "p_bill_file_names", "", "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_descriptions", "", "VARCHAR", "I");

                DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");
                DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O");

                var res = DALOR.RunStoredProcedureDsRetError("G_SP_InsertOrUpdateDARecord", arrList);


                responseModal.code = res.Ret;
                responseModal.msg = res.ErrorMsg;
                responseModal.data = string.Empty;
            }
            catch (Exception ex)
            {
                responseModal.code = -1;
                responseModal.msg = $"Failed to {(dAApproveRejectReq.IsApproved ? "Approve" : "Reject")} DA!";
                responseModal.data = "0";
            }

            return responseModal;
        }




    }
}
