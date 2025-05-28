using MobilePortalManagementLibrary.Models;

namespace Services.Interfaces
{
    public interface IPunchingService
    {
        Task<ResponseModel> GetPunchingReportData(ReportRequest reportRequest);
        Task<ResponseModel> AddEpunchRecord(EPunchRequestModel ePunchRequestModel);

        Task<ResponseModel> GetAssignedSchool(string empId);
    }
}
