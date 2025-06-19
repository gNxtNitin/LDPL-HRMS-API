using MobilePortalManagementLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePortalManagementLibrary.Interface
{
    public interface ILeaveManagementService
    {
        Task<ResponseModel> AddUpdateGetLeaveRecord(LeaveRequestModel lrm);
        Task<ResponseModel> AddUpdateGetTeamLeaveRecord(LeaveRequestModel lrm);
    }
}
