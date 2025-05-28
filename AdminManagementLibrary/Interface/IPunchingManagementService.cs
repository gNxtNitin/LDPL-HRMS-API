using MobilePortalManagementLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePortalManagementLibrary.Interface
{
    public interface IPunchingManagementService
    {
        Task<ResponseModel> GetPunchingReportData(ReportRequest reportRequest);
        Task<ResponseModel> AddEpunchRecord(EPunchRequestModel ePunchRequestModel);

        Task<ResponseModel> GetAssignedSchool(string empId);
    }
    
}
