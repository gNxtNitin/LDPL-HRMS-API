using MobilePortalManagementLibrary.Interface;
using MobilePortalManagementLibrary.Models;
using Services.Interfaces;

namespace Services.Implementation
{
    public class VisitService : IVisitService
    {
        private readonly IVisitsManagementService _visitsManagementService;
        public VisitService(IVisitsManagementService visitsManagementService)
        {
            _visitsManagementService = visitsManagementService;
        }

        public async Task<ResponseModel> GetVisitReportData(string userId, bool isTeamData = false)
        {
            ResponseModel resp = await _visitsManagementService.GetVisitsReportData(userId, isTeamData);

            return resp;
        }
    }
}
