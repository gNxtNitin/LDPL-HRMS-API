
using CustomerManagementLibrary;
using MobilePortalManagementLibrary.Implementation;
using MobilePortalManagementLibrary.Interface;
using MobilePortalManagementLibrary.Models;
using Services.Interfaces;

namespace Services.Implementation
{
    public class DAService : IDAService
    {
        private readonly IDAManagementService _iDAManagementService;
        public DAService(IDAManagementService iDAManagementService)
        {
            _iDAManagementService = iDAManagementService;
        }

        public async Task<ResponseModel> GetDAReportData(ReportRequest reportRequest)
        {
            ResponseModel resp = await _iDAManagementService.GetDAReportData(reportRequest);

            return resp;
        }

        public async Task<ResponseModel> GetKMVaueByDateRange(string userId, DateTime from, DateTime to)
        {
            ResponseModel resp = await _iDAManagementService.GetKMByDateRange(userId, from, to);

            return resp;
        }

        public async Task<ResponseModel> AddDARecord(DARequestModel dARequestModel)
        {
            ResponseModel resp = await _iDAManagementService.AddDARecord(dARequestModel);
            return resp;
        }

        public async Task<ResponseModel> ApproveRejectDA(DAApproveRejectReq dAApproveRejectReq)
        {
            ResponseModel resp = await _iDAManagementService.ApproveRejectDA(dAApproveRejectReq);

            return resp;
        }
    }
}