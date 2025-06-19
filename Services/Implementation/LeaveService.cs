using MobilePortalManagementLibrary.Interface;
using MobilePortalManagementLibrary.Models;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class LeaveService:ILeaveService
    {
        private readonly ILeaveManagementService _leaveManagementService;
        public LeaveService(ILeaveManagementService leaveManagementService)
        {
            _leaveManagementService = leaveManagementService;
        }
        public async Task<ResponseModel> AddUpdateGetLeaveRecord(LeaveRequestModel lrm)
        {

            ResponseModel response = await _leaveManagementService.AddUpdateGetLeaveRecord(lrm);
            return response;
        }
        public async Task<ResponseModel> AddUpdateGetTeamLeaveRecord(LeaveRequestModel lrm)
        {

            ResponseModel response = await _leaveManagementService.AddUpdateGetTeamLeaveRecord(lrm);
            return response;
        }
    }
}
