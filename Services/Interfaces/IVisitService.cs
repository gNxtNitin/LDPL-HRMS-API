using MobilePortalManagementLibrary.Models;

namespace Services.Interfaces
{
    public interface IVisitService
    {
        public Task<ResponseModel> GetVisitReportData(string userId, bool isTeamData = false);
    }
}
