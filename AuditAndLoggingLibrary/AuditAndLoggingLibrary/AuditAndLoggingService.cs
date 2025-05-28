using AuditAndLoggingLibrary.Models;
using Azure;
using DatabaseManager;
using ModelsLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditAndLoggingLibrary
{
    public class AuditAndLoggingService : IAuditAndLoggingService
    {
        MiscDataSetting _miscDataSetting = new MiscDataSetting();



        /// <summary>
        /// Maintain AuditLog of a User by calling a stored procedure.
        /// </summary>
        public async Task<ResponseModel> AuditLogAction(AuditMasterReqModel req)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                ArrayList arrList = new ArrayList();
                DAL.spArgumentsCollection(arrList, "@UserID", req.UserID, "INT", "I");
                DAL.spArgumentsCollection(arrList, "@IPAddress", req.IPAddress ?? "", "NVARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@Module", req.Module ?? "", "NVARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@Action", req.Action ?? "", "NVARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@ActionStatus", req.ActionStatus ?? "", "VARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@SessionID", req.SessionID ?? "", "VARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@flag", "C", "CHAR", "I");
                DAL.spArgumentsCollection(arrList, "@Ret", "", "INT", "O");
                DAL.spArgumentsCollection(arrList, "@ErrorMsg", "", "VARCHAR", "O");
                var res = DAL.RunStoredProcedureRetError("sp_GetSetDelete_UserAuditLogs", arrList);
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


    }
}
