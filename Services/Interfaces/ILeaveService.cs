using MobilePortalManagementLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ILeaveService
    {
        Task<ResponseModel> AddUpdateGetLeaveRecord(LeaveRequestModel lrm);
        Task<ResponseModel> AddUpdateGetTeamLeaveRecord(LeaveRequestModel lrm);
    }
}
