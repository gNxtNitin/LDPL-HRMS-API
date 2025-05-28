using MobilePortalManagementLibrary.Models;
//using CustomerManagementLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Implementation;
using Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace LDPLWEBAPI.Controllers
{
    [Authorize(Roles = "ADMIN")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        [HttpGet("GetAllZones")]
        public async Task<IActionResult> GetAllZones()
        {
            try
            {
                ZoneRequestModel zrm = new ZoneRequestModel {
                    flag = "G"
                };
                    
                    
                ResponseModel response = await _adminService.GetCreateUpdateDeleteZone(zrm);
                return Ok(response);
            }
            catch (Exception ex)
            {
                //await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }
        [HttpPost("CreateUpdateDeleteZone")]
        public async Task<IActionResult> CreateUpdateDeleteZone([FromBody] ZoneRequestModel zrm)
        {
            try
            {
                //List<Menu> menuList = new List<Menu>();
                var res = await _adminService.GetCreateUpdateDeleteZone(zrm);
                return Ok(res);
            }
            catch (Exception ex)
            {
                //await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }
        [HttpGet("GetAllCity")]
        public async Task<IActionResult> GetAllCity()
        {
            try
            {
                CityRequestModel crm = new CityRequestModel
                {
                    flag = "G"
                };


                ResponseModel response = await _adminService.GetCreateUpdateDeleteCity(crm);
                return Ok(response);
            }
            catch (Exception ex)
            {
                //await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }
        [HttpPost("CreateUpdateDeleteCity")]
        public async Task<IActionResult> CreateUpdateDeleteCity([FromBody] CityRequestModel crm)
        {
            try
            {
                //List<Menu> menuList = new List<Menu>();
                var res = await _adminService.GetCreateUpdateDeleteCity(crm);
                return Ok(res);
            }
            catch (Exception ex)
            {
                //await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
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
        [HttpGet("GetAllEmployees")]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                EmployeeRequestModel erm = new EmployeeRequestModel
                {
                    flag = "G"
                };


                ResponseModel response = await _adminService.GetCreateUpdateDeleteEmployees(erm);
                return Ok(response);
            }
            catch (Exception ex)
            {
                //await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }
        [HttpPost("CreateUpdateDeleteEmployee")]
        public async Task<IActionResult> CreateUpdateDeleteEmployee([FromBody] EmployeeRequestModel erm)
        {
            try
            {
                //List<Menu> menuList = new List<Menu>();
                var res = await _adminService.GetCreateUpdateDeleteEmployees(erm);
                return Ok(res);
            }
            catch (Exception ex)
            {
                //await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }
        [HttpGet("GetSchoolIncharge")]
        public async Task<IActionResult> GetSchoolIncharge()
        {
            try
            {
                SchoolRequestModel srm = new SchoolRequestModel
                {
                    flag = "G"
                };


                ResponseModel response = await _adminService.AssignSchoolIncharge(srm);
                return Ok(response);
            }
            catch (Exception ex)
            {
                //await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }
        [HttpPost("AssignSchoolIncharge")]
        public async Task<IActionResult> AssignSchoolIncharge([FromBody] SchoolRequestModel srm)
        {
            try
            {
                //List<Menu> menuList = new List<Menu>();
                var res = await _adminService.AssignSchoolIncharge(srm);
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
