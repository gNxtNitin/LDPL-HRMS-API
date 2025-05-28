//using CustomerManagementLibrary.Models;

using MobilePortalManagementLibrary.Interface;
using MobilePortalManagementLibrary.Models;
using Services.Interfaces;

namespace Services.Implementation
{
    public class AdminService: IAdminService
    {
        private readonly IAdminManagementService _adminManagementService;
        public AdminService(IAdminManagementService adminManagementService) 
        {
            _adminManagementService = adminManagementService;
        }
        public async Task<ResponseModel> GetCreateUpdateDeleteZone(ZoneRequestModel zrm)
        {
            ResponseModel response = await _adminManagementService.GetCreateUpdateDeleteZone(zrm);
            return response;
        }
        public async Task<ResponseModel> GetCreateUpdateDeleteCity(CityRequestModel crm)
        {
            ResponseModel response = await _adminManagementService.GetCreateUpdateDeleteCity(crm);
            return response;
        }
        public async Task<ResponseModel> GetData(string flag, string cid1)
        {
            ResponseModel response = await _adminManagementService.GetData(flag, cid1);
            return response;
        }
        public async Task<ResponseModel> GetCreateUpdateDeleteEmployees(EmployeeRequestModel erm)
        {
            ResponseModel response = await _adminManagementService.GetCreateUpdateDeleteEmployees(erm);
            return response;
        }
        public async Task<ResponseModel> GetCreateUpdateDeleteSchool(SchoolRequestModel srm)
        {
            ResponseModel response = await _adminManagementService.GetCreateUpdateDeleteSchool(srm);
            return response;
        }
        public async Task<ResponseModel> AssignSchoolIncharge(SchoolRequestModel srm)
        {
            ResponseModel response = await _adminManagementService.AssignSchoolIncharge(srm);
            return response;
        }
    }
}
