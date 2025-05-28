
using CustomerManagementLibrary;
using MobilePortalManagementLibrary.Implementation;
using MobilePortalManagementLibrary.Interface;
using MobilePortalManagementLibrary.Models;
using Services.Interfaces;

namespace Services.Implementation
{
    public class PunchingService : IPunchingService
    {
        private readonly IPunchingManagementService _punchingManagementService;
        public PunchingService(IPunchingManagementService punchingManagementService)
        {
            _punchingManagementService = punchingManagementService;
        }

        public async Task<ResponseModel> GetPunchingReportData(ReportRequest reportRequest)
        {
           ResponseModel resp = await _punchingManagementService.GetPunchingReportData(reportRequest);

           return resp;
        }

        public async Task<ResponseModel> AddEpunchRecord(EPunchRequestModel ePunchRequestModel)
        {
            ResponseModel resp = await _punchingManagementService.AddEpunchRecord(ePunchRequestModel);
            return resp;
        }

        public async Task<ResponseModel> GetAssignedSchool(string empId)
        {
            ResponseModel resp = await _punchingManagementService.GetAssignedSchool(empId);
            return resp;
        }

    }
}