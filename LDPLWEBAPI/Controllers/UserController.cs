using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobilePortalManagementLibrary.Models;
using Services.Interfaces;

namespace LDPLWEBAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public UserController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        [HttpGet("GetAllSchool")]
        public async Task<IActionResult> GetAllSchool(string cid1)
        {
            try
            {
                SchoolRequestModel srm = new SchoolRequestModel
                {
                    flag = "G",
                    EmpId = cid1
                };


                ResponseModel response = await _adminService.GetCreateUpdateDeleteSchool(srm);
                return Ok(response);
            }
            catch (Exception ex)
            {
                //await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }

       
        [HttpGet("GetData")]
        public async Task<IActionResult> GetData(string cid1, string? cid2)
        {
            try
            {

                ResponseModel response = await _adminService.GetData(cid1, cid2);
                return Ok(response);
            }
            catch (Exception ex)
            {
                //await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }
        [HttpPost("CreateUpdateDeleteSchool")]
        public async Task<IActionResult> CreateUpdateDeleteSchool([FromBody] SchoolRequestModel srm)
        {
            try
            {
                //List<Menu> menuList = new List<Menu>();
                var res = await _adminService.GetCreateUpdateDeleteSchool(srm);
                return Ok(res);
            }
            catch (Exception ex)
            {
                //await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }
    }
}
