using DatabaseManager;
using MobilePortalManagementLibrary.Interface;
using MobilePortalManagementLibrary.Models;
using Newtonsoft.Json;
using System.Collections;
using System.Data;

namespace MobilePortalManagementLibrary.Implementation
{
    public class VisitsManagementService : IVisitsManagementService
    {
        public async Task<ResponseModel> GetVisitsReportData(string userId, bool isTeamData = true)
        {
            ResponseModel responseModal = new ResponseModel();

            ArrayList arrList = new ArrayList();

            try
            {

                DALOR.spArgumentsCollection(arrList, "p_userId", userId, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_isTeam", isTeamData ? "1" : "0", "INT", "I");

                DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");
                DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O");
                DALOR.spArgumentsCollection(arrList, "o_dailyvisitdata", "", "REFCURSOR", "O");

                DataSet ds = new DataSet();
                var res = DALOR.RunStoredProcedureDsRetError("G_SP_GetDailyVisitReportByRole", arrList, ds);

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
    }
}
