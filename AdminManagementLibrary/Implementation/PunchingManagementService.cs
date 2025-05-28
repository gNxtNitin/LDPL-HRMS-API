using DatabaseManager;
using MobilePortalManagementLibrary.Interface;
using MobilePortalManagementLibrary.Models;
using Newtonsoft.Json;
using System.Collections;
using System.Data;
using System.Globalization;

namespace MobilePortalManagementLibrary.Implementation
{
    public class PunchingManagementService : IPunchingManagementService
    {
        public async Task<ResponseModel> AddEpunchRecord(EPunchRequestModel ePunchRequestModel)
        {
            ResponseModel responseModal = new ResponseModel();

            ArrayList arrList = new ArrayList();

            try
            {

                DALOR.spArgumentsCollection(arrList, "p_flag", "C", "CHAR", "I", 1);
                DALOR.spArgumentsCollection(arrList, "p_empId", ePunchRequestModel.EmpID, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_lattitude", ePunchRequestModel.Latitude.ToString(), "NUMBER", "I");
                DALOR.spArgumentsCollection(arrList, "p_longitude", ePunchRequestModel.Longitude.ToString(), "NUMBER", "I");
                DALOR.spArgumentsCollection(arrList, "p_ephoto", ePunchRequestModel.FileName, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_km", ePunchRequestModel.KM.ToString(), "NUMBER", "I");
                DALOR.spArgumentsCollection(arrList, "p_address", ePunchRequestModel.Address, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_location", ePunchRequestModel.Location, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_schoolId", ePunchRequestModel.SchoolId, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_fromDate", string.Empty, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_toDate", string.Empty, "VARCHAR", "I");
               
                DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");
                DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O");
                DALOR.spArgumentsCollection(arrList, "o_dailyepunch", "", "REFCURSOR", "O");

                var res = DALOR.RunStoredProcedureDsRetError("G_SP_GetSetDailyEPunch", arrList);

                responseModal.code = res.Ret;
                responseModal.msg = res.ErrorMsg;
                
            }

            catch (Exception ex)
            {
                responseModal.code = -1;
                responseModal.msg = "Failed to Save EPunch Data!";
                responseModal.data = string.Empty;
            }

            return responseModal;
        }

        public async Task<ResponseModel> GetPunchingReportData(ReportRequest reportRequest)
        {
            ResponseModel responseModal = new ResponseModel();

            ArrayList arrList = new ArrayList();
            DateTime today = DateTime.Today;
            DateTime monthStartDate = new DateTime(today.Year, today.Month, 1);

            reportRequest.DTRangeFrom = reportRequest.DTRangeFrom == DateTime.MinValue ? monthStartDate : reportRequest.DTRangeFrom;
            reportRequest.DTRangeTo = reportRequest.DTRangeTo == DateTime.MinValue ? today : reportRequest.DTRangeTo;

            try
            {

                DALOR.spArgumentsCollection(arrList, "p_userId", reportRequest.EmpId, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_isTeam", reportRequest.IsTeamData ? "1" : "0", "INT", "I");
                DALOR.spArgumentsCollection(arrList, "p_dt_from", reportRequest.DTRangeFrom.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture), "DATE", "I");
                DALOR.spArgumentsCollection(arrList, "p_dt_to", reportRequest.DTRangeTo.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture), "DATE", "I");

                DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");
                DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O");
                DALOR.spArgumentsCollection(arrList, "o_dailypunchingdata", "", "REFCURSOR", "O");



                DataSet ds = new DataSet();
                var res = DALOR.RunStoredProcedureDsRetError("G_SP_GetDailyPunchingReportByRole", arrList, ds);

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


        public async Task<ResponseModel> GetAssignedSchool(string empId)
        {
            ResponseModel responseModal = new ResponseModel();

            ArrayList arrList = new ArrayList();

            try
            {

                DALOR.spArgumentsCollection(arrList, "p_flag", "S", "CHAR", "I", 1);
                DALOR.spArgumentsCollection(arrList, "p_empId", empId, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_lattitude", "0", "NUMBER", "I");
                DALOR.spArgumentsCollection(arrList, "p_longitude", "0", "NUMBER", "I");
                DALOR.spArgumentsCollection(arrList, "p_ephoto", "", "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_km", "0", "NUMBER", "I");
                DALOR.spArgumentsCollection(arrList, "p_address", "", "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_location", "", "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_schoolId", "", "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_fromDate", string.Empty, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_toDate", string.Empty, "VARCHAR", "I");

                DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");
                DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O");
                DALOR.spArgumentsCollection(arrList, "o_dailyepunch", "", "REFCURSOR", "O");

                DataSet ds = new DataSet();
                var res = DALOR.RunStoredProcedureDsRetError("G_SP_GetSetDailyEPunch", arrList, ds);

                responseModal.code = res.Ret;
                responseModal.msg = res.ErrorMsg;
                if(res.Ret > 0 && ds != null && ds.Tables.Count > 0)
                {
                    responseModal.data = JsonConvert.SerializeObject(ds.Tables[0]);
                }

            }

            catch (Exception ex)
            {
                responseModal.code = -1;
                responseModal.msg = "Failed to Get School records!";
                responseModal.data = string.Empty;
            }

            return responseModal;
        }
    }


}
