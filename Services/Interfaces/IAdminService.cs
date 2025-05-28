using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobilePortalManagementLibrary.Models;

namespace Services.Interfaces
{
    public interface IAdminService
    {
        Task<ResponseModel> GetCreateUpdateDeleteZone(ZoneRequestModel zrm);
        Task<ResponseModel> GetCreateUpdateDeleteCity(CityRequestModel crm);
        Task<ResponseModel> GetData(string flag, string cid1);
        Task<ResponseModel> GetCreateUpdateDeleteSchool(SchoolRequestModel srm);
        Task<ResponseModel> GetCreateUpdateDeleteEmployees(EmployeeRequestModel erm);
        Task<ResponseModel> AssignSchoolIncharge(SchoolRequestModel srm);
    }
}
