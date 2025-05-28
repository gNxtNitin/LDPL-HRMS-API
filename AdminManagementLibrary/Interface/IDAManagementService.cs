using MobilePortalManagementLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePortalManagementLibrary.Interface
{
    public interface IDAManagementService
    {
        Task<ResponseModel> GetDAReportData(ReportRequest ReportRequest);

        Task<ResponseModel> AddDARecord(DARequestModel dARequestModel);

        Task<ResponseModel> GetKMByDateRange(string userId, DateTime fromDate, DateTime toDate);

        Task<ResponseModel> ApproveRejectDA(DAApproveRejectReq dAApproveRejectReq);
    }
}
