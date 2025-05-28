using MobilePortalManagementLibrary.Models;

namespace Services.Interfaces
{
    public interface IDAService
    {
        public Task<ResponseModel> GetDAReportData(ReportRequest reportRequest);

        Task<ResponseModel> GetKMVaueByDateRange(string userId, DateTime from, DateTime to);

        Task<ResponseModel> AddDARecord(DARequestModel dARequestModel);
        Task<ResponseModel> ApproveRejectDA(DAApproveRejectReq dAApproveRejectReq);
    }
}
