using AuthLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobilePortalManagementLibrary.Models;
using Services.Implementation;
using Services.Interfaces;

namespace LDPLWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveController : ControllerBase
    {
        private readonly ILeaveService _leaveService;
        public LeaveController(ILeaveService leaveService)
        {
            _leaveService = leaveService;
        }
        [HttpPost("AddUpdateLeaveReport")]
        public async Task<IActionResult> AddUpdateLeaveReport([FromBody] LeaveRequestModel lrm)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {

                responseModel = await _leaveService.AddUpdateGetLeaveRecord(lrm);

                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                //await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }
        [HttpGet("GetLeaveReport")]
        public async Task<IActionResult> GetLeaveReport(string cid1)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                LeaveRequestModel lrm = new LeaveRequestModel
                {
                    flag = "G",
                    EmpId = cid1
                };
                responseModel = await _leaveService.AddUpdateGetLeaveRecord(lrm);

                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                //await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }
        [HttpGet("GetTeamLeaveReport")]
        public async Task<IActionResult> GetTeamLeaveReport(string cid1)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                LeaveRequestModel lrm = new LeaveRequestModel
                {
                    flag = "G",
                    EmpId = cid1
                };
                responseModel = await _leaveService.AddUpdateGetTeamLeaveRecord(lrm);

                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                //await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }
        [HttpPost("ApproveTeamLeaveReport")]
        public async Task<IActionResult> ApproveTeamLeaveReport([FromBody] LeaveRequestModel lrm)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {

                responseModel = await _leaveService.AddUpdateGetTeamLeaveRecord(lrm);

                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                //await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }
        [HttpPost("MultiApproveTeamLeaveReport")]
        public async Task<IActionResult> MultiApproveTeamLeaveReport([FromBody] List<LeaveRequestModel> lrm)
        {
            ResponseModel responseModel = new ResponseModel();
            if (lrm == null || !lrm.Any())
            {
                return BadRequest("Request list cannot be null or empty.");
            }

            var results = new List<int>();
            bool allSuccessful = true;

            try
            {
                foreach (var item in lrm)
                {
                    if (item.Id == null ||
                        item.Status == null ||
                        string.IsNullOrWhiteSpace(item.EmpId))
                    {
                        allSuccessful = false;
                        results.Add(-1);
                        continue;
                    }

                    try
                    {
                        var response = await _leaveService.AddUpdateGetTeamLeaveRecord(item);
                        results.Add(response.code);
                    }
                    catch (Exception ex)
                    {
                        allSuccessful = false;
                        results.Add(-1);
                    }
                }
                ResponseModel resultSummary = new ResponseModel
                {
                    code = allSuccessful ? 1 : -1,
                    msg = allSuccessful ? "All requests processed successfully." : "Some requests failed.",
                    data = new MultiApproveStatus()
                    {
                        SuccessCount = results.Count(r => r > 0),
                        FailureCount = results.Count(r => r <= 0)
                    }
                };

                return Ok(resultSummary);
            }
            catch (Exception ex)
            {
                //await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = "An error occurred while processing the request.", Details = ex.Message });

            }
        }
    }
}
